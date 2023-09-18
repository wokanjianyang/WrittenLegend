using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Arc : SkillGraphic
    {
        public SkillGraphic_Arc(APlayer player, SkillPanel skill) : base(player, skill)
        {
        }

        public override void PlayAnimation(List<Vector3Int> cells, Vector3Int scale)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells, scale));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells, Vector3Int scale)
        {
            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, true);
            if (effectCom != null)
            {
                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);

                //Log.Info("arc self :" + SelfPlayer.Cell.ToString());
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;

                var cellDuration = 0.5f / cells.Count;

                foreach (Vector3Int cell in cells)
                {
                    //Log.Info("arc cell :" + cell.ToString());

                    var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                    //Log.Info("arc targetPos :" + targetPos.ToString());
                    effectCom.transform.DOLocalMove(targetPos, cellDuration).SetEase(Ease.InQuad);
                    yield return new WaitForSeconds(cellDuration);
                }

                var duration = Math.Max(this.SkillPanel.Duration, 0.5f);
                yield return new WaitForSeconds(duration);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
