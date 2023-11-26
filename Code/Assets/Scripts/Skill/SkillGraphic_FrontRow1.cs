using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_FrontRow1 : SkillGraphic
    {
        SkillModelConfig SkillModelConfig;
        public SkillGraphic_FrontRow1(APlayer player, SkillPanel skill) : base(player, skill)
        {
            SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();
        }

        public override void PlayAnimation(List<Vector3Int> cells)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cells));
        }

        private IEnumerator IE_Attack(List<Vector3Int> cells)
        {
            Vector3Int startCell = cells[0];
            Vector3Int endCell = cells[cells.Count - 1];
            Vector3Int selfCell = SelfPlayer.Cell;

            Vector3 scale = new Vector3(1, 1, 0);

            float rotation = 0;  //���ұ�

            if (startCell.x == selfCell.x && startCell.y > selfCell.y) //������
            {
                rotation = 270;
                Log.Info("������");
            }
            else if (startCell.x == selfCell.x && startCell.y < selfCell.y) //������
            {
                rotation = 90f;
                Log.Info("������");
            }
            else if (startCell.x < selfCell.x && startCell.y == selfCell.y) //�����
            {
                rotation = -180f;
                Log.Info("�����");
            }
            else {
                rotation = 0;
                scale = new Vector3(-1, 1, 0);
                Log.Info("������");
            }

            //Log.Info("scale :" + scale.ToString());

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, rotation, (float)SkillModelConfig.ModelTime);

            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(selfCell); //startCell

                Sequence sequence = DOTween.Sequence();

                // �������Ŷ���
                sequence.Append(effectCom.transform.DOScale(scale, (float)SkillModelConfig.ModelTime)); // ���ŵ�Ŀ���С������1��

                // �����ƶ�����
                //sequence.Append(effectCom.transform.DOLocalMove(GameProcessor.Inst.MapData.GetWorldPosition(endCell), (float)SkillModelConfig.ModelTime)); // �ƶ���Ŀ��λ�ã�����1��

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