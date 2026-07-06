using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Babel_Myth : APlayer
    {
        public int Progeress;
        public int Record;

        MonsterBabelMythConfig Config { get; set; }

        public Monster_Babel_Myth(long progress, int type) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.Babel;
            this.Quality = 2 + type;

            this.Progeress = (int)progress;
            this.Record = AppHelper.BabelMythRecord;

            this.Config = MonsterBabelMythConfigCategory.Instance.GetByProgress(progress);

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = "穷奇";
            this.Level = Progeress;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseProgres = this.Progeress - Config.StartLevel;

            double riseHp = Math.Pow(Config.HpRise, riseProgres);
            double riseDef = Math.Pow(Config.DefRise, riseProgres);
            double riseAtk = Math.Pow(Config.AttrRise, riseProgres);

            double hp = StringHelper.StringToNumber(Config.HP);
            double attr = StringHelper.StringToNumber(Config.Attr);
            double def = StringHelper.StringToNumber(Config.Def);


            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * riseHp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * riseAtk);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * riseAtk);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * riseAtk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * riseDef);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, Config.CritRateResist);
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroBase, Config.CritDamageResist);
            AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);


            //回满当前血量
            SetMaxHp();

            //Debug.Log("MISS:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Miss));
        }

        private void SetSkill()
        {
            int[] SkillRate = { 100, 10, 20 };

            int[][] SkillPlan = new int[][]
           {
            new int[] { 2002, 2008  },
            new int[] { 1002, 1004 },
            new int[] { 1012 }
           };

            int[] SkillList = new int[] { 2007, 3008 };

            //加载技能
            List<SkillData> list = new List<SkillData>();

            List<int> IdList = new List<int>();

            //先随机一个方案
            int p = RandomHelper.RandomNumber(0, SkillPlan.Length);
            int[] plan = SkillPlan[p];

            //if (Quality == 5)
            //{
            //    plan = SkillPlan[2];
            //}

            for (int i = 0; i < plan.Length; i++)
            {
                IdList.Add(plan[i]);
            }

            if (Quality >= 4 || RandomHelper.RandomRate(3)) //首领以上必随机一个技能，首领以下1/3概率随机一个技能
            {
                int rd = RandomHelper.RandomNumber(0, SkillList.Length);
                IdList.Add(SkillList[rd]);
            }

            //if (Quality >= 4)
            //{
            //    Debug.Log("skill id list :" + JsonConvert.SerializeObject(IdList, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }));
            //}

            for (int i = 0; i < IdList.Count; i++)
            {
                int skillId = IdList[i];
                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = 1;
                list.Add(skillData);
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillConfig.Id, 4);
                List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillConfig.Id, 99);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false, RuleType.Normal, 0);

                //Debug.Log(skillData.SkillConfig.Name + " Percent  :" + skillPanel.Percent);
                //Debug.Log(skillData.SkillConfig.Name + " Damage  :" + skillPanel.Damage);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }


        public override void OnHit(DamageResult dr)
        {
            //if (this.Quality == 3)
            //{
            //    Debug.Log("monster babel myth " + this.Progeress + " hit damage:" + StringHelper.FormatNumber(dr.Damage));
            //}

            base.OnHit(dr);
        }
    }
}
