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

    private long MapTime = 0;

    private const int MaxQuanlity = 30;

    protected override RuleType ruleType => RuleType.Pill;

    public BattleRule_Pill(Dictionary<string, object> param)
    {
        //param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("Layer", out object layer);

        //this.MapTime = (long)mapTime;
        this.Layer = (int)layer;

        Start = true;
    }

    public override void DoMapLogic(int roundNum)
    {
        if (!Start)
        {
            return;
        }

        User user = GameProcessor.Inst.User;
        double time = user.PillTime.Data;

        if (time <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Pill, Message = "您已经没有了挑战时间！" });

            GameOver();

            Start = false;
            return;
        }

        user.PillTime.Data -= 0.2;
        GameProcessor.Inst.EventCenter.Raise(new ShowPillInfoEvent() { Time = time });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        int count = MaxQuanlity - enemys.Count;

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
