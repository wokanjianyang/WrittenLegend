using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_Defend : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    private long Progress = 0;

    private const int MaxProgress = 100; //

    private int[] MonsterList = new int[] { 5, 4, 4, 4, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1 };

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
        if (!this.Over)
        {
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        if (enemys.Count <= 0 && this.Progress <= MaxProgress && this.Start)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "第" + this.Progress + "波发起了进攻" });

            //Load All
            for (int i = 0; i < MonsterList.Length; i++)
            {
                var enemy = new Monster_Defend(this.Progress, MonsterList[i]);
                GameProcessor.Inst.PlayerManager.LoadMonsterDefend(enemy);
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowDefendInfoEvent() { Count = Progress });

            this.Start = false;

            return;
        }

        User user = GameProcessor.Inst.User;

        if (enemys.Count <= 0 && !this.Start)
        {
            //check 
            long progess = user.GetAchievementProgeress(AchievementSourceType.Defend);
            if (progess < this.Progress)
            {
                user.MagicRecord[AchievementSourceType.Defend].Data = this.Progress;
            }

            this.Start = true;
            this.Progress++;

            DefendRecord record = user.DefendData.GetCurrentRecord();

            record.Progress.Data = this.Progress;
            record.Hp.Data = defendPlayer.HP;

            return;
        }

        if (this.Progress > MaxProgress && this.Over)
        {
            this.Over = false;

            user.DefendData.Complete();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫成功" });

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);

            return;
        }
    }

    public override void CheckGameResult()
    {
        var defend = GameProcessor.Inst.PlayerManager.GetDefend();
        if (defend.HP <= 0)
        {
            this.Over = false;
            GameProcessor.Inst.User.DefendData.Complete();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫失败" });

            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);
        }
    }
}
