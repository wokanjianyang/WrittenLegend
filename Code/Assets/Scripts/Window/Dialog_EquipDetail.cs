using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_EquipDetail : MonoBehaviour,IBattleLife
    {

        [Title("道具数据")]
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

        // Start is called before the first frame update
        void Start()
        {
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
            this.transform.position = e.Position;
            this.transform.localScale = Vector3.one;
            this.tmp_Title.text = e.Item.Name;
            this.item = e.Item;
            this.boxId = e.BoxId;

            this.btn_Equip.gameObject.SetActive(this.boxId!=-1);
            this.btn_UnEquip.gameObject.SetActive(this.boxId==-1);
            var equip = e.Item as Equip;
            var titleColor = "FFFFFF";
            switch(equip.Quality)
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
            tmp_Title.text = string.Format("<color=#{0}>{1}</color>",titleColor,equip.Name);

            int index = 0;
            foreach (var a in equip.AttrList)
            {
                var child = tran_BaseAttribute.Find(string.Format("Attribute_{0}", index));
                child.GetComponent<TextMeshProUGUI>().text = string.Format(" •+{0}点{1}",a.Value, PlayerHelper.PlayerAttributeMap[((AttributeEnum)a.Key).ToString()]);
                child.gameObject.SetActive(true);
                index++;
            }
            string color = "green";
            if (equip.Level > UserData.Load().Level)
            {
                color = "red";
            }
            tran_BaseAttribute.Find("NeedLevel").GetComponent<TextMeshProUGUI>().text = string.Format("<color={0}>需要等级{1}</color>",color,equip.Level);
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
    }
}
