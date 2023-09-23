using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Single : SkillGraphic
    {
        public SkillGraphic_Single(APlayer player, SkillPanel skill) : base(player, skill)
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
            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false);
            if (effectCom != null)
            {

                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;
                effectCom.transform.DOLocalMove(targetPos, ConfigHelper.SkillAnimaTime);

                yield return new WaitForSeconds(ConfigHelper.SkillAnimaTime);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
