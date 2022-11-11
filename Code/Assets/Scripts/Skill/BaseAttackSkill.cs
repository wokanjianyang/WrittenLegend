using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BaseAttackSkill : ASkill
    {
        public override void PlayAnimation(params int[] tids)
        {
            base.PlayAnimation();

            GameProcessor.Inst.StartCoroutine(IE_Attack(tids));
        }
        
        IEnumerator IE_Attack(params int[] tids)
        {
            var mainTarget = GameProcessor.Inst.PlayerManager.GetPlayer(tids[0]);
            var distance = mainTarget.Cell - this.SelfPlayer.Cell;
            Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f)*120;
            var targetPos = GameProcessor.Inst.MapProcessor.GetWorldPosition(this.SelfPlayer.Cell);
            this.SelfPlayer.Transform.DOLocalMove(targetPos+offset, 0.5f);
            yield return new WaitForSeconds(0.5f);
            this.SelfPlayer.Transform.DOLocalMove(targetPos, 0.5f);
            foreach (var tid in tids)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(tid);
                var damage = this.CalcFormula(enemy, 1f);
                enemy.OnHit(damage);
            }
        }

        public override float CalcFormula(APlayer player, float ratio)
        {
            return this.SelfPlayer.Logic.GetAttributeFloat(AttributeEnum.Atk) * 10 * ratio;
        }
    }
}
