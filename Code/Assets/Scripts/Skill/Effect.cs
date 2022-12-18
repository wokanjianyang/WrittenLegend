using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect 
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        public int CongigId { get; set; }
        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }

        public int Level { get; set; }

        public int Duration { get; set; }

        public AttributeEnum Attr { get; set; }

        public long Total { get; set; }

        public EffectConfig Config { get; set; }

        /// <summary>
        /// 已生效次数
        /// </summary>
        public int DoCount { get; set; }

        public APlayer SelfPlayer { get; set; }

        public Effect(APlayer player)
        {
            this.SelfPlayer = player;
        }

        public void Do()
        {
            if (Attr == AttributeEnum.CurrentHp)
            {
                //效果扣血,需要给APlayer封装一个方法，传入伤害数值，Player扣血以及计算后续死亡以及UI
                SelfPlayer.SetHP(SelfPlayer.HP - Total);
            }
            else {
                SelfPlayer.AttributeBonus.SetAttr(Attr, AttributeFrom.Skill, Total);
            }

            DoCount++;

            if (DoCount >= Config.CD) {
                return;
            }

        }
    }
}
