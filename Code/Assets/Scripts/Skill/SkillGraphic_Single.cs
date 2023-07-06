using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Single : SkillGraphic
    {
        public SkillGraphic_Single(APlayer player, SkillConfig skillConfig) : base(player, skillConfig)
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
            var effectCom = EffectLoader.CreateEffect(this.SkillConfig.ModelName, false);
            if (effectCom != null)
            {

                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;
                effectCom.transform.DOLocalMove(targetPos, 0.5f);

                var duration = Mathf.Max(this.SkillConfig.Duration, 1f);
                yield return new WaitForSeconds(duration);
                //effectCom.gameObject.SetActive(false);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}