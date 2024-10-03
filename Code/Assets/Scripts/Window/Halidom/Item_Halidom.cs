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

        private int Count_Normal = 10;
        private int Count_Tupo = 20;

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
            int rise = (int)currentLevel * 1;
            int upNumber = 1 + rise;

            if (currentLevel < maxLevel)
            {
                long total = user.Bags.Where(m => m.Item.Type == ItemType.Halidom && m.Item.ConfigId == Config.ItemId).Select(m => m.MagicNubmer.Data).Sum();

                if (total < upNumber)
                {
                    if (currentLevel <= 0)
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "激活所需遗物数量不足", ToastType = ToastTypeEnum.Failure });
                        return;
                    }

                    //使用粉尘升级
                    upNumber *= Count_Normal; //粉尘消耗*10
                    total = user.GetMaterialCount(ItemHelper.SpecialId_Halidom_Chip);

                    if (total < upNumber)
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "升级所需遗物，粉尘数量不足", ToastType = ToastTypeEnum.Failure });
                        return;
                    }
                    else
                    {
                        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认用遗物粉尘升级遗物", true,
                        () =>
                        {
                            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "消耗" + upNumber + "个遗物粉尘升级成功", ToastType = ToastTypeEnum.Success });
                            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                            {
                                Type = ItemType.Material,
                                ItemId = ItemHelper.SpecialId_Halidom_Chip,
                                Quantity = upNumber
                            });

                            halidomData.Data++;
                            this.SetContent(this.Config, halidomData.Data);
                            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
                        }, () =>
                        {
                        });



                        //GameProcessor.Inst.SaveData();
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

                    halidomData.Data++;
                    this.SetContent(this.Config, halidomData.Data);
                    GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

                    GameProcessor.Inst.SaveData();
                }
            }
            else
            {
                int tupoLevel = maxLevel - 4;
                if (currentLevel < maxLevel + tupoLevel)
                {
                    //突破，使用粉尘
                    upNumber *= 20;

                    long total = user.GetMaterialCount(ItemHelper.SpecialId_Halidom_Chip);
                    if (total < upNumber)
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "突破所需遗物粉尘数量不足", ToastType = ToastTypeEnum.Failure });
                        return;
                    }

                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "消耗" + upNumber + "个遗物粉尘突破成功", ToastType = ToastTypeEnum.Success });
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = ItemType.Material,
                        ItemId = ItemHelper.SpecialId_Halidom_Chip,
                        Quantity = upNumber
                    });

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
        }

        public void SetContent(HalidomConfig config, long level)
        {
            this.Config = config;

            string color = level > 8 ? QualityConfigHelper.GetQualityColor(7) : QualityConfigHelper.GetQualityColor(6);

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, config.Name);

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
