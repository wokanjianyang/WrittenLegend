using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Box_Drop : MonoBehaviour
    {
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Item GameItem { get; private set; }


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

        public void SetItem(Item item)
        {
            this.GameItem = item;

            this.Txt_Name.text = item.Name;

            int quality = item.GetQuality();
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

            if (this.GameItem != null)
            {
                if (GameItem.Type == ItemType.Exclusive)
                {
                    ExclusiveItem exclusive = GameItem as ExclusiveItem;
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
                else if (GameItem.Type == ItemType.Equip)
                {
                    Equip equip = GameItem as Equip;
                    if (equip.GetQuality() > 5 && equip.Part <= 10)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}