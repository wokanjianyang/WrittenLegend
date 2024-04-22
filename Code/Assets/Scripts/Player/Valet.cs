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
            this.RuleType = player.RuleType;

            this.Init();

            User user = GameProcessor.Inst.User;

            user.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateAllSkillEvent);
        }

        private void Init()
        {
            this.Camp = PlayerType.Valet;
            this.Level = SkillPanel.SkillData.MagicLevel.Data;

            this.ModelConfig = ValetModelConfigCategory.Instance.GetAll().Values.Where(m => m.FromSkillId == SkillPanel.SkillId).FirstOrDefault();

            this.ModelType = (MondelType)ModelConfig.ModelType;
            this.Name = ModelConfig.Name + "(" + Master.Name + ")";

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            int sp = (int)this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.SkillValetSpeed);

            this.SetAttackSpeed(ModelConfig.SpeedRate + sp);

            int role = SkillPanel.SkillData.SkillConfig.Role;

            double roleAttr = Master.GetRoleAttack(role, false) * (100.0 + SkillPanel.AttrIncrea) / 100.0; //职业攻击

            double InheritIncrea = (SkillPanel.InheritIncrea + ModelConfig.AdvanceRate) / 100.0;
            double InheritAdvance = this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.InheritAdvance) / 100.0;
            double valteHp = 1 + this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.SkillValetHp) / 100.0;

            //Debug.Log("valet InheritIncrea:" + InheritIncrea);
            //Debug.Log("valet InheritAdvance:" + InheritAdvance);

            //技能系数
            double baseAttr = roleAttr * (SkillPanel.Percent + Master.GetRolePercent(role) + InheritIncrea) / 100 + SkillPanel.Damage + Master.GetRoleDamage(role);  // *百分比系数 + 固定数值

            double pr = RuleType == RuleType.HeroPhantom ? ConfigHelper.PvpRate : 1;

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, baseAttr * ModelConfig.HpRate * pr * valteHp / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, baseAttr * ModelConfig.AttrRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, baseAttr * ModelConfig.AttrRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, baseAttr * ModelConfig.AttrRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, baseAttr * ModelConfig.DefRate / 100.0); //降低50%继承

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageIncrea, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageResist, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamage, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamageResist, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRate, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRateResist, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Lucky, false) * InheritIncrea);

            if (ModelConfig.RestorePercent > 0)
            {
                AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, ModelConfig.RestorePercent);
            }

            //队友的光环
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageIncrea, false));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageResist, false));

            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Accuracy) * InheritAdvance);

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage) * InheritAdvance);

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageResist) * InheritAdvance);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            if (this.ModelConfig.SkillList != null)
            {
                foreach (int skillId in this.ModelConfig.SkillList)
                {
                    SkillData skillData = GameProcessor.Inst.User.SkillList.Where(m => m.SkillConfig.Id == skillId).FirstOrDefault();

                    if (skillData != null && Master.Camp == PlayerType.Hero)
                    {
                        User user = GameProcessor.Inst.User;

                        List<SkillRuneConfig> buffRuneList = null;
                        if (RuleType == RuleType.Defend)
                        {
                            buffRuneList = user.DefendData.GetBuffRuneList(skillData.SkillId);
                        }

                        List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, buffRuneList);
                        List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

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
        }

        private void OnHeroUpdateAllSkillEvent(HeroUpdateSkillEvent e)
        {
            this.SetSkill();
        }

        //public override APlayer CalcEnemy()
        //{
        //    //攻击主人的目标
        //    var mm = this.Master.CalcEnemy();

        //    return mm != null ? mm : base.CalcEnemy();
        //}
    }
}
