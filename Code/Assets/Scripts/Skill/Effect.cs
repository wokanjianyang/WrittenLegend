using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int CongigId { get; set; }
        /// <summary>
        /// ����ʱ���
        /// </summary>
        public long CreateTime { get; set; }

        public int Level { get; set; }

        public int Duration { get; set; }

        public long Total { get; set; }

        public EffectConfig Config { get; set; }

        public long BaseValue { get; set; }  //��Ч�Ļ�׼ֵ,���簴��������Ĺ�������Ѫ�������Ѫ��

        /// <summary>
        /// ����Ч����
        /// </summary>
        public int DoCount { get; set; }

        public APlayer SelfPlayer { get; set; }

        public Effect(APlayer player,EffectConfig config, long total)
        {
            this.SelfPlayer = player;
            this.Config = config;
            this.Total = total;
        }

        public void Do()
        {
            if (Config.TargetAttr == ((int)AttributeEnum.CurrentHp))
            {
                //Ч����Ѫ,��Ҫ��APlayer��װһ�������������˺���ֵ��Player��Ѫ�Լ�������������Լ�UI
                if (Config.Type == (int)EffectTypeEnum.Sub)
                {
                    SelfPlayer.Logic.OnDamage(SelfPlayer.ID, Total);
                }
                else {
                    SelfPlayer.Logic.OnReply(Total);
                }
            }
            else
            {
                SelfPlayer.AttributeBonus.SetAttr((AttributeEnum)Config.TargetAttr, AttributeFrom.Skill, Total * Config.Type);
            }

            DoCount++;
        }

        public enum EffectTypeEnum { 
            Sub = -1,  //��������
            Add = 1  //��������
        }
    }
}
