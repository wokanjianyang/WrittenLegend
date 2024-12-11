using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_DefendNew : APlayer
    {
        public int Progress;
        DefendConfig Config { get; set; }
        QualityConfig QualityConfig { get; set; }

        private int Layer = 0;

        public Monster_DefendNew(int layer, long progess, int quality) : base()
        {
            this.GroupId = 2;
            this.Layer = layer;
            this.Progress = (int)progess;
            this.Quality = quality;


            this.Config = DefendConfigCategory.Instance.GetByLayerAndLevel(this.Layer, this.Progress);

            this.QualityConfig = QualityConfigCategory.Instance.Get(this.Quality);

            this.Init();
            //this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = Config.Name + QualityConfig.MonsterTitle;
            this.Level = Layer * 100 + this.Progress;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double riseHp = Math.Pow(Config.RiseHp, Progress - Config.StartLevel);
            double riseAttr = Math.Pow(Config.RiseAttr, Progress - Config.StartLevel);
            double riseDef = Math.Pow(Config.RiseDef, Progress - Config.StartLevel);
            //Debug.Log("pw:" + (Progress - Config.StartLevel));
            //Debug.Log("RiseHp:" + riseHp);
            //Debug.Log("riseAttr:" + riseAttr);
            //Debug.Log("riseDef:" + riseDef);

            double hp = Double.Parse(Config.HP) * (1 + riseHp);
            double attr = Double.Parse(Config.Attr) * (1 + riseAttr);
            double def = Double.Parse(Config.Def) * (1 + riseDef);

            //Debug.Log("Defend " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * QualityConfig.HpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * QualityConfig.DefRate);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroBase, Config.MulDamageResist);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss);
            AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }



        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            if (Quality + this.Layer >= 6)
            {
                //random model
                List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.StartMapId == 0).ToList();
                int index = RandomHelper.RandomNumber(0, models.Count);
                PlayerModel model = models[index];

                if (model.SkillList != null)
                {
                    for (int i = 0; i < model.SkillList.Length; i++)
                    {
                        list.Add(new SkillData(model.SkillList[i], i)); //增加默认技能
                    }
                }

                this.Name = model.Name + "·" + Config.Name;
            }

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }
    }
}
