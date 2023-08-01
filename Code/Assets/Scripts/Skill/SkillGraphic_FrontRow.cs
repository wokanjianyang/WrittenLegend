using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_FrontRow : SkillGraphic
    {
        public SkillGraphic_FrontRow(APlayer player, SkillConfig skillConfig) : base(player, skillConfig)
        {
        }

        public override void PlayAnimation(List<Vector3Int> cells, Vector3Int scale)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells, scale));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells, Vector3Int scale)
        {
            var effectCom = EffectLoader.CreateEffect(this.SkillConfig.ModelName, true);
            if (effectCom != null)
            {
                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);

                //Log.Info("self :" + SelfPlayer.Cell.ToString());
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;

                foreach (Vector3Int cell in cells)
                {
                    //Log.Info("cell :" + cell.ToString());

                    var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                    //Log.Info("targetPos :" + targetPos.ToString());
                    effectCom.transform.DOLocalMove(targetPos, 0.5f);
                }

                var duration = Mathf.Max(this.SkillConfig.Duration, 1f);
                yield return new WaitForSeconds(duration);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
