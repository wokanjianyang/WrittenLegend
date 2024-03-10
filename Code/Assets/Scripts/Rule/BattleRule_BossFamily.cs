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
    private int MapLayer = 1;
    private int MapRate = 1;

    private List<int> QualityList;

    private const int MaxQuanlity = 10; //最多数量
    private const int MaxFreshQuanlity = 1; //最多刷新数量
    protected override RuleType ruleType => RuleType.BossFamily;

    public Battle_BossFamily(Dictionary<string, object> param)
    {
        param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("MapLevel", out object mapLevel);
        param.TryGetValue("MapRate", out object mapRate);

        this.MapTime = (long)mapTime;
        this.MapLayer = (int)mapLevel;
        this.MapRate = (int)mapRate;
        this.Start = true;

        QualityList = new List<int>();

        List<BossConfig> list = BossConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Layer == MapLayer).ToList();

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

    public override void DoMapLogic(int roundNum)
    {
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        int bossCount = enemys.Count + QualityList.Count;

        if (enemys.Count < MaxQuanlity && QualityList.Count > 0)
        {
            int count = Math.Min(MaxFreshQuanlity, MaxQuanlity - enemys.Count);

            for (int i = 0; i < count; i++)
            {
                if (QualityList.Count > 0)
                {
                    BossConfig bossConfig = BossConfigCategory.Instance.Get(QualityList[0]);
                    GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(bossConfig.Id, bossConfig.MapId, RuleType.BossFamily, 2 * this.MapRate, 0));

                    QualityList.RemoveAt(0);
                }
            }
        }

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
