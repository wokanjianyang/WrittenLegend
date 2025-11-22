using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Shengxiao : APlayer
{
    MonsterShengxiaoConfig config;

    private double[] HpRateist = { 1, 5, 10, 25, 100 };
    private double[] AttrRateist = { 1, 1.1, 1.2, 1.3, 1.5 };
    private double[] DefRateist = { 1, 1.1, 1.15, 1.2, 1.25 };

    private double[] DropRateList = { 1, 2, 4, 10, 40 };
    private double[] QualityRateList = { 1, 1.1, 1.2, 1.5, 2 };

    private string[] NameList = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

    private int NameId = 1;

    public Monster_Shengxiao(int mapId, int quality)
    {
        this.GroupId = 2;
        this.RuleType = RuleType.Shengxiao;
        this.Quality = quality;

        config = MonsterShengxiaoConfigCategory.Instance.Get(mapId);
        NameId = RandomHelper.RandomNumber(7, 25) % 12 + 1;

        this.Init();

        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.MonsterName + NameList[NameId - 1];
        this.Level = config.Id * 10 + this.Quality;
        this.ModelType = MondelType.Nomal;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private int[] SkillRate = { 100, 10, 20 };

    private int[][] SkillList = new int[][]
    {
        new int[] { 2002, 1002,3002  },
        new int[] { 2007, 1004, 2009 },
        new int[] {  1005, 3008 }
    };

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();

        List<int> IdList = new List<int>();


        for (int i = 0; i < 3; i++)
        {
            if (RandomHelper.RandomRate(SkillRate[i]))
            {
                int rd = RandomHelper.RandomNumber(0, SkillList[i].Length);
                IdList.Add(SkillList[i][rd]);
            }
        }

        if (Quality >= 5 && !IdList.Contains(3008))
        {
            IdList.Add(3008);
        }
        if (Quality >= 5 && !IdList.Contains(1012))
        {
            IdList.Add(1012);
        }
        if (Quality >= 4 && !IdList.Contains(2007))
        {
            IdList.Add(2007);
        }

        for (int i = 0; i < IdList.Count; i++)
        {
            int skillId = IdList[i];
            SkillData skillData = new SkillData(skillId, i);
            skillData.MagicLevel.Data = config.Id * 10;
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
        double HpRate = HpRateist[this.Quality - 1];
        double attrRate = AttrRateist[this.Quality - 1]; ;
        double defRate = DefRateist[this.Quality - 1]; ;

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Double.Parse(config.HP) * HpRate);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr) * attrRate);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr) * attrRate);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Double.Parse(config.Attr) * attrRate);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Double.Parse(config.Def) * defRate);

        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, config.DamageIncrea);
        AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, config.DamageResist);
        AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, config.CritRateResist);
        AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroBase, config.CritDamageResist);
        //AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroBase, config.ResotrePercent);
        AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, config.Protect);

        this.SetAttackSpeed(config.Speed);
        this.SetMoveSpeed(config.Speed);

        //Debug.Log("hp:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
        //Debug.Log("attr:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.PhyAtt));
        //Debug.Log("def:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Def));

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

    private void MakeReward(DeadRewarddEvent dead)
    {
        BuildReword();
    }

    private void BuildReword()
    {
        int maxQuality = this.config.Id + 5;

        User user = GameProcessor.Inst.User;

        double exp = (this.config.Exp * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
        double gold = (this.config.Gold * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

        long BurstIncrea = Math.Min(user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea), 1500000);
        long QualityIncrea = Math.Min(user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea), 1500000);

        double dropRate = 400.0 / (BurstIncrea / 750000.0 + 1) / DropRateList[Quality - 1];
        double qualityRate = (QualityIncrea / 750000.0 + 1) * QualityRateList[Quality - 1];

        //生肖掉落
        List<Item> items = new List<Item>();
        items.Add(ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao, Quality * this.config.Id));

        //Debug.Log("this dropRate :" + dropRate + "  qualityRate:" + qualityRate + " nameId:" + NameId + " - " + Name + " quality:" + Quality);

        if (RandomHelper.RandomResult(dropRate))
        {
            //AppHelper.TempRecord++;
            //Debug.Log("shengxiao count:" + AppHelper.TempRecord);
            //生肖
            items.Add(ShengxiaoConfigCategory.Instance.Build(NameId, qualityRate, maxQuality, 0));
        }

        double rs = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
        int itemCount = MathHelper.RandomBurstMul(rs);

        bool showMessage = QualityConfigHelper.GetMaxColor(items) >= user.InfoColor;
        if (showMessage)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType,
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items, itemCount)
            });
        }

        if (itemCount > 0)
        {
            exp += exp * itemCount;
            gold += gold * itemCount;
            items.AddRange(ItemHelper.BurstMul(items, itemCount, qualityRate, RuleType.Normal, maxQuality));
        }

        //先回收
        List<Item> recoveryList = user.CheckRecovery(items, out long recoveryGold, out int recoveryCount);
        if (recoveryCount > 0 && showMessage)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType,
                Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryCount, recoveryList, recoveryGold)
            });
        }

        user.AddExpAndGold(exp, gold);

        if (items.Count > 0)
        {
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }
    }
}
