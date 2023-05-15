using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Valet : APlayer
    {
        public APlayer Master { get; set; }
        private SkillPanel SkillPanel { get; set; }

        public Valet(APlayer player,SkillPanel skill) : base()
        {
            this.GroupId = player.GroupId;
            this.Master = player;
            this.SkillPanel = skill;

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Valet;
            this.Level = SkillPanel.SkillData.Level;
            this.Name = "骷髅(" + Master.Name + ")";

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            int role = SkillPanel.SkillData.SkillConfig.Role;

            long roleAttr = Master.GetRoleAttack(role) * (100 + SkillPanel.AttrIncrea) / 100; //职业攻击

            //技能系数
            long baseAttr = roleAttr * (SkillPanel.Percent + Master.GetRolePercent(role)) / 100 + SkillPanel.Damage + Master.GetRoleDamage(role);  // *百分比系数 + 固定数值

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, baseAttr * 10);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, baseAttr);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, baseAttr);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, baseAttr);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, baseAttr / 2);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }
    }
}
