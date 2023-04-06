using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Tower : Monster
{
    public void SetLevelConfigAttr(TowerConfig config)
    {
        //TODO 测试增加被动属性
        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, 1);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, 0);

        this.MonsterId = config.Id;
        this.Gold = 0;
        this.Exp = config.OfflineExp;
        this.Name = config.Name;
    }

    protected override void MakeReward(DeadRewarddEvent dead)
    {
        Log.Info("Monster :" + this.ToString() + " dead");

        Hero hero = GameProcessor.Inst.PlayerManager.GetHero();

        long exp = this.Exp;

        //增加经验,金币

        hero.Exp += exp;
        hero.Gold += this.Gold;
        hero.TowerFloor++;
        hero.EventCenter.Raise(new HeroInfoUpdateEvent());
        if (hero.Exp >= hero.UpExp)
        {
            hero.EventCenter.Raise(new HeroChangeEvent
            {
                Type = Hero.HeroChangeType.LevelUp
            });
        }

        //生成道具奖励

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            RoundNum = hero.RoundCounter,
            MonsterId = this.MonsterId,
            Exp = exp,
            Gold = this.Gold,
            Drops = new List<Item>(),
            BattleType = BattleType.Tower
        });

        //存档
        UserData.Save();
    }
}
