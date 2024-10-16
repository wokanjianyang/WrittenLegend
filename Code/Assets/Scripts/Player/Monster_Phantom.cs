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
        double attrRate = this.attrConfig.GetAttrRate(Layer);
        double advanceRate = this.attrConfig.GetAttrAdvanceRate(Layer);

        //Debug.Log("attrRate:" + attrRate);
        //Debug.Log("advanceRate:" + advanceRate);

        double attr = Double.Parse(attrConfig.Attr) * attrRate;
        double hp = Double.Parse(attrConfig.Hp) * attrRate;
        double def = Double.Parse(attrConfig.Def) * attrRate;


        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

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
                double attrRise = (Layer - 1) * attrConfig.AttrRiseList[i];
                double total = attrValue + attrRise;
                if (attrId == (int)AttributeEnum.MulDamageResist)
                {
                    total = MathHelper.CalRealResist(total);
                }

                AttributeBonus.SetAttr((AttributeEnum)attrId, AttributeFrom.HeroBase, total);
            }
        }

        double MaxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        double CurrentHp = Percent * MaxHP / 10;

        //Debug.Log("Phan CurrentHp:" + CurrentHp);
        SetHP(CurrentHp);
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

        double maxHp = this.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
        double maxDamage = maxHp / 10;
        dr.Damage = Math.Min(dr.Damage, maxDamage);

        base.OnHit(dr);

        int nowPercent = (int)(this.HP * 10 / maxHp);

        if (HP > 0 && HpPercent > nowPercent && Real)  //只有本体，从90%开始,过了每10%的界限
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
        var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
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
