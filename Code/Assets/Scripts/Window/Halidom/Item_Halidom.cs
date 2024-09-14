using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Halidom : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Attr_Rise;
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Attr_Current;

        public HalidomConfig Config { get; set; }

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

            MagicData halidomData = user.HalidomData[Config.Id];
            int maxLevel = user.GetHolidomLimit();

            //Debug.Log("Holidom maxLEvel:" + maxLevel);

            long currentLevel = halidomData.Data;
            if (currentLevel < maxLevel)
            {
                int rise = (int)currentLevel * 1;

                long total = user.Bags.Where(m => m.Item.Type == ItemType.Halidom && m.Item.ConfigId == Config.ItemId).Select(m => m.MagicNubmer.Data).Sum();
                int upNumber = 1 + rise;

                if (total < upNumber)
                {
                    if (currentLevel > 0)
                    { //尝试使用碎片
                        upNumber *= 5; //5个碎片当1个整体
                    }
                    else
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "对应的遗物数量不足", ToastType = ToastTypeEnum.Failure });
                        return;
                    }

                    total = user.GetMaterialCount(ItemHelper.SpecialId_Halidom_Chip);
                    if (total < upNumber)
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "对应的遗物数量不足，且遗物粉尘数量不足", ToastType = ToastTypeEnum.Failure });
                        return;
                    }
                    else
                    {
                        //使用粉尘升级
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "消耗" + upNumber + "个遗物粉尘升级成功", ToastType = ToastTypeEnum.Success });
                        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                        {
                            Type = ItemType.Material,
                            ItemId = ItemHelper.SpecialId_Halidom_Chip,
                            Quantity = upNumber
                        });
                    }
                }
                else
                {
                    //使用遗物升级
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "消耗" + upNumber + "个" + Config.Name + "升级成功", ToastType = ToastTypeEnum.Success });
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = ItemType.Halidom,
                        ItemId = Config.ItemId,
                        Quantity = upNumber
                    });
                }

                halidomData.Data++;
                this.SetContent(this.Config, halidomData.Data);
                GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

                GameProcessor.Inst.SaveData();
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        public void SetContent(HalidomConfig config, long level)
        {
            this.Config = config;

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(6), config.Name);

            if (level > 0)
            {
                long val = config.AttrValue + (level - 1) * config.RiseAttr;
                this.Txt_Level.text = ConfigHelper.LayerNameList[level - 1] + "阶";
                this.Txt_Attr_Current.text = StringHelper.FormatAttrText(config.AttrId, val);
                this.Txt_Attr_Rise.text = "升阶增加:" + StringHelper.FormatAttrValueText(config.AttrId, config.RiseAttr);
            }
            else
            {

                this.Txt_Level.text = "未激活";
                this.Txt_Attr_Current.text = StringHelper.FormatAttrValueName(config.AttrId);
                this.Txt_Attr_Rise.text = "激活增加:" + StringHelper.FormatAttrValueText(config.AttrId, config.AttrValue);
            }
        }
    }
}
