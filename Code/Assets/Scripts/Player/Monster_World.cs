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
        this.Quality = 5;
        this.Level = level;
        this.Step = step;

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

        double hp = StringHelper.StringToNumber(Config.Hp);
        //Debug.Log("Config " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));
        //hp += hp * Config.HpRise * riseLevel;

        double attr = StringHelper.StringToNumber(Config.Attr);
        //Debug.Log("Config " + this.Progress + " Attr:" + StringHelper.FormatNumber(attr));
        //attr += attr * Config.AttrRise * riseLevel;

        double def = StringHelper.StringToNumber(Config.Def);
        //Debug.Log("Config " + this.Progress + " Def:" + StringHelper.FormatNumber(def));
        //def += def * Config.DefRise * riseLevel;

        double damageMul = StringHelper.StringToNumber(Config.DamageMul);
        //damageMul += damageMul * Config.MulRise * riseLevel;

        double strong = StringHelper.StringToNumber(Config.Strong);

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
        //AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

        //AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy);
        //AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss);
        //AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroBase, Config.MulDamageResist);

        AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);

        AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
        AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

    public override void OnHit(DamageResult dr)
    {

        double maxHp = this.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        double maxDamage = maxHp / 1000;
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

                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.World, Message = this.Name + "进入第" + Step + "阶段!" });
                //sepcial logic
                var enemy = new Monster_World(Config.Id, this.Level, Step);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
        }
    }
}
