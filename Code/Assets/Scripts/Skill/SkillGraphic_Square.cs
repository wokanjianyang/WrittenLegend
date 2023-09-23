using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_Square : SkillGraphic
    {
        public SkillGraphic_Square(APlayer player, SkillPanel skill) : base(player, skill)
        {
        }

        public override void PlayAnimation(List<Vector3Int> cells, Vector3Int scale)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells, scale));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells, Vector3Int scale)
        {
            var startPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);
            var endPos = GameProcessor.Inst.MapData.GetCenterPosition(cells);

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, true);
            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = startPos;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(effectCom.transform.DOScale(scale, ConfigHelper.SkillAnimaTime));
                sequence.Append(effectCom.transform.DOLocalMove(endPos, ConfigHelper.SkillAnimaTime));
                sequence.OnComplete(() =>
                {
                    GameObject.Destroy(effectCom.gameObject);
                });

                // ∆Ù∂Ø∂Øª≠–Ú¡–
                sequence.Play();
                yield return null;
            }
        }
    }
}
