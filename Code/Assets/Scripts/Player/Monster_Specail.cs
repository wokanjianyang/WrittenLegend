using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Specail : APlayer
{
    MonsterSpecialConfig config;

    public Monster_Specail(int id)
    {
        this.GroupId = 2;

        config = MonsterSpecialConfigCategory.Instance.Get(id);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.Name;
        this.ModelType = MondelType.Boss;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI

        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();
        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

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
        long attr = config.Attr;
        long hp = config.HP;
        long def = config.Def;


        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

    public override void OnHit(DamageResult dr)
    {
        dr.Damage = 1;
        base.OnHit(dr);
    }

    private void MakeReward(DeadRewarddEvent dead)
    {
        //Log.Info("Monster :" + this.ToString() + " dead");
        BuildReword();
    }

    private void BuildReword()
    {
        User user = GameProcessor.Inst.User;

        //生成道具奖励
        List<KeyValuePair<int, DropConfig>> dropList = new List<KeyValuePair<int, DropConfig>>();
        for (int i = 0; i < config.DropIdList.Length; i++)
        {
            DropConfig dropConfig = DropConfigCategory.Instance.Get(config.DropIdList[i]);
            dropList.Add(new KeyValuePair<int, DropConfig>((int)(config.DropRateList[i]), dropConfig));
        }

        List<Item> items = DropHelper.BuildDropItem(dropList, 1);

        if (items.Count > 0)
        {
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildBossDeadMessage(this, 0, 0, items)
        });
    }
}
