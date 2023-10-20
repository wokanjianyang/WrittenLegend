using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Defend : APlayer
{
    public Defend(long hp)
    {
        this.GroupId = 1;

        this.Init(hp);
    }

    private void Init(long hp)
    {
        this.Camp = PlayerType.Defend;
        this.Name = "沙城";
        this.ModelType = MondelType.Boss;

        this.SetAttr(hp);  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
    }

    private void SetAttr(long hp)
    {
        Debug.Log("Defend HP:" + hp);
        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, 0);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, 0);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, 0);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, 0);

        SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
    }

    public override float DoEvent()
    {
        return 1f;
        //this.OnRestore(0, 1);
    }

    public override void OnHit(DamageResult dr)
    {
        dr.Damage = 1;
        base.OnHit(dr);
    }
}
