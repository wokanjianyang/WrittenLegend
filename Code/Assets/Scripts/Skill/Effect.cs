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

        public long Total { get; set; }

        public EffectConfig Config { get; set; }

        public long BaseValue { get; set; }  //特效的基准值,比如按攻击计算的攻击，按血量计算的血量

        public int FromId { get; }

        /// <summary>
        /// 已生效次数
        /// </summary>
        public int DoCount { get; set; }

        public APlayer SelfPlayer { get; set; }

        public Effect(APlayer player,EffectConfig config,int fromId, long total,int duration)
        {
            this.SelfPlayer = player;
            this.Config = config;
            this.FromId = fromId;
            this.Total = total;
            this.Duration = duration;
            this.DoCount = 0;
        }

        public void Do()
        {
            DoCount++;

            if (Config.TargetAttr == ((int)AttributeEnum.CurrentHp))
            {
                //效果扣血,需要给APlayer封装一个方法，传入伤害数值，Player扣血以及计算后续死亡以及UI
                if (Config.Type == (int)EffectTypeEnum.Sub)
                {
                    SelfPlayer.Logic.OnDamage(SelfPlayer.ID, Total);
                }
                else
                {
                    SelfPlayer.Logic.OnRestore(Total);
                }
            }
            else
            {
                if (DoCount == 1) //第一次增加属性
                {
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Config.TargetAttr, FromId, Total * Config.Type);
                }
                else if (DoCount >= Duration)  //最后一次，移除属性
                {
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Config.TargetAttr, FromId, 0);
                }
            }
        }

        public void DoCell(APlayer player)
        {
            if (player.GroupId == SelfPlayer.GroupId)
            { //不对队友产生伤害
                return;
            }

            player.Logic.OnDamage(SelfPlayer.ID, Total);
        }

        public enum EffectTypeEnum { 
            Sub = -1,  //增加属性
            Add = 1  //减少属性
        }
    }
}
