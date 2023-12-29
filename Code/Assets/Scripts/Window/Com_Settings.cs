using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.UI.Dropdown;
using System;
using Game.Data;

namespace Game
{
    public class Com_Settings : MonoBehaviour
    {
        [LabelText("名字输入框")]
        public InputField if_Name;
        [LabelText("修改")]
        public Button btn_ChangeName;
        [LabelText("兑换码输入框")]
        public InputField if_Code;
        [LabelText("兑换")]
        public Button btn_Code;

        [LabelText("确认")]
        public Button btn_Done;

        private const int CHARACTER_LIMIT = 10;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Done.onClick.AddListener(this.OnClick_Done);

            this.btn_ChangeName.onClick.AddListener(this.OnClick_ChangeName);
            this.btn_Code.onClick.AddListener(this.OnClick_Code);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick_Done()
        {

            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());
        }

        public void OnClick_ChangeName()
        {
            var name = this.SplitNameByUTF8(this.if_Name.text.Trim());
            GameProcessor.Inst.User.Name = name;
            UserData.Save();
            //设置名称
            GameProcessor.Inst.User.EventCenter.Raise(new SetPlayerNameEvent
            {
                Name = name
            });
            GameProcessor.Inst.PlayerManager.GetHero().EventCenter.Raise(new SetPlayerNameEvent
            {
                Name = name
            });
        }

        public void OnClick_Code()
        {
            string code = if_Code.text;
            if (code != null)
            {
                code = code.Trim();

                if (code.Length > 20)
                {
                    SpecialCode(code);
                }
                else
                {
                    NormalCode(code);
                }

            }
        }

        private void NormalCode(string code)
        {
            User user = GameProcessor.Inst.User;

            if (user.GiftList.ContainsKey(code))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经使用了兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            List<CodeConfig> list = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            List<CodeConfig> configs = list.Where(m => m.code == code).ToList();

            if (configs.Count != 1)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有这个兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兑换成功", ToastType = ToastTypeEnum.Success });

            CodeConfig config = configs[0];

            List<Item> items = new List<Item>();

            for (int i = 0; i < config.ItemTypeList.Count(); i++)
            {
                int quantity = 1;
                if (config.ItemQuanlityList != null && config.ItemQuanlityList.Count() > i)
                {
                    quantity = config.ItemQuanlityList[i];
                }

                ItemType type = (ItemType)config.ItemTypeList[i];

                if (type == ItemType.Gold)
                {
                    user.AddExpAndGold(0, 100000000L * quantity);
                }
                else
                {
                    Item item = ItemHelper.BuildItem(type, config.ItemIdList[i], 0, quantity);
                    items.Add(item);
                }
            }

            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

            user.GiftList[code] = true;
        }

        private void SpecialCode(string code)
        {
            User user = GameProcessor.Inst.User;

            if (user.GiftList.ContainsKey(code))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经使用了兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (UserData.tapAccount == "")
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先在其他设置里面,绑定Tap帐号", ToastType = ToastTypeEnum.Failure });
                return;
            }

            string skey = getKey();

#if UNITY_EDITOR
            skey = "fb2d1feffd645dae1c574954fd702a80";
#endif

            string realCode = EncryptionHelper.AesDecrypt(code, UserData.tapAccount);

            Debug.Log("realCode1:" + realCode);

            realCode = EncryptionHelper.AesDecrypt(realCode, skey);

            Debug.Log("realCode1:" + realCode);

            CodeConfig config = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.code == realCode).FirstOrDefault();

            if (config == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有这个兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            user.AdData.SaveCode(realCode);
        }

        //4、UTF8编码格式（汉字3byte，英文1byte）,//UTF8编码格式,目前是最常用的 
        private string SplitNameByUTF8(string temp)
        {
            string outputStr = "";
            int count = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                string tempStr = temp.Substring(i, 1);
                byte[] encodedBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(tempStr);//Unicode用两个字节对字符进行编码
                string output = "[" + temp + "]";
                for (int byteIndex = 0; byteIndex < encodedBytes.Length; byteIndex++)
                {
                    output += Convert.ToString((int)encodedBytes[byteIndex], 2) + "  ";//二进制
                }

                int byteCount = System.Text.ASCIIEncoding.UTF8.GetByteCount(tempStr);

                if (byteCount > 1)
                {
                    count += 2;
                }
                else
                {
                    count += 1;
                }
                if (count <= CHARACTER_LIMIT)
                {
                    outputStr += tempStr;
                }
                else
                {
                    break;
                }
            }
            return outputStr;
        }

        private static string getKey()
        {
#if UNITY_EDITOR
            return "";
#endif
            //string pn = Application.identifier;
            //pn = EncryptionHelper.AesEncrypt(pn) + EncryptionHelper.Md5(pn + "8932kMD5#>>");
            //if (pn != "CZiSFbEnJLzHUa2n4QiF3a5EgGe+458f4EBvGvm+xZQ=ebe5d8b49fc4c8e07ebb7ddf8cb95fa5")
            //{
            //	return false;
            //}
            //UserData.pn = EncryptionHelper.Md5(pn + "z1!");

            // 获取Android的PackageManager    
            AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

            // 获取当前Android应用的包名
            string packageName = Activity.Call<string>("getPackageName");

            // 调用PackageManager的getPackageInfo方法来获取签名信息数组    
            int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
            AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
            AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");

            // 获取当前的签名的哈希值，判断其与我们签名的哈希值是否一致
            if (Signatures != null && Signatures.Length > 0)
            {
                byte[] bytes = Signatures[0].Call<byte[]>("toByteArray");

                string hashCode = EncryptionHelper.GetMD5(bytes).ToUpper();

                hashCode = EncryptionHelper.Md5(hashCode + "12sd#$kd0z54");

                //UserData.sk = EncryptionHelper.Md5(hashCode + "#2A");

                return hashCode;
            }

            return null;
        }
    }
}
