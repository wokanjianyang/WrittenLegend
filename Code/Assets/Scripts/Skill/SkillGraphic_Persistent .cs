using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Persistent : SkillGraphic
    {
        public SkillGraphic_Persistent(APlayer player,SkillPanel skill) : base(player,skill)
        {
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
            //yield return new WaitForSeconds(0.5f);
            var duration = Math.Max(this.SkillPanel.Duration, 0.5f);
            bool loop = duration > 1;

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, loop);
            if (effectCom != null)
            {
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = targetPos;

                yield return new WaitForSeconds(duration); //因为现在1s才是一个回合
                //effectCom.gameObject.SetActive(false);
                GameObject.Destroy(effectCom.gameObject);

            }
            //yield return null;
        }
    }
}
