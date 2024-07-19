using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Gift_Item : MonoBehaviour
    {
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Toggle toggle;

        public BoxItem BoxItem { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            //this.toggle.onValueChanged.AddListener((isOn) =>
            //{
            //    if (isOn)
            //    {
            //        ShowDetail();
            //    }
            //});
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(BoxItem box, ToggleGroup group)
        {
            this.BoxItem = box;
            this.toggle.group = group;


            this.Txt_Name.text = box.Item.Name;

            int quality = BoxItem.Item.GetQuality();
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
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[(exclusive.GetLayer() - 1)] + "½×"; ;
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                    if (exclusive.GetLevel() > 1)
                    {
                        this.Txt_Level.text = exclusive.GetLevel() + "¼¶";
                        this.Txt_Level.gameObject.SetActive(true);
                    }
                }
                else if (BoxItem.Item.Type == ItemType.Equip)
                {
                    Equip equip = BoxItem.Item as Equip;
                    if (equip.GetQuality() > 5 && equip.Part <= 10)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "½×";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (BoxItem.Item.Count > 1)
                    {
                        this.Txt_Level.text = BoxItem.Item.Count + "";
                        this.Txt_Level.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
