using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Pill : APlayer
{
    MonsterPillConfig config;

    int Layer = 0;

    public Monster_Pill(int layer)
    {
        this.GroupId = 2;
        this.Layer = layer;

        config = MonsterPillConfigCategory.Instance.Get(this.Layer);

        this.Init();
        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = " 幻境魔物";
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


        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = SkillRuneHelper.GetAllRune(skillData.SkillConfig.Id, 4);
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
        AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, config.Miss);
        AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, config.Protect);

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
        double maxDamage = maxHp / 100;
        dr.Damage = Math.Min(dr.Damage, maxDamage);

        base.OnHit(dr);
    }

    private void MakeReward(DeadRewarddEvent dead)
    {
        BuildReward();
    }

    private void BuildReward()
    {
        User user = GameProcessor.Inst.User;
    }
}
