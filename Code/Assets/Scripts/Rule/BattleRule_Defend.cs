using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_Defend : ABattleRule
{
    private bool Start = true;

    private long Progress = 0;

    private const int MaxProgress = 100; //

    protected override RuleType ruleType => RuleType.Defend;

    public Battle_Defend(Dictionary<string, object> param)
    {
        param.TryGetValue("progress", out object progress);
        param.TryGetValue("hp", out object hp);

        Debug.Log("record:" + progress + "," + hp);

        this.Progress = (long)progress;

        this.LoadDefend((long)hp);
    }

    private Defend defendPlayer = null;

    private void LoadDefend(long hp)
    {
        this.defendPlayer = new Defend(hp);
        GameProcessor.Inst.PlayerManager.LoadDefend(this.defendPlayer);

        this.Start = true;
    }

    public override void DoMapLogic(int roundNum)
    {
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        if (enemys.Count <= 0 && this.Progress < MaxProgress && this.Start)
        {
            //Load All
            for (int i = 0; i < 10; i++)
            {
                int tempMapId = RandomHelper.RandomNumber(0, 5);
                tempMapId = 1001 + tempMapId;

                MapConfig mapConfig = MapConfigCategory.Instance.Get(tempMapId);
                var enemy = MonsterHelper.BuildMonster(mapConfig, 1, 1, 0);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowDefendInfoEvent() { Count = Progress });

            this.Start = false;

            return;
        }

        User user = GameProcessor.Inst.User;

        if (enemys.Count <= 0 && !this.Start)
        {
            this.Start = true;
            this.Progress++;

            DefendRecord record = user.DefendData.GetCurrentRecord();

            record.Progress.Data = this.Progress;
            record.Hp.Data = defendPlayer.HP;

            return;
        }

        if (this.Progress >= MaxProgress && this.Start)
        {
            user.DefendData.Complete();

            GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = "Success" });

            GameProcessor.Inst.HeroDie(RuleType.Phantom, 0);

            return;
        }
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Defend, 0);
        }
        

        var defend = GameProcessor.Inst.PlayerManager.GetHero();
    }
}
