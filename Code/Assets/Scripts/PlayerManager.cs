
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class PlayerManager : MonoBehaviour, IBattleLife
    {

        public class ValetData
        {
            public int OwnerId { get; set; }
            public int SkillId { get; set; }
            public List<Valet> Valets { get; set; } = new List<Valet>();
        }
        private Hero hero;
        private List<APlayer> AllPlayers = new List<APlayer>();
        private int playerId = 0;
        private List<ValetData> valetCache = new List<ValetData>();

        public Hero GetHero()
        {
            return hero;
        }

        public Boss GetBoss()
        {
            var list = this.AllPlayers.FindAll(p => p.Camp == PlayerType.Enemy && p.ModelType == MondelType.Boss).ToList(); ;
            return list.Count > 0 ? list[0] as Boss : null;
        }

        private void AddPlayer(APlayer player)
        {
            player.ID = ++this.playerId;
            player.Transform.name = $"{player.Camp.ToString()}_{player.ID}";
            this.AllPlayers.Add(player);
        }

        public APlayer GetPlayer(int id)
        {
            return this.AllPlayers.FirstOrDefault(p => p.IsSurvice && p.ID == id);
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

            var tempCells = GameProcessor.Inst.MapData.AllCells.ToList();
            var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
            tempCells.RemoveAll(p => allPlayerCells.Contains(p));


            var index = RandomHelper.RandomNumber(0, tempCells.Count);
            var bornCell = tempCells[index];
            hero.SetPosition(bornCell, true);
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

                var cache = valetCache.FirstOrDefault(c => c.OwnerId == player.ID && c.SkillId == skill.SkillId);
                if (cache == null)
                {
                    valetCache.Add(new ValetData()
                    {
                        OwnerId = player.ID,
                        SkillId = skill.SkillId,
                        Valets = new List<Valet>(){valet}
                    });
                }
                else
                {
                    cache.Valets.Add(valet);
                }
                
            }
            
            return valet;
        }

        public void UnloadValet(APlayer player, SkillPanel skill)
        {
            var cache = valetCache.FirstOrDefault(c => c.OwnerId == player.ID && c.SkillId == skill.SkillId);
            if (cache != null)
            {
                foreach (var valet in cache.Valets)
                {
                    if (valet.IsSurvice)
                    {
                        valet.OnHit(new DamageResult(player.ID, valet.HP, MsgType.Damage));
                    }
                }
                cache.Valets.Clear();
            }
        }

        public List<Valet> GetValets(APlayer player, SkillPanel skill)
        {
            List<Valet> valets = new List<Valet>();
            var cache = valetCache.FirstOrDefault(c => c.OwnerId == player.ID && c.SkillId == skill.SkillId);
            if (cache != null)
            {
                foreach (var valet in cache.Valets)
                {
                    if (valet.IsSurvice)
                    {
                        valets.Add(valet);
                    }
                }
            }

            return valets;
        }

        public List<Valet> GetValets(APlayer player)
        {
            List<Valet> valets = new List<Valet>();
            var cache = valetCache.Where(c => c.OwnerId == player.ID).ToList();
            if (cache != null)
            {
                foreach (var vg in cache)
                {
                    foreach (var valet in vg.Valets)
                    {
                        if (valet.IsSurvice)
                        {
                            valets.Add(valet);
                        }
                    }
                }
            }
            return valets;
        }

        public Vector3Int RandomCell(Vector3Int currentCell)
        {
            var tempCells = GameProcessor.Inst.MapData.AllCells.ToList();
            var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
            tempCells.RemoveAll(p => allPlayerCells.Contains(p));

            if (tempCells.Count > 0)
            {
                var index = Random.Range(0, tempCells.Count);
                currentCell = tempCells[index];
            }

            return currentCell;
        }

        public void RemoveDeadPlayers(APlayer player)
        {
            this.AllPlayers.Remove(player);
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
            //UserData.Save();
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
