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

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells)
        {
            if (this.SelfPlayer.Camp == PlayerType.Enemy)
            {
                Debug.Log(cells.ListToString());
            }

            Vector3Int scale = new Vector3Int(SkillPanel.Column, SkillPanel.Row, 0);

            var startPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);
            //var endPos = GameProcessor.Inst.MapData.GetCenterPosition(cells);

            var endPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Enemy.Cell);

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false);
            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = startPos;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(effectCom.transform.DOLocalMove(endPos, ConfigHelper.SkillAnimaTime/2));
                sequence.Append(effectCom.transform.DOScale(scale, ConfigHelper.SkillAnimaTime/2));
                sequence.OnComplete(() =>
                {
                    GameObject.Destroy(effectCom.gameObject);
                });

                // ������������
                sequence.Play();
                yield return null;
            }
        }
    }
}
