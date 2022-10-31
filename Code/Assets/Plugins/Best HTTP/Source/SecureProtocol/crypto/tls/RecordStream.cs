#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

using BestHTTP.PlatformSupport.Memory;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /// <summary>An implementation of the TLS 1.0/1.1/1.2 record layer, allowing downgrade to SSLv3.</summary>
    internal sealed class RecordStream
    {
        private const int DEFAULT_PLAINTEXT_LIMIT = (1 << 14);

        internal const int TLS_HEADER_SIZE = 5;
        internal const int TLS_HEADER_TYPE_OFFSET = 0;
        internal const int TLS_HEADER_VERSION_OFFSET = 1;
        internal const int TLS_HEADER_LENGTH_OFFSET = 3;

        private TlsProtocol mHandler;
        private Stream mInput;
        private Stream mOutput;
        private TlsCompression mPendingCompression = null, mReadCompression = null, mWriteCompression = null;
        private TlsCipher mPendingCipher = null, mReadCipher = null, mWriteCipher = null;
        private SequenceNumber mReadSeqNo = new SequenceNumber(), mWriteSeqNo = new SequenceNumber();
        private MemoryStream mBuffer = new MemoryStream();

        private TlsHandshakeHash mHandshakeHash = null;
        private readonly BaseOutputStream mHandshakeHashUpdater;

        private ProtocolVersion mReadVersion = null, mWriteVersion = null;
        private bool mRestrictReadVersion = true;

        private int mPlaintextLimit, mCompressedLimit, mCiphertextLimit;

        internal RecordStream(TlsProtocol handler, Stream input, Stream output)
        {
            this.mHandler = handler;
            this.mInput = input;
            this.mOutput = output;
            this.mReadCompression = new TlsNullCompression();
            this.mWriteCompression = this.mReadCompression;
            this.mHandshakeHashUpdater = new HandshakeHashUpdateStream(this);
        }

        internal /*virtual*/ void Init(TlsContext context)
        {
            this.mReadCipher = new TlsNullCipher(context);
            this.mWriteCipher = this.mReadCipher;
            this.mHandshakeHash = new DeferredHash();
            this.mHandshakeHash.Init(context);

            SetPlaintextLimit(DEFAULT_PLAINTEXT_LIMIT);
        }

        internal /*virtual*/ int GetPlaintextLimit()
        {
            return mPlaintextLimit;
        }

        internal /*virtual*/ void SetPlaintextLimit(int plaintextLimit)
        {
            this.mPlaintextLimit = plaintextLimit;
            this.mCompressedLimit = this.mPlaintextLimit + 1024;
            this.mCiphertextLimit = this.mCompressedLimit + 1024;
        }

        internal /*virtual*/ ProtocolVersion ReadVersion
        {
            get { return mReadVersion; }
            set { this.mReadVersion = value; }
        }

        internal /*virtual*/ void SetWriteVersion(ProtocolVersion writeVersion)
        {
            this.mWriteVersion = writeVersion;
        }

        /**
         * RFC 5246 E.1. "Earlier versions of the TLS specification were not fully clear on what the
         * record layer version number (TLSPlaintext.version) should contain when sending ClientHello
         * (i.e., before it is known which version of the protocol will be employed). Thus, TLS servers
         * compliant with this specification MUST accept any value {03,XX} as the record layer version
         * number for ClientHello."
         */
        internal /*virtual*/ void SetRestrictReadVersion(bool enabled)
        {
            this.mRestrictReadVersion = enabled;
        }

        internal /*virtual*/ void SetPendingConnectionState(TlsCompression tlsCompression, TlsCipher tlsCipher)
        {
            this.mPendingCompression = tlsCompression;
            this.mPendingCipher = tlsCipher;
        }

        internal /*virtual*/ void SentWriteCipherSpec()
        {
            if (mPendingCompression == null || mPendingCipher == null)
                throw new TlsFatalAlert(AlertDescription.handshake_failure);

            this.mWriteCompression = this.mPendingCompression;
            this.mWriteCipher = this.mPendingCipher;
            this.mWriteSeqNo = new SequenceNumber();
        }

        internal /*virtual*/ void ReceivedReadCipherSpec()
        {
            if (mPendingCompression == null || mPendingCipher == null)
                throw new TlsFatalAlert(AlertDescription.handshake_failure);

            this.mReadCompression = this.mPendingCompression;
            this.mReadCipher = this.mPendingCipher;
            this.mReadSeqNo = new SequenceNumber();
        }

        internal /*virtual*/ void FinaliseHandshake()
        {
            if (mReadCompression != mPendingCompression || mWriteCompression != mPendingCompression
                || mReadCipher != mPendingCipher || mWriteCipher != mPendingCipher)
            {
                throw new TlsFatalAlert(AlertDescription.handshake_failure);
            }
            this.mPendingCompression = null;
            this.mPendingCipher = null;
        }

        internal /*virtual*/ void CheckRecordHeader(byte[] recordHeader)
        {
            byte type = TlsUtilities.ReadUint8(recordHeader, TLS_HEADER_TYPE_OFFSET);

            /*
             * RFC 5246 6. If a TLS implementation receives an unexpected record type, it MUST send an
             * unexpected_message alert.
             */
            CheckType(type, AlertDescription.unexpected_message);

            if (!mRestrictReadVersion)
            {
                int version = TlsUtilities.ReadVersionRaw(recordHeader, TLS_HEADER_VERSION_OFFSET);
                if ((version & 0xffffff00) != 0x0300)
                    throw new TlsFatalAlert(AlertDescription.illegal_parameter);
            }
            else
            {
                ProtocolVersion version = TlsUtilities.ReadVersion(recordHeader, TLS_HEADER_VERSION_OFFSET);
                if (mReadVersion == null)
                {
                    // Will be set later in 'readRecord'
                }
                else if (!version.Equals(mReadVersion))
                {
                    throw new TlsFatalAlert(AlertDescription.illegal_parameter);
                }
            }

            int length = TlsUtilities.ReadUint16(recordHeader, TLS_HEADER_LENGTH_OFFSET);

            CheckLength(length, mCiphertextLimit, AlertDescription.record_overflow);
        }

        internal /*virtual*/ bool ReadRecord()
        {
            BufferSegment recordHeader = TlsUtilities.ReadAllOrNothing(TLS_HEADER_SIZE, mInput);
            if (recordHeader == BufferSegment.Empty )
                return false;

            byte type = TlsUtilities.ReadUint8(recordHeader.Data, TLS_HEADER_TYPE_OFFSET);

            /*
             * RFC 5246 6. If a TLS implementation receives an unexpected record type, it MUST send an
             * unexpected_message alert.
             */
            CheckType(type, AlertDescription.unexpected_message);

            if (!mRestrictReadVersion)
            {
                int version = TlsUtilities.ReadVersionRaw(recordHeader.Data, TLS_HEADER_VERSION_OFFSET);
                if ((version & 0xffffff00) != 0x0300)
                    throw new TlsFatalAlert(AlertDescription.illegal_parameter);
            }
            else
            {
                ProtocolVersion version = TlsUtilities.ReadVersion(recordHeader.Data, TLS_HEADER_VERSION_OFFSET);
                if (mReadVersion == null)
                {
                    mReadVersion = version;
                }
                else if (!version.Equals(mReadVersion))
                {
                    throw new TlsFatalAlert(AlertDescription.illegal_parameter);
                }
            }

            int length = TlsUtilities.ReadUint16(recordHeader.Data, TLS_HEADER_LENGTH_OFFSET);

            BufferPool.Release(recordHeader.Data);

            CheckLength(length, mCiphertextLimit, AlertDescription.record_overflow);

            BufferSegment plaintext = DecodeAndVerify(type, mInput, length);
            mHandler.ProcessRecord(type, plaintext.Data, plaintext.Offset, plaintext.Count);
            BufferPool.Release(plaintext);

            return true;
        }

        internal /*virtual*/ BufferSegment DecodeAndVerify(byte type, Stream input, int len)
        {
            BufferSegment buf = TlsUtilities.ReadFullyOptimized(len, input);

            long seqNo = mReadSeqNo.NextValue(AlertDescription.unexpected_message);
            BufferSegment decoded = mReadCipher.DecodeCiphertext(seqNo, type, buf.Data, buf.Offset, buf.Count);

            if (buf.Data != decoded.Data)
                BufferPool.Release(buf);

            CheckLength(decoded.Count, mCompressedLimit, AlertDescription.record_overflow);

            /*
             * TODO 5246 6.2.2. Implementation note: Decompression functions are responsible for
             * ensuring that messages cannot cause internal buffer overflows.
             */
            //Stream cOut = mReadCompression.Decompress(mBuffer);
            //if (cOut != mBuffer)
            //{
            //    cOut.Write(decoded, 0, decoded.Length);
            //    cOut.Flush();
            //    decoded = GetBufferContents();
            //}

            /*
             * RFC 5246 6.2.2. If the decompression function encounters a TLSCompressed.fragment that
             * would decompress to a length in excess of 2^14 bytes, it should report a fatal
             * decompression failure error.
             */
            //CheckLength(decoded.Length, mPlaintextLimit, AlertDescription.decompression_failure);

            /*
             * RFC 5246 6.2.1 Implementations MUST NOT send zero-length fragments of Handshake, Alert,
             * or ChangeCipherSpec content types.
             */
            if (decoded.Count < 1 && type != ContentType.application_data)
                throw new TlsFatalAlert(AlertDescription.illegal_parameter);

            return decoded;
        }

        internal /*virtual*/ void WriteRecord(byte type, byte[] plaintext, int plaintextOffset, int plaintextLength)
        {
            // Never send anything until a valid ClientHello has been received
            if (mWriteVersion == null)
                return;

            /*
             * RFC 5246 6. Implementations MUST NOT send record types not defined in this document
             * unless negotiated by some extension.
             */
            CheckType(type, AlertDescription.internal_error);

            /*
             * RFC 5246 6.2.1 The length should not exceed 2^14.
             */
            CheckLength(plaintextLength, mPlaintextLimit, AlertDescription.internal_error);

            /*
             * RFC 5246 6.2.1 Implementations MUST NOT send zero-length fragments of Handshake, Alert,
             * or ChangeCipherSpec content types.
             */
            if (plaintextLength < 1 && type != ContentType.application_data)
                throw new TlsFatalAlert(AlertDescription.internal_error);

            Stream cOut = mWriteCompression.Compress(mBuffer);

            long seqNo = mWriteSeqNo.NextValue(AlertDescription.internal_error);

            BufferSegment ciphertext;
            if (cOut == mBuffer)
            {
                ciphertext = mWriteCipher.EncodePlaintext(seqNo, type, plaintext, plaintextOffset, plaintextLength);
            }
            else
            {
                cOut.Write(plaintext, plaintextOffset, plaintextLength);
                cOut.Flush();
                byte[] compressed = GetBufferContents();

                /*
                 * RFC 5246 6.2.2. Compression must be lossless and may not increase the content length
                 * by more than 1024 bytes.
                 */
                CheckLength(compressed.Length, plaintextLength + 1024, AlertDescription.internal_error);

                ciphertext = mWriteCipher.EncodePlaintext(seqNo, type, compressed, 0, compressed.Length);
            }

            /*
             * RFC 5246 6.2.3. The length may not exceed 2^14 + 2048.
             */
            CheckLength(ciphertext.Count, mCiphertextLimit, AlertDescription.internal_error);

            int recordLength = ciphertext.Count + TLS_HEADER_SIZE;
            byte[] record = BestHTTP.PlatformSupport.Memory.BufferPool.Get(recordLength, true);
            TlsUtilities.WriteUint8(type, record, TLS_HEADER_TYPE_OFFSET);
            TlsUtilities.WriteVersion(mWriteVersion, record, TLS_HEADER_VERSION_OFFSET);
            TlsUtilities.WriteUint16(ciphertext.Count, record, TLS_HEADER_LENGTH_OFFSET);
            Array.Copy(ciphertext.Data, ciphertext.Offset, record, TLS_HEADER_SIZE, ciphertext.Count);
            BufferPool.Release(ciphertext);
            mOutput.Write(record, 0, recordLength);
            BestHTTP.PlatformSupport.Memory.BufferPool.Release(record);
            mOutput.Flush();
        }

        internal /*virtual*/ void NotifyHelloComplete()
        {
            this.mHandshakeHash = mHandshakeHash.NotifyPrfDetermined();
        }

        internal /*virtual*/ TlsHandshakeHash HandshakeHash
        {
            get { return mHandshakeHash; }
        }

        internal /*virtual*/ Stream HandshakeHashUpdater
        {
            get { return mHandshakeHashUpdater; }
        }

        internal /*virtual*/ TlsHandshakeHash PrepareToFinish()
        {
            TlsHandshakeHash result = mHandshakeHash;
            this.mHandshakeHash = mHandshakeHash.StopTracking();
            return result;
        }

        internal /*virtual*/ void SafeClose()
        {
            try
            {
                BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.Dispose(mInput);
            }
            catch (IOException)
            {
            }

            try
            {
                BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.Dispose(mOutput);
            }
            catch (IOException)
            {
            }
        }

        internal /*virtual*/ void Flush()
        {
            mOutput.Flush();
        }

        private byte[] GetBufferContents()
        {
            byte[] contents = mBuffer.ToArray();
            mBuffer.SetLength(0);
            return contents;
        }

        private static void CheckType(byte type, byte alertDescription)
        {
            switch (type)
            {
            case ContentType.application_data:
            case ContentType.alert:
            case ContentType.change_cipher_spec:
            case ContentType.handshake:
            //case ContentType.heartbeat:
                break;
            default:
                throw new TlsFatalAlert(alertDescription);
            }
        }

        private static void CheckLength(int length, int limit, byte alertDescription)
        {
            if (length > limit)
                throw new TlsFatalAlert(alertDescription);
        }

        private class HandshakeHashUpdateStream
            : BaseOutputStream
        {
            private readonly RecordStream mOuter;
            public HandshakeHashUpdateStream(RecordStream mOuter)
            {
                this.mOuter = mOuter;
            }

            public override void Write(byte[] buf, int off, int len)
            {
                mOuter.mHandshakeHash.BlockUpdate(buf, off, len);
            }
        }

        private class SequenceNumber
        {
            private long value = 0L;
            private bool exhausted = false;

            internal long NextValue(byte alertDescription)
            {
                if (exhausted)
                {
                    throw new TlsFatalAlert(alertDescription);
                }
                long result = value;
                if (++value == 0)
                {
                    exhausted = true;
                }
                return result;
            }
        }
    }
}
#pragma warning restore
#endif
