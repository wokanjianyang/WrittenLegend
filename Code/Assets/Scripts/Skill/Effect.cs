using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect
    {
        public APlayer SelfPlayer { get;  }
        public EffectData Data { get;  }
        public int FromId { get; }
        public long Total { get; set; }

        public int Duration { get; set; }
        public int Max { get; }

        /// <summary>
        /// 已生效次数
        /// </summary>
        public int DoCount { get; set; }

        public Effect(APlayer player,EffectData effectData, long total)
        {
            this.SelfPlayer = player;
            this.Data = effectData;
            this.Total = total;

            this.FromId = Data.FromId;
            this.Duration = Data.Duration;
            this.Max = Data.Max;

            this.DoCount = 0;
        }

        public void Do()
        {
            DoCount++;

            if (Data.Config.Type == 2)
            {  //控制特效,延迟触发
                //
            }
            else if (Data.Config.TargetAttr == ((int)AttributeEnum.CurrentHp))
            {
                //效果扣血,需要给APlayer封装一个方法，传入伤害数值，Player扣血以及计算后续死亡以及UI
                if (Data.Config.Type == (int)EffectType.Sub)
                {
                    SelfPlayer.Logic.OnDamage(new DamageResult(SelfPlayer.ID, Total, MsgType.Damage));
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
                    Debug.Log("Effect " + this.Data.Config.Id + " Gain:" + Total);
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, FromId, Total * Data.Config.Type);
                }
                else if (DoCount >= Duration)  //最后一次，移除属性
                {
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, FromId, 0);
                }
            }
        }

        public void DoCell(APlayer player)
        {
            if (player.GroupId == SelfPlayer.GroupId)
            { //不对队友产生伤害
                return;
            }

            player.Logic.OnDamage(new DamageResult(SelfPlayer.ID, Total, MsgType.Damage));
        }
    }
}
