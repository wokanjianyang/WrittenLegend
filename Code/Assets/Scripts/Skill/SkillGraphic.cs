using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic
    {
        public APlayer SelfPlayer { get; set; }

        private IEnumerator lastIE = null;
        private bool isIEEnd = false;

        public SkillGraphic(APlayer player)
        {
            this.SelfPlayer = player;
        }

        public void PlayAnimation(int tid)
        {
            if(lastIE!=null && !this.isIEEnd)
            {
                GameProcessor.Inst.StopCoroutine(lastIE);
            }
            this.isIEEnd = false;
            GameProcessor.Inst.StartCoroutine(IE_Attack(tid));
        }

        IEnumerator IE_Attack(int tid)
        {
            var mainTarget = GameProcessor.Inst.PlayerManager.GetPlayer(tid);
            var distance = mainTarget.Cell - this.SelfPlayer.Cell;
            Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f) * GameProcessor.Inst.MapProcessor.CellSize.x;
            var targetPos = GameProcessor.Inst.MapProcessor.GetWorldPosition(this.SelfPlayer.Cell);
            this.SelfPlayer.Transform.DOLocalMove(targetPos + offset, 0.5f);
            yield return new WaitForSeconds(0.5f);
            this.SelfPlayer.Transform.DOLocalMove(targetPos, 0.5f);
            this.isIEEnd = true;
        }
    }
}
