using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class BattleRule_Survivors : ABattleRule, IPointerClickHandler
    {
        private PlayerActionType actionType = PlayerActionType.None;
        private Vector3Int lastClickCell = default;
        private float defaultWaitInputTime = 5f;

        public override void DoHeroLogic()
        {
            this.actionType = PlayerActionType.None;

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            
            if (this.lastClickCell != default && this.lastClickCell != hero.Cell)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(hero.Cell, lastClickCell);
                if (endPos == hero.Cell)
                {
                    this.lastClickCell = hero.Cell;
                    hero.DoEvent();
                }
                else
                {
                    if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                    {
                        hero.Move(endPos);
                    }
                }
            }
            else
            {
                hero.DoEvent();
            }
        }

        public override void DoValetLogic()
        {
        }
        public override void DoMonsterLogic()
        {
            var heros = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Hero);
            if (heros != null && heros.Count > 0)
            {
                var deadEnemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy, false);
                foreach (var deadEnemy in deadEnemys)
                {
                    deadEnemy.DoEvent();
                    if (deadEnemy.GetComponent<HalfLife>().IsCanRevive())
                    {
                        deadEnemy.Logic.ResetData();
                    }
                }

                var hero = heros[0];
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
                var monster = MonsterHelper.BuildMonster(hero.Level);

                var player = GameProcessor.Inst.PlayerManager.LoadMonster(monster);
                if (player != null)
                {
                    var halfLife = player.Transform.gameObject.AddComponent<HalfLife>();
                    halfLife.SetParent(player);
                }
            }
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            this.lastClickCell = hero.Cell;
        }

        override public void OnUpdate()
        {
            if (!this.isGameOver && GameProcessor.Inst.IsGameRunning())
            {
                this.currentRoundTime += Time.deltaTime;
                if (this.currentRoundTime >= roundTime * Time.timeScale)
                {
                    switch (this.roundNum % 2)
                    {
                        case (int) RoundType.Hero:
                            switch (this.actionType)
                            {
                                case PlayerActionType.None:
                                {
                                    this.actionType = PlayerActionType.WaitingInput;
                                    var heros = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Hero);
                                    var hero = heros[0];
                                    hero.EventCenter.Raise(new ShowMsgEvent()
                                    {
                                        Content = "等待操作"
                                    });
                                }
                                    break;
                                case PlayerActionType.WaitingInput:
                                    if (this.currentRoundTime >= this.defaultWaitInputTime)
                                    {
                                        this.actionType = PlayerActionType.InputEnd;
                                    }

                                    break;
                                case PlayerActionType.InputEnd:
                                    this.DoHeroLogic();
                                    this.roundNum++;
                                    this.currentRoundTime = 0;
                                    break;
                            }

                            break;
                        case (int) RoundType.Monster:
                            this.DoMonsterLogic();
                            this.roundNum++;
                            this.currentRoundTime = 0;
                            break;
                    }

                    // this.isGameOver = this.CheckGameResult();
                    if (this.isGameOver)
                    {
                        Debug.Log($"{(this.winCamp == PlayerType.Hero ? "玩家" : "怪物")}获胜！！");
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.actionType == PlayerActionType.WaitingInput)
            {
                var pressPos = eventData.position;
                this.lastClickCell = GameProcessor.Inst.MapData.GetLocalCell(pressPos);
                this.actionType = PlayerActionType.InputEnd;
            }
        }
    }
}
