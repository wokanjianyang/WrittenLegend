using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Valet : ASkill
    {
        public List<Valet> ValetList = new List<Valet>();
        public int MaxValet = 0;

        public Skill_Valet(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            this.skillGraphic = null;
            MaxValet = skillPanel.EnemyMax;
        }

        public override bool IsCanUse()
        {
            if (MaxValet > 0 && MaxValet > ValetList.Count)
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
            foreach (Valet valet in ValetList)
            {
                valet.OnHit(SelfPlayer.ID, valet.HP);
            }
            ValetList.Clear();

            //创造新的
            for (int i = 0; i < MaxValet; i++)
            {
                Valet valet = GameProcessor.Inst.PlayerManager.LoadValet(SelfPlayer, this.SkillPanel);
                ValetList.Add(valet);
            }
        }

        public void ClearValet() {
            //销毁之前的
            foreach (Valet valet in ValetList)
            {
                valet.OnHit(SelfPlayer.ID, valet.HP);
            }
            ValetList.Clear();
        }
    }
}
