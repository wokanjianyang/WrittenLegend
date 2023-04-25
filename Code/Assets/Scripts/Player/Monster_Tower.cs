using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Tower : APlayer
{
    public int TowerId { get; }

    public TowerConfig config { get; set; }

    public Monster_Tower(int floor) {
        this.TowerId = floor;
        this.GroupId = 2;

        Load();
    }

    public override void Load()
    {
        base.Load();

        this.Camp = PlayerType.Enemy;

        TowerConfig config = TowerConfigCategory.Instance.Get(TowerId);

        this.config = config;
        this.Name = config.Name;

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

        Logic.SetData(null);
    }


}
