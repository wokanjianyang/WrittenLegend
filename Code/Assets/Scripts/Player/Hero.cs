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
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.Level;

            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            this.Config = config;

            this.SetAttr();  //设置属性值
            this.SetSkill(user); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Config.Def);

            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.ExpIncrea, AttributeFrom.Test, 1000);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill(User user)
        {
            //加载技能
            List<SkillData> list = user.SkillList.FindAll(m => m.Status == SkillStatus.Equip).OrderBy(m => m.Position).ToList();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {

                List<SkillRune> runeList = GetRuneList(skillData.SkillId);
                List<SkillSuit> suitList = GetSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }
    }
}
