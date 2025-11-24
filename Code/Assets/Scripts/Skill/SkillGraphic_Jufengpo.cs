using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Jufengpo : SkillGraphic
    {
        SkillModelConfig SkillModelConfig;
        public SkillGraphic_Jufengpo(APlayer player, SkillPanel skill) : base(player, skill)
        {
            SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();
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
            //var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, true, 0, (float)SkillModelConfig.ModelTime);

            //if (effectCom != null)
            //{
            //    var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
            //    effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
            //    effectCom.transform.localPosition = targetPos;

            //    yield return new WaitForSeconds((float)SkillModelConfig.ModelTime); //因为现在1s才是一个回合
            //    GameObject.Destroy(effectCom.gameObject);
            //}
            //yield return null;

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, 0, (float)SkillModelConfig.ModelTime);
            if (effectCom != null)
            {

                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;
                effectCom.transform.DOLocalMove(targetPos, (float)SkillModelConfig.ModelTime);

                yield return new WaitForSeconds((float)SkillModelConfig.ModelTime);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
