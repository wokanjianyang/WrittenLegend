using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Tower : Monster
{
    public void SetLevelConfigAttr(TowerConfig config)
    {
        //TODO �������ӱ�������
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

        //���Ӿ���,���

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

        //���ɵ��߽���

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            RoundNum = hero.RoundCounter,
            MonsterId = this.MonsterId,
            Exp = exp,
            Gold = this.Gold,
            Drops = new List<Item>(),
            BattleType = BattleType.Tower
        });

        //�浵
        UserData.Save();
    }
}
