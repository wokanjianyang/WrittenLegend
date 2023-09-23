using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Normal : SkillGraphic
    {
        const float speed = 0.2f;

        public SkillGraphic_Normal(APlayer player, SkillPanel skill) : base(player, skill)
        {
        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells[0]));
        }

        IEnumerator IE_Attack(Vector3Int cell)
        {
            var distance = cell - this.SelfPlayer.Cell;
            Vector3 offset = new Vector3(distance.x * 0.2f, distance.y * 0.2f) * GameProcessor.Inst.MapData.CellSize.x;
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(this.SelfPlayer.Cell);
            this.SelfPlayer.Transform.DOLocalMove(targetPos + offset, speed);

            yield return new WaitForSeconds(speed);
            this.SelfPlayer.Transform.DOLocalMove(targetPos, speed);
            yield return null;
        }
    }
}
