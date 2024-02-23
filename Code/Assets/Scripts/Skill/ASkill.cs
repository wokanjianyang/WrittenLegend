using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AttackData
    {
        public int Tid { get; set; }

        public Vector3Int Cell { get; set; }
        public float Ratio { get; set; }

    }
    abstract public class ASkill : IPlayer
    {
        public APlayer SelfPlayer { get; set; }
        public SkillPanel SkillPanel { get; set; }

        protected List<AttackData> attackDataCache { get; set; }

        protected SkillGraphic skillGraphic { get; set; }

        public ASkill(APlayer player, SkillPanel skill)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skill;
        }
        abstract public void Do();
        public virtual void Do(double baseHp)
        {

        }

        public void DoEffect(APlayer enemy, APlayer self, double damage, long rolePercent, EffectData data)
        {
            EffectConfig config = data.Config;

            var effectTarget = config.TargetType == 1 ? this.SelfPlayer : enemy; //1 为作用自己 2 为作用敌人

            if (data.Duration > 0)
            {  //持续Buff
                effectTarget.AddEffect(effectTarget, data, damage, rolePercent);
            }
            else
            {
                effectTarget.RunEffect(effectTarget, data, damage, rolePercent);
            }
        }
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }

        abstract public bool IsCanUse();
    }
}
