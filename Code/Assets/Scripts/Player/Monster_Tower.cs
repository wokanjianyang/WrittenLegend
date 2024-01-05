using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monster_Tower : APlayer
{
    public long Floor { get; }
    public int Index { get; }

    public TowerConfig config { get; set; }

    private bool isEquip = false;
    private bool isSepecialEquip = false;

    public Monster_Tower(long floor, int index)
    {
        this.Floor = floor;
        this.GroupId = 2;
        this.Index = index;

        if (floor % 1000 == 0)
        {
            isSepecialEquip = true;
        }
        else if (floor % 30 == 0)
        {
            isEquip = true;
        }

        this.Init();
    }

    private void Init()
    {
        TowerConfig config = TowerConfigCategory.Instance.GetByFloor(Floor);
        this.config = config;

        this.Camp = PlayerType.Enemy;

        if (isSepecialEquip)
        {
            this.Name = "守关将军";
        }
        else if (isEquip)
        {
            this.Name = "守关亲卫";
        }
        else
        {
            this.Name = "守关小兵";
        }


        this.Level = Floor;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();
        //if (config.SkillIdList.Length > 0)
        //{
        //    int i = this.Index % config.SkillIdList.Length;
        //    SkillData skill = new SkillData(config.SkillIdList[i], 1);
        //    SkillData skill = new SkillData(10004, i); //测试技能代码
        //    skill.SkillConfig.CD = 2;
        //    skill.Status = SkillStatus.Equip;
        //    skill.Position = 1;
        //    skill.Level = 1;
        //    SkillList.Add(skill);
        //}
        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        if (isEquip)
        {
            list.Add(new SkillData(12002, 2)); //增加怪物雷电术
        }
        else if (isSepecialEquip)
        {
            list.Add(new SkillData(11002, 1)); //怪物刺杀
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

    private void SetAttr()
    {
        long rise = Floor - config.StartLevel;

        long attr = config.StartAttr + (long)(rise * config.RiseAttr);
        long hp = config.StartHp + (long)(rise * config.RiseHp);
        long def = config.StartDef + (long)(rise * config.RiseDef);

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, config.DamageIncrea);
        //AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, config.DamageResist);
        //AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, config.CritRate);
        //AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, config.CritDamage);


        //回满当前血量
        SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));

        if (Floor % 30 == 0)
        { //每30层,掉落装备一件(),每1000层一件4件套

        }
    }
}
