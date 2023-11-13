using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillGraphic_FrontRow : SkillGraphic
    {
        SkillModelConfig SkillModelConfig;
        public SkillGraphic_FrontRow(APlayer player, SkillPanel skill) : base(player, skill)
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

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, rotation, (float)SkillModelConfig.ModelTime);

            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(startCell);

                Sequence sequence = DOTween.Sequence();

                // 添加缩放动画
                sequence.Append(effectCom.transform.DOScale(scale, (float)SkillModelConfig.ModelTime)); // 缩放到目标大小，持续1秒

                // 添加移动动画
                //sequence.Append(effectCom.transform.DOLocalMove(targetPos, 1.0f)); // 移动到目标位置，持续1秒

                // 在动画结束时执行回调
                sequence.OnComplete(() =>
                {
                    GameObject.Destroy(effectCom.gameObject);
                    //Debug.Log("拉升和移动动画完成！");
                });

                // 启动动画序列
                sequence.Play();

                yield return null;
            }
        }
    }
}
