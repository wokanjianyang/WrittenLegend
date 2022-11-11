using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    abstract public class ABattleRule : MonoBehaviour,IBattleLife
    {
        protected bool isGameOver = false;

        protected int roundNum = 1;

        protected PlayerType winCamp;

        protected const float roundTime = 1f;
        protected float currentRoundTime = 0f;

        protected bool isBattleStart = false;

        virtual public void OnBattleStart()
        {
            this.isBattleStart = true;
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

        virtual public void Update()
        {
            if (!this.isGameOver && this.isBattleStart)
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
            var heroCamp = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Hero);
            if (heroCamp.Count == 0)
            {
                this.winCamp = PlayerType.Enemy;
                return true;
            }
            var enemyCamp = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
            if (enemyCamp.Count == 0)
            {
                this.winCamp = PlayerType.Hero;
                return true;
            }

            return false;
        }
    }
}
