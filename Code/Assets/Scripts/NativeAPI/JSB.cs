using SA.Android.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class jsb
    {
        static private jsb inst;
        static public jsb reflection
        {
            get
            {
                if (inst == null)
                {
                    inst = new jsb();
                }
                return inst;
            }
        }

        public void callStaticMethod(string className,string methodName,params object[] arg)
        {
            try
            {
                var jc = new AndroidJavaClass(className.Replace("/", "."));
                if(arg.Length==0)
                {
                    jc.CallStatic(methodName);
                }
                else if(arg.Length==1)
                {
                    jc.CallStatic(methodName, arg[0]);
                }
                else
                {
                    jc.CallStatic(methodName, arg);
                }
            

                ////Android端Activity必须持有的对象，是一个FrameLayout
                //AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                ////UnityPlayer构造方法取药一个hostActivity
                //AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                ////自定义Android和Unity3D交互的一个类
                //var androidCall = new AndroidJavaObject("com.unity3d.androidcall.AndroidCall",
                //                                    new System.Object[] { currentActivity });
                //androidCall.Call("onVoidCall", arg);
                //BDebug.Log(className + "/" + methodName + "/" + arg);
            }
            catch(Exception e)
            {
                AN_Logger.LogError(e);
            }
            /*
            //JNI
            if(args.Length<2)
            {
                throw new Exception("At least two parameters are required here.");
            }
            //获得类
            IntPtr clz = AndroidJNI.FindClass(args[0]);
            //获得方法
            IntPtr methodId = AndroidJNI.GetMethodID(clz, args[1], "()V");
            //参数
            jvalue[] v;
            if(args.Length<=3)
            {
                v = new jvalue[0];
            }
            else
            {
                v = new jvalue[args.Length - 3];
            }
            if(args.Length==3)
            {
                methodId = AndroidJNI.GetMethodID(clz, args[1], args[2]);
            }
            else if(args.Length>3)
            {
                for(int i=3;i<args.Length;i++)
                {
                    jvalue jv = new jvalue();
                    jv.l = AndroidJNI.NewStringUTF(args[i]);
                    v[i-3] = jv;
                }
            }
            AndroidJNI.CallStaticStringMethod(clz, methodId, v);

            AndroidJNI.DeleteLocalRef(clz);
            AndroidJNI.DeleteLocalRef(methodId);
            */
        }

        public T callStaticMethod<T>(string className, string methodName,params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var jc = new AndroidJavaClass(className.Replace("/", "."));
                return jc.CallStatic<T>(methodName, args);

            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {

            }
            return default(T);
        }

        public T callMethod<T>(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (var jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        return jo.Call<T>(methodName,args);
                    }
                }

            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {

            }
            return default(T);
        }
    }
}
