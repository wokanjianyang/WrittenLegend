using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Valet : ASkill
    {
        public int MaxValet = 0;

        public Skill_Valet(APlayer player, SkillPanel skillPanel, bool isShow) : base(player, skillPanel)
        {
            this.skillGraphic = null;
            MaxValet = skillPanel.EnemyMax + (int)SelfPlayer.AttributeBonus.GetAttackAttr(AttributeEnum.SkillValetCount);
        }

        public override bool IsCanUse()
        {
            var valets = GameProcessor.Inst.PlayerManager.GetValets(SelfPlayer, this.SkillPanel);
            if (MaxValet > 0 && MaxValet > valets.Count)
            {
                return true;
            }
            return false;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //销毁之前的
            // foreach (Valet valet in ValetList)
            // {
            //     valet.OnHit(SelfPlayer.ID, valet.HP);
            // }
            // ValetList.Clear();

            var valets = GameProcessor.Inst.PlayerManager.GetValets(SelfPlayer, this.SkillPanel);

            //创造新的
            for (int i = valets.Count; i < MaxValet; i++)
            {
                Valet valet = GameProcessor.Inst.PlayerManager.LoadValet(SelfPlayer, this.SkillPanel);
            }
        }
    }
}
