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

        [LabelText("装备名称")]
        public Button btn_Equip;

        private RectTransform rectTransform;

        private int itemId;
        private int boxId;

        // Start is called before the first frame update
        void Start()
        {
            this.rectTransform = this.transform.GetComponent<RectTransform>();
            this.btn_Equip.onClick.AddListener(this.OnEquip);
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
            this.tmp_Title.text = EquipConfigCategory.Instance.Get(e.ItemId).Name;
            this.itemId = e.ItemId;
            this.boxId = e.BoxId;
        }

        private void OnEquip()
        {
            this.transform.localScale = Vector3.zero;

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                ItemId = this.itemId,
                BoxId = this.boxId
            });
        }
    }
}
