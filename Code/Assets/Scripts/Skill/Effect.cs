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

        public AttributeEnum Attr { get; set; }

        public long Total { get; set; }

        public EffectConfig Config { get; set; }

        /// <summary>
        /// ����Ч����
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
                //Ч����Ѫ,��Ҫ��APlayer��װһ�������������˺���ֵ��Player��Ѫ�Լ�������������Լ�UI
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
