using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Legacy : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;
    private int Layer = 0;

    private long MapTime = 0;
    protected override RuleType ruleType => RuleType.Legacy;

    public BattleRule_Legacy(Dictionary<string, object> param)
    {
        //param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("MapId", out object mapId);
        param.TryGetValue("Layer", out object layer);

        //this.MapTime = (long)mapTime;
        this.MapId = (int)mapId;
        this.Layer = (int)layer;

        Start = true;
    }


    public override void DoMapLogic(int roundNum, double currentRoundTime)
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

        if (checkAuto())
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Legacy, Message = "当前积分已经达标，或者阶数已满，自动停止挑战！" });
            Start = false;
            return;
        }


        long LecacyCount = user.LegacyTikerCount.Data;

        GameProcessor.Inst.EventCenter.Raise(new ShowLegacyInfoEvent() { Count = LecacyCount });

        if (LecacyCount > 0)
        {
            var enemy = new Monster_Legacy(MapId, Layer);
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


    private bool checkAuto()
    {
        User user = GameProcessor.Inst.User;

        LegacyMapConfig legacyMapConfig = LegacyMapConfigCategory.Instance.Get(MapId);

        long needNumber = legacyMapConfig.PowerList[MapId - 1] + (this.Layer - 1) * 500;

        //Debug.Log("needNumber:" + needNumber);

        long[] powerList = new long[] { 0, 0, 0 };
        long minCount = 0;

        foreach (var kv in user.LegacyLayer)
        {
            LegacyConfig legacy = LegacyConfigCategory.Instance.Get(kv.Key);

            for (int i = 0; i < legacy.PowerList.Length; i++)
            {
                powerList[i] += kv.Value.Data * legacy.PowerList[i];
            }

            if (legacy.Role == MapId && kv.Value.Data < this.Layer)
            {
                minCount++;
            }
        }

        long totalNumber = powerList[MapId - 1];
        //Debug.Log("totalNumber:" + totalNumber);

        //Debug.Log("minCount:" + minCount);

        if (minCount <= 0 || totalNumber >= needNumber)
        {
            return true;
        }

        return false;
    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Legacy, 0);
    }
}
