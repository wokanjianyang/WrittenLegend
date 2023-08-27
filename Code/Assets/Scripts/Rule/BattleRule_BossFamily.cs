using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_BossFamily : ABattleRule
{
    private bool Start = false;

    private long MapTime = 0;

    private List<int> QualityList;

    private const int MaxQuanlity = 10; //�������
    private const int MaxFreshQuanlity = 5; //���ˢ������
    protected override RuleType ruleType => RuleType.BossFamily;

    public Battle_BossFamily(Dictionary<string, object> param)
    {
        param.TryGetValue("MapTime", out object mapTime);

        this.MapTime = (long)mapTime;
        this.Start = true;

        QualityList = new List<int>();

        List<BossConfig> list = BossConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            int bossId = list[i].Id;
            QualityList.Add(bossId);
            QualityList.Add(bossId);
            QualityList.Add(bossId);
            QualityList.Add(bossId);
            QualityList.Add(bossId);
        }

        TaskHelper.CheckTask(TaskType.ToCopy, 1);
    }

    public override void DoHeroLogic()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        hero.DoEvent();
    }

    public override void DoMonsterLogic()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        enemys.Sort((a, b) =>
        {
            var distance = a.Cell - hero.Cell;
            var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

            distance = b.Cell - hero.Cell;
            var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

            if (l0 < l1)
            {
                return -1;
            }
            else if (l0 > l1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        });

        foreach (var enemy in enemys)
        {
            enemy.DoEvent();
        }


        if (enemys.Count < MaxQuanlity && QualityList.Count > 0)
        {
            int count = Math.Min(MaxFreshQuanlity, MaxQuanlity - enemys.Count);

            for (int i = 0; i < count; i++)
            {
                if (QualityList.Count > 0)
                {

                    BossConfig bossConfig = BossConfigCategory.Instance.Get(QualityList[0]);
                    GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(bossConfig.Id, bossConfig.MapId, 2));

                    QualityList.RemoveAt(0);
                }
            }
        }

        int bossCount = enemys.Count + QualityList.Count;
        GameProcessor.Inst.EventCenter.Raise(new ShowBossFamilyInfoEvent() { Count = bossCount });

        if (Start && enemys.Count <= 0 && QualityList.Count <= 0)
        {
            Start = false;


            GameProcessor.Inst.HeroDie(RuleType.BossFamily, MapTime);
        }
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.BossFamily, MapTime);
        }
    }
}