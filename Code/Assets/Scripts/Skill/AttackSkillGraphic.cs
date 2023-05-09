using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AttackSkillGraphic : SkillGraphic
    {
        public AttackSkillGraphic(APlayer player,SkillConfig skillConfig) : base(player,skillConfig)
        {
        }

        public override void PlayAnimation(Vector3Int cell)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cell));
        }

        private IEnumerator IE_Attack(Vector3Int cell)
        {
            //yield return new WaitForSeconds(0.5f);
            var duration = Math.Max(this.SkillConfig.Duration, 1);
            bool loop = duration > 1;

            var effectCom = EffectLoader.CreateEffect(this.SkillConfig.ModelName,loop);
            if (effectCom != null)
            {
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = targetPos;

                yield return new WaitForSeconds(duration * 2); //因为现在2s才是一个回合
                //effectCom.gameObject.SetActive(false);
                GameObject.Destroy(effectCom.gameObject);

            }
            //yield return null;
        }
    }
}
