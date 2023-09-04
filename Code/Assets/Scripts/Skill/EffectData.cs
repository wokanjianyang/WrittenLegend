using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class EffectData
    {
        public EffectConfig Config { get; set; }
        public int FromId { get; }
        /// <summary>
        /// 伤害比例加成
        /// </summary>
        public int Percent { get; }
        /// <summary>
        /// 伤害固定加成
        /// </summary>
        public long Damage { get; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 叠加层数
        /// </summary>
        public int Max { get; }


        public EffectData(int configId, int fromId, int percent, long damage, int duration, int max)
        {
            this.Config = EffectConfigCategory.Instance.Get(configId);
            this.FromId = fromId;

            this.Duration = duration;
            this.Max = max;
            this.Percent = percent;
            this.Damage = damage;
        }

    }

    public enum EffectType
    {
        Sub = -1,  //增加属性
        Add = 1,  //减少属性
        Pause = 2,
        IgnorePause = 3,
    }
}
