using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Pill : ABattleRule
{
    private bool Start = false;

    private int Layer = 0;

    private double MapTime = 0;

    private const int MaxQuanlity = 30;

    protected override RuleType ruleType => RuleType.Pill;

    public BattleRule_Pill(Dictionary<string, object> param)
    {
        //param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("Layer", out object layer);

        //this.MapTime = (long)mapTime;
        this.Layer = (int)layer;

        Start = true;

        User user = GameProcessor.Inst.User;
        user.PillTime.Time.Data -= 3;

        MapTime = 0;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!Start)
        {
            return;
        }
        Debug.Log("create pill currentRoundTime:" + currentRoundTime);

        MapTime += currentRoundTime;
        Debug.Log("create pill MapTime:" + MapTime);

        User user = GameProcessor.Inst.User;
        double time = user.PillTime.Time.Data;

        if (time <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Pill, Message = "您已经没有了挑战时间！" });

            GameOver();

            Start = false;
            return;
        }

        user.PillTime.Time.Data -= currentRoundTime;
        GameProcessor.Inst.EventCenter.Raise(new ShowPillInfoEvent() { Time = time });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        int count = MaxQuanlity - enemys.Count;

        //Debug.Log("create pill monster:" + count);
        for (int i = 0; i < count; i++)
        {
            var enemy = new Monster_Pill(Layer);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }
    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Pill, 0);
    }
}
