using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Infinite : APlayer
    {
        public int Progeress;
        InfiniteConfig Config { get; set; }
        QualityConfig QualityConfig { get; set; }

        public Monster_Infinite(long progress, int quality) : base()
        {
            this.Progeress = (int)progress;
            this.GroupId = 2;
            this.Quality = quality;

            this.Config = InfiniteConfigCategory.Instance.GetByLevel(progress);
            this.QualityConfig = QualityConfigCategory.Instance.Get(this.Quality);

            this.Init();
        }

        private void Init()
        {
            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(this.Quality);

            this.Camp = PlayerType.Enemy;
            this.Name = "无尽守卫" + qualityConfig.MonsterTitle;
            this.Level = Progeress * 100;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hp = Double.Parse(Config.HP);
            double attr = Double.Parse(Config.Attr);
            double def = Double.Parse(Config.Def);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * QualityConfig.HpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * QualityConfig.DefRate);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroBase, Config.MulDamageResist);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            List<int> rdList = SkillConfigCategory.Instance.RandomList(Quality);

            for (int i = 0; i < rdList.Count; i++)
            {
                int skillId = rdList[i];
                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = this.Progeress;
                list.Add(skillData);
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //add default skill

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneHelper.GetAllRune(skillData.SkillId, this.Quality);
                List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillId, this.Quality);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }
    }
}
