using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BottomNav : MonoBehaviour,IBattleLife
    {
        [Title("底部导航")]
        [LabelText("包裹")]
        public Button btn_Bag;

        [LabelText("战斗")]
        public Button btn_Battle;

        [LabelText("世界地图")]
        public Button btn_Map;

        [LabelText("无尽塔")]
        public Button btn_Tower;

        [LabelText("技能")]
        public Button btn_Skill;

        [LabelText("锻造")]
        public Button btn_Forge;
        
        // Start is called before the first frame update
        void Start()
        {
            this.btn_Bag.onClick.AddListener(this.OnClick_Bag);
            this.btn_Battle.onClick.AddListener(this.OnClick_Battle);
            this.btn_Map.onClick.AddListener(this.OnClick_Map);
            this.btn_Tower.onClick.AddListener(this.OnClick_Tower);
            this.btn_Skill.onClick.AddListener(this.OnClick_Skill);
            this.btn_Forge.onClick.AddListener(this.OnClick_Forge);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.gameObject.SetActive(true);
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
            this.ChangePage(ViewPageType.View_More);
        }
        private void OnClick_Skill()
        {
            this.ChangePage(ViewPageType.View_Skill);
        }
        
        private void OnClick_Forge()
        {
            this.ChangePage(ViewPageType.View_Forge);
        }

        private void ChangePage(ViewPageType page)
        {
            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { 
                Page = page
            });
        }
    }
}
