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

        [Title("装备数据")]
        [LabelText("装备名称")]
        public TextMeshProUGUI tmp_Title;

        [LabelText("穿戴")]
        public Button btn_Equip;

        [LabelText("卸下")]
        public Button btn_UnEquip;

        private RectTransform rectTransform;

        private Item item;
        private int boxId;

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
            this.transform.position = e.Position;
            this.transform.localScale = Vector3.one;
            this.tmp_Title.text = e.Item.Name;
            this.item = e.Item;
            this.boxId = e.BoxId;

            this.btn_Equip.gameObject.SetActive(this.boxId!=-1);
            this.btn_UnEquip.gameObject.SetActive(this.boxId==-1);
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
