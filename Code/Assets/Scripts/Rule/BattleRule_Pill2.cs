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

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Myth, Message = "挑战通关！" });
            GameProcessor.Inst.User.PillTime.Time.Data -= currentRoundTime;

            BuildReward();

            GameProcessor.Inst.CloseBattle(RuleType.Pill2, 0);
        }
    }

    private void BuildReward()
    {
        List<Item> items = new List<Item>();

        items.Add(ItemHelper.BuildItem(ItemType.Material, ItemHelper.SpecialId_Pill2, 1, Layer * 10 + 110));

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        string message = "练气秘境" + Layer + "层通关奖励";
        GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });
    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Pill2, 0);
    }
}
