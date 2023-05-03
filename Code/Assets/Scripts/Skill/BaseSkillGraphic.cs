using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BaseSkillGraphic : SkillGraphic
    {
        private IEnumerator lastIE = null;
        private bool isIEEnd = false;

        public BaseSkillGraphic(APlayer player, SkillConfig skillConfig) : base(player, skillConfig)
        {
        }

        public override void PlayAnimation(Vector3Int cell)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cell));
        }

        IEnumerator IE_Attack(Vector3Int cell)
        {
            yield return new WaitForSeconds(0.5f);
            var distance = cell - this.SelfPlayer.Cell;
            Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f) * GameProcessor.Inst.MapData.CellSize.x;
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(this.SelfPlayer.Cell);
            this.SelfPlayer.Transform.DOLocalMove(targetPos + offset, 0.5f);

            yield return new WaitForSeconds(0.5f);
            this.SelfPlayer.Transform.DOLocalMove(targetPos, 0.5f);
            this.isIEEnd = true;
        }
    }
}
