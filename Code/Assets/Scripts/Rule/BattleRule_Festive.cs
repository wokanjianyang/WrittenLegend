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

    private int MaxTime = 60;
    private int CurrentLayer = 1;
    private int[] LayerCount = new int[] { 20, 15, 10, 8, 6 };

    protected override RuleType ruleType => RuleType.Festive;

    public BattleRule_Festive(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        this.MapId = (int)mapId;
        this.LoadHero();
    }

    private void LoadHero()
    {
        HeroMyth hero = new HeroMyth(true);
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

        if (CurrentLayer <= LayerCount.Length && MapTime >= 1 && ((MapTime >= MaxTime && enemys.Count <= 5) || enemys.Count <= 0))
        {
            for (int i = 0; i < LayerCount[CurrentLayer - 1]; i++)
            {
                var enemy = new Monster_Festive(MapId, CurrentLayer);
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
            BuildReward(MapId);

            GameProcessor.Inst.CloseBattle(RuleType.Festive, 17);
        }
    }

    private void BuildReward(int mapId)
    {
        //如果不是节日时间，则没有奖励
        if (!DropLimitConfigCategory.Instance.CheckIsTime())
        {
            //Debug.Log("非时间内");
            return;
        }


        User user = GameProcessor.Inst.User;

        List<Item> items = new List<Item>();

        FestiveCopyConfig config = FestiveCopyConfigCategory.Instance.Get(mapId);

        if (mapId > user.FestiveMapData01.Record)
        {
            //首通
            for (int i = 0; i < config.FirstItemIdList.Length; i++)
            {
                items.Add(ItemHelper.BuildItem((ItemType)config.FirstItemType[i], config.FirstItemIdList[i], 1, config.FirstItemQuantity[i]));
            }
        }
        else
        {
            //非首通
            for (int i = 0; i < config.ItemIdList.Length; i++)
            {
                items.Add(ItemHelper.BuildItem((ItemType)config.ItemType[i], config.ItemIdList[i], 1, config.ItemQuantity[i]));
            }
        }

        user.FestiveMapData01.Record = mapId;
        user.FestiveMapData01.Number.Data -= 1;

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        string message = "节日副本" + config.MapName + "首通奖励";
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
