using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewEndlessTower : AViewPage
    {
        [Title("无尽塔")]
        [LabelText("下下层")]
        public Text tmp_Floor_2;

        [LabelText("下层")]
        public Text tmp_Floor_1;

        [LabelText("当前层")]
        public Text tmp_Floor_0;

        [LabelText("当前层")]
        public Text tmp_CurrentFloor;

        [LabelText("经验加成")]
        public Text tmp_ExpAdd;

        [LabelText("ͨ奖励")]
        public Text tmp_Reward;

        [LabelText("暴击抵抗�")]
        public Text tmp_Cri;

        [LabelText("开始")]
        public Button btn_Start;

        void Start()
        {
            //this.btn_Start.onClick.AddListener(this.OnClick_Start);
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
            //GameProcessor.Inst.EventCenter.Raise(new ShowTowerWindowEvent());
            //GameProcessor.Inst.DelayAction(0.1f, () =>
            //{
            //    GameProcessor.Inst.OnDestroy();
            //    var map = GameObject.Find("Canvas").GetComponentInChildren<WindowEndlessTower>().transform;
            //    GameProcessor.Inst.LoadMap(RuleType.Tower, 0, 0, map);
            //});
        }

        private void UpdateFloorInfo()
        {
            User user = GameProcessor.Inst.User;

            var maxFloor = TowerConfigCategory.Instance.GetAll().Count;
            long minFloor = 0;
            if (user.MagicTowerFloor.Data == maxFloor)
            {
                minFloor = user.MagicTowerFloor.Data - 2;
            }
            else if (user.MagicTowerFloor.Data == maxFloor - 1)
            {
                minFloor = user.MagicTowerFloor.Data - 1;
            }
            else
            {
                minFloor = user.MagicTowerFloor.Data;
            }

            this.tmp_Floor_0.text = $"{(minFloor)}";
            this.tmp_Floor_1.text = $"{(minFloor + 1)}";
            this.tmp_Floor_2.text = $"{(minFloor + 2)}";

            var config = TowerConfigCategory.Instance.GetByFloor(user.MagicTowerFloor.Data);
            this.tmp_CurrentFloor.text = $"{(user.MagicTowerFloor.Data)}";
            this.tmp_ExpAdd.text = $"{config.RiseExp}";
            this.tmp_Reward.text = "暂无";
            this.tmp_Cri.text = "暂无";
        }

        private void OnUpdateTowerWindowEvent(UpdateTowerWindowEvent msg)
        {
            this.UpdateFloorInfo();
        }
    }
}
