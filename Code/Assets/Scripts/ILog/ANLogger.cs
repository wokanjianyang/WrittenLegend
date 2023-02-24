#if !NOT_UNITY
using SA.Android.Utilities;
using System;

namespace Game
{
    public class ANLogger: ILog
    {
        public void Trace(string msg)
        {
            AN_Logger.Log(msg);

        }

        public void Debug(string msg)
        {
            AN_Logger.Log(msg);
        }

        public void Info(string msg)
        {
            AN_Logger.Log(msg);
        }

        public void Warning(string msg)
        {
            AN_Logger.LogWarning(msg);
        }

        public void Error(string msg)
        {
            AN_Logger.LogError(msg);
        }

        public void Error(Exception e)
        {
            AN_Logger.LogError(e);
        }

        public void Trace(string message, params object[] args)
        {
            AN_Logger.Log(string.Format(message, args));
        }

        public void Warning(string message, params object[] args)
        {
            AN_Logger.LogWarning(string.Format(message, args));
        }

        public void Info(string message, params object[] args)
        {
            AN_Logger.Log(string.Format(message, args));
        }

        public void Debug(string message, params object[] args)
        {
            AN_Logger.Log(string.Format(message, args));
        }

        public void Error(string message, params object[] args)
        {
            AN_Logger.LogError(string.Format(message, args));
        }
    }
}
#endif