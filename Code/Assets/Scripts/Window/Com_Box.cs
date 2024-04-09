using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Com_Box : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        [Title("物品格")]
        [LabelText("道具名")]
        public Text tmp_Title;

        [LabelText("数量")]
        public Text tmp_Count;

        public GameObject go_Lock;

        public Text Tag;
        public Text Layer;

        public BoxItem BoxItem { get; private set; }
        public int boxId { get; private set; }

        public int BagType { get; private set; }

        public int EquipPosition { get; private set; }
        public long Count { get; private set; }

        public ComBoxType Type { get; set; } = ComBoxType.Bag;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            this.ShowName();
        }

        private void ShowName() {
            if (this.BoxItem != null)
            {
                if (BoxItem.Item.Type == ItemType.Exclusive)
                {
                    ExclusiveItem exclusive = BoxItem.Item as ExclusiveItem;
                    if (exclusive.GetLevel() > 1)
                    {
                        this.Layer.text = ConfigHelper.LayerChinaList[(exclusive.GetLevel() - 1)] + "阶"; ;
                        this.Layer.gameObject.SetActive(true);
                    }
                }
                else if (BoxItem.Item.Type == ItemType.Equip)
                {
                    Equip equip = BoxItem.Item as Equip;
                    if (equip.GetQuality() > 5)
                    {
                        this.Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Layer.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.BoxItem == null) return;

            this.BoxItem.Item.IsNew = false;
            this.Tag.gameObject.SetActive(false);

            if (this.BoxItem.Item.Type == ItemType.GiftPack)
            {
                GiftPack giftPack = this.BoxItem.Item as GiftPack;

                if (giftPack.Config.GiftType == 1)  //自选包;
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowSelectEvent() { boxItem = this.BoxItem });
                    return;
                }
            }
            else if (this.BoxItem.Item.Type == ItemType.Exclusive)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveCardEvent()
                {
                    boxItem = this.BoxItem,
                    EquipPosition = this.EquipPosition,
                    Type = this.Type
                });
                return;
            }
            else if (this.BoxItem.Item.Type == ItemType.Equip)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
                {
                    boxItem = this.BoxItem,
                    EquipPosition = this.EquipPosition,
                    Type = this.Type
                });
                return;
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
            {
                boxItem = this.BoxItem,
                Type = this.Type
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
        public void SetItem(BoxItem item)
        {
            this.tmp_Title.text = item.Item.Name;

            this.BoxItem = item;

            this.Count = item.MagicNubmer.Data;
            this.BagType = item.GetBagType();

            this.go_Lock.gameObject.SetActive(item.Item.IsLock);

            this.tmp_Count.transform.gameObject.SetActive(this.Count > 1);
            this.tmp_Count.text = this.Count.ToString();

            if (item.Item.IsNew && (item.Item.Type == ItemType.Equip || item.Item.Type == ItemType.Exclusive))
            {
                if (item.Item.Type == ItemType.Equip)
                {
                    Equip equip = item.Item as Equip;
                    if (equip.Part > 10)
                    {
                        item.Item.IsNew = false;
                        return;
                    }
                }

                this.Tag.gameObject.SetActive(true);
                this.Tag.text = $"<color=#{QualityConfigHelper.GetEquipTagColor(item.Item.IsKeep)}>New</color>";
            }

            this.ShowName();
        }

        public void SetBoxId(int id)
        {
            this.boxId = id;
        }

        public void SetEquipPosition(int position)
        {
            this.EquipPosition = position;
        }

        public void AddStack(long quantity)
        {
            this.Count += quantity;
            this.tmp_Count.transform.gameObject.SetActive(this.Count != 1);
            this.tmp_Count.text = this.Count.ToString();
        }

        public void RemoveStack(long quantity)
        {
            this.Count -= quantity;
            this.tmp_Count.transform.gameObject.SetActive(this.Count != 1);
            this.tmp_Count.text = this.Count.ToString();
        }

        public void SetLock(bool isLock)
        {
            this.go_Lock.gameObject.SetActive(isLock);
        }

        public void SetType(ComBoxType type)
        {
            this.Type = type;
        }
    }

    public enum ComBoxType
    {
        Bag = 0,
        Exclusive_Devour = 1,
    }
}