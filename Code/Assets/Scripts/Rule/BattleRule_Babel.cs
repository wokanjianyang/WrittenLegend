using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Babel : ABattleRule
{
    private bool Start = false;
    private bool Over = false;

    private long Progress = 0;

    private double TimeTotal = 180;

    private int[] MonsterList1 = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2 };
    private int[] MonsterList2 = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 2 };
    private int[] MonsterList3 = new int[] { 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 };

    protected override RuleType ruleType => RuleType.Babel;

    public BattleRule_Babel(Dictionary<string, object> param)
    {
        //param.TryGetValue("progress", out object progress);
        param.TryGetValue("Progress", out object progress);

        this.Progress = (long)progress;
        TimeTotal = 180;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (Over)
        {
            return;
        }

        if (!Start)
        {
            Start = true;
            int[] types = CalTypes(Progress);

            foreach (int type in types)
            {
                var RealBoss = new Monster_Babel(Progress, type);  //ˢ�±���,10������Ѫ
                GameProcessor.Inst.PlayerManager.LoadMonster(RealBoss);
            }

            return;
        }

        User user = GameProcessor.Inst.User;

        TimeTotal -= currentRoundTime;

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP <= 0 || TimeTotal <= 0)
        {
            Over = true;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "��սʧ�ܣ�" });
            GameProcessor.Inst.HeroDie(RuleType.Babel, 0);
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (!Over && enemys.Count <= 0)
        {
            //Over = true;

            user.BabelData.Data++;
            user.BabelCount.Data--;
            BuildReward(this.Progress);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "��" + Progress + "����ս�ɹ���" });

            if (user.BabelCount.Data <= 0)
            {
                GameProcessor.Inst.CloseBattle(RuleType.Babel, 0);
            }
            else
            {
                Start = false;
            }

            return;
        }
    }

    private int[] CalTypes(long progress)
    {
        if (progress % 100 == 0)
        {
            return MonsterList3;
        }
        else if (progress % 10 == 0)
        {

            return MonsterList2;
        }
        else
        {
            return MonsterList1;
        }
    }

    private void BuildReward(long progress)
    {
        BabelConfig rewardConfig = BabelConfigCategory.Instance.GetByProgress(progress);

        //�������
        List<Item> items = new List<Item>();
        items.Add(rewardConfig.BuildItem(progress));

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Babel,
            Message = BattleMsgHelper.BuildRewardMessage("ͨ��������:" + progress + "����:", 0, 0, items)
        });
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero != null && hero.HP == 0)
        {
            User user = GameProcessor.Inst.User;
            InfiniteRecord record = user.InfiniteData.GetCurrentRecord();

            if (record == null)
            {
                return;
            }

            record.Count.Data--;
            GameProcessor.Inst.EventCenter.Raise(new ShowInfiniteInfoEvent() { Count = record.Progress.Data, PauseCount = record.Count.Data });

            if (record.Count.Data > 0)
            {
                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
                GameProcessor.Inst.HeroDie(RuleType.Infinite, 0);
            }
            else
            {
                this.Over = false;
                user.InfiniteData.Complete();
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Infinite, Message = "�޾�����ʧ�ܣ�����������" });
                GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            }
        }
    }
}
