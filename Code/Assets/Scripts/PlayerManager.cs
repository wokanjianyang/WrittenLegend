
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerManager : MonoBehaviour, IBattleLife
    {

        private Hero hero;
        private List<APlayer> AllPlayers = new List<APlayer>();
        private int playerId = 0;

        public Hero GetHero()
        {
            return hero;
        }

        private void AddPlayer(APlayer player)
        {
            player.ID = ++this.playerId;
            this.AllPlayers.Add(player);
        }

        public APlayer GetPlayer(int id)
        {
            return this.AllPlayers.FirstOrDefault(p => p.ID == id);
        }

        public APlayer GetPlayer(Vector3Int cell)
        {
            return this.AllPlayers.FirstOrDefault(p => p.Cell == cell);
        }

        public List<APlayer> GetAllPlayers(bool includDeath = false)
        {
            var playerList = new List<APlayer>();
            foreach (var player in this.AllPlayers)
            {
                if (includDeath)
                {
                    playerList.Add(player);
                }
                else if (player.IsSurvice)
                {
                    playerList.Add(player);
                }
            }
            return playerList;
        }

        public List<APlayer> GetPlayersByCamp(PlayerType camp, bool isSurvice = true)
        {
            return this.AllPlayers.FindAll(p => p.Camp == camp && p.IsSurvice == isSurvice);
        }

        public bool IsCellCanMove(Vector3Int cell)
        {
            var allCells = this.AllPlayers.Where(p => p.IsSurvice).Select(p => p.Cell).ToList();
            return !allCells.Contains(cell);
        }

        public void OnBattleStart()
        {
        }

        public int Order
        {
            get
            {
                return (int)ComponentOrder.PlayerManager;
            }
        }

        public void LoadHero()
        {
            hero = new Hero();

            var coms = hero.Transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IPlayer _com)
                {
                    _com.SetParent(hero);
                }
            }
            hero.GetComponent<SkillProcessor>().InitSkill(hero);

            var x = GameProcessor.Inst.MapData.ColCount / 2;
            var y = GameProcessor.Inst.MapData.RowCount / 2;
            hero.SetPosition(new Vector3(x, y), true);
            this.AddPlayer(hero);

        }

        public APlayer LoadMonster(APlayer enemy)
        {
            var tempCells = GameProcessor.Inst.MapData.AllCells.ToList();
            var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
            tempCells.RemoveAll(p => allPlayerCells.Contains(p));

            if (tempCells.Count > 0)
            {
                var bornCell = Vector3Int.zero;
                if (tempCells.Count > 1)
                {
                    var index = UnityEngine.Random.Range(0, tempCells.Count);
                    bornCell = tempCells[index];
                }
                else
                {
                    bornCell = tempCells[0];
                }


                var coms = enemy.Transform.GetComponents<MonoBehaviour>();
                foreach (var com in coms)
                {
                    if (com is IPlayer _com)
                    {
                        _com.SetParent(enemy);
                    }
                }
                enemy.GetComponent<SkillProcessor>().InitSkill(enemy);
                enemy.SetPosition(bornCell, true);
                this.AddPlayer(enemy);
            }

            return enemy;
        }

        public Valet LoadValet(APlayer player,SkillPanel skill)
        {
            Valet valet = null;

            var centerCell = hero.Cell;

            var tempCells = GameProcessor.Inst.MapData.AllCells.ToList();
            var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
            tempCells.RemoveAll(p => allPlayerCells.Contains(p));

            tempCells = tempCells.OrderBy(m => Mathf.Abs(m.x - centerCell.x) + Mathf.Abs(m.y - centerCell.y) + Mathf.Abs(m.z - centerCell.z)).ToList();

            if (tempCells.Count > 0)
            {
                var bornCell = tempCells[0];

                valet = new Valet(player,skill);

                var coms = valet.Transform.GetComponents<MonoBehaviour>();
                foreach (var com in coms)
                {
                    if (com is IPlayer _com)
                    {
                        _com.SetParent(valet);
                    }
                }

                valet.SetPosition(bornCell, true);
                this.AddPlayer(valet);
            }

            return valet;
        }

        public void RemoveAllDeadPlayers()
        {
            for (var i = this.AllPlayers.Count - 1; i >= 0; i--)
            {
                var player = this.AllPlayers[i];
                if (!player.IsSurvice)
                {
                    //player.Transform.gameObject.SetActive(false);
                    player.OnDestroy();
                    this.AllPlayers.RemoveAt(i);
                }
            }
        }

        public void Save()
        {
            UserData.Save();
        }

        public void OnDestroy()
        {
            foreach (var player in this.AllPlayers)
            {
                player.OnDestroy();
            }
            this.AllPlayers.Clear();
        }
    }
}
