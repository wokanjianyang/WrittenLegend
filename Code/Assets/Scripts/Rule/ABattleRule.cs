using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    abstract public class ABattleRule : IBattleLife
    {
        protected bool isGameOver = false;

        protected int roundNum = 0;

        protected PlayerType winCamp;

        protected const float roundTime = 1f;
        protected float currentRoundTime = 0f;
        protected bool needRefreshGraphic = false;
        virtual public void OnBattleStart()
        {
        }

        public int Order
        {
            get
            {
                return (int) ComponentOrder.BattleRule;
            }
        }

        abstract public void DoHeroLogic();
        abstract public void DoMonsterLogic();

        abstract public void DoValetLogic();

        virtual public void OnUpdate()
        {
            if (!this.isGameOver)
            {
                this.currentRoundTime += Time.deltaTime * Time.timeScale;
                
                if (this.currentRoundTime >= roundTime)
                {
                    this.currentRoundTime = 0;
                    this.needRefreshGraphic = true;
                    GameProcessor.Inst.PlayerManager.RemoveAllDeadPlayers();
                    switch (this.roundNum%2)
                    {
                        case (int)RoundType.Hero:
                            this.DoHeroLogic();
                            this.DoValetLogic();
                            break;
                        case (int)RoundType.Monster:
                            this.DoMonsterLogic();
                            break;
                    }

                    this.roundNum++;
                    this.isGameOver = this.CheckGameResult();
                    if (this.isGameOver)
                    {
                        Debug.Log($"{(this.winCamp == PlayerType.Hero?"玩家":"怪物")}获胜！！");
                    }
                }
                else if (this.needRefreshGraphic && this.currentRoundTime >= roundTime * 0.5f)
                {
                    this.needRefreshGraphic = false;
                    var allPlayers = GameProcessor.Inst.PlayerManager.GetAllPlayers(true);
                    foreach (var player in allPlayers)
                    {
                        player.Logic.RaiseEvents();
                    }
                }
            }
        }
        
        protected bool CheckGameResult()
        {
            var result = false;
            var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
            if (heroCamp.HP == 0)
            {
                this.winCamp = PlayerType.Enemy;
                result = true;
            }

            return result && this.roundNum > 1;
        }
    }
}
