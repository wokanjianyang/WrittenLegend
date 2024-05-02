using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_EquipCopy : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;
    private int MapRate = 1;
    private long MapTime = 0;

    private List<int> QualityList;

    private const int MaxQuanlity = 20; //最多数量
    private const int MaxFreshQuanlity = 1; //最多刷新数量
    protected override RuleType ruleType => RuleType.EquipCopy;

    public BattleRule_EquipCopy(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);
        param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("MapRate", out object mapRate);

        this.MapId = (int)mapId;
        this.MapTime = (long)mapTime;
        this.MapRate = (int)mapRate;
        this.Start = true;

        QualityList = new List<int>();

        for (int i = 0; i < 100; i++)
        {
            QualityList.Add(1);
        }
        for (int i = 0; i < 20; i++)
        {
            QualityList.Add(2);
        }
        for (int i = 0; i < 10; i++)
        {
            QualityList.Add(3);
        }
        for (int i = 0; i < 5; i++)
        {
            QualityList.Add(4);
        }
        for (int i = 0; i < 1; i++)
        {
            QualityList.Add(5);
        }

        //增加玉兔
        MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);

        List<MonsterSpecialConfig> configs = MonsterSpecialConfigCategory.Instance.GetAll().Values.Where(m => m.MapLevel == mapConfig.Level).ToList();
        foreach (MonsterSpecialConfig config in configs)
        {
            GameProcessor.Inst.PlayerManager.LoadMonster(new Monster_Specail(config.Id, MapRate, RuleType.EquipCopy));
        }

        TaskHelper.CheckTask(TaskType.ToCopy, 1);
    }

    public override void DoMapLogic(int roundNum)
    {
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);

        int mc1 = QualityList.Where(m => m == 1).Count() + enemys.Where(m => m.Quality == 1).Count();
        int mc2 = QualityList.Where(m => m == 2).Count() + enemys.Where(m => m.Quality == 2).Count();
        int mc3 = QualityList.Where(m => m == 3).Count() + enemys.Where(m => m.Quality == 3).Count();
        int mc4 = QualityList.Where(m => m == 4).Count() + enemys.Where(m => m.Quality == 4).Count();
        int mc5 = QualityList.Where(m => m == 5).Count() + enemys.Where(m => m.Quality == 5).Count();

        GameProcessor.Inst.EventCenter.Raise(new ShowCopyInfoEvent() { Mc1 = mc1, Mc2 = mc2, Mc3 = mc3, Mc4 = mc4, Mc5 = mc5 });

        if (enemys.Count < MaxQuanlity && QualityList.Count > 0)
        {
            int count = Math.Min(MaxFreshQuanlity, MaxQuanlity - enemys.Count);

            for (int i = 0; i < count; i++)
            {
                if (QualityList.Count > 0)
                {
                    if (QualityList[0] < 5)
                    {
                        var enemy = MonsterBaseCategory.Instance.BuildMonster(mapConfig, QualityList[0], MapRate, 0, RuleType.EquipCopy);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }
                    else
                    {
                        BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);
                        GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.BoosId, mapConfig.Id, RuleType.EquipCopy, MapRate, 0));
                    }
                    QualityList.RemoveAt(0);
                }
            }
        }

        if (Start && mc5 <= 0 && mc4 <= 0 && mc3 <= 0 && mc2 <= 0 && mc1 <= 0)
        {
            Start = false;

            User user = GameProcessor.Inst.User;
            if (user.MapId == mapConfig.Id)
            {
                //闯关成功
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildCopySuccessMessage(),
                    Type = RuleType.EquipCopy
                });

                user.MapId = mapConfig.Id + 1;
            }

            GameProcessor.Inst.HeroDie(RuleType.EquipCopy, MapTime);
        }
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.EquipCopy, MapTime);
        }
    }
}
