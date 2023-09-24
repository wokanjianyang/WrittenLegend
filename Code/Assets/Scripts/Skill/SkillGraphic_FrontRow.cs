using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_FrontRow : SkillGraphic
    {
        public SkillGraphic_FrontRow(APlayer player, SkillPanel skill) : base(player, skill)
        {
        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells)
        {
            Vector3Int startCell = cells[0];
            Vector3Int endCell = cells[cells.Count - 1];
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

            //Log.Info("scale :" + scale.ToString());

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, rotation);

            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(startCell);

                Sequence sequence = DOTween.Sequence();

                // ������Ŷ���
                sequence.Append(effectCom.transform.DOScale(scale, ConfigHelper.SkillAnimaTime1)); // ���ŵ�Ŀ���С������1��

                // ����ƶ�����
                //sequence.Append(effectCom.transform.DOLocalMove(targetPos, 1.0f)); // �ƶ���Ŀ��λ�ã�����1��

                // �ڶ�������ʱִ�лص�
                sequence.OnComplete(() =>
                {
                    GameObject.Destroy(effectCom.gameObject);
                    //Debug.Log("�������ƶ�������ɣ�");
                });

                // ������������
                sequence.Play();

                yield return null;
            }
        }
    }
}
