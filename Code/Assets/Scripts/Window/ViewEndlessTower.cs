using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewEndlessTower : AViewPage
    {
        [Title("�޾���")]
        [LabelText("���²�")]
        public Text tmp_Floor_2;

        [LabelText("�²�")]
        public Text tmp_Floor_1;

        [LabelText("��ǰ��")]
        public Text tmp_Floor_0;

        [LabelText("��ǰ��")]
        public Text tmp_CurrentFloor;

        [LabelText("����ӳ�")]
        public Text tmp_ExpAdd;

        [LabelText("ͨ�ؽ���")]
        public Text tmp_Reward;

        [LabelText("�����ֿ�")]
        public Text tmp_Cri;

        [LabelText("��ʼ")]
        public Button btn_Start;

        void Start()
        {
            this.btn_Start.onClick.AddListener(this.OnClick_Start);
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<UpdateTowerWindowEvent>(this.OnUpdateTowerWindowEvent);
            this.UpdateFloorInfo();

        }
        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Tower;
        }

        private void OnClick_Start()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowTowerWindowEvent());
            GameProcessor.Inst.DelayAction(0.1f, ()=> { 
                GameProcessor.Inst.OnDestroy();
                var map = GameObject.Find("Canvas").GetComponentInChildren<WindowEndlessTower>().transform;
                GameProcessor.Inst.LoadMap(RuleType.Tower, 0, map);
            });
        }

        private void UpdateFloorInfo()
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            var maxFloor = TowerConfigCategory.Instance.GetAll().Count;
            var minFloor = 0;
            if (hero.TowerFloor == maxFloor)
            {
                minFloor = hero.TowerFloor - 2;
            }
            else if (hero.TowerFloor == maxFloor - 1)
            {
                minFloor = hero.TowerFloor - 1;
            }
            else
            {
                minFloor = hero.TowerFloor;
            }

            this.tmp_Floor_0.text = $"{(minFloor)}";
            this.tmp_Floor_1.text = $"{(minFloor + 1)}";
            this.tmp_Floor_2.text = $"{(minFloor + 2)}";

            var config = TowerConfigCategory.Instance.Get(hero.TowerFloor - 1);
            this.tmp_CurrentFloor.text = $"{(hero.TowerFloor)}";
            this.tmp_ExpAdd.text = $"{config.OfflineExp}";
            this.tmp_Reward.text = "����";
            this.tmp_Cri.text = "����";
        }

        private void OnUpdateTowerWindowEvent(UpdateTowerWindowEvent msg)
        {
            this.UpdateFloorInfo();
        }
    }
}
