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
        public SkillPanel SkillPanel{ get; set; }

        protected List<AttackData> attackDataCache { get; set; }

        protected SkillGraphic skillGraphic { get; set; }

        public ASkill(APlayer player, SkillPanel skill)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skill;
        }
        virtual public void Do()
        {
            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);
                var damage = this.CalcFormula(enemy, attackData.Ratio);
                enemy.OnHit(attackData.Tid, damage);

                SkillConfig skillConfig = SkillPanel.SkillData.SkillConfig;
                if (skillConfig.EffectIdList != null && skillConfig.EffectIdList.Length > 0)
                {
                    foreach (int EffectId in skillConfig.EffectIdList)
                    {
                        if (EffectId > 0)
                        {
                            EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

                            var effectTarget = config.TargetType == 1 ? this.SelfPlayer : enemy; //1 为作用自己 2 为作用敌人

                            int fromId = (int)AttributeFrom.Skill * 100000 + SkillPanel.SkillId + EffectId;
                            long total = 0;
                            int duration = SkillPanel.Duration;
                            if (config.Duration > 0)
                            {  //持续Buff
                                effectTarget.AddEffect(EffectId, this.SelfPlayer, fromId, total, duration);
                            }
                            else
                            {
                                effectTarget.RunEffect(EffectId, this.SelfPlayer, fromId, total, duration);
                            }
                        }
                    }
                }

                this.skillGraphic?.PlayAnimation(enemy.Cell);
            }
        }

        virtual public bool IsCanUse() {
            return true;
        }

        virtual public long CalcFormula(APlayer player,float ratio)
        {
            return 0;
        }

        abstract public List<AttackData> GetAllTargets();
        
 
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
