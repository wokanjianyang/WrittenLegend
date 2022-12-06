
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerManager : MonoBehaviour, IBattleLife
    {

        public Hero hero;
        private List<APlayer> AllPlayers = new List<APlayer>();
        private int playerId = 0;

        public Hero GetHero()
        {
            return hero;
        }

        private void AddPlayer(APlayer player)
        {
            player.ID = this.playerId++;
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

        public void LoadHero(Dictionary<AttributeEnum, object> data = null)
        {
            if (data == null)
            {
                data = new Dictionary<AttributeEnum, object>();
                data[AttributeEnum.Color] = Color.white;
                data[AttributeEnum.Name] = "传奇";
                data[AttributeEnum.Level] = 1;
                data[AttributeEnum.HP] = 100L;
                data[AttributeEnum.PhyAtt] = 2;
            }
            hero.Load();
            hero.Logic.SetData(data);


            var coms = hero.Transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IPlayer _com)
                {
                    _com.SetParent(hero);
                }
            }
            hero.GetComponent<SkillProcessor>().InitSkill(hero);

            var x = GameProcessor.Inst.MapProcessor.ColCount / 2;
            var y = GameProcessor.Inst.MapProcessor.RowCount / 2;
            hero.SetPosition(new Vector3(x, y), true);
            this.AddPlayer(hero);

        }

        public APlayer LoadMonster(Dictionary<AttributeEnum, object> data = null)
        {
            APlayer enemy = null;
            if (data == null)
            {
                data = new Dictionary<AttributeEnum, object>();
                data[AttributeEnum.Color] = Color.red;
                data[AttributeEnum.Name] = "小怪";
                data[AttributeEnum.Level] = 1;
                data[AttributeEnum.HP] = 100L;
                data[AttributeEnum.PhyAtt] = 1f;
            }

            var tempCells = GameProcessor.Inst.MapProcessor.AllCells.ToList();
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

                enemy = new Monster();
                enemy.Load();
                enemy.Logic.SetData(data);

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

        public void RemoveAllDeadPlayers()
        {
            for (var i = this.AllPlayers.Count - 1; i >= 0; i--)
            {
                var player = this.AllPlayers[i];
                if (!player.IsSurvice)
                {
                    player.Transform.gameObject.SetActive(false);
                    this.AllPlayers.RemoveAt(i);
                }
            }
        }

        public void Save()
        {
            UserData.Save(this.hero);
        }

        void OnDestroy()
        {
        }
    }
}
