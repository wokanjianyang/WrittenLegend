using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Abyss : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;

    private double MapTime = 0;

    private int RefreshCount = 0;

    private int[] Ms = { 50, 40, 30, 20, 10 };
    private List<int> MonsterList = new List<int>();

    protected override RuleType ruleType => RuleType.Abyss;

    public BattleRule_Abyss(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        for (int i = 0; i < Ms.Length; i++)
        {
            for (int j = 0; j < Ms[i]; j++)
            {
                MonsterList.Add(i + 1);
            }
        }

        this.MapId = (int)mapId;

        AppHelper.Abyss_Id = MapId;

        this.LoadHero();
    }

    private void LoadHero()
    {
        HeroMyth hero = new HeroMyth(false);
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

        MapTime += currentRoundTime;

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        int mc = 150 + enemys.Count - RefreshCount;
        GameProcessor.Inst.EventCenter.Raise(new ShowAbyssInfoEvent() { Count = mc, Time = (int)MapTime });

        //Debug.Log("create pill MapTime:" + MapTime);

        if (MonsterList.Count > 0 && enemys.Count <= 25)
        {
            int q = RandomHelper.RandomNumber(0, MonsterList.Count);

            var enemy = new Monster_Abyss(MapId, MonsterList[q]);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);

            RefreshCount++;

            MonsterList.RemoveAt(q);
            return;
        }

        if (MonsterList.Count <= 0 && enemys.Count <= 0)
        {
            this.Start = false;

            AbyssCopyConfig mapConfig = AbyssCopyConfigCategory.Instance.Get(this.MapId);

            GameProcessor.Inst.User.MythData.SetOver(this.MapId);
            BuildReward(mapConfig);

            GameProcessor.Inst.CloseBattle(RuleType.Abyss, 22);
        }
    }

    private static int[] rates = { 10, 20, 40, 60, 80, 100, 120, 140 };

    private void BuildReward(AbyssCopyConfig mapConfig)
    {
        int cycle = mapConfig.Cycle;
        int rate = mapConfig.Stage;

        List<Item> items = new List<Item>();

        IDictionary<int, int> dropDict = new Dictionary<int, int>();

        for (int i = 0; i < rate; i++)
        {
            int role = RandomHelper.RandomNumber(0, 3);  //道具职业
            int part = MathHelper.RandomArrayIndex(rates, 1); //道具部位
            int dropId = 70000000 + cycle * 10 + part;

            if (!dropDict.ContainsKey(dropId))
            {
                dropDict[dropId] = 0;
            }

            dropDict[dropId]++;
        }

        var dd = dropDict.OrderByDescending(m => m.Key);

        foreach (var sp in dd)
        {
            items.Add(ItemHelper.BuildItem(ItemType.Abyss, sp.Key, 0, sp.Value));
        }

        User user = GameProcessor.Inst.User;

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Abyss,
            Message = BattleMsgHelper.BuildRewardMessage(mapConfig.MapName + "通关奖励", 0, 0, items),
        });

        if (items.Count > 0)
        {
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        //if (!user.SpiritOfflineFlag)
        //{  //没有启用的时候，刷新记录
        //    user.SpiritOfflineLog[1] = MapId;
        //    user.SpiritOfflineLog[2] = (int)MapTime;

        //    GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        //    {
        //        Type = RuleType.Spirit,
        //        Message = BattleMsgHelper.BuildRewardMessage("已刷新通关记录", 0, 0, null),
        //    });
        //}
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.IsDie())
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Myth, Message = "挑战失败！" });
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Abyss, 13);
        }
    }
}
