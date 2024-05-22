using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Legacy : ABattleRule
{
    private bool Start = false;

    private int LegacyId = 0;
    private int Layer = 0;

    private long MapTime = 0;
    protected override RuleType ruleType => RuleType.Legacy;

    public BattleRule_Legacy(Dictionary<string, object> param)
    {
        param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("LegacyId", out object legacyId);
        param.TryGetValue("Layer", out object layer);

        this.MapTime = (long)mapTime;
        this.LegacyId = (int)legacyId;
        this.Layer = (int)layer;

        Start = true;
    }


    public override void DoMapLogic(int roundNum)
    {
        if (!Start)
        {
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        User user = GameProcessor.Inst.User;
        int LecacyCount = 1;

        if (LecacyCount > 0)
        {
            var enemy = new Monster_Legacy(LegacyId, Layer);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }
        else
        {
            //reward
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Legacy, Message = "您已经没有了挑战次数！" });

            GameOver();

            Start = false;
            return;
        }
    }



    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.HeroDie(RuleType.Legacy, MapTime);
    }
}
