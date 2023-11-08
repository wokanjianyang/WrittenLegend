using Assets.Scripts;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game
{
    abstract public class ABattleRule : IBattleLife
    {

        protected int roundNum = 0;


        protected const float roundTime = 0.2f;

        protected float currentRoundTime = 0f;
        //protected bool needRefreshGraphic = false;

        virtual protected RuleType ruleType => RuleType.Normal;
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

        //abstract public void DoHeroLogic();
        //abstract public void DoMonsterLogic();

        //virtual public void DoValetLogic() {
        //    var valets = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Valet);

        //    foreach (var valet in valets)
        //    {
        //        //valet.DoEvent();
        //    }
        //}

        virtual public void DoMapLogic(int roundNum) { 

        }
        public void DoMapCellLogic()
        {
            var cells = GameProcessor.Inst.MapData.MapCells.ToList();
            var skillCells = cells.FindAll(m => m.skills.Count > 0);
            foreach (MapCell cell in cells)
            {
                if (cell.skills.Count > 0)
                {
                    cell.DoEvent();
                }
            }
        }


        virtual public void OnUpdate()
        {
            this.currentRoundTime += Time.unscaledDeltaTime;
            if (this.currentRoundTime >= roundTime)
            {
                this.currentRoundTime = 0;
                this.DoMapLogic(roundNum);


                var roundType = this.roundNum % 5;
                switch (roundType)
                {
                    case 0:
                        this.DoMapCellLogic();
                        break;
                    case 1:
                        this.CheckGameResult();
                        break;
                }

                if (this.roundNum % 25 == 0)
                {
                    GameProcessor.Inst.PlayerManager.RemoveAllDeadPlayers();
                }

                this.roundNum++;
            }
        }

        virtual public void CheckGameResult()
        {
            var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
            if (heroCamp.HP == 0)
            {
                this.currentRoundTime = 0;

                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
                
                Log.Debug($"{(GameProcessor.Inst.winCamp == PlayerType.Hero?"玩家":"怪物")}获胜！！");
                GameProcessor.Inst.HeroDie(this.ruleType,0);
            }
        }
    }
}
