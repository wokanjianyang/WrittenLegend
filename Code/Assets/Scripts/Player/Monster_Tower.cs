using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Tower : APlayer
{
    public int TowerId { get; }
    public int Index { get; }

    public TowerConfig config { get; set; }

    public Monster_Tower(int floor,int index) {
        this.TowerId = floor;
        this.GroupId = 2;
        this.Index = index;
        Load();
    }

    public override void Load()
    {
        TowerConfig config = TowerConfigCategory.Instance.Get(TowerId);
        this.config = config;

        this.Camp = PlayerType.Enemy;
        this.Name = config.Name;

        //加载技能
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

        base.Load();

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

        Logic.SetData(null);
    }


}
