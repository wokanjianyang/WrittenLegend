using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

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

            this.FashionId = ModelConfig.ModelType;
            this.Name = ModelConfig.Name + "(" + Master.Name + ")";

            if (this.SkillPanel.SkillId == 3012)
            {
                //白虎新继承
                this.SetAttr12();
                this.SetSkill();
            }
            else
            {
                this.SetAttr();  //设置属性值
                this.SetSkill(); //设置技能
            }

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

            double maxInheritIncrea = Math.Min(InheritIncrea, 1);
            double MaxInheritAdvance = Math.Min(InheritAdvance, 1);

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
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRate, false) * maxInheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRateResist, false) * InheritIncrea);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Lucky, false) * InheritIncrea);

            if (ModelConfig.RestorePercent > 0)
            {
                AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, ModelConfig.RestorePercent);
            }

            //队友的光环
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageIncrea, false));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageResist, false));

            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss) * MaxInheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Accuracy) * MaxInheritAdvance);

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage) * InheritAdvance);

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea) * InheritAdvance);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageResist) * MaxInheritAdvance);

            double sd = Master.AttributeBonus.GetAttackAttr(AttributeEnum.SkillDivine3010);
            if (sd > 0)
            {
                sd = sd / 100.0;

                AttributeBonus.SetAttr(AttributeEnum.DefIgnore, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DefIgnore) * sd);
                AttributeBonus.SetAttr(AttributeEnum.DefendRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DefendRate) * sd);
                AttributeBonus.SetAttr(AttributeEnum.SpRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpRate) * sd);
                AttributeBonus.SetAttr(AttributeEnum.RealHpDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealHpDamage) * sd);
                AttributeBonus.SetAttr(AttributeEnum.RealCritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealCritRate) * sd);
                AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Strong) * sd);

                AttributeBonus.SetAttr(AttributeEnum.LuckyHit, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.LuckyHit) * sd * 0.01);
                AttributeBonus.SetAttr(AttributeEnum.Relic3, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic3) * sd * 0.01);
                AttributeBonus.SetAttr(AttributeEnum.Relic4, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic4) * sd * 0.01);
                AttributeBonus.SetAttr(AttributeEnum.Relic5, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic5) * sd * 0.01);
                //Debug.Log("lucky hit:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.LuckyHit) * AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Lucky));
            }

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

            //if (this.RuleType == RuleType.Myth || 1 == 1)
            //{
            //    Debug.Log("dupulication myth RealHpDamage:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealHpDamage));
            //    Debug.Log("dupulication myth SpRate:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpRate));
            //    Debug.Log("dupulication myth RealCritRate:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealCritRate));
            //}
        }

        private void SetAttr12()
        {
            int sp = (int)(this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.SkillValetSpeed) + this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.Speed));

            this.SetAttackSpeed(ModelConfig.SpeedRate + sp);
            this.SetMoveSpeed(ModelConfig.SpeedRate + sp);

            int role = SkillPanel.SkillData.SkillConfig.Role;

            double roleAttr = Master.GetRoleAttack(role, false); //职业攻击
            double roleHp = Master.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            double roleDef = Master.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Def);

            //Debug.Log("base attr " + roleAttr + " roleHp" + roleHp + "def " + roleDef);

            double InheritIncrea = 1 + this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.InheritIncrea) / 100.0;
            double InheritAdvance = 1 + this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.InheritAdvance) / 100.0;
            double valteHp = 1 + this.Master.AttributeBonus.GetAttackAttr(AttributeEnum.SkillValetHp) / 100.0; //无极属性

            double skillRate = 1 + SkillPanel.Percent / 100;

            //Debug.Log("普通继承属性比例 InheritIncrea:" + InheritIncrea);

            //Debug.Log("高级继承属性比例 InheritAdvance:" + InheritAdvance);

            //Debug.Log("无极生命比例 SkillValetHp:" + valteHp);


            //Debug.Log("valet InheritIncrea:" + InheritIncrea);
            //Debug.Log("valet InheritAdvance:" + InheritAdvance);

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, roleHp * valteHp * InheritIncrea * InheritAdvance * skillRate * ModelConfig.HpRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, roleAttr * InheritIncrea * InheritAdvance * skillRate * ModelConfig.AttrRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, roleAttr * InheritIncrea * InheritAdvance * skillRate * ModelConfig.AttrRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, roleAttr * InheritIncrea * InheritAdvance * skillRate * ModelConfig.AttrRate / 100.0);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, roleDef * ModelConfig.DefRate / 100.0); //降低50%继承

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageIncrea, false));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageResist, false));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamage, false) * InheritAdvance); //白虎爆伤继承无极倍率
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamageResist, false));
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRate, false));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRateResist, false));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Lucky, false));

            if (ModelConfig.RestorePercent > 0)
            {
                AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, ModelConfig.RestorePercent);
            }

            //队友的光环
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageIncrea, false));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageResist, false));

            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss) + SkillPanel.Miss);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Accuracy) + SkillPanel.Accuracy);

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage));

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea) * InheritAdvance); //白虎增伤继承无极倍率
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.CalMulDamageResistAttack());

            AttributeBonus.SetAttr(AttributeEnum.DefIgnore, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DefIgnore));
            AttributeBonus.SetAttr(AttributeEnum.DefendRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DefendRate));
            AttributeBonus.SetAttr(AttributeEnum.SpRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpRate));
            AttributeBonus.SetAttr(AttributeEnum.RealHpDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealHpDamage));
            AttributeBonus.SetAttr(AttributeEnum.RealCritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RealCritRate));
            AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Strong));

            AttributeBonus.SetAttr(AttributeEnum.LuckyHit, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.LuckyHit));
            AttributeBonus.SetAttr(AttributeEnum.Relic2, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic2));
            AttributeBonus.SetAttr(AttributeEnum.Relic3, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic3));
            AttributeBonus.SetAttr(AttributeEnum.Relic4, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic4));
            AttributeBonus.SetAttr(AttributeEnum.Relic5, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Relic5));
            AttributeBonus.SetAttr(AttributeEnum.Shatter, AttributeFrom.HeroPanel, Master.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Shatter));

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

            //Debug.Log("白虎 速度:" + StringHelper.FormatNumber(ModelConfig.SpeedRate + sp));
        }

        private void SetSkill()
        {
            //加载技能
            if (this.ModelConfig.SkillList != null)
            {
                foreach (int skillId in this.ModelConfig.SkillList)
                {
                    SkillData skillData = GameProcessor.Inst.User.SkillList.Where(m => m.SkillConfig.Id == skillId).FirstOrDefault();


                    if (Master.Camp == PlayerType.Hero)
                    {
                        User user = GameProcessor.Inst.User;

                        List<SkillRuneConfig> buffRuneList = null;

                        if (skillData == null)
                        {
                            //白虎的飓风破
                            skillData = new SkillData(skillId, (int)SkillPosition.Default);
                            skillData.MagicLevel.Data = SkillPanel.Level;
                        }

                        int petRate = user.GetPetSkillRate(skillData.SkillConfig.Role);

                        SkillPanel from = null;
                        if (skillData.SkillConfig.FromId > 0)
                        {
                            SkillData fromData = user.SkillList.Where(m => m.SkillId == skillData.SkillConfig.FromId).FirstOrDefault();

                            if (fromData == null)
                            {
                                fromData = new SkillData(skillId, (int)SkillPosition.Default);
                                fromData.MagicLevel.Data = SkillPanel.Level;
                            }

                            from = new SkillPanel(fromData, user.GetRuneList(fromData.SkillId, null), user.GetSuitList(fromData.SkillId), true, RuleType, petRate);
                        }

                        List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, buffRuneList);
                        List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                        //if (skillId == 4004)
                        //{
                        //    //飓风破，使用白虎的吸血效果
                        //    SkillRune rune = user.GetRuneList(3012, null).Where(m => m.EffectId == 101).FirstOrDefault();
                        //    if (rune != null)
                        //    {
                        //        runeList.Add(rune);
                        //    }
                        //}

                        SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false, RuleType, 0);

                        SkillState skill = new SkillState(this, skillPanel, from, skillData.Position, 0);
                        SelectSkillList.Add(skill);
                    }
                    else
                    {
                        skillData = new SkillData(skillId, (int)SkillPosition.Default);
                        skillData.MagicLevel.Data = SkillPanel.Level;

                        List<SkillRune> runeList = new List<SkillRune>();
                        List<SkillSuit> suitList = new List<SkillSuit>();


                        SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                        SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                        SelectSkillList.Add(skill);
                    }
                }
            }

            if (Master.Camp == PlayerType.Hero) //继承神技-道力盾
            {
                User user = GameProcessor.Inst.User;

                SkillData skillData = user.SkillList.Where(m => m.SkillConfig.Id == 3005).FirstOrDefault();
                if (skillData != null)
                {
                    List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
                    List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                    SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false, RuleType, 0);

                    if (skillPanel.DivineLevel > 0)
                    {
                        //Debug.Log("dld Percent:" + skillPanel.Percent);
                        int dp = (int)(skillPanel.DivineAttrConfig.Param * skillPanel.DivineLevel);
                        //Debug.Log("dld dp:" + dp);
                        skillPanel.Percent = skillPanel.Percent * dp / 100;
                        //Debug.Log("dld Percent:" + skillPanel.Percent);
                        SkillState skill = new SkillState(this, skillPanel, 0, 0);
                        SelectSkillList.Insert(0, skill);
                    }
                }
            }

            if (Master.Camp == PlayerType.Hero) //继承护体戒指
            {
                User user = GameProcessor.Inst.User;
                SkillData skillData = user.SkillList.Where(m => m.SkillConfig.Id == 3010).FirstOrDefault();

                int[] ringId = { 2, 4, }; //6
                int[] skillId = { 4002, 1008 }; //3008

                if (skillData != null && skillData.GetDivineLevel() > 0)
                {
                    for (int i = 0; i < ringId.Length; i++)
                    {
                        long ringLevel = user.GetRingLevel(ringId[i]);
                        SkillData sd = new SkillData(skillId[i], 0);
                        long rp = Math.Max(1, ringLevel * skillData.GetDivineLevel() * 20 / 100);
                        sd.MagicLevel.Data = rp;

                        SkillPanel skillPanel = new SkillPanel(sd, null, null, false, RuleType, 0);
                        SkillState skill = new SkillState(this, skillPanel, 0, 0);
                        SelectSkillList.Add(skill);
                    }

                    //for (int i = 0; i < ringId.Length; i++)
                    //{
                    //    long ringLevel = user.GetRingLevel(ringId[i]);
                    //    SkillData sd = new SkillData(skillId[i], 0);
                    //    long rp = Math.Max(1, ringLevel * skillData.GetDivineLevel() * 20 / 100);
                    //    sd.MagicLevel.Data = rp;

                    //    List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
                    //    List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                    //    SkillPanel skillPanel = new SkillPanel(sd, runeList, suitList, false, RuleType, 0);

                    //    SkillState skill = new SkillState(this, skillPanel, 0, 0);
                    //    SelectSkillList.Add(skill);
                    //}
                }
            }
        }

        private void SetSkill12()
        {

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

        public override void OnHit(DamageResult dr)
        {
            //Debug.Log("valet hit damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));

            base.OnHit(dr);
        }
    }
}
