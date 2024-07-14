using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Forge_Main : MonoBehaviour
    {
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Toggle toggle;

        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public delegate void ForgeMainSelectEvent(Item_Forge_Main item);
        private ForgeMainSelectEvent selectEvent;

        public Item GameItem { get; private set; }

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

        public void Init(Item item, ToggleGroup toggleGroup)
        {
            this.toggle.group = toggleGroup;

            this.GameItem = item;

            this.Txt_Name.text = GameItem.Name;

            int quality = GameItem.GetQuality();
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

        private void OnSelect(bool isOn)
        {
            if (isOn)
            {
                selectEvent?.Invoke(this);
            }
        }

        public void AddListener(ForgeMainSelectEvent e)
        {
            this.selectEvent = e;
        }

    }
}