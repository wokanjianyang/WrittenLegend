using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Move : ASkill
    {
        public Skill_Move(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = null;
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //是否被控制？
            //if (this.SelfPlayer.GetIsPause())
            //{
            //    RandomTransport();
            //}
            //else
            //{
            //    double rate = this.SelfPlayer.HP * 1d / this.SelfPlayer.AttributeBonus.GetAttackAttr(AttributeEnum.HP);
            //    if (rate < 0.8)
            //    {
            //        //是否血量低于80%
            //        RandomTransport();
            //    }
            //}

            RandomTransport();

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long total = DamageHelper.GetEffectFromTotal(this.SelfPlayer.AttributeBonus, SkillPanel, effect);

                //Debug.Log("Effect " + effect.Config.Id + " _Percetn:" + total);

                DoEffect(this.SelfPlayer, this.SelfPlayer, total, effect);
            }
        }

        private void RandomTransport()
        {

            var tempCells = GameProcessor.Inst.MapData.AllCells.ToList();
            var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
            tempCells.RemoveAll(p => allPlayerCells.Contains(p));

            if (tempCells.Count > 0)
            {
                var bornCell = Vector3Int.zero;
                if (tempCells.Count > 1)
                {
                    var index = RandomHelper.RandomNumber(0, tempCells.Count);
                    bornCell = tempCells[index];
                    this.SelfPlayer.SetPosition(bornCell, true);
                }
            }
        }
    }
}
