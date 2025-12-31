using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Spirit : ABattleRule
{
    private bool Start = false;
    private bool Next = false;

    private int MapId = 0;

    private double MapTime = 0;

    private int MaxTime = 180;

    private int CurrentLayer = 1;  //此模式分4阶段，1 小兵 2 紫怪  3 boss 4 boss+紫怪
    private int[] LayerCount = new int[] { 20, 15, 10, 8, 6 };

    private int MaxMonster = 20;
    private int total = 0;

    protected override RuleType ruleType => RuleType.Spirit;

    public BattleRule_Spirit(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        this.MapId = (int)mapId;
        this.LoadHero();

        AppHelper.Spirit_Id = MapId;
    }

    private void LoadHero()
    {
        HeroSpirit hero = new HeroSpirit();
        GameProcessor.Inst.PlayerManager.LoadHero(hero);

        Start = true;
        MapTime = 0;
        total = -MaxMonster;
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

        GameProcessor.Inst.EventCenter.Raise(new ShowSpiritInfoEvent() { Stage = CurrentLayer, Time = (int)MapTime, Count = Math.Max(0, total) });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (CurrentLayer == 1)
        {
            if (MapTime < MaxTime)
            {
                //刷怪
                if (enemys.Count < MaxMonster)
                {
                    int count = Math.Min(MaxMonster - enemys.Count, 3);
                    for (int i = 0; i < count; i++)
                    {
                        int quality = BuildQuality();
                        var enemy = new Monster_Spirit(MapId, quality);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }
                    total += count;
                }

                return;
            }
            else
            {
                CurrentLayer = 2; //时间满，进入第二阶段
                Next = true;
                return;
            }
        }
        else if (CurrentLayer == 2)
        {
            if (enemys.Count <= 0)
            {
                if (Next)
                {
                    Next = false;

                    for (int i = 0; i < 20; i++)
                    {
                        var enemy = new Monster_Spirit(MapId, 3);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }

                    return;
                }
                else
                {
                    CurrentLayer = 3;
                    Next = true;
                    return;
                }
            }
        }
        else if (CurrentLayer == 3)
        {
            if (enemys.Count <= 0)
            {
                if (Next)
                {
                    Next = false;

                    for (int i = 0; i < 10; i++)
                    {
                        var enemy = new Monster_Spirit(MapId, 4);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }

                    return;
                }
                else
                {
                    CurrentLayer = 4;
                    Next = true;
                    return;
                }
            }
        }
        else if (CurrentLayer == 4)
        {
            if (enemys.Count <= 0)
            {
                if (Next)
                {
                    Next = false;

                    for (int i = 0; i < 10; i++)
                    {
                        var enemy = new Monster_Spirit(MapId, 5);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }

                    return;
                }
                else
                {
                    CurrentLayer = 5;
                    //Next = true;
                    //this.BuildReward();
                    return;
                }
            }
        }



        if (CurrentLayer == 5 && enemys.Count <= 0)
        {
            //GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Spirit, Message = "挑战通关！" });
            BuildReward();

            GameProcessor.Inst.CloseBattle(RuleType.Spirit, 19);
        }
    }

    private int BuildQuality()
    {
        int rd = RandomHelper.RandomNumber(1, 21);

        if (rd >= 20)
        {
            return 3;
        }
        else if (rd >= 16)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void RefreshMonster()
    {

    }

    private void BuildReward()
    {
        this.Start = false;

        int stage = this.CurrentLayer - 1;

        if (stage <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Spirit,
                Message = BattleMsgHelper.BuildRewardMessage("挑战失败，没通关第一阶段，无奖励。", 0, 0, null),
            });
            return;
        }

        User user = GameProcessor.Inst.User;

        List<Item> items = new List<Item>();

        SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(this.MapId);

        List<SpiritDropConfig> dropList = SpiritDropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.MapId == this.MapId && m.Stage <= stage).ToList();

        //Debug.Log("spirit drop list :" + dropList.Count);

        IDictionary<int, int> dropDict = new Dictionary<int, int>();

        int rate = 1 + Math.Min(4, this.total / 500);
        for (int i = 0; i < rate; i++)
        {
            foreach (SpiritDropConfig sdpConfig in dropList)
            {
                if (RandomHelper.RandomRate(sdpConfig.DropRate))
                {
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(sdpConfig.DropId);
                    int index = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                    int dropId = dropConfig.ItemIdList[index];
                    if (!dropDict.ContainsKey(dropId))
                    {
                        dropDict[dropId] = 0;
                    }

                    dropDict[dropId]++;
                }
            }
        }

        dropDict.OrderBy(m => m.Key);

        foreach (var sp in dropDict)
        {
            items.Add(ItemHelper.BuildItem(ItemType.Spirit, sp.Key, 0, sp.Value));
        }

        string message = stage >= 4 ? "挑战通关" : "通过第" + stage + "阶段";
        message += ",累计积分" + total + "，获取" + rate + "倍奖励";

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Spirit,
            Message = BattleMsgHelper.BuildRewardMessage(message, 0, 0, items),
        });

        if (items.Count > 0)
        {
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        if (!user.SpiritOfflineFlag && stage >= 4)
        {  //没有启用的时候，刷新记录
            user.SpiritOfflineLog[1] = MapId;
            user.SpiritOfflineLog[2] = (int)MapTime;
            user.SpiritOfflineLog[3] = total;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Spirit,
                Message = BattleMsgHelper.BuildRewardMessage("已刷新通关记录", 0, 0, null),
            });
        }
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP <= 0)
        {
            if (Start)
            {
                this.BuildReward();
                //GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Spirit, Message = "挑战结束！" });
                GameProcessor.Inst.CloseBattle(RuleType.Spirit, 19);
            }
        }
    }
}
