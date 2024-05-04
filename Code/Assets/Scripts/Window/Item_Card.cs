using Game.Data;
using Sirenix.OdinInspector;
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
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            User user = GameProcessor.Inst.User;

            long maxLevel = user.GetCardLimit(Config);
            long cardLevel = user.GetCardLevel(Config.Id);

            if (cardLevel < maxLevel)
            {
                long rise = cardLevel / Config.RiseLevel * Config.RiseNumber;

                int itemId = Config.Id;
                long upNumber = 1 + rise;

                if (Config.StoneNumber > 0)
                {
                    itemId = ItemHelper.SpecialId_Card_Stone;
                    upNumber = Config.StoneNumber + rise;
                }

                long total = user.GetItemMeterialCount(itemId);

                if (total < upNumber)
                {
                    //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的材料不足", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                user.UseItemMeterialCount(itemId, upNumber);
                user.SaveCardLevel(Config.Id);

                this.Show();


                GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
            }
            else
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        private void Show()
        {
            User user = GameProcessor.Inst.User;

            long cardLevel = user.GetCardLevel(Config.Id);


            if (cardLevel > 0)
            {
                long val = Config.AttrValue + (cardLevel - 1) * Config.LevelIncrea;
                this.Txt_Level.text = $"{cardLevel}级";
                this.Txt_Attr_Current.text = StringHelper.FormatAttrText(Config.AttrId, val);
                this.Txt_Attr_Rise.text = "升级增加:" + StringHelper.FormatAttrValueText(Config.AttrId, Config.LevelIncrea);
            }
            else
            {
                this.Txt_Level.text = "未激活";
                this.Txt_Attr_Current.text = " ???? ";
                this.Txt_Attr_Rise.text = "激活增加:" + StringHelper.FormatAttrValueText(Config.AttrId, Config.AttrValue);
            }

            long rise = cardLevel / Config.RiseLevel * Config.RiseNumber;

            int itemId = Config.Id;
            long upNumber = 1 + rise;

            if (Config.StoneNumber > 0)
            {
                itemId = ItemHelper.SpecialId_Card_Stone;
                upNumber = Config.StoneNumber + rise;
            }

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
