using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Card : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Attr_Rise;
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Attr_Current;
        public Text Txt_Fee;
        public CardConfig Config { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void OnEnable()
        {
            if (Config != null)
            {
                this.Show();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            User user = GameProcessor.Inst.User;

            long maxLevel = user.GetCardLimit(Config);
            long cardLevel = user.GetCardLevel(Config.Id);

            if (cardLevel < maxLevel)
            {
                int itemId = Config.RiseId;
                long upNumber = Config.CalNewUpNumber(cardLevel);

                long total = user.GetItemMeterialCount(itemId);

                if (total < upNumber)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的材料不足", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                user.UseItemMeterialCount(itemId, upNumber);
                user.SaveCardLevel(Config.Id, 1);

                this.Show();


                GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;

            int groupLevel = user.GetCardSpecialGroupLevel();

            long cardLevel = user.GetCardLevel(Config.Id);

            long riseLevel = user.GetCardRiseLevel(Config.Quality, cardLevel, groupLevel);

            Debug.Log(string.Format("card cardLevel : {0},groupLevel:{1},riseLevel{2}", cardLevel, groupLevel, riseLevel));

            long totalLevel = cardLevel + riseLevel;
            long val = Config.AttrValue * totalLevel;

            long riseValue = Config.GetCardRiseValue(totalLevel, groupLevel);

            if (Config.AttrId > 0)
            {
                string txtCurrent = StringHelper.FormatAttrText(Config.AttrId, val);

                if (riseValue > 0)
                {
                    txtCurrent += "+" + StringHelper.FormatAttrValueText(Config.AttrId, riseValue);
                }
                this.Txt_Attr_Current.text = txtCurrent;
                this.Txt_Attr_Rise.text = "升级增加:" + StringHelper.FormatAttrValueText(Config.AttrId, Config.AttrValue);
            }
            else
            {
                string txtCurrent = string.Format(Config.Des, val);
                if (riseValue > 0)
                {
                    txtCurrent += "+" + riseValue + "%";
                }

                this.Txt_Attr_Current.text = txtCurrent;
                this.Txt_Attr_Rise.text = "升级增加:1%";
            }

            if (riseLevel > 0)
            {
                this.Txt_Level.text = $"等级{cardLevel}+{riseLevel}";
            }
            else
            {
                this.Txt_Level.text = $"等级{cardLevel}";
            }

            int itemId = Config.RiseId;
            long upNumber = Config.CalNewUpNumber(cardLevel);

            long total = user.GetItemMeterialCount(itemId);

            string color = total >= upNumber ? "#FFFF00" : "#FF0000";

            Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, total, upNumber);
        }

        public void SetContent(CardConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), config.Name);

            this.Show();
        }
    }
}
