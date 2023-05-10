using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRule_Tower : ABattleRule
{
    private bool start = false;

    public override void DoHeroLogic()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        hero.DoEvent();
    }

    public override void DoMonsterLogic()
    {
        User user = GameProcessor.Inst.User;

        Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
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

        if (enemys.Count == 0) //TODO 测试减少刷新数量
        {
            if (start == true) { //爬塔成功
                MakeReward();
            }

            var monsters = MonsterTowerHelper.BuildMonster(user.TowerFloor);
            if (monsters != null && monsters.Count > 0)
            {
                start = true;
                monsters.ForEach(enemy =>
                {
                    GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                });
            }
        }
    }
    protected override bool CheckGameResult()
    {
        var result = false;
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP == 0)
        {
            this.winCamp = PlayerType.Enemy;
            result = true;
        }

        return result && this.roundNum > 1;
    }

    protected void MakeReward()
    {
        Log.Info("Tower Success");

        start = false;
        User user = GameProcessor.Inst.User;
        var config = TowerConfigCategory.Instance.Get(user.TowerFloor);

        //增加泡点经验，爬塔层数
        user.AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, config.TotalExp);
        user.TowerFloor++;

        //生成道具奖励
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildTowerSuccessMessage(config.RiseExp, user.TowerFloor),
            BattleType = BattleType.Tower
        });

        //存档
        UserData.Save();
    }
}
