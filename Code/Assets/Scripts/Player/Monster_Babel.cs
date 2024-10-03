using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Babel : APlayer
    {
        public int Progeress;
        public int Type;

        MonsterBabelConfig MonsterConfig { get; set; }

        public Monster_Babel(long progress, int type) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.Babel;
            this.Quality = type + 2;

            this.Progeress = (int)progress;
            this.Type = type;

            this.MonsterConfig = MonsterBabelConfigCategory.Instance.GetByProgressAndType(progress, type);

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = MonsterConfig.Name;
            this.Level = Progeress;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }


        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int p1 = Math.Min(this.Progeress, 10000);
            //int riseLevel = this.Progeress - Config.Start;
            double riseRate = Math.Pow(1.003, p1);

            int p2 = this.Progeress - 10000;
            if (p2 > 0)
            {
                riseRate *= Math.Pow(1.005, p2);
            }
            //Debug.Log("riseRate:" + riseRate);

            riseRate *= MonsterConfig.AttrRate;

            long day = (TimeHelper.ClientNowSeconds() - 1724083200) / 86400;
            day = Math.Min(day, 60);
            double dayRate = Math.Pow(0.95, day);
            //Debug.Log("dayRate:" + dayRate);

            double hp = 999000000000000000000000.0 * dayRate;
            double attr = 300000000000.0 * dayRate;
            double def = 100000000000000.0 * dayRate;

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * riseRate);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Progeress * 0.1);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Progeress * 0.05);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Progeress * 0.05);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Progeress * 0.1);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Progeress * 0.005);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Progeress * 0.005);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            int[] SkillIdList = MonsterConfig.SkillIdList;
            for (int i = 0; i < SkillIdList.Length; i++)
            {
                int skillId = SkillIdList[i];

                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = skillData.SkillConfig.MaxLevel;
                list.Add(skillData);
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //add default skill

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, 4);
                List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillId, 4);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
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
        }
    }
}
