using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Single_Sequence : SkillGraphic
    {
        SkillModelConfig SkillModelConfig;
        public SkillGraphic_Single_Sequence(APlayer player, SkillPanel skill) : base(player, skill)
        {
            SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();
        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            cells.Insert(0, SelfPlayer.Cell);

            for (int i = 0; i < cells.Count - 1; i++)
            {
                var from = cells[i];
                var to = cells[i + 1];

                GameProcessor.Inst.StartCoroutine(IE_Attack(from, to));
            }
        }

        private IEnumerator IE_Attack(Vector3Int from, Vector3Int to)
        {
            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, 0, (float)SkillModelConfig.ModelTime);
            if (effectCom != null)
            {
                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(from);
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(to);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;
                effectCom.transform.DOLocalMove(targetPos, (float)SkillModelConfig.ModelTime);

                yield return new WaitForSeconds((float)SkillModelConfig.ModelTime);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
