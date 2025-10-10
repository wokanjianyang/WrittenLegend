using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Pill2 : APlayer
{
    MonsterPillConfig config;

    int Type = 2;
    int Layer = 0;

    public Monster_Pill2(int type, int layer)
    {
        this.GroupId = 2;
        this.Layer = layer;
        this.RuleType = RuleType.Pill;
        this.Type = type;

        config = MonsterPillConfigCategory.Instance.GetByTypeAndLayer(this.Type, this.Layer);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.MonsterName;
        this.Level = Layer;
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
            skillData.MagicLevel.Data = this.Level * 100;
            list.Add(skillData);
        }

        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillConfig.Id, 99);
            List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillConfig.Id, 99);

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {
        double hp = StringHelper.StringToNumber(config.HP);
        //Debug.Log(this.Layer + " Hp:" + StringHelper.FormatNumber(hp));

        double attr = StringHelper.StringToNumber(config.Attr);
        //Debug.Log(this.Layer + " Attr:" + StringHelper.FormatNumber(attr));

        double def = StringHelper.StringToNumber(config.Def);
        //Debug.Log(this.Layer + " Def:" + StringHelper.FormatNumber(def));

        double damageMul = StringHelper.StringToNumber(config.DamageMul);

        double strong = StringHelper.StringToNumber(config.Strong);

        double parry = StringHelper.StringToNumber(config.Parray);

        //Debug.Log(this.Layer + " strong:" + strong.ToString());

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, config.DamageIncrea);
        AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, config.DamageResist);
        AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, config.CritRateResist);
        AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, config.CritDamage);
        AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroBase, config.ResotrePercent);

        AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, config.Miss);
        AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, config.Accuracy);
        AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, config.Protect);

        AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
        AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);
        AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroBase, parry);

        SetMoveSpeed((int)config.Speed);
        SetAttackSpeed((int)config.Speed);

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

    public override void OnHit(DamageResult dr)
    {
        //Debug.Log("monster pill2 hit damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));

        base.OnHit(dr);
    }
}
