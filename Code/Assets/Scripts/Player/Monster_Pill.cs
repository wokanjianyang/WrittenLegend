using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Pill : APlayer
{
    MonsterPillConfig config;

    int Type = 1;
    int Layer = 0;

    public Monster_Pill(int layer)
    {
        this.GroupId = 2;
        this.Layer = layer;
        this.RuleType = RuleType.Pill;

        config = MonsterPillConfigCategory.Instance.GetByTypeAndLayer(this.Type,this.Layer);

        this.Init();
        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name =  config.MonsterName;
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
            List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillConfig.Id, 4);
            List<SkillSuit> suitList = SkillSuitHelper.GetAllSuit(skillData.SkillConfig.Id, 4);

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

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

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

    private void MakeReward(DeadRewarddEvent dead)
    {
        BuildReward();
    }

    private void BuildReward()
    {
        User user = GameProcessor.Inst.User;

        List<Item> items = DropLimitHelper.Build((int)DropLimitType.Pill, 0, 1, 1, 9999999, 1);

        foreach (Item item in items)
        {
            item.Count *= Layer;
        }

        if (items.Count > 0)
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        double rs = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
        int itemCount = MathHelper.RandomBurstMul(rs);

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType,
            Message = BattleMsgHelper.BuildMonsterDeadMessage(this, 0, 0, items, itemCount)
        });

        //if (itemCount > 0)
        //{
        //    items.AddRange(ItemHelper.BurstMul(items, itemCount, 1));
        //}


    }
}
