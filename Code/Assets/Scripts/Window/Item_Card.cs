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

            MagicData cardData = user.CardData[Config.Id];

            if (cardData.Data < Config.MaxLevel)
            {
                long total = user.Bags.Where(m => m.Item.Type == ItemType.Card && m.Item.ConfigId == Config.Id).Select(m => m.MagicNubmer.Data).Sum();

                if (total < 1)
                {
                    long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Card_Stone).Select(m => m.MagicNubmer.Data).Sum();

                    if (stoneTotal < Config.StoneNumber)
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您没有此图鉴与足够的图鉴碎片", ToastType = ToastTypeEnum.Failure });
                        return;
                    }

                    GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否使用"+ Config.StoneNumber + "个图鉴碎片升级？", true, () =>
                    {
                        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                        {
                            Type = ItemType.Material,
                            ItemId = ItemHelper.SpecialId_Card_Stone,
                            Quantity = Config.StoneNumber
                        });
                        cardData.Data++;
                        this.SetContent(this.Config, cardData.Data);
                        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
                    }, null);

                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = ItemType.Card,
                        ItemId = Config.Id,
                        Quantity = 1
                    });

                    cardData.Data++;
                    this.SetContent(this.Config, cardData.Data);
                    GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
                }

                //
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        public void SetContent(CardConfig config, long level)
        {
            this.Config = config;

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), config.Name);

            if (level > 0)
            {
                long val = config.AttrValue + (level - 1) * config.LevelIncrea;
                this.Txt_Level.text = $"{level}级";
                this.Txt_Attr_Current.text = StringHelper.FormatAttrText(config.AttrId, val);
                this.Txt_Attr_Rise.text = "升级增加:" + StringHelper.FormatAttrValueText(config.AttrId, config.LevelIncrea);
            }
            else
            {

                this.Txt_Level.text = "未激活";
                this.Txt_Attr_Current.text = " ???? ";
                this.Txt_Attr_Rise.text = "激活增加:" + StringHelper.FormatAttrValueText(config.AttrId, config.AttrValue);
            }
        }
    }
}
