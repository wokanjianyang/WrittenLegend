using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Infinite : APlayer
    {
        public int Progress;
        InfiniteConfig Config { get; set; }
        InfiniteModelConfig ModelConfig { get; set; }

        private double[] HpRate = { 1, 2.5, 5, 7.5, 10 };
        private double[] DefRate = { 1, 2, 3, 4, 5 };
        private double[] AttrRate = { 1, 1.25, 1.5, 1.75, 2 };
        private int[] excludeSuitList = { 6 };

        public Monster_Infinite(long progress, int quality) : base()
        {
            this.Progress = (int)progress;
            this.GroupId = 2;
            this.Quality = quality;
            this.RuleType = RuleType.Infinite;

            this.Config = InfiniteConfigCategory.Instance.GetByLevel(progress);

            this.ModelConfig = InfiniteModelConfigCategory.Instance.RandomConfig();

            this.Init();
        }

        private void Init()
        {
            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(this.Quality);

            this.Camp = PlayerType.Enemy;
            this.Name = ModelConfig.Name + qualityConfig.MonsterTitle;
            this.Level = Progress * 100;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseLevel = this.Progress - Config.StartLevel;

            double hp = StringHelper.StringToNumber(Config.HP);
            //Debug.Log("Config " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));
            hp += hp * Config.HpRise * riseLevel;

            double attr = StringHelper.StringToNumber(Config.Attr);
            //Debug.Log("Config " + this.Progress + " Attr:" + StringHelper.FormatNumber(attr));
            attr += attr * Config.AttrRise * riseLevel;

            double def = StringHelper.StringToNumber(Config.Def);
            //Debug.Log("Config " + this.Progress + " Def:" + StringHelper.FormatNumber(def));
            def += def * Config.DefRise * riseLevel;

            double damageMul = StringHelper.StringToNumber(Config.DamageMul);
            damageMul += damageMul * Config.MulRise * riseLevel;

            double strong = StringHelper.StringToNumber(Config.Strong);
            strong += strong * Config.StrongRise * riseLevel;

            double parry = StringHelper.StringToNumber(Config.Parry);
            parry += parry * Config.ParrayRise * riseLevel;

            //Debug.Log("Infinit " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));
            //Debug.Log("Infinit " + this.Progress + " Def:" + StringHelper.FormatNumber(def));
            //Debug.Log("Infinit " + this.Progress + " Attr:" + StringHelper.FormatNumber(attr));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * HpRate[Quality - 1]);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * AttrRate[Quality - 1]);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * AttrRate[Quality - 1]);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * AttrRate[Quality - 1]);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * DefRate[Quality - 1]);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroBase, Config.MulDamageResist);
            AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);

            AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);

            AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroBase, parry);

            SetAttackSpeed(Config.Speed);
            SetMoveSpeed(Config.Speed);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            List<int> rdList = SkillConfigCategory.Instance.RandomList(Quality, this.Progress);

            for (int i = 0; i < rdList.Count; i++)
            {
                int skillId = rdList[i];
                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = this.Progress * skillData.SkillConfig.MaxLevel / 100;
                list.Add(skillData);
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //add default skill

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, this.Quality);
                List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillId, this.Quality, excludeSuitList);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                SkillPanel from = null;
                if (skillPanel.SkillData.SkillConfig.FromId > 0)
                {
                    SkillData fromData = new SkillData(skillPanel.SkillData.SkillConfig.FromId, 0);

                    if (fromData == null)
                    {
                        continue;
                    }

                    from = new SkillPanel(fromData, SkillRuneConfigCategory.Instance.GetAllRune(fromData.SkillId, this.Quality), SkillSuitHelper.GetAllSuit(skillData.SkillId, this.Quality), false);
                }

                SkillState skill = new SkillState(this, skillPanel, from, skillData.Position, 0);
                SelectSkillList.Add(skill);

                //职业专精技能的属性
                if (skillData.SkillConfig.Type == (int)SkillType.Expert)
                {
                    int attrKey = (int)AttributeFrom.Skill * 10000 + skillData.SkillId;

                    if (skillData.SkillConfig.Role == (int)RoleType.Warrior)
                    {
                        AttributeBonus.SetAttr(AttributeEnum.WarriorSkillPercent, attrKey, skillPanel.Percent);
                        AttributeBonus.SetAttr(AttributeEnum.WarriorSkillDamage, attrKey, skillPanel.Damage);
                    }
                    else if (skillData.SkillConfig.Role == (int)RoleType.Mage)
                    {
                        AttributeBonus.SetAttr(AttributeEnum.MageSkillPercent, attrKey, skillPanel.Percent);
                        AttributeBonus.SetAttr(AttributeEnum.MageSkillDamage, attrKey, skillPanel.Damage);
                    }
                    else if (skillData.SkillConfig.Role == (int)RoleType.Warlock)
                    {
                        AttributeBonus.SetAttr(AttributeEnum.WarlockSkillPercent, attrKey, skillPanel.Percent);
                        AttributeBonus.SetAttr(AttributeEnum.WarlockSkillDamage, attrKey, skillPanel.Damage);
                    }
                }
                else if (skillData.SkillId == 3010)
                {
                    AttributeBonus.SetAttr(AttributeEnum.InheritAdvance, AttributeFrom.Skill, skillPanel.Percent);
                    AttributeBonus.SetAttr(AttributeEnum.SkillValetHp, AttributeFrom.Skill, skillPanel.Damage);
                }
            }

            base.SetSkillAfter();
        }
    }
}
