using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Box_Select : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;


        public BoxItem BoxItem { get; private set; }
        public int boxId { get; private set; }

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

        public void SetItem(BoxItem item, ComBoxType type)
        {
            this.BoxItem = item;
            this.Type = type;

            this.Txt_Name.text = item.Item.Name;

            int quality = item.Item.GetQuality();
            image_Background.sprite = list_Backgrounds[quality - 1];

            Color color = ColorHelper.HexToColor(QualityConfigHelper.GetQualityColor(quality));
            Txt_Name.color = color;
            Txt_Layer.color = color;
            Txt_Level.color = color;

            this.ShowName();
        }

        private void ShowName()
        {
            this.Txt_Layer.gameObject.SetActive(false);
            this.Txt_Level.gameObject.SetActive(false);

            if (this.BoxItem != null)
            {
                if (BoxItem.Item.Type == ItemType.Exclusive)
                {
                    ExclusiveItem exclusive = BoxItem.Item as ExclusiveItem;
                    if (exclusive.GetLayer() > 1)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[(exclusive.GetLayer() - 1)] + "阶"; ;
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                    if (exclusive.GetLevel() > 1)
                    {
                        this.Txt_Level.text = exclusive.GetLevel() + "级";
                        this.Txt_Level.gameObject.SetActive(true);
                    }
                }
                else if (BoxItem.Item.Type == ItemType.Equip)
                {
                    Equip equip = BoxItem.Item as Equip;
                    if (equip.GetQuality() > 5 && equip.Part <= 10)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.BoxItem == null) return;

            if (this.Type == ComBoxType.Exclusive_Up_Main)
            {
                GameProcessor.Inst.EventCenter.Raise(new BoxSelectEvent() { Box = this, Type = this.Type });
                return;
            }
            else if (this.Type == ComBoxType.Exclusive_Up_Material)
            {
                GameProcessor.Inst.EventCenter.Raise(new BoxSelectEvent() { Box = this, Type = this.Type });
                return;
            }
            else if (this.Type == ComBoxType.Exclusive_Devour_Main)
            {
                GameProcessor.Inst.EventCenter.Raise(new BoxSelectEvent() { Box = this, Type = this.Type });
                return;
            }
            else if (this.Type == ComBoxType.Exclusive_Devour_Material)
            {
                GameProcessor.Inst.EventCenter.Raise(new BoxSelectEvent() { Box = this, Type = this.Type });
                return;
            }
            else if (this.Type == ComBoxType.Box_Ready)
            {
                if (this.BoxItem.Item.Type == ItemType.Exclusive)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveCardEvent()
                    {
                        boxItem = this.BoxItem,
                        Type = this.Type
                    });
                    return;
                }
                else if (this.BoxItem.Item.Type == ItemType.Equip)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
                    {
                        boxItem = this.BoxItem,
                        Type = this.Type
                    });
                    return;
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent() { boxItem = this.BoxItem, Type = this.Type });
                    return;
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }


    }
}