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

        public RectTransform Container;
        public ToggleGroup toggleGroup;

        private BoxItem boxItem;
        private int ConfigId;

        private List<SelectItem> ItemList = new List<SelectItem>();

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            Btn_OK.onClick.AddListener(OnClick_OK);
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
            var pref = Resources.Load<GameObject>("Prefab/Window/Item/Select_Item");

            for (int i = 0; i < config.ItemIdList.Length; i++)
            {
                var itemUI = GameObject.Instantiate(pref, Container);

                SelectItem item = itemUI.GetComponent<SelectItem>();

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
            SelectItem select = ItemList.Where(m => m.Tg_Bg.isOn).FirstOrDefault();

            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //选择第N个装备
            GameProcessor.Inst.EventCenter.Raise(new SelectGiftEvent()
            {
                BoxItem = boxItem,
                Item = select.BoxItem.Item
            });

            this.gameObject.SetActive(false);
        }
    }
}
