using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Move : ASkill
    {
        public Skill_Move(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
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

            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
            {
                Type = MsgType.SkillName,
                Content = SkillPanel.SkillData.SkillConfig.Name
            });

            bool isLoss = this.SelfPlayer.HP < this.SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP); //是否损失了血量

            //是否被控制？
            if (this.SelfPlayer.GetIsPause() || isLoss)
            {
                RandomTransport();
            }

            //RandomTransport();

            this.SelfPlayer.ClearEnemy();

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long rolePercent = DamageHelper.GetRolePercent(this.SelfPlayer.AttributeBonus, SkillPanel.SkillData.SkillConfig.Role);

                //Debug.Log("Effect " + effect.Config.Id + " _Percetn:" + total);

                DoEffect(this.SelfPlayer, this.SelfPlayer, 0, rolePercent, effect);
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
                    this.SelfPlayer.Move(bornCell);
                }
            }
        }
    }
}
