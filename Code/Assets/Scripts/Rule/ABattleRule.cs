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


        protected const float roundTime = 0.5f;
        protected float currentRoundTime = 0f;
        protected bool needRefreshGraphic = false;

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

        abstract public void DoHeroLogic();
        abstract public void DoMonsterLogic();

        virtual public void DoValetLogic() {
            var valets = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Valet);

            foreach (var valet in valets)
            {
                valet.DoEvent();
            }
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
            this.currentRoundTime += Time.deltaTime * Time.timeScale;
                
            if (this.currentRoundTime >= roundTime)
            {
                this.currentRoundTime = 0;
                this.needRefreshGraphic = true;
                GameProcessor.Inst.PlayerManager.RemoveAllDeadPlayers();
                var roundType = (RoundType)(this.roundNum % 2);
                //GameProcessor.Inst.EventCenter.Raise(new HideAttackIcon (){RoundType = roundType});

                switch (roundType)
                {
                    case RoundType.Hero:
                        this.DoHeroLogic();
                        this.DoValetLogic();
                        break;
                    case RoundType.Monster:
                        this.DoMonsterLogic();
                        this.DoMapCellLogic();
                        break;
                }

                this.roundNum++;
            }
            else if (this.needRefreshGraphic && this.currentRoundTime >= roundTime * 0.5f)
            {
                this.needRefreshGraphic = false;
                var allPlayers = GameProcessor.Inst.PlayerManager.GetAllPlayers(true);
                foreach (var player in allPlayers)
                {
                    player.Logic.RaiseEvents();
                }
                this.CheckGameResult();
            }
        }

        protected void CheckGameResult()
        {
            var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
            if (heroCamp.HP == 0)
            {
                this.currentRoundTime = 0;

                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
                
                Log.Debug($"{(GameProcessor.Inst.winCamp == PlayerType.Hero?"玩家":"怪物")}获胜！！");
                GameProcessor.Inst.HeroDie(this.ruleType);
            }
        }
    }
}
