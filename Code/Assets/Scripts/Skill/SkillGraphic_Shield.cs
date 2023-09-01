using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Shield : SkillGraphic
    {
        private SkillPanel SkillPanel { get; set; }

        public SkillGraphic_Shield(APlayer player, SkillPanel skillPanel) : base(player, skillPanel.SkillData.SkillConfig)
        {
            this.SkillPanel = skillPanel;
        }

        public override void PlayAnimation(List<Vector3Int> cells, Vector3Int scale)
        {
            foreach (Vector3Int cell in cells)
            {
                GameProcessor.Inst.StartCoroutine(IE_Attack(cell));
            }
        }

        private IEnumerator IE_Attack(Vector3Int cell)
        {
            var effectCom = EffectLoader.CreateEffectShield(this.SkillPanel.SkillData.SkillConfig.Name, this.SkillPanel.Duration);//

            if (effectCom != null)
            {
                effectCom.transform.SetParent(this.SelfPlayer.Transform);
                effectCom.transform.localPosition = new Vector3(0,0,0);


                yield return new WaitForSeconds(this.SkillPanel.Duration); //因为现在1s才是一个回合
                GameObject.Destroy(effectCom.gameObject);

            }
            yield return null;
        }
    }
}
