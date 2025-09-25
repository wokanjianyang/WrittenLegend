using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_World : APlayer
{
    MonsterWorldConfig Config;

    private int Step = 1;

    public Monster_World(int mapId, long level, int step)
    {
        this.GroupId = 2;
        this.RuleType = RuleType.World;
        this.FashionId = mapId;
        this.Level = level;
        this.Step = step;

        //Debug.Log("MonsterWorldConfig：" + mapId + " - " + step);

        Config = MonsterWorldConfigCategory.Instance.GetByMapIdAndStep(mapId, step);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = Config.MonsterName;
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


        for (int i = 0; i < Config.SkillIdList.Length; i++)
        {
            int skillId = Config.SkillIdList[i];
            SkillData skillData = new SkillData(skillId, i);
            skillData.MagicLevel.Data = Config.SkillLevelList[i];
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
        long riseLevel = this.Level - 1;

        double hp = StringHelper.StringToNumber(Config.Hp);
        double hpRise = Math.Pow(Config.RiseHp, riseLevel);
        hp = hp * hpRise;

        //Debug.Log("hpRise " + StringHelper.FormatNumber(hpRise) + " hp:" + StringHelper.FormatNumber(hp));

        double attr = StringHelper.StringToNumber(Config.Attr);
        double attrRise = Math.Pow(Config.AttrRise, riseLevel);
        attr = attr * attrRise;

        //Debug.Log("attrRise " + StringHelper.FormatNumber(attrRise) + " attr:" + StringHelper.FormatNumber(attr));

        double def = StringHelper.StringToNumber(Config.Def);
        double defRise = Math.Pow(Config.DefRise, riseLevel);
        def = def * defRise;

        //Debug.Log("defRise " + StringHelper.FormatNumber(defRise) + " def:" + StringHelper.FormatNumber(def));

        double damageMul = StringHelper.StringToNumber(Config.DamageMul);
        double mulRise = Math.Pow(Config.MulRise, riseLevel);
        damageMul = damageMul * mulRise;

        //Debug.Log("mulRise " + StringHelper.FormatNumber(mulRise) + " damageMul:" + StringHelper.FormatNumber(damageMul));

        double strong = StringHelper.StringToNumber(Config.Strong);
        double strongRise = Math.Pow(Config.StrongRise, riseLevel);
        strong = strong * strongRise;

        double parray = StringHelper.StringToNumber(Config.Parray);
        if (parray > 0)
        {
            double parrayRise = Math.Pow(Config.ParrayRise, riseLevel);
            parray = parray * parrayRise;
        }
        //Debug.Log("strongRise " + StringHelper.FormatNumber(strongRise) + " strong:" + StringHelper.FormatNumber(strong));

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate + riseLevel * 1);
        AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage + riseLevel * 10);
        AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, Config.CritRateResist + riseLevel * 10);
        AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroBase, Config.CritDamageResist + riseLevel * 10);

        AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy + riseLevel * Config.AccuracyRise);
        AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss + riseLevel * Config.MissRise);
        //Debug.Log("Miss " + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Miss));
        AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);

        AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
        AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroBase, parray);
        AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);

        //回满当前血量
        this.SetAttackSpeed(Config.Speed);
        this.SetMoveSpeed(Config.Speed);

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

    public override void OnHit(DamageResult dr)
    {
        //Debug.Log("damage:" + StringHelper.FormatNumber(dr.Damage));

        double maxHp = this.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        double maxDamage = maxHp * Config.LoseRate / 1000;
        dr.Damage = Math.Min(dr.Damage, maxDamage);
        dr.ExtendDamage = Math.Min(dr.ExtendDamage, maxDamage);

        base.OnHit(dr);

        if (Config.Step == 1 && Step < 5)
        {
            int nowPercent = (int)(this.HP * 100 / maxHp);
            int stepPercent = 100 - this.Step * 20;

            if (HP > 0 && stepPercent >= nowPercent)  //只有本体，从90%开始,过了每10%的界限
            {
                Step++;

                //GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.World, Message = this.Name + "进入第" + Step + "阶段!" });
                //sepcial logic
                var enemy = new Monster_World(Config.MapId, this.Level, Step);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
        }
    }
}
