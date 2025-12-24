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
            this.IsFestive = player.IsFestive;

            this.BirthDay = TimeHelper.ClientNowSeconds();

            this.Init();
        }

        private void Init()
        {
            if (this.Master.Camp == PlayerType.Hero)
            {
                this.Camp = PlayerType.Duplication;
            }
            else
            {
                this.Camp = this.Master.Camp;
            }
            this.Level = Master.Level;
            this.ModelType = Master.ModelType;
            this.FashionId = Master.FashionId;
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

            this.SetAttackSpeed((int)(Master.AttributeBonus.GetTotalAttr(AttributeEnum.Speed)));
            this.SetMoveSpeed((int)(Master.AttributeBonus.GetTotalAttr(AttributeEnum.MoveSpeed)));

            double magicAtt = Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt);
            double phyAtt = Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt);
            double spiritAtt = Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt);

            if (this.RuleType == RuleType.Myth)
            {
                //Debug.Log("dupulication myth PhyAtt:" + phyAtt);
            }

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, phyAtt * 0.5);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, magicAtt * rate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, spiritAtt * 0.5);

            //Debug.Log("dupulication ruleType:" + this.RuleType);
            if (!this.IsFestive)
            {

            }
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Def) * 0.2);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, 0);
            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            AttributeBonus.SetAttr(AttributeEnum.InheritIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.InheritIncrea));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHp, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHp));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHpPercent));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Miss));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Accuracy));

            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageResist));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage));

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageResist));

            if (this.Master.Camp != PlayerType.Hero)
            {
                AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Protect));
            }

            double sd = Master.AttributeBonus.GetAttackAttr(AttributeEnum.SkillDivine2010);
            if (sd > 0)
            {
                sd = sd / 100.0;

                AttributeBonus.SetAttr(AttributeEnum.DefIgnore, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DefIgnore) * sd);
                AttributeBonus.SetAttr(AttributeEnum.DefendRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DefendRate) * sd);
                AttributeBonus.SetAttr(AttributeEnum.SpRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpRate) * sd);
                AttributeBonus.SetAttr(AttributeEnum.RealHpDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealHpDamage) * sd);
                AttributeBonus.SetAttr(AttributeEnum.RealCritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealCritRate) * sd);
                AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Strong) * sd);

                AttributeBonus.SetAttr(AttributeEnum.LuckyHit, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.LuckyHit) * sd);
                AttributeBonus.SetAttr(AttributeEnum.Relic3, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic3) * sd);
                AttributeBonus.SetAttr(AttributeEnum.Relic4, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic4) * sd);
                AttributeBonus.SetAttr(AttributeEnum.Relic5, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic5) * sd);
                AttributeBonus.SetAttr(AttributeEnum.Shatter, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Shatter) * sd);
            }

            this.RingType = Master.RingType;

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

            //if (this.RuleType == RuleType.Myth || 1==1)
            //{
            //    Debug.Log("dupulication myth RealHpDamage:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealHpDamage));
            //    Debug.Log("dupulication myth SpRate:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpRate));
            //    Debug.Log("dupulication myth RealCritRate:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealCritRate));
            //}
        }

        private void SetSkill()
        {
            //加载技能
            for (int i = 0; i < Master.SelectSkillList.Count; i++)
            {
                SkillState mss = Master.SelectSkillList[i];

                if (mss.SkillPanel.SkillId != SkillPanel.SkillId && mss.SkillPanel.SkillData.SkillConfig.Type != (int)SkillType.Valet) //not loop
                {
                    SkillState skill = new SkillState(this, mss.SkillPanel, mss.FromSkill, mss.Position, 0);
                    SelectSkillList.Add(skill);

                    //if (this.RuleType == RuleType.Myth)
                    //{
                    //    Debug.Log("dup " + mss.SkillPanel.SkillData.SkillConfig.Name + ":" + mss.SkillPanel.Percent);
                    //}
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

        public override void OnHit(DamageResult dr)
        {
            //if (this.GroupId == 1 && dr.Damage > 200000000)
            //{
            //    Debug.Log("duplication player hit damage:" + StringHelper.FormatNumber(dr.Damage));
            //}

            base.OnHit(dr);
        }
    }
}
