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

        protected List<AttackData> attackDataCache { get; set; }

        private SkillGraphic skillGraphic;

        public ASkill(APlayer player, SkillData skillData)
        {
            this.SelfPlayer = player;
            this.SkillData = skillData;
            this.skillGraphic = new SkillGraphic(player);
        }
        public void Do(int tid)
        {
            attackDataCache = this.GetAllTargets(tid);
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);
                var damage = (int)this.CalcFormula(enemy, attackData.Ratio);
                enemy.OnHit(tid, damage);
            }

            this.skillGraphic?.PlayAnimation(tid);
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
