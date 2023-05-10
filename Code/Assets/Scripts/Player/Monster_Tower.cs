using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Tower : APlayer
{
    public int TowerId { get; }
    public int Index { get; }

    public TowerConfig config { get; set; }

    public Monster_Tower(int floor, int index)
    {
        this.TowerId = floor;
        this.GroupId = 2;
        this.Index = index;

        this.Init();
    }

    private void Init()
    {
        TowerConfig config = TowerConfigCategory.Instance.Get(TowerId);
        this.config = config;

        this.Camp = PlayerType.Enemy;
        this.Name = config.Name;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();
        if (config.SkillIdList.Length > 0)
        {
            int i = this.Index % config.SkillIdList.Length;
            SkillData skill = new SkillData(config.SkillIdList[i], 1);
            //SkillData skill = new SkillData(10004, i); //测试技能代码
            skill.SkillConfig.CD = 2;
            skill.Status = SkillStatus.Equip;
            skill.Position = 1;
            skill.Level = 1;
            SkillList.Add(skill);
        }
        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = new List<SkillRune>();
            List<SkillSuit> suitList = new List<SkillSuit>();

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {
        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);
    }
}
