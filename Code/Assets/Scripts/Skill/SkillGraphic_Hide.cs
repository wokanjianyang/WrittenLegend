using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Hide : SkillGraphic
    {

        public SkillGraphic_Hide(APlayer player, SkillPanel skill) : base(player, skill)
        {

        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            foreach (Vector3Int cell in cells)
            {
                GameProcessor.Inst.StartCoroutine(IE_Attack(cell));
            }
        }

        private IEnumerator IE_Attack(Vector3Int cell)
        {
            this.SelfPlayer.EventCenter.Raise(new ShowHideEvent() { IsHide = true });
            yield return new WaitForSeconds(this.SkillPanel.Duration);

            this.SelfPlayer.IsHide = false;
            this.SelfPlayer.EventCenter.Raise(new ShowHideEvent() { IsHide = false });

            yield return null;
        }
    }
}
