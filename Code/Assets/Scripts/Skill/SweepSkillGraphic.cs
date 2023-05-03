using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SweepSkillGraphic : SkillGraphic
    {
        public SweepSkillGraphic(APlayer player,string skillName) : base(player,skillName)
        {
        }

        public override void PlayAnimation(Vector3Int cell)
        {
            GameProcessor.Inst.StartCoroutine(IE_Attack(cell));
        }

        private IEnumerator IE_Attack(Vector3Int cell)
        {

            var effectCom = EffectLoader.CreateEffect(this.SkillName);
            if (effectCom != null)
            {
                
                var selfPos = GameProcessor.Inst.MapData.GetWorldPosition(SelfPlayer.Cell);
                var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
                effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
                effectCom.transform.localPosition = selfPos;
                effectCom.transform.DOLocalMove(targetPos, 0.5f);

                yield return new WaitForSeconds(1f);
                //effectCom.gameObject.SetActive(false);
                GameObject.Destroy(effectCom.gameObject);
            }
        }
    }
}
