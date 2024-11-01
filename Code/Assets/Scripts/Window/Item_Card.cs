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

            long cardLevel = user.GetCardLevel(Config.Id);

            long percent = user.GetCardQualityLevel(Config.Quality);

            long riseLevel = cardLevel * percent / 100;

            long totalLevel = cardLevel + riseLevel;
            long val = Config.AttrValue * totalLevel;
            this.Txt_Level.text = $"等级{cardLevel}+{riseLevel}";

            if (Config.AttrId > 0)
            {
                this.Txt_Attr_Current.text = StringHelper.FormatAttrText(Config.AttrId, totalLevel);
                this.Txt_Attr_Rise.text = "升级增加:" + StringHelper.FormatAttrValueText(Config.AttrId, Config.AttrValue);
            }
            else
            {
                this.Txt_Attr_Current.text = string.Format(Config.Des, totalLevel);
                this.Txt_Attr_Rise.text = "升级增加:1%";
            }

            int itemId = Config.RiseId;
            long upNumber = Config.CalNewUpNumber(cardLevel);

            long total = user.GetItemMeterialCount(itemId);

            string color = total >= upNumber ? "#FFFF00" : "#FF0000";

            Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, upNumber, total);
        }

        public void SetContent(CardConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), config.Name);

            this.Show();
        }
    }
}
