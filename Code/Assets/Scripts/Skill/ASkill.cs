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
        virtual public void Do()
        {
            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);
                var damage = (int)this.CalcFormula(enemy, attackData.Ratio);
                enemy.OnHit(attackData.Tid, damage);

                if (SkillData.SkillConfig.EffectIdList!=null && SkillData.SkillConfig.EffectIdList.Length > 0)
                {
                    foreach (int EffectId in SkillData.SkillConfig.EffectIdList)
                    {
                        EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

                        if (config.Duration > 0)
                        {  //³ÖÐøBuff
                            enemy.AddEffect(EffectId);
                        }
                        else
                        {
                            enemy.RunEffect(EffectId);
                        }
                    }
                }

                this.skillGraphic?.PlayAnimation(attackData.Tid);
            }
        }

        virtual public bool IsCanUse() {
            return true;
        }

        virtual public float CalcFormula(APlayer player,float ratio)
        {
            return 0;
        }

        virtual public List<AttackData> GetAllTargets()
        {
            return new List<AttackData>();
        }
        
 
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
