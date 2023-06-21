using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Square : SkillGraphic
    {
        public SkillGraphic_Square(APlayer player, SkillConfig skillConfig) : base(player, skillConfig)
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

                //Log.Info("selfPos :" + selfPos.ToString());

                var targetPos = GameProcessor.Inst.MapData.GetCenterPosition(cells);

                //Log.Info("centerPos :" + targetPos.ToString());

                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;
                effectCom.transform.DOLocalMove(targetPos, 0.3f);
                effectCom.transform.DOScale(scale, 0.5f);

                var duration = Mathf.Max(this.SkillConfig.Duration, 1f);
                yield return new WaitForSeconds(duration);
                //GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
