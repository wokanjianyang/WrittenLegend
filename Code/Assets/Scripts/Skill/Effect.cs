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
        public double Damage { get; set; }
        public long RolePercent { get; set; }

        public int UID { get; private set; }

        public int Duration { get; set; }

        public int Max { get; }

        public int Layer { get; set; }

        private float TotalTime = 0;

        private float Interval = 1f;

        private float RunTime = 0f;

        private int RunCount = 0;

        public bool Active = true;

        public Effect(APlayer player, EffectData effectData, double damage, long rolePercent, int layer)
        {
            this.SelfPlayer = player;
            this.Data = effectData;

            this.Damage = damage;
            this.RolePercent = rolePercent;

            this.FromId = Data.FromId;
            this.Duration = Data.Duration; //延迟触发的,要多1s
            this.Max = Data.Max;

            this.Layer = layer;
            this.UID = FromId + this.Layer;
        }

        public void Do(float time)
        {
            if (!Active)
            {
                return; //已经结束的
            }

            TotalTime += time;
            RunTime += time;

            if (RunTime < Interval)
            {
                return; //还没到触发时间
            }

            RunTime = 0;
            RunCount++;

            if (TotalTime > Duration)
            {
                Active = false;
            }

            if (Data.Config.Type == ((int)EffectType.HP))
            {
                DamageAndRestore();
            }
            else if (Data.Config.Type == ((int)EffectType.Attr))
            {
                ChangeAttr();
            }
            else if (Data.Config.Type == (int)EffectType.Pause)
            {
                //show message
                this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent
                {
                    Type = MsgType.Other,
                    Content = Data.Config.Name
                });
            }

            //Debug.Log("Do Effect:" + TotalTime);
        }

        private double CalBaseValue()
        {
            double m = Data.Percent;

            if (Data.Config.ExpertRise > 0)
            {
                m += RolePercent * Data.Config.ExpertRise / 100;
            }

            if (Data.Config.SourceAttr == -1)
            {
                m = Damage;
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
            double hp = CalBaseValue();

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
            double attr = CalBaseValue() * Data.Config.CalType;

            if (RunCount == 1) //第一次增加属性
            {
                //Debug.Log("Skill" + Data.Config.Id + " attr:" + attr);

                if (Data.Config.TargetAttr == (int)AttributeEnum.PanelHp)
                {
                    this.SelfPlayer.ChangeMaxHp(FromId, attr);
                }
                else
                {
                    SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, UID, attr);
                }
            }
            else if (!Active) //最后一次，移除属性
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
            this.Active = false;

            if (Data.Config.Type == (int)EffectType.Attr)
            {
                if (Data.Config.TargetAttr == (int)AttributeEnum.PanelHp)
                {
                    this.SelfPlayer.ChangeMaxHp(UID, 0);
                }
                else
                {
                    this.SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Data.Config.TargetAttr, UID, 0);
                }
            }
        }
    }
}
