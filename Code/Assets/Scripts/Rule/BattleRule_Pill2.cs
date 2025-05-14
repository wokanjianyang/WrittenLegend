using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Pill2 : ABattleRule
{
    private bool Start = false;

    private int Layer = 0;

    private double MapTime = 0;

    private const int MaxQuanlity = 10;

    protected override RuleType ruleType => RuleType.Pill2;

    public BattleRule_Pill2(Dictionary<string, object> param)
    {
        //param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("Layer", out object layer);

        //this.MapTime = (long)mapTime;
        this.Layer = (int)layer;

        Debug.Log("pill2 layer:" + layer);

        Start = true;

        for (int i = 0; i < 10; i++)
        {
            var enemy = new Monster_Pill2(Layer);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }

        MapTime = 0;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!Start)
        {
            return;
        }


        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count <= 0)
        {
            this.Start = false;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Myth, Message = "ÌôÕ½Í¨¹Ø£¡" });
            GameProcessor.Inst.User.MythData.SetOver(this.Layer);
            BuildReward();

            GameProcessor.Inst.CloseBattle(RuleType.Pill2, 14);
        }
    }

    private void BuildReward()
    {

    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Pill, 0);
    }
}
