using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Festive : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;

    private double MapTime = 0;

    private int MaxTime = 20;
    private int CurrentLayer = 1;
    private int[] LayerCount = new int[] { 10, 8, 6, 4, 2 };

    protected override RuleType ruleType => RuleType.Festive;

    public BattleRule_Festive(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        this.MapId = (int)mapId;
        this.LoadHero();
    }

    private void LoadHero()
    {
        HeroMyth hero = new HeroMyth();
        GameProcessor.Inst.PlayerManager.LoadHero(hero);

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

        if (CurrentLayer <= LayerCount.Length)
        {
            MapTime += currentRoundTime;
        }
        GameProcessor.Inst.EventCenter.Raise(new ShowFestiveInfoEvent() { Layer = CurrentLayer, Time = (int)(MaxTime - MapTime) });

        //Debug.Log("create pill MapTime:" + MapTime);
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (CurrentLayer <= LayerCount.Length && MapTime >= 1 && (MapTime >= MaxTime || enemys.Count <= 0))
        {
            for (int i = 0; i < LayerCount[CurrentLayer - 1]; i++)
            {
                var enemy = new Monster_Myth(MapId, CurrentLayer);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            CurrentLayer++;
            MapTime = 0;

            return;
        }

        if (CurrentLayer > LayerCount.Length && enemys.Count <= 0)
        {
            this.Start = false;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Festive, Message = "挑战通关！" });
            GameProcessor.Inst.User.MythData.SetOver(this.MapId);
            BuildReward(MapId);

            GameProcessor.Inst.CloseBattle(RuleType.Festive, 17);
        }
    }

    private void BuildReward(int mapId)
    {

        User user = GameProcessor.Inst.User;

        List<Item> items = new List<Item>();

        FestiveCopyConfig mythConfig = FestiveCopyConfigCategory.Instance.Get(mapId);

        if (mapId > user.FestiveMapData.Record)
        {
            //首通
            for (int i = 0; i < mythConfig.FirstItemIdList.Length; i++)
            {
                items.Add(ItemHelper.BuildItem((ItemType)mythConfig.FirstItemType[i], mythConfig.FirstItemIdList[i], 1, mythConfig.FirstItemQuantity[i]));
            }
        }
        else
        {
            //非首通
            for (int i = 0; i < mythConfig.ItemIdList.Length; i++)
            {
                items.Add(ItemHelper.BuildItem((ItemType)mythConfig.ItemType[i], mythConfig.ItemIdList[i], 1, mythConfig.ItemQuantity[i]));
            }
        }

        user.FestiveMapData.Record = mapId;
        user.FestiveMapData.Number.Data -= 1;

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        string message = "节日副本" + mythConfig.MapName + "通关奖励";
        GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Festive, Message = "挑战失败！" });
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Festive, 17);
        }
    }
}
