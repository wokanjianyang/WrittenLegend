using BestHTTP.JSON.LitJson;
using SA.Android.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game
{




    public class NativeAPI : MonoBehaviour
    {
        public class AndroidToUnity : AndroidJavaProxy
        {
            public AndroidToUnity() : base("com.pocket.zxpa.IAndroidToUnity")
            {

            }


            public void OnShowAD(string param)
            {
                //param������ʽ: "xxx(0, 0, \"string1\", \"string2\", \"string3\");"
                AN_Logger.Log("get message from android:" + param);

                param = Regex.Replace(param, @"\s", "");//ȥ�����пո�
                List<string> tmp = new List<string>(param.Split(new char[] { '(' }));
                string callbackName = tmp[0];//�ص���������
                string argsStr = tmp[1].Substring(0, tmp[1].Length - 2);//�ص��������еĲ���
                object[] args = JsonMapper.ToObject<object[]>('[' + argsStr + ']');
                object callBack;
                if (callBacks.TryGetValue(callbackName, out callBack))
                {
                    ((Action<object[]>)callBack).Invoke(args);
                }
            }
        }

        private static Dictionary<string, object> callBacks;
        public void Awake()
        {
            AN_Logger.Log("Awake");
            DontDestroyOnLoad(this);
            callBacks = new Dictionary<string, object>();
        }

        void Start()
        {
            AN_Logger.Log("start:NativeAPI");

            if (Application.platform == RuntimePlatform.Android)
            {

                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidToUnity androidToUnity = new AndroidToUnity();
                currentActivity.Call("SetCallBack", androidToUnity);
            }
        }

        void Update()
        {

        }
        

        public static void AddListener(string key, Action<object[]> action)
        {
            AddCallBack(key, action);
        }
        private static void AddCallBack(string key, object action)
        {
            if (callBacks.ContainsKey(key))
            {
                callBacks[key] = action;
            }
            else
            {
                callBacks.Add(key, action);
            }
        }

        public static void RemoveAllListeners(string key)
        {
            if (callBacks.ContainsKey(key))
            {
                callBacks.Remove(key);
            }
        }
    }
}