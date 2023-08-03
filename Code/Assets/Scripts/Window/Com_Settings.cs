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

                User user = GameProcessor.Inst.User;

                if (user.GiftList.ContainsKey(code))
                {
                    Debug.Log("您已经使用了兑换码");
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经使用了兑换码",ToastType = ToastTypeEnum.Failure});
                    return;
                }

                List<CodeConfig> list = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

                List<CodeConfig> configs = list.Where(m => m.code == code).ToList();

                if (configs.Count != 1)
                {
                    Debug.Log("没有这个兑换码");
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有这个兑换码",ToastType = ToastTypeEnum.Failure });
                    return;
                }
                
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兑换成功",ToastType = ToastTypeEnum.Success });

                user.GiftList[code] = true;

                CodeConfig config = configs[0];

                List<Item> items = new List<Item>();

                for (int i = 0; i < config.ItemTypeList.Count(); i++)
                {
                    Item item = ItemHelper.BuildItem((ItemType)config.ItemTypeList[i], config.ItemIdList[i], 0, 1);
                    items.Add(item);
                }

                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
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
                    output += Convert.ToString((int)encodedBytes[byteIndex], 2)+"  ";//二进制
                }
                Debug.Log(output);

                int byteCount = System.Text.ASCIIEncoding.UTF8.GetByteCount(tempStr);
                Debug.Log("字节数=" + byteCount);

                if (byteCount>1)
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
    }
}
