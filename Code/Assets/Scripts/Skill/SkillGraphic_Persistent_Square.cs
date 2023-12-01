using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Persistent_Square : SkillGraphic
    {
        SkillModelConfig SkillModelConfig;
        public SkillGraphic_Persistent_Square(APlayer player, SkillPanel skill) : base(player, skill)
        {
            SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();
        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells)
        {
            var duration = this.SkillPanel.Duration;

            var endPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Enemy.Cell);
            Vector3Int scale = new Vector3Int(SkillPanel.Column, SkillPanel.Row, 0);

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, true, 0, (float)SkillModelConfig.ModelTime);
            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = endPos;
                effectCom.transform.localScale = new Vector3Int(2, 2, 1);

                yield return new WaitForSeconds(duration); //因为现在1s才是一个回合
                GameObject.Destroy(effectCom.gameObject);
            }

            yield return null;
        }
    }
}
