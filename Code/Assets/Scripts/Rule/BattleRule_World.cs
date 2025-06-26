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

    protected override RuleType ruleType => RuleType.Myth;

    private WorldConfig Config;

    public BattleRule_World(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);
        param.TryGetValue("Layer", out object layer);

        this.MapId = (int)mapId;
        this.Layer = (int)layer;

        this.Config = WorldConfigCategory.Instance.Get(MapId);

        this.Load();
    }

    private void Load()
    {
        Start = true;

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

            User user = GameProcessor.Inst.User;

            int type = 100 + MapId;
            long progess = user.GetRecordData(type);

            int ap = 1;
            if (progess > this.Layer)  //如果历史最高记录大于当前，跳关
            {
                long ar = progess / 400 + 1;
                ap = (int)Math.Min(progess - this.Layer, ar * 5);
            }

            //Debug.Log("this Layer:" + this.Layer + " progress :" + progess + " ap:" + ap);

            if (this.Layer > progess) //如果当前进度大于历史记录，更新历史最高记录
            {
                user.SaveRecordData(type, this.Layer);
            }

            for (int i = 0; i < ap; i++)
            {
                BuildReward(MapId, this.Layer + i);
            }

            user.WorldData.SetOver(this.MapId, ap);

            GameProcessor.Inst.CloseBattle(RuleType.World, 14);
        }
    }

    private void BuildReward(int mapId, int layer)
    {
        //Debug.Log("BuildReward layer:" + layer);

        User user = GameProcessor.Inst.User;

        //掉落道具
        List<Item> items = new List<Item>();

        //掉落道具
        int itemId = user.WorldData.GetDropId(mapId, layer);
        if (itemId > 0)
        {
            items.Add(ItemHelper.BuildItem(ItemType.Material, itemId, 1, 1));
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.World,
            Message = BattleMsgHelper.BuildRewardMessage(Config.MapName + layer + "轮奖励:", 0, 0, items)
        });

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
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
