using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BottomNav : MonoBehaviour
    {
        [Title("�ײ�����")]
        [LabelText("����")]
        public Button btn_Bag;

        [LabelText("ս��")]
        public Button btn_Battle;

        [LabelText("�����ͼ")]
        public Button btn_Map;

        [LabelText("�޾���")]
        public Button btn_Tower;

        [LabelText("����")]
        public Button btn_Skill;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Bag.onClick.AddListener(this.OnClick_Bag);
            this.btn_Battle.onClick.AddListener(this.OnClick_Battle);
            this.btn_Map.onClick.AddListener(this.OnClick_Map);
            this.btn_Tower.onClick.AddListener(this.OnClick_Tower);
            this.btn_Skill.onClick.AddListener(this.OnClick_Skill);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnClick_Bag()
        {
            this.ChangePage(ViewPageType.View_Bag);
        }
        private void OnClick_Battle()
        {
            this.ChangePage(ViewPageType.View_Battle);
        }
        private void OnClick_Map()
        {
            this.ChangePage(ViewPageType.View_Map);
        }
        private void OnClick_Tower()
        {
            this.ChangePage(ViewPageType.View_Tower);
        }
        private void OnClick_Skill()
        {
            this.ChangePage(ViewPageType.View_Skill);
        }

        private void ChangePage(ViewPageType page)
        {
            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { 
                Page = page
            });
        }
    }
}