using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class BattleRule_Survivors : ABattleRule, IPointerClickHandler
    {
        private PlayerActionType actionType = PlayerActionType.None;
        private Vector3Int lastClickCell = default;
        private float defaultWaitInputTime = 5f;
        protected override RuleType ruleType => RuleType.Survivors;

        public override void DoMapLogic(int roundNum)
        {
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            this.lastClickCell = hero.Cell;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.actionType == PlayerActionType.WaitingInput)
            {
                var pressPos = eventData.position;
                this.lastClickCell = GameProcessor.Inst.MapData.GetLocalCell(pressPos);
                this.actionType = PlayerActionType.InputEnd;
            }
        }
    }
}
