using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class OrbState
    {
        public APlayer SelfPlayer { get; set; }

        public int Priority { get; }
        public long LastUseTime { get; private set; } = 0;

        public int Rate { get; private set; } = 0;

        OrbEffectConfig Config { get; }

        private float CD = 0;

        public OrbState(APlayer player, OrbEffectConfig config, int priority, int useRound)
        {
            this.SelfPlayer = player;
            this.Priority = priority; // - skillPanel.SkillData.SkillConfig.Priority;
            this.LastUseTime = useRound;
            this.Config = config;
            this.CD = 0;
        }

        public bool IsCanUse(long Now)
        {
            return (CD <= 0);
        }

        public void RunCD(float time)
        {
            this.CD -= time;
        }

        public void Do()
        {
            this.CD = Config.CD;
            this.LastUseTime = TimeHelper.ClientNowSeconds();
        }

        public void Do(double baseHp)
        {
            this.CD = Config.CD;
            this.LastUseTime = TimeHelper.ClientNowSeconds();
        }

        public void SetLastUseTime(long time)
        {
            this.LastUseTime = time;
        }

        public void AddRate(int rate)
        {
            this.Rate += rate;
        }
    }
}
