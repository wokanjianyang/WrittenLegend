using SA.Android.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PocketAD
    {
        public static PocketAD Inst { get
            {
                if(_inst==null)
                {
                    _inst = new PocketAD();
                }
                return _inst;
            }
        }
        private static PocketAD _inst;

        public delegate void AdStateCallBack(int rv, AdStateEnum state, AdTypeEnum adType);


        public void ShowAD(string action,AdStateCallBack logicCallback)
        {
            string callbackName = "adStateCallBack_" + action;
            NativeAPI.AddListener(callbackName, (args) => {
                System.Diagnostics.Debug.Assert(args.Length == 5);
                AdStateEnum state = (AdStateEnum)((int)args[0]);//AdStateEnum
                AdTypeEnum adType = (AdTypeEnum)((int)args[1]);//��AdTypeEnum
                string adCodeID = (string)args[2];//���λid
                string ad_name = (string)args[3];//���λ����
                string sdkName = (string)args[4];//sdk����

                AN_Logger.Log(string.Format("state:{0} adType:{1} adCodeID:{2} ad_name:{3} sdkName:{4}", state, adType, adCodeID, ad_name, sdkName));
                bool hasReward = false;

                if (state == AdStateEnum.Show)
                {
                    if (logicCallback != null) logicCallback(0, state, adType);
                }
                else if (state == AdStateEnum.NotSupport || state == AdStateEnum.LoadFail)
                {
                    int rv = 548;//��沥��ʧ�ܣ�������!
                    if (logicCallback != null) logicCallback(rv, state, adType);
                }
                else if (state == AdStateEnum.Reward)
                {
                    hasReward = true;
                }
                else if (state == AdStateEnum.VideoComplete)
                {
                    hasReward = true;
                }
                else if (state == AdStateEnum.Click)
                {
                    if (logicCallback != null) logicCallback(0, state, adType);
                }
                else if (state == AdStateEnum.Close)
                {

                    if (adType == AdTypeEnum.RewardVideo && !hasReward)
                    {
                        int rv = 555;//��沥���������޽���
                        if (logicCallback != null) logicCallback(rv, state, adType);
                    }
                    else
                    {
                        if (logicCallback != null) logicCallback(0, state, adType);
                    }
                }
            });
            if (Application.platform == RuntimePlatform.Android)
            {
                int r = jsb.reflection.callMethod<int>("showAD");
                if (r != 0)
                {
                    int rv = 548;//��沥��ʧ�ܣ�������!
                    if (logicCallback != null) logicCallback(rv, AdStateEnum.LoadFail, AdTypeEnum.RewardVideo);
                    NativeAPI.RemoveAllListeners(callbackName);
                }
                return;
            }
            if (logicCallback != null) logicCallback(0, AdStateEnum.Close, AdTypeEnum.RewardVideo);
            NativeAPI.RemoveAllListeners(callbackName);
        }

        public void closeAd(string action)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jsb.reflection.callMethod<int>("closeAD", action);
            }
        }
    }
}