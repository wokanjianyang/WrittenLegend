using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    public class Valet : APlayer
    {
        public APlayer Master { get; set; }
        private SkillPanel SkillPanel { get; set; }

        private ValetModelConfig ModelConfig { get; set; }
        public Valet(APlayer player, SkillPanel skill) : base()
        {
            this.GroupId = player.GroupId;
            this.Master = player;
            this.SkillPanel = skill;

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Valet;
            this.Level = SkillPanel.SkillData.MagicLevel.Data;

            this.ModelConfig = ValetModelConfigCategory.Instance.GetAll().Values.Where(m => m.FromSkillId == SkillPanel.SkillId).FirstOrDefault();

            this.Name = ModelConfig.Name + "(" + Master.Name + ")";

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.SetAttackSpeed(ModelConfig.SpeedRate);

            int role = SkillPanel.SkillData.SkillConfig.Role;

            long roleAttr = Master.GetRoleAttack(role) * (100 + SkillPanel.AttrIncrea) / 100; //职业攻击

            int InheritIncrea = SkillPanel.InheritIncrea + ModelConfig.AdvanceRate;

            //技能系数
            long baseAttr = roleAttr * (SkillPanel.Percent + Master.GetRolePercent(role) + InheritIncrea) / 100 + SkillPanel.Damage + Master.GetRoleDamage(role);  // *百分比系数 + 固定数值

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, baseAttr * ModelConfig.HpRate / 100);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, baseAttr * ModelConfig.AttrRate / 100);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, baseAttr * ModelConfig.AttrRate / 100);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, baseAttr * ModelConfig.AttrRate / 100);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, baseAttr * ModelConfig.DefRate / 100); //降低50%继承

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea) * InheritIncrea / 100);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist) * InheritIncrea / 100);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage) * InheritIncrea / 100);
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist) * InheritIncrea / 100);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate) * InheritIncrea / 100);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist) * InheritIncrea / 100);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky) * InheritIncrea / 100);

            if (ModelConfig.RestorePercent > 0) {
                AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, ModelConfig.RestorePercent);
            }

            //队友的光环
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageResist));


            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            if (this.ModelConfig.SkillList != null)
            {
                foreach (int skillId in this.ModelConfig.SkillList)
                {
                    SkillData skillData = GameProcessor.Inst.User.SkillList.Where(m => m.SkillConfig.Id == skillId).FirstOrDefault();

                    if (skillData != null && Master.Camp == PlayerType.Hero)
                    {
                        List<SkillRune> runeList = GameProcessor.Inst.User.GetRuneList(skillData.SkillId);
                        List<SkillSuit> suitList = GameProcessor.Inst.User.GetSuitList(skillData.SkillId);

                        SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                        SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                        SelectSkillList.Add(skill);
                    }
                    else
                    {
                        skillData = new SkillData(skillId, (int)SkillPosition.Default);

                        List<SkillRune> runeList = new List<SkillRune>();
                        List<SkillSuit> suitList = new List<SkillSuit>();

                        SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                        SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                        SelectSkillList.Add(skill);
                    }
                }
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能
        }

        //public override APlayer CalcEnemy()
        //{
        //    //攻击主人的目标
        //    var mm = this.Master.CalcEnemy();

        //    return mm != null ? mm : base.CalcEnemy();
        //}
    }
}
