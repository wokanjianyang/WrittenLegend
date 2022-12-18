using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_EquipDetail : MonoBehaviour, IBattleLife
    {
        [LabelText("容器")]
        public RectTransform rect_Content;

        [Title("道具数据")]
        [LabelText("背景")]
        public Image img_Background;

        [LabelText("背景图片")]
        public Sprite[] list_BackgroundImgs;

        [LabelText("名称")]
        public TextMeshProUGUI tmp_Title;

        [LabelText("基础属性")]
        public Transform tran_BaseAttribute;

        [LabelText("隐藏属性")]
        public Transform tran_HideAttribute;

        [LabelText("品质属性")]
        public Transform tran_QualityAttribute;

        [LabelText("技能属性")]
        public Transform tran_SkillAttribute;

        [LabelText("套装属性")]
        public Transform tran_SuitAttribute;

        [Title("导航")]
        [LabelText("穿戴")]
        public Button btn_Equip;

        [LabelText("卸下")]
        public Button btn_UnEquip;


        private Item item;
        private int boxId;

        private RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            this.rectTransform = this.transform.GetComponent<RectTransform>();
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowEquipDetailEvent>(this.OnShowEquipDetailEvent);
        }

        private void OnShowEquipDetailEvent(ShowEquipDetailEvent e)
        {
            this.transform.position = this.GetBetterPosition(e.Position);
            this.transform.localScale = Vector3.one;
            this.tmp_Title.text = e.Item.Name;
            this.item = e.Item;
            this.boxId = e.BoxId;

            this.btn_Equip.gameObject.SetActive(this.boxId != -1);
            this.btn_UnEquip.gameObject.SetActive(this.boxId == -1);
            var equip = e.Item;
            var titleColor = "FFFFFF";
            switch (equip.Quality)
            {
                case 1:
                    titleColor = "CBFFC2";
                    break;
                case 2:
                    titleColor = "CCCCCC";
                    break;
                case 3:
                    titleColor = "76B0FF";
                    break;
                case 4:
                    titleColor = "D800FF";
                    break;
            }
            this.img_Background.sprite = this.list_BackgroundImgs[equip.Quality - 1];
            tmp_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, equip.Name);

            int index = 0;
            if(equip.AttrList!=null)
            {
                foreach (var a in equip.AttrList)
                {
                    var child = tran_BaseAttribute.Find(string.Format("Attribute_{0}", index));
                    child.GetComponent<TextMeshProUGUI>().text = string.Format(" •+{0}点{1}", a.Value, PlayerHelper.PlayerAttributeMap[((AttributeEnum)a.Key).ToString()]);
                    child.gameObject.SetActive(true);
                    index++;
                }
            }
            string color = "green";
            if (equip.Level > UserData.Load().Level)
            {
                color = "red";
            }
            tran_BaseAttribute.Find("NeedLevel").GetComponent<TextMeshProUGUI>().text = string.Format("<color={0}>需要等级{1}</color>", color, equip.Level);

            var size = this.rect_Content.sizeDelta;
            size.x = 289;
            this.rectTransform.sizeDelta = size;
        }

        private void OnEquip()
        {
            this.transform.localScale = Vector3.zero;

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                Item = this.item,
                BoxId = this.boxId
            });
        }

        private void OnUnEquip()
        {
            this.transform.localScale = Vector3.zero;

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                IsWear = false,
                Item = this.item,
                BoxId = this.boxId
            });
        }

        private Vector3 GetBetterPosition(Vector3 position)
        {

            var maxRight = position.x + this.rectTransform.sizeDelta.x;
            var maxDown = position.y - this.rectTransform.sizeDelta.y;
            Vector3 offset = Vector3.zero;
            if (maxDown < 0)
            {
                offset.y = 0 - maxDown;
            }
            if (maxRight > 1017)
            {
                offset.x = 1017 - maxRight;
            }

            Vector3 betterPosition = position + offset;


            return betterPosition;
        }
    }
}
