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
        private int Record = 0;

        MonsterBabelConfig MonsterConfig { get; set; }

        public Monster_Babel(long progress, int type) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.Babel;
            this.Quality = type + 2;

            this.Progeress = (int)progress;
            this.Type = type;
            this.Record = AppHelper.BabelRecord;

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

            double riseMiss = 0;
            double riseAccuracy = 0;

            double riseRate = 1;
            double riseHpRate = 1;
            double riseStrong = 1;
            double riseMulAttr = 1;
            double riseParry = 1;
            double riseShatterError = 1;
            double riseShatter = 1;

            if (Progeress >= 55000)
            {
                riseRate *= 1E30; //守关难度关卡，开新难度去掉
            }

            if (Progeress >= 50000)
            {
                riseParry *= 10000; // * Math.Pow(1.01, Progeress - 50000)
                riseShatter *= 10000 * Math.Pow(1.01, Progeress - 50000);
            }

            if (Progeress > 45000)
            {
                riseParry *= Math.Pow(1.02, Progeress - 45000);

                int sep = Math.Min(50000, this.Progeress);
                riseShatterError *= Math.Pow(1.02, sep - 45000);

                riseStrong *= Math.Pow(1.01, Progeress - 45000);
                riseMulAttr *= Math.Pow(1.01, Progeress - 45000) * 1000;
                riseMiss += 180;
                riseAccuracy += 360;
            }

            if (Progeress > 40000)
            {
                riseMulAttr *= Math.Pow(1.01, Progeress - 40000);
            }

            if (Progeress > 35000)
            {
                riseRate *= Math.Pow(1.009, Progeress - 35000);
                riseHpRate *= Math.Pow(1.012, Progeress - 35000);

                riseStrong *= Math.Pow(1.01, Progeress - 35000);
            }

            if (Progeress > 30000)
            {
                riseRate *= Math.Pow(1.009, Math.Min(Progeress, 35000) - 30000);
                riseHpRate *= Math.Pow(1.009, Math.Min(Progeress, 35000) - 30000);
            }

            if (Progeress > 15000)
            {
                riseRate *= Math.Pow(1.007, Math.Min(Progeress, 30000) - 15000);
                riseHpRate *= Math.Pow(1.007, Math.Min(Progeress, 30000) - 15000);
            }

            if (Progeress > 10000)
            {
                riseRate *= Math.Pow(1.005, Math.Min(Progeress, 15000) - 10000);
                riseHpRate *= Math.Pow(1.005, Math.Min(Progeress, 15000) - 10000);
            }

            riseRate *= Math.Pow(1.003, Math.Min(Progeress, 10000));
            riseHpRate *= Math.Pow(1.003, Math.Min(Progeress, 10000));

            if (Record > 0 && this.Progeress + 1000 < Record)
            {
                int lose = Record - this.Progeress - 1000;

                double loseRate = Math.Max(Math.Pow(0.997, lose), 0.0000001);
                //Debug.Log("loseRate:" + loseRate);

                riseRate *= loseRate;
                riseHpRate *= loseRate;
            }
            //Debug.Log(this.Progeress + " riseRate:" + riseRate);

            riseRate *= MonsterConfig.AttrRate;

            double hp = 999000000000000000000000.0;
            double attr = 300000000000.0;
            double def = 100000000000000.0;
            double strong = 10000 * riseStrong;
            double mulAtt = 10000;
            double parray = 10000;
            double shatter = 10000;


            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * riseHpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * riseRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * riseRate);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Progeress * 0.1);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Progeress * 0.05);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Progeress * 0.05);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Progeress * 0.1);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, riseAccuracy + Progeress * 0.005);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, riseMiss + Progeress * 0.005);

            AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, 90);


            if (this.Progeress > 40000)
            {
                AttributeBonus.SetAttr(AttributeEnum.MulAttr, AttributeFrom.HeroBase, mulAtt * riseMulAttr);
            }
            if (this.Progeress > 45000)
            {
                AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroBase, parray * riseParry);
                strong = strong / (1 + 100 * riseShatterError);
            }
            if (this.Progeress > 35000)
            {
                AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
            }
            if (this.Progeress >= 50000)
            {
                AttributeBonus.SetAttr(AttributeEnum.Shatter, AttributeFrom.HeroBase, shatter * riseShatter);
            }

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

            int speed = (this.Progeress - 30000) / 5000;

            this.SetAttackSpeed(speed * 50 + 100);
            this.SetMoveSpeed(speed * 50 + 100);

            //Debug.Log("MISS:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Miss));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            List<int> SkillIdList = MonsterConfig.SkillIdList.ToList();

            for (int i = 0; i < SkillIdList.Count; i++)
            {
                int skillId = SkillIdList[i];

                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = skillData.SkillConfig.MaxLevel;
                list.Add(skillData);
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //add default skill

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, MonsterConfig.RuneCount);
                List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillId, MonsterConfig.RuneCount);

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

            base.SetSkillAfter();
        }


        public override void OnHit(DamageResult dr)
        {
            //if (this.Quality == 3)
            //{
            //    Debug.Log("monster babel " + this.Progeress + " hit damage:" + StringHelper.FormatNumber(dr.Damage));
            //}

            base.OnHit(dr);
        }
    }
}
