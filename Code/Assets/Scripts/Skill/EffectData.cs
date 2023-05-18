using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class EffectData
    {
        public EffectConfig Config { get; set; }
        public int FromId { get; }
        public int Percent { get; }

        public long Damage { get; }
        public int Duration { get; set; }
        public int Max { get; }


        public EffectData(int configId, int fromId, int percent,long damage, int duration, int max)
        {
            this.Config = EffectConfigCategory.Instance.Get(configId);
            this.FromId = fromId;

            this.Duration = duration;
            this.Max = max;
            this.Percent = percent;
            this.Damage = damage;
        }
    }
}
