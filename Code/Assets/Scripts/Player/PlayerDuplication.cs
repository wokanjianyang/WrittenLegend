using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    public class PlayerDuplication : APlayer
    {
        public APlayer Master { get; set; }
        private SkillPanel SkillPanel { get; set; }

        private int Life = 0;

        public PlayerDuplication(APlayer player, SkillPanel skill) : base()
        {
            this.GroupId = player.GroupId;
            this.Master = player;

            this.SkillPanel = skill;
            this.RuleType = player.RuleType;
            this.Life = skill.Duration;

            this.BirthDay = TimeHelper.ClientNowSeconds();

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Duplication;
            this.Level = Master.Level;
            this.ModelType = Master.ModelType;
            this.Name = "分身" + "(" + Master.Name + ")";

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double rate = SkillPanel.Percent / 100.0;

            this.SetAttackSpeed((int)(Master.AttributeBonus.GetTotalAttr(AttributeEnum.Speed) * rate));
            this.SetMoveSpeed((int)(Master.AttributeBonus.GetTotalAttr(AttributeEnum.MoveSpeed) * rate));

            double attr = Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP) * rate);

            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, attr * rate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, attr * rate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, attr * rate);

            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Def) * rate);
            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Speed) * rate);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky) * rate);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate) * rate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage) * rate);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist) * rate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist) * rate);
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea) * rate);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist) * rate);
            AttributeBonus.SetAttr(AttributeEnum.InheritIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.InheritIncrea) * rate);
            AttributeBonus.SetAttr(AttributeEnum.RestoreHp, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHp) * rate);
            AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHpPercent) * rate);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Miss) * rate);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Accuracy) * rate);

            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageIncrea) * rate);
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageResist) * rate);

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage) * rate);
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage) * rate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage) * rate);

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea) * rate);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageResist) * rate);


            this.RingType = Master.RingType;

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            for (int i = 0; i < Master.SelectSkillList.Count; i++)
            {
                SkillState mss = Master.SelectSkillList[i];

                if (mss.SkillPanel.SkillId != SkillPanel.SkillId && mss.SkillPanel.SkillData.SkillConfig.Type != (int)SkillType.Valet) //not loop
                {
                    SkillState skill = new SkillState(this, mss.SkillPanel, mss.Position, 0);
                    SelectSkillList.Add(skill);
                }
            }
        }

        public override float DoEvent()
        {
            long now = TimeHelper.ClientNowSeconds();
            long lf = now - BirthDay;
            //Debug.Log("life:" + lf);
            if (lf >= Life) //auto dead
            {
                this.HP = 0;

                GameProcessor.Inst.PlayerManager.RemoveDeadPlayers(this);

                return 999f;
            }
            else
            {
                return base.DoEvent();
            }
        }
    }
}
