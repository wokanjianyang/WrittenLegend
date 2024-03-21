using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public static class NetworkHelper
    {
        private static string home = "http://127.0.0.1:11111/public/";

        public static string[] GetAddressIPs()
        {
            List<string> list = new List<string>();
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                {
                    continue;
                }
                foreach (UnicastIPAddressInformation add in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    list.Add(add.Address.ToString());
                }
            }
            return list.ToArray();
        }

        public static IPEndPoint ToIPEndPoint(string host, int port)
        {
            return new IPEndPoint(IPAddress.Parse(host), port);
        }

        public static IPEndPoint ToIPEndPoint(string address)
        {
            int index = address.LastIndexOf(':');
            string host = address.Substring(0, index);
            string p = address.Substring(index + 1);
            int port = int.Parse(p);
            return ToIPEndPoint(host, port);
        }

        public static string BuildSign()
        {
            string deviceId = AppHelper.GetDeviceIdentifier();
            string account = GameProcessor.Inst.User.DeviceId;

            string skey = AppHelper.getKey();

            string code = EncryptionHelper.AesEncrypt(deviceId, skey);
            code = EncryptionHelper.AesEncrypt(code, account);

            return code;
        }

        public static IEnumerator CreateAccount(string account, Action<string> successAction, Action failAction)
        {
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(account);

            return SendRequest("create_user", bytes, successAction, failAction);
        }

        public static IEnumerator UpdateInfo(string data, Action<string> successAction, Action failAction)
        {
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(data);

            return SendRequest("update_info", bytes, successAction, failAction);
        }

        public static IEnumerator UploadData(byte[] bytes, Action<string> successAction, Action failAction)
        {
            return SendRequest("save_user_file", bytes, successAction, failAction);
        }

        public static IEnumerator DownData(Action<byte[]> successAction, Action failAction)
        {
            string url = home + "down_user_file";

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                string path = UserData.getSavePath();
                using (var db = new DownloadHandlerBuffer())
                {
                    string account = GameProcessor.Inst.User.DeviceId;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string key = GameProcessor.Inst.User.Pwd;
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("key", key);
                    request.SetRequestHeader("sign", sign);

                    request.downloadHandler.Dispose();
                    request.downloadHandler = db;
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Down Error:" + request.error);
                        failAction?.Invoke();
                    }
                    else
                    {
                        byte[] data = ((DownloadHandlerBuffer)request.downloadHandler).data;
                        successAction?.Invoke(data);
                    }
                }
            }
        }

        public static IEnumerator SendRequest(string ac, byte[] bytes, Action<string> successAction, Action failAction)
        {
            string url = home + ac;

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var uh = new UploadHandlerRaw(bytes))
                {
                    string account = GameProcessor.Inst.User.DeviceId;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string key = GameProcessor.Inst.User.Pwd;
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("key", key);
                    request.SetRequestHeader("sign", sign);

                    request.uploadHandler.Dispose();
                    request.uploadHandler = uh;
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Upload Error:" + request.error);
                        failAction?.Invoke();
                    }
                    else
                    {
                        Debug.Log("Upload complete! Server response: " + request.downloadHandler.text);
                        successAction?.Invoke(request.downloadHandler.text);
                    }
                }
            }
        }
    }
}
