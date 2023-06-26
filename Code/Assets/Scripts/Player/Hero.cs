using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Hero : APlayer
    {
        LevelConfig Config { get; set; }

        public Hero() : base()
        {
            this.GroupId = 1;

            this.Init();

            this.EventCenter.AddListener<HeroLevelUp>(LevelUp);
            this.EventCenter.AddListener<HeroAttrChangeEvent>(HeroAttrChange);
        }

        private void LevelUp(HeroLevelUp e)
        {
            User user = GameProcessor.Inst.User;
            this.Level = user.Level;

            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            this.Config = config;

            this.SetAttr(user);  //设置属性值
            this.Logic.SetData(null); //设置UI
        }

        public void HeroAttrChange(HeroAttrChangeEvent e)
        {
            User user = GameProcessor.Inst.User;
            this.SetAttr(user);  //设置属性值
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.Level;

            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            this.Config = config;

            this.SetAttr(user);  //设置属性值
            this.SetSkill(user); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //把用户面板属性，当做战斗的基本属性
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.HP));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.MagicAtt));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Def));
            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            AttributeBonus.SetAttr(AttributeEnum.InheritIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.InheritIncrea));

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            //加载技能
            List<SkillData> list = user.SkillList.FindAll(m => m.Status == SkillStatus.Equip).OrderBy(m => m.Position).ToList();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            //TEST
            //list.Add(new SkillData(2003, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId);
                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

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
            }
        }


    }
}
