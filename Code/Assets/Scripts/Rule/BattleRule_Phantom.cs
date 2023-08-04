using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Phantom : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;

    private List<int> QualityList;

    private int MaxTime = 180; 

    protected override RuleType ruleType => RuleType.Tower;

    public BattleRule_Phantom(int mapId)
    {
      
    }

    public override void DoHeroLogic()
    {
        if (MaxTime <= 0)
            return;
        MaxTime--;

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        hero.DoEvent();
    }

    public override void DoMonsterLogic()
    {
        if (MaxTime <= 0)
            return;
        MaxTime--;

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        foreach (var enemy in enemys)
        {
            enemy.DoEvent();
        }

        if (enemys.Count <= 0 && MaxTime > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildCopySuccessMessage(),
                BattleType = BattleType.Tower
            });

            GameProcessor.Inst.HeroDie(RuleType.Tower);
        }
    }
}
