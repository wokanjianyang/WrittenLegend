using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Myth : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;

    private double MapTime = 0;

    private const int MaxQuanlity = 30;

    protected override RuleType ruleType => RuleType.Pill;

    public BattleRule_Myth(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        this.MapId = (int)mapId;

        Start = true;

        MapTime = 0;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!Start)
        {
            return;
        }
        //Debug.Log("create pill currentRoundTime:" + currentRoundTime);

        MapTime += currentRoundTime;
        //Debug.Log("create pill MapTime:" + MapTime);

        if (MapTime >= 600)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Pill, Message = "ÃÙ’Ω ß∞‹£°" });

            GameOver();

            Start = false;
            return;
        }

        //GameProcessor.Inst.EventCenter.Raise(new ShowPillInfoEvent() { Time = time });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        int count = 1;

        //Debug.Log("create pill monster:" + count);
        for (int i = 0; i < count; i++)
        {
            var enemy = new Monster_Myth(1);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }
    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Pill, 0);
    }
}
