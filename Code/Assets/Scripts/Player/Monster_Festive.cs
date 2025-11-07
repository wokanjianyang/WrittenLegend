using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Festive : APlayer
{
    MonsterFestiveConfig config;

    private double[] HpRateist = { 1, 1.2, 1.4, 1.6, 2 };
    private double[] AttrRateist = { 1, 1.1, 1.2, 1.3, 1.5 };
    private double[] DefRateist = { 1, 1.1, 1.15, 1.2, 1.25 };


    public Monster_Festive(int mapId, int quality)
    {
        this.GroupId = 2;
        this.RuleType = RuleType.Myth;
        this.Quality = quality;

        config = MonsterFestiveConfigCategory.Instance.Get(mapId);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.MonsterName;
        this.Level = config.Id * 10 + this.Quality;
        this.ModelType = MondelType.Nomal;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();


        for (int i = 0; i < config.SkillIdList.Length; i++)
        {
            int skillId = config.SkillIdList[i];
            SkillData skillData = new SkillData(skillId, i);
            skillData.MagicLevel.Data = config.Id * 10;
            list.Add(skillData);
        }

        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillConfig.Id, 4);
            List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillConfig.Id, 4);

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false, RuleType.Normal, 0);

            //Debug.Log(skillData.SkillConfig.Name + " Percent  :" + skillPanel.Percent);
            //Debug.Log(skillData.SkillConfig.Name + " Damage  :" + skillPanel.Damage);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {
        double HpRate = HpRateist[this.Quality - 1];
        double attrRate = AttrRateist[this.Quality - 1]; ;
        double defRate = DefRateist[this.Quality - 1]; ;

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Double.Parse(config.HP) * HpRate);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr) * attrRate);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr) * attrRate);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr) * attrRate);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Double.Parse(config.Def) * defRate);

        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, config.DamageIncrea);
        AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, config.DamageResist);
        AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, config.CritRateResist);
        AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroBase, config.CritDamageResist);
        //AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroBase, config.ResotrePercent);
        AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, config.Protect);

        this.SetAttackSpeed(config.Speed);
        this.SetMoveSpeed(config.Speed);

        Debug.Log("hp:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
        Debug.Log("attr:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.PhyAtt));
        Debug.Log("def:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Def));

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

}
