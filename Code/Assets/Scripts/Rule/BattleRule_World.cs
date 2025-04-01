using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_World : ABattleRule
{
    private bool Start = false;

    private int Layer = 0;
    private int MapId = 0;

    private double MapTime = 0;

    private int MaxTime = 20;
    private int CurrentLayer = 1;

    protected override RuleType ruleType => RuleType.Myth;

    public BattleRule_World(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);
        param.TryGetValue("Layer", out object layer);

        this.MapId = (int)mapId;
        this.Layer = (int)layer;

        this.Load();
    }

    private void Load()
    {
        Start = true;
        MapTime = 0;

        var enemy = new Monster_World(MapId, Layer, 1);
        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!Start)
        {
            return;
        }

        //Debug.Log("create pill currentRoundTime:" + currentRoundTime);


        //Debug.Log("create pill MapTime:" + MapTime);
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);


        if (Start && enemys.Count <= 0)
        {
            this.Start = false;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Myth, Message = "挑战通关！" });
            GameProcessor.Inst.User.MythData.SetOver(this.MapId);
            BuildReward(MapId);

            GameProcessor.Inst.CloseBattle(RuleType.World, 14);
        }
    }

    private void BuildReward(int mapId)
    {
        List<Item> items = new List<Item>();

        MythConfig mythConfig = MythConfigCategory.Instance.Get(mapId);

        for (int i = 0; i < mythConfig.ItemIdList.Length; i++)
        {
            items.Add(ItemHelper.BuildItem((ItemType)mythConfig.ItemType[i], mythConfig.ItemIdList[i], 1, mythConfig.ItemQuantity[i]));
        }

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        string message = "仙界神兽" + mythConfig.MapName + "通关奖励";
        GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.World, Message = "挑战失败！" });
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.World, 14);
        }
    }
}
