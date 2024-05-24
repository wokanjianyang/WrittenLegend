using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Legacy : APlayer
{
    LegacyMonsterConfig config;

    int Role = 0;
    int Layer = 0;

    public Monster_Legacy(int role, int layer)
    {
        this.GroupId = 2;
        this.Role = role;
        this.Layer = layer;

        config = LegacyMonsterConfigCategory.Instance.GetByRole(role);

        this.Init();
        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.Name + "(" + Layer + "阶)";
        this.Level = Layer;
        this.ModelType = MondelType.Boss;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();

        if (config.SkillIdList != null)
        {
            for (int i = 0; i < config.SkillIdList.Length; i++)
            {
                list.Add(new SkillData(config.SkillIdList[i], i)); //增加默认技能
            }
        }

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
        double attrRate = Layer;
        double advanceRate = Layer;

        //Debug.Log("attrRate:" + attrRate);
        //Debug.Log("advanceRate:" + advanceRate);

        double attr = Double.Parse(config.Attr) * attrRate;
        double hp = Double.Parse(config.HP) * attrRate;
        double def = Double.Parse(config.Def) * attrRate;


        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

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
        LegacyConfig dropLegacy = LegacyConfigCategory.Instance.GetDropItem(Role);
        int layer = LegacyConfigCategory.Instance.GetDropLayer(Layer);

        User user = GameProcessor.Inst.User;
        user.LegacyTikerCount.Data--;

        long legacyLayer = user.GetLegacyLayer(dropLegacy.Id);

        string message = "掉落 " + string.Format("<color=#{0}>{1}</color> ", QualityConfigHelper.GetQualityColor(6), dropLegacy.Name + "(" + layer + "阶) ");

        int recoveryStone = 0;
        if (layer > legacyLayer)
        {
            user.SaveLegacyLayer(dropLegacy.Id, layer);

            message += ",自动装备";
            //auto Replace
            if (legacyLayer > 0)
            {
                recoveryStone += dropLegacy.GetRecoveryNumber(legacyLayer);

                message += ",并且回收之前的获得" + recoveryStone + "个<color=#" + QualityConfigHelper.GetQualityColor(6) + ">传世精华</color>";
            }
        }
        else
        {
            recoveryStone += dropLegacy.GetRecoveryNumber(layer);

            message += ",自动回收获得" + recoveryStone + "个<color=#" + QualityConfigHelper.GetQualityColor(6) + ">传世精华</color>";
        }

        if (recoveryStone > 0)
        {
            Item item = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Legacy_Stone, recoveryStone);

            List<Item> items = new List<Item>();
            items.Add(item);

            if (items.Count > 0)
            {
                GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }


        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Legacy,
            Message = message
        });
    }
}
