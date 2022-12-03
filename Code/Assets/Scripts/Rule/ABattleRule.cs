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

        virtual public void OnUpdate()
        {
            if (!this.isGameOver)
            {
                this.currentRoundTime += Time.deltaTime;
                if (this.currentRoundTime >= roundTime * Time.timeScale)
                {
                    this.currentRoundTime = 0;
                    GameProcessor.Inst.PlayerManager.RemoveAllDeadPlayers();
                    switch (this.roundNum%2)
                    {
                        case (int)RoundType.Hero:
                            this.DoHeroLogic();
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
            var enemyCamp = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
            if (enemyCamp.Count == 0)
            {
                this.winCamp = PlayerType.Hero;
                result = true;
            }

            return result && this.roundNum > 1;
        }
    }
}
