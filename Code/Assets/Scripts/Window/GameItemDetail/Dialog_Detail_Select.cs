using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Detail_Select : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;
        public Button Btn_OK;
        public Button Btn_Query;
        public Button Btn_OK_All;

        public RectTransform Container;
        public ToggleGroup toggleGroup;

        private BoxItem boxItem;
        private int ConfigId;

        private List<Gift_Item> ItemList = new List<Gift_Item>();

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            Btn_OK.onClick.AddListener(OnClick_OK);
            Btn_OK_All.onClick.AddListener(OnClick_OK_All);
            Btn_Query.onClick.AddListener(OnClick_Query);
        }


        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowSelectEvent>(this.OnShow);
        }

        private void Init()
        {
            //clear
            foreach (var si in ItemList)
            {
                GameObject.Destroy(si.gameObject);
            }
            ItemList.Clear();

            GiftPackConfig config = GiftPackConfigCategory.Instance.Get(this.ConfigId);
            var pref = Resources.Load<GameObject>("Prefab/Window/GameItem/Gift_Item");

            for (int i = 0; i < config.ItemIdList.Length; i++)
            {
                var itemUI = GameObject.Instantiate(pref, Container);
                itemUI.transform.localScale = Vector3.one;

                Gift_Item item = itemUI.GetComponent<Gift_Item>();

                Item newItem = ItemHelper.BuildItem((ItemType)config.ItemTypeList[i], config.ItemIdList[i], 1, config.ItemCountList[i]);

                BoxItem boxItem = new BoxItem();
                boxItem.Item = newItem;
                boxItem.MagicNubmer.Data = 1;
                boxItem.BoxId = -1;

                item.SetItem(boxItem, toggleGroup);

                ItemList.Add(item);
            }

            Debug.Log(config.Id + " " + config.Name + "" + config.OpenType);
            if (config.OpenType == 1)
            {
                this.Btn_OK_All.gameObject.SetActive(true);
            }
            else
            {
                this.Btn_OK_All.gameObject.SetActive(false);
            }
        }

        public void OnShow(ShowSelectEvent e)
        {
            this.boxItem = e.boxItem;
            this.ConfigId = this.boxItem.Item.ConfigId;
            this.Init();
            this.gameObject.SetActive(true);
        }


        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        public void OnClick_OK()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();

            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //判断空格
            int ic = GameProcessor.Inst.User.GetBagIdleCount(select.BoxItem.GetBagType());
            if (ic < 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
                return;
            }


            this.gameObject.SetActive(false);

            //选择第N个装备
            GameProcessor.Inst.EventCenter.Raise(new SelectGiftEvent()
            {
                BoxItem = boxItem,
                Item = select.BoxItem.Item,
                Nubmer = 1
            });
        }


        public void OnClick_OK_All()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();

            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //判断空格
            int ic = GameProcessor.Inst.User.GetBagIdleCount(select.BoxItem.GetBagType());
            if (ic < 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.SetActive(false);

            //选择第N个装备
            GameProcessor.Inst.EventCenter.Raise(new SelectGiftEvent()
            {
                BoxItem = boxItem,
                Item = select.BoxItem.Item,
                Nubmer = boxItem.MagicNubmer.Data
            });
        }

        public void OnClick_Query()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();
            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (select.BoxItem.Item.Type == ItemType.Exclusive)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveCardEvent()
                {
                    boxItem = select.BoxItem,
                    EquipPosition = -2,
                    Type = ComBoxType.Gift,
                });
                return;
            }
            else if (select.BoxItem.Item.Type == ItemType.Equip)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
                {
                    boxItem = select.BoxItem,
                    EquipPosition = -2,
                    Type = ComBoxType.Gift,
                });
            }
            else if (select.BoxItem.Item.Type == ItemType.Shengxiao)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowShengxiaoDetailEvent()
                {
                    boxItem = select.BoxItem,
                    EquipPosition = -2,
                    Type = ComBoxType.Gift,
                });
            }
            else if (select.BoxItem.Item.Type == ItemType.Pet)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowPetDetailEvent()
                {
                    boxItem = select.BoxItem,
                });
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
                {
                    boxItem = select.BoxItem,
                    Type = ComBoxType.Gift,
                });
            }
        }
    }
}
