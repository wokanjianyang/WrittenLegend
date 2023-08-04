using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Phantom : ABattleRule
{
    private bool PhanStart = false;

    private int PhanId = 0;
    private int Layer = 0;

    private List<int> QualityList;

    private int MaxTime = 180;

    protected override RuleType ruleType => RuleType.Phantom;


    public BattleRule_Phantom(int pid, int layer)
    {
        PhanId = pid;
        Layer = layer;
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

        if (!PhanStart)
        {
            var enemy = new Monster_Phantom(PhanId, Layer, true, 10);  //刷新本体,10代表满血
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            PhanStart = true;
        }

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
