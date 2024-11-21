using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Myth : APlayer
{
    MonsterMythConfig config;

    int MapId = 0;

    public Monster_Myth(int mapId, int layer)
    {
        this.GroupId = 2;
        this.MapId = mapId;
        this.RuleType = RuleType.Myth;

        config = MonsterMythConfigCategory.Instance.GetByMapIdAndLayer(mapId, layer);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = " 幻境魔物";
        this.Level = 1000;
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


        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillConfig.Id, 4);
            List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillConfig.Id, 4);

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {
        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Double.Parse(config.HP));
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr));
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr));
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr));
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Double.Parse(config.Def));

        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, config.DamageIncrea);
        AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, config.DamageResist);
        AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, config.CritRateResist);
        AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroBase, config.ResotrePercent);

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

}
