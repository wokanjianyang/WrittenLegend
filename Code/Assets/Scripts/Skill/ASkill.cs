using System.Collections.Generic;

namespace Game
{
    public class AttackData
    {
        public int Tid { get; set; }
        public float Ratio { get; set; }

    }
    abstract public class ASkill : IPlayer
    {
        public APlayer SelfPlayer { get; set; }
        public SkillData SkillData { get; set; }

        public ASkill(APlayer player, SkillData skillData)
        {
            this.SelfPlayer = player;
            this.SkillData = skillData;
        }
        public void Do(int tid)
        {
            this.PlayAnimation(tid);
        }

        virtual public void PlayAnimation(int tid)
        {
            
        }

        virtual public float CalcFormula(APlayer player,float ratio)
        {
            return 0;
        }

        virtual public List<AttackData> GetAllTargets(int tid)
        {
            return new List<AttackData>() {new AttackData()
            {
                Tid = tid,
                Ratio = 1
            }};
        }
        
 
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
