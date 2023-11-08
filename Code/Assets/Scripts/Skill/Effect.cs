using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect
    {
        public APlayer SelfPlayer { get; }
        public EffectData Data { get; }
        public int FromId { get; }
        public long Total { get; set; }

        public int UID { get; private set; }

        public int Duration { get; set; }
        public int Max { get; }

        /// <summary>
        /// 已生效次数
        /// </summary>
        public int DoCount { get; set; }

        public Effect(APlayer player, EffectData effectData, long total)
        {
            this.SelfPlayer = player;
            this.Data = effectData;

            this.Total = total;

            this.FromId = Data.FromId;
            this.Duration = Data.Duration;
            this.Max = Data.Max;

            this.UID = FromId + Data.Layer;

            this.DoCount = 0;
        }

        public void Do()
        {
            DoCount++;

            if (Data.Config.Type == ((int)EffectType.HP))
            {
                DamageAndRestore();

            }
            else if (Data.Config.Type == ((int)EffectType.Attr))
            {
                ChangeAttr();
            }
        }

        private long CalBaseValue()
        {
            long m = Data.Percent;

            if (Data.Config.SourceAttr == -1)
            {
                m = Total;
            }
            else if (Data.Config.SourceAttr == 0)
            {
                m = m * SelfPlayer.HP / 100;
            }
            else if (Data.Config.SourceAttr >= 1)
            {
                m = m * SelfPlayer.AttributeBonus.GetTotalAttr((AttributeEnum)Data.Config.SourceAttr) / 100;
            }

            return m;
        }

        private void DamageAndRestore()
        {
            long hp = CalBaseValue();

            if (Data.Config.CalType > 0) //回血
            {
                SelfPlayer.OnRestore(0, hp);
            }
            else //伤害
            {
                SelfPlayer.OnHit(new DamageResult(0, hp, MsgType.Damage, RoleType.All));
            }
        }

        private void ChangeAttr()
        {
            long attr = CalBaseValue() * Data.Config.CalType;

            if (DoCount == 1) //第一次增加属性
            {
                Debug.Log("Skill" + Data.Config.Id + " attr:" + attr);

                if (Data.Config.TargetAttr == (int)AttributeEnum.PanelHp)
                {
                    this.SelfPlayer.ChangeMaxHp(FromId, attr);
                }
                else
                {
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, UID, attr);
                }
            }
            else if (DoCount >= Duration)  //最后一次，移除属性
            {
                if (Data.Config.TargetAttr == (int)AttributeEnum.PanelHp)
                {
                    this.SelfPlayer.ChangeMaxHp(FromId, 0);
                }
                else
                {
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, UID, 0);
                }
            }
        }

        public void Clear()
        {
            this.DoCount = this.Duration + 999999;

            if (Data.Config.Type == (int)EffectType.Attr)
            {
                if (Data.Config.TargetAttr == (int)AttributeEnum.PanelHp)
                {
                    this.SelfPlayer.ChangeMaxHp(FromId + Data.Layer, 0);
                }
                else
                {
                    this.SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, UID, 0);
                }
            }
        }
    }
}
