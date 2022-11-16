using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BaseAttackSkill : ASkill
    {
        public override void PlayAnimation(int tid)
        {
            base.PlayAnimation(tid);
            
            GameProcessor.Inst.StartCoroutine(IE_Attack(tid));
        }
        
        IEnumerator IE_Attack(int tid)
        {
            var mainTarget = GameProcessor.Inst.PlayerManager.GetPlayer(tid);
            var distance = mainTarget.Cell - this.SelfPlayer.Cell;
            Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f)*120;
            var targetPos = GameProcessor.Inst.MapProcessor.GetWorldPosition(this.SelfPlayer.Cell);
            this.SelfPlayer.Transform.DOLocalMove(targetPos+offset, 0.5f);
            yield return new WaitForSeconds(0.5f);
            this.SelfPlayer.Transform.DOLocalMove(targetPos, 0.5f);

            var allEnemy = this.GetAllTargets(tid);
            foreach (var attackData in allEnemy)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);
                var damage = (int)this.CalcFormula(enemy, attackData.Ratio);
                enemy.OnHit(damage);
            }
        }

        public override float CalcFormula(APlayer player, float ratio)
        {
            return this.SelfPlayer.Logic.GetAttributeFloat(AttributeEnum.PhyAtt) * ratio;
        }

    }
}
