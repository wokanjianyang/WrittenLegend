using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    abstract public class ABattleRule : MonoBehaviour,IBattleLife
    {
        private bool isGameOver = false;

        private int roundNum = 1;

        private PlayerType winCamp;

        private const float roundTime = 1f;
        private float currentRoundTime = 0f;

        private bool isBattleStart = false;

        
        public void OnBattleStart()
        {
            this.isBattleStart = true;
        }

        abstract public void DoHeroLogic();
        abstract public void DoMonsterLogic();

        private void Update()
        {
            if (!this.isGameOver && this.isBattleStart)
            {
                this.currentRoundTime += Time.deltaTime;
                if (this.currentRoundTime >= roundTime * Time.timeScale)
                {
                    this.currentRoundTime = 0;
                
                    switch (this.roundNum%2)
                    {
                        case 0:
                            this.DoHeroLogic();
                            break;
                        case 1:
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
        
        private bool CheckGameResult()
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
