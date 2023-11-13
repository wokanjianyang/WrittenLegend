using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Arc : SkillGraphic
    {
        SkillModelConfig SkillModelConfig;

        public SkillGraphic_Arc(APlayer player, SkillPanel skill) : base(player, skill)
        {
            SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();
        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells)
        {
            Vector3Int startCell = this.SelfPlayer.Cell;
            Vector3Int endCell = cells[cells.Count / 2];
            Vector3 scale = endCell - startCell;

            float rotation = 0;
            if (startCell.x == endCell.x)
            {
                rotation = 90f;
                scale.x = 1;
            }
            else
            {
                scale.y = 1;
            }
            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, rotation, (float)SkillModelConfig.ModelTime);

            //Log.Info("scale :" + scale.ToString());
            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(startCell);

                Sequence sequence = DOTween.Sequence();

                sequence.Append(effectCom.transform.DOScale(scale * 1.2f, (float)SkillModelConfig.ModelTime));

                sequence.OnComplete(() => { GameObject.Destroy(effectCom.gameObject); });
                sequence.Play();

                yield return null;
            }
        }
    }
}
