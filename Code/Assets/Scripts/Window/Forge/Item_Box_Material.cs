using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Box_Material : MonoBehaviour
    {
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Toggle toggle;

        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public delegate void BoxMaterialSelectEvent(Item_Box_Material item);
        private BoxMaterialSelectEvent selectEvent;

        public BoxItem Box_Item { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                OnSelect(isOn);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            this.ShowName();
        }

        public void Init(BoxItem item, ToggleGroup toggleGroup)
        {
            this.toggle.group = toggleGroup;

            this.Box_Item = item;

            this.Txt_Name.text = Box_Item.Item.Name;

            int quality = Box_Item.Item.GetQuality();
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

            if (this.Box_Item != null)
            {
                Item GameItem = Box_Item.Item;
                if (GameItem.Type == ItemType.Exclusive)
                {
                    ExclusiveItem exclusive = GameItem as ExclusiveItem;
                    if (exclusive.GetLayer() > 1)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[(exclusive.GetLayer() - 1)] + "阶"; ;
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                    if (exclusive.GetLevel() >= 1)
                    {
                        this.Txt_Level.text = exclusive.GetLevel() + "级";
                        this.Txt_Level.gameObject.SetActive(true);
                    }
                }
                else if (GameItem.Type == ItemType.Equip)
                {
                    Equip equip = GameItem as Equip;
                    if (equip.GetQuality() > 5 && equip.Part <= 10)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
                else if (GameItem.Type == ItemType.Shengxiao)
                {
                    Shengxiao shengxiao = GameItem as Shengxiao;
                    if (shengxiao.LayerData.Data > 0)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[(shengxiao.LayerData.Data)] + "阶"; ;
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                    if (shengxiao.LevelData.Data > 0)
                    {
                        this.Txt_Level.text = shengxiao.LevelData.Data + "级";
                        this.Txt_Level.gameObject.SetActive(true);
                    }

                }
            }
        }

        public void Refresh()
        {
            this.ShowName();
        }

        private void OnSelect(bool isOn)
        {
            if (isOn)
            {
                selectEvent?.Invoke(this);
            }
        }

        public void AddListener(BoxMaterialSelectEvent e)
        {
            this.selectEvent = e;
        }

    }
}