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

            int riseLevel = Progress - Config.StartLevel;

            double riseHp = Math.Pow(Config.RiseHp, riseLevel);
            double riseAttr = Math.Pow(Config.RiseAttr, riseLevel);
            double riseDef = Math.Pow(Config.RiseDef, riseLevel);
            double riseStrong = Math.Pow(Config.RiseStrong, riseLevel);
            double riseMul = Math.Pow(Config.MulRise, riseLevel);
            double riseMiss = Config.RiseMiss * riseLevel;
            double RiseAccuracy = Config.RiseAccuracy * riseLevel;
            double riseParry = Math.Pow(Config.ParryRise, riseLevel);
            //Debug.Log("pw:" + (Progress - Config.StartLevel));

            //if (Progress >= 100)
            //{
            //    Debug.Log("RiseHp:" + riseHp);
            //    Debug.Log("riseAttr:" + riseAttr);
            //    Debug.Log("riseDef:" + riseDef);
            //}

            double hp = StringHelper.StringToNumber(Config.HP) * (1 + riseHp);
            double attr = StringHelper.StringToNumber(Config.Attr) * (1 + riseAttr);
            double def = StringHelper.StringToNumber(Config.Def) * (1 + riseDef);

            double strong = StringHelper.StringToNumber(Config.Strong) * (1 + riseStrong);
            double damageMul = StringHelper.StringToNumber(Config.DamageMul) * (1 + riseMul);
            double parry = StringHelper.StringToNumber(Config.Parry) * (1 + riseParry);
            //if (Progress >= 100)
            //{
            //    Debug.Log("hp:" + hp);
            //    Debug.Log("attr:" + attr);
            //    Debug.Log("def:" + def);
            //}
            //Debug.Log("Defend " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * QualityConfig.HpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroBase, Config.MulDamageResist);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy + RiseAccuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss + riseMiss);
            AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);

            AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);
            AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroBase, parry);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

            //回满当前血量
            this.SetAttackSpeed(Config.Speed);
            this.SetMoveSpeed(Config.Speed);
        }



        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            if (this.Layer >= 6)
            {
                int mapId = 0;
                if (this.Layer == 7)
                {
                    mapId = 1176;
                }

                List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Quality >= 5 && m.StartMapId >= mapId).ToList();
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
            else if (Quality + this.Layer >= 6)
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

        public override float AttackLogic()
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            if (hero != null)
            {
                this._enemy = hero;
            }

            return base.AttackLogic();
        }
    }
}
