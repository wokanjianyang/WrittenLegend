using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillState
    {
        public SkillData Data { get; set; }
        public APlayer SelfPlayer { get; set; }

        public int Priority { get; set; }

        private ASkill skillLogic;

        private int lastUseRound = 0;

        public SkillState(APlayer player, SkillData data, int position,int useRound)
        {
            this.SelfPlayer = player;
            this.Data = data;
            this.Priority = position - data.Priority;
            this.lastUseRound = useRound;
            if (data.Type == (int)SkillType.Attack)
            {
                this.skillLogic = new Skill_Sweep(player, data);
            }
            else if (data.Type == (int)SkillType.Valet)
            {
                this.skillLogic = new Skill_Valet(player, data);
            }
            else if (data.Type == (int)SkillType.Map)
            {
                this.skillLogic = new Skill_Map(player, data);
            }
            else
            {
                this.skillLogic = new BaseAttackSkill(player, data);
            }

        }

        public bool IsCanUse()
        {
            return (this.lastUseRound == 0 || this.SelfPlayer.RoundCounter - lastUseRound > this.Data.CD) && this.skillLogic.IsCanUse();
        }

        public List<AttackData> GetAllTarget()
        {
            return skillLogic.GetAllTargets();
        }

        public void Do(List<AttackData> targets)
        {
            this.lastUseRound = this.SelfPlayer.RoundCounter;

            foreach (AttackData attack in targets)
            {
                this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
                {
                    TargetId = attack.Tid,
                    Content = this.Data.Name
                });
            }

            this.skillLogic.Do();
        }

        public void Destory() {
            if (skillLogic is Skill_Valet) {
                Skill_Valet sv = skillLogic as Skill_Valet;
                sv.ClearValet();
            }
        }
    }
}
