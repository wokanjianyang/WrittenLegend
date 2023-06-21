using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Normal : SkillGraphic
    {
        private IEnumerator lastIE = null;
        private bool isIEEnd = false;

        const float speed = 0.3f;

        public SkillGraphic_Normal(APlayer player, SkillConfig skillConfig) : base(player, skillConfig)
        {
        }

        public override void PlayAnimation(List<Vector3Int> cells, Vector3Int scale)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells[0]));
        }

        IEnumerator IE_Attack(Vector3Int cell)
        {
            yield return new WaitForSeconds(speed);
            var distance = cell - this.SelfPlayer.Cell;
            Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f) * GameProcessor.Inst.MapData.CellSize.x;
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(this.SelfPlayer.Cell);
            this.SelfPlayer.Transform.DOLocalMove(targetPos + offset, speed);

            yield return new WaitForSeconds(speed);
            this.SelfPlayer.Transform.DOLocalMove(targetPos, speed);
            this.isIEEnd = true;
        }
    }
}
