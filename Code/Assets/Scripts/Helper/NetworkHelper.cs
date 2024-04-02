using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public static class NetworkHelper
    {
        //private static string home = "http://127.0.0.1:11111/public/";
        private static string home = "http://47.120.73.196/public/";


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
            string fileId = GameProcessor.Inst.User.DeviceId;
            string skey = AppHelper.getKey();

            string code = EncryptionHelper.AesEncrypt(deviceId, skey);
            //Debug.Log("code:" + code);

            code = EncryptionHelper.Md5(code + fileId);
            //Debug.Log("code:" + code);

            return code;
        }

        public static IEnumerator CreateAccount(string account, string pwd, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("account", account);
            dict.Add("pwd", pwd);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("create_user", bytes, successAction, failAction);
        }

        public static IEnumerator UpdateInfo(string data, Action<WebResultWrapper> successAction, Action failAction)
        {
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(data);

            return SendRequest("update_info", bytes, successAction, failAction);
        }

        public static IEnumerator UploadData(byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("save_user_file", bytes, headers, successAction, failAction);
        }

        public static IEnumerator GetDownParam(Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("get_user_file", Encoding.UTF8.GetBytes(""), successAction, failAction);
        }

        public static IEnumerator DownData(Action<byte[]> successAction, Action failAction)
        {
            string url = home + "down_user_file";

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var db = new DownloadHandlerBuffer())
                {
                    string account = GameProcessor.Inst.User.Account;
                    string fileId = GameProcessor.Inst.User.DeviceId;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("deviceId", deviceId);
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

        public static IEnumerator SendRequest(string action, byte[] bytes, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest(action, bytes, null, successAction, failAction);
        }
        public static IEnumerator SendRequest(string action, byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            string url = home + action;

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var uh = new UploadHandlerRaw(bytes))
                {
                    string account = GameProcessor.Inst.User.Account;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string fileId = GameProcessor.Inst.User.DeviceId;
                    string level = GameProcessor.Inst.User.MagicLevel.Data + "";
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("sign", sign);
                    request.SetRequestHeader("level", level);

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.SetRequestHeader(header.Key, header.Value);
                        }
                    }

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

                        WebResultWrapper result = JsonConvert.DeserializeObject<WebResultWrapper>(request.downloadHandler.text);

                        if (result.Code == StatusMessage.BlackList)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                        }
                        else if (result.Version > ConfigHelper.Version)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new NewVersionEvent() { Version = result.Version });
                        }

                        successAction?.Invoke(result);
                    }
                }
            }
        }
    }
}
