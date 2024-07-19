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
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "����ѡ��һ������", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //ѡ���N��װ��
            GameProcessor.Inst.EventCenter.Raise(new SelectGiftEvent()
            {
                BoxItem = boxItem,
                Item = select.BoxItem.Item
            });

            this.gameObject.SetActive(false);
        }

        public void OnClick_Query()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();
            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "����ѡ��һ������", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (select.BoxItem.Item.Type == ItemType.Exclusive)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveCardEvent()
                {
                    boxItem = select.BoxItem,
                    EquipPosition = -2,
                    Type = ComBoxType.Bag,
                });
                return;
            }
            else if (select.BoxItem.Item.Type == ItemType.Equip)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
                {
                    boxItem = select.BoxItem,
                    EquipPosition = -2
                });
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
                {
                    boxItem = select.BoxItem,
                });
            }
        }
    }
}
