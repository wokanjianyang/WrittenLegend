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

        private SkillGraphic skillGraphic;

        public ASkill(APlayer player, SkillPanel skill)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skill;
            this.skillGraphic = new SkillGraphic(player);
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
                if (skillConfig.EffectIdList!=null && skillConfig.EffectIdList.Length > 0)
                {
                    foreach (int EffectId in skillConfig.EffectIdList)
                    {
                        EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

                        var effectTarget = config.TargetType == 1 ? this.SelfPlayer : enemy; //1 为作用自己 2 为作用敌人

                        if (config.Duration > 0)
                        {  //持续Buff
                            effectTarget.AddEffect(EffectId,this.SelfPlayer);
                        }
                        else
                        {
                            effectTarget.RunEffect(EffectId,this.SelfPlayer);
                        }
                    }
                }

                this.skillGraphic?.PlayAnimation(attackData.Tid);
            }
        }

        virtual public bool IsCanUse() {
            return true;
        }

        virtual public long CalcFormula(APlayer player,float ratio)
        {
            return 0;
        }

        public long GetRoleAttack()
        {
            long attack = 0;
            switch (SkillPanel.SkillData.SkillConfig.Role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.MagicAtt);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt);
                        break;
                    }
            }

            return attack;
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
