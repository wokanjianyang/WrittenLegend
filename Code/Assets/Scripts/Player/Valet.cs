using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Valet : APlayer
    {
        public APlayer Master { get; set; }
        private SkillPanel CallSkill { get; set; }

        public Valet(APlayer player,SkillPanel skill) : base()
        {
            this.GroupId = player.GroupId;
            this.Master = player;
            this.CallSkill = skill;

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Valet;
            this.Level = CallSkill.SkillData.Level;
            this.Name = "骷髅(" + Master.Name + ")";

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            long roleAttr = Master.GetRoleAttack(CallSkill.SkillData.SkillConfig.Role);
            long baseAttr = roleAttr * (CallSkill.Percent + Master.AttributeBonus.GetTotalAttr(AttributeEnum.InheritIncrea)) / 100 + CallSkill.Damage;

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
