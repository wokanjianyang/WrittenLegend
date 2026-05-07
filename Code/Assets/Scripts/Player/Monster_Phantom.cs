using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Phantom : APlayer
{
    bool Real = true;

    PhantomConfig config;
    PhantomAttrConfig attrConfig;
    int Layer = 0;
    int Percent = 10;
    int HpPercent = 9;

    public Monster_Phantom(int id, int layer, bool real, int percent)
    {
        this.GroupId = 2;
        this.Real = real;
        this.Layer = layer;
        this.Percent = percent;

        config = PhantomConfigCategory.Instance.Get(id);
        attrConfig = PhantomConfigCategory.Instance.GetAttrConfig(id, layer);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.Name;
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
        if (Real)
        {
            if (attrConfig.SkillIdList != null)
            {
                for (int i = 0; i < attrConfig.SkillIdList.Length; i++)
                {
                    list.Add(new SkillData(attrConfig.SkillIdList[i], i)); //增加默认技能
                }
            }
        }
        else
        {
            if (attrConfig.PhanSkillIdList != null)
            {
                for (int i = 0; i < attrConfig.PhanSkillIdList.Length; i++)
                {
                    list.Add(new SkillData(attrConfig.PhanSkillIdList[i], i)); //增加默认技能
                }
            }
        }
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
        double advanceRate = this.attrConfig.GetAttrAdvanceRate(Layer);

        int riseLevel = Layer - 1;

        //Debug.Log("attrRate:" + attrRate);
        //Debug.Log("advanceRate:" + advanceRate);

        LargeNumber hp = StringHelper.StringToLargNumber(attrConfig.Hp);
        LargeNumber hpRise = LargeNumber.Pow(attrConfig.HpRise, riseLevel);
        hp.Mul(hpRise);

        //if (Percent >= 10)
        //{
        //    Debug.Log("hpRise " + hpRise + " hp:" + hp);
        //}

        double attr = StringHelper.StringToNumber(attrConfig.Attr);
        LargeNumber attrRise = LargeNumber.Pow(attrConfig.AttrRise, riseLevel);

        //if (Percent >= 10)
        //{
        //    Debug.Log("attrRise " + attrRise + " attr:" + attr);
        //}

        LargeNumber def = StringHelper.StringToLargNumber(attrConfig.Def);
        LargeNumber defRise = LargeNumber.Pow(attrConfig.DefRise, riseLevel);
        def.Mul(defRise);

        LargeNumber damageMul = StringHelper.StringToLargNumber(attrConfig.DamageMul);
        LargeNumber mulRise = LargeNumber.Pow(attrConfig.MulRise, riseLevel);
        damageMul.Mul(mulRise);

        //if (Percent >= 10)
        //{
        //    Debug.Log("mulRise " + mulRise + " damageMul:" + damageMul);
        //}

        LargeNumber strong = StringHelper.StringToLargNumber(attrConfig.Strong);
        LargeNumber strongRise = LargeNumber.Pow(attrConfig.StrongRise, riseLevel);
        strong.Mul(strongRise);

        LargeNumber parray = StringHelper.StringToLargNumber(attrConfig.Parray);
        if (parray.data > 0)
        {
            LargeNumber parrayRise = LargeNumber.Pow(attrConfig.ParrayRise, riseLevel);
            parray.Mul(parrayRise);
        }

        int speed = attrConfig.Speed + (int)(Layer * attrConfig.SpeedRise);

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, 1);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, 1);

        AttributeBonus.SetAttrLarge(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttrLarge(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        AttributeBonus.SetAttr(AttributeEnum.LargeAtk, AttributeFrom.HeroBase, 1);
        AttributeBonus.SetAttrLarge(AttributeEnum.LargeAtk, AttributeFrom.HeroBase, attrRise);

        AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, attrConfig.CritRate + advanceRate);
        AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, attrConfig.CritDamage + advanceRate);
        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, attrConfig.DamageIncrea + advanceRate);
        AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, attrConfig.DamageResist + advanceRate);

        if (attrConfig.AttrIdList != null)
        {
            for (int i = 0; i < attrConfig.AttrIdList.Length; i++)
            {
                int attrId = attrConfig.AttrIdList[i];
                double attrValue = attrConfig.AttrValueList[i];
                double attRise = (Layer - 1) * attrConfig.AttrRiseList[i];
                double total = attrValue + attRise;
                if (attrId == (int)AttributeEnum.MulDamageResist)
                {
                    total = MathHelper.CalRealResist(total);
                }

                AttributeBonus.SetAttr((AttributeEnum)attrId, AttributeFrom.HeroBase, total);
            }
        }

        AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, attrConfig.Accuracy + riseLevel * attrConfig.AccuracyRise);
        AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, attrConfig.Miss + riseLevel * attrConfig.MissRise);

        AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroBase, 1);
        AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroBase, 1);
        AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, 1);

        AttributeBonus.SetAttrLarge(AttributeEnum.Strong, AttributeFrom.HeroBase, strong);
        AttributeBonus.SetAttrLarge(AttributeEnum.Parry, AttributeFrom.HeroBase, parray);
        AttributeBonus.SetAttrLarge(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);

        this.SetAttackSpeed(speed);
        this.SetMoveSpeed(speed);

        LargeNumber MaxHP = AttributeBonus.GetTotalAttrLarge(AttributeEnum.HP);
        MaxHP.Mul(Percent).Div(10);

        //Debug.Log("Phan CurrentHp:" + CurrentHp);
        SetHP(MaxHP);
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

    public override void OnHit(DamageResult dr)
    {
        if (attrConfig.ResistType > 0)
        {
            if (dr.RoleType != RoleType.All && (int)dr.RoleType != attrConfig.ResistType)
            {
                this.EventCenter.Raise(new ShowMsgEvent
                {
                    Type = MsgType.SkillName,
                    Content = "抵抗"
                });
                return;
            }
        }

        LargeNumber maxHp = this.AttributeBonus.GetTotalAttrLarge(AttributeEnum.HP);
        LargeNumber maxDamage = new LargeNumber(maxHp.data, maxHp.size).Div(10);

        dr.DamageLg = dr.DamageLg.Compare(maxDamage) == -1 ? dr.DamageLg : new LargeNumber(maxDamage.data, maxDamage.size);
        dr.ExtendDamageLg = dr.ExtendDamageLg.Compare(maxDamage) == -1 ? dr.ExtendDamageLg : new LargeNumber(maxDamage.data, maxDamage.size);

        base.OnHit(dr);

        int nowPercent = (int)(new LargeNumber(this.HP.data, this.HP.size).Mul(10).Div(maxHp).ConvertToDouble());

        if (!IsDie() && HpPercent > nowPercent && Real)  //只有本体，从90%开始,过了每10%的界限
        {
            HpPercent = nowPercent;
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Phantom, Message = this.Name + "：看我鬼影无踪!" });
            //sepcial logic
            var enemy = new Monster_Phantom(config.Id, Layer, false, nowPercent);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);

            RandomTransport();
        }
    }

    private void RandomTransport()
    {

        var tempCells = GameProcessor.Inst.MapData.AllCells.ToList();
        var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers(true).Select(p => p.Cell).ToList();
        tempCells.RemoveAll(p => allPlayerCells.Contains(p));

        if (tempCells.Count > 0)
        {
            var bornCell = UnityEngine.Vector3Int.zero;
            if (tempCells.Count > 1)
            {
                var index = RandomHelper.RandomNumber(0, tempCells.Count);
                bornCell = tempCells[index];
                this.Move(bornCell);
            }
        }
    }
}
