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

            float rotation = 0;  //打右边

            if (startCell.x == selfCell.x && startCell.y > selfCell.y) //打上面
            {
                rotation = 270;
            }
            else if (startCell.x == selfCell.x && startCell.y < selfCell.y) //打下面
            {
                rotation = 90f;
            }
            else if (startCell.x < selfCell.x && startCell.y == selfCell.y) //打左边
            {
                rotation = -180f;
            }
            else {
                rotation = 0;
                scale = new Vector3(-1, 1, 0);
            }

            //Log.Info("scale :" + scale.ToString());

            var effectCom = EffectLoader.CreateEffect(this.SkillPanel.SkillData.SkillConfig.ModelName, false, rotation, (float)SkillModelConfig.ModelTime);

            if (effectCom != null)
            {
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(selfCell); //startCell

                Sequence sequence = DOTween.Sequence();

                // 添加缩放动画
                sequence.Append(effectCom.transform.DOScale(scale, (float)SkillModelConfig.ModelTime)); // 缩放到目标大小，持续1秒

                // 添加移动动画
                //sequence.Append(effectCom.transform.DOLocalMove(GameProcessor.Inst.MapData.GetWorldPosition(endCell), (float)SkillModelConfig.ModelTime)); // 移动到目标位置，持续1秒

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
