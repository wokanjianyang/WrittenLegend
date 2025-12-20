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

    private int MaxTime = 3;

    private int CurrentLayer = 1;  //此模式分4阶段，1 小兵 2 紫怪  3 boss 4 boss+紫怪
    private int[] LayerCount = new int[] { 20, 15, 10, 8, 6 };

    protected override RuleType ruleType => RuleType.Spirit;

    public BattleRule_Spirit(Dictionary<string, object> param)
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

        GameProcessor.Inst.EventCenter.Raise(new ShowSpiritInfoEvent() { Stage = CurrentLayer, Time = (int)MapTime });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (CurrentLayer == 1)
        {
            if (MapTime < MaxTime)
            {
                //刷怪
                if (enemys.Count < 20)
                {
                    for (int i = 0; i < 20 - enemys.Count; i++)
                    {
                        int quality = BuildQuality();
                        var enemy = new Monster_Spirit(MapId, quality);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }
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
                        var enemy = new Monster_Spirit(MapId, 4);
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
                        var enemy = new Monster_Spirit(MapId, 5);
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
                        var enemy = new Monster_Spirit(MapId, 4);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }
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
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Spirit, Message = "挑战通关！" });
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

        User user = GameProcessor.Inst.User;

        List<Item> items = new List<Item>();

        SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(this.MapId);

        int stage = this.CurrentLayer - 1;
        List<SpiritDropConfig> dropList = SpiritDropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.MapId == this.MapId && m.Stage <= stage).ToList();

        IDictionary<int, int> dropDict = new Dictionary<int, int>();

        for (int i = 0; i < 10; i++)
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

        foreach (var sp in dropDict)
        {
            items.Add(ItemHelper.BuildItem(ItemType.Spirit, sp.Key, 0, sp.Value));
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Spirit,
            Message = BattleMsgHelper.BuildRewardMessage("英灵奖励" + stage + "奖励:", 0, 0, items),
        });
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP <= 0)
        {
            this.BuildReward();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Spirit, Message = "挑战结束！" });


            GameProcessor.Inst.CloseBattle(RuleType.Spirit, 19);
        }
    }
}
