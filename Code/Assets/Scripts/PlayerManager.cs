
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerManager : MonoBehaviour,IBattleLife
    {

        private List<APlayer> AllPlayers =  new List<APlayer>();

        public void AddPlayer(APlayer player)
        {
            player.ID = this.AllPlayers.Count;
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

        public List<APlayer> GetPlayersByCamp(PlayerType camp,bool isSurvice = true)
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
            this.LoadHero();
        }

        public int Order
        {
            get
            {
                return (int)ComponentOrder.PlayerManager;
            }
        }

        private void LoadHero(Dictionary<AttributeEnum, object> data = null)
        {
            if (data == null)
            {
                data  = new Dictionary<AttributeEnum, object>();
                data[AttributeEnum.Color] = Color.white;
                data[AttributeEnum.Name] = "传奇";
                data[AttributeEnum.Level] = 1;
                data[AttributeEnum.HP] = 100;
                data[AttributeEnum.PhyAtt] = 2;
            }
            
            var hero = new Hero();
            hero.Logic.SetData(data);

            var coms = hero.Transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IPlayer _com)
                {
                    _com.SetParent(hero);
                }
            }
            hero.GetComponent<SkillProcessor>().AddSkill(hero,new SkillData()
            {
                ID = 10001,
                Name = "斩血",
                CD = 1
            });
            hero.SetPosition(new Vector3(0,0),true);
            GameProcessor.Inst.PlayerManager.AddPlayer(hero);
            
        }

        public APlayer LoadMonster(Dictionary<AttributeEnum, object> data = null)
        {
            APlayer enemy = null;
            if (data == null)
            {
                data  = new Dictionary<AttributeEnum, object>();
                data[AttributeEnum.Color] = Color.red;
                data[AttributeEnum.Name] = "小怪";
                data[AttributeEnum.Level] = 1;
                data[AttributeEnum.HP] = 100f;
                data[AttributeEnum.PhyAtt] = 1f;
            }
            
            var tempCells = GameProcessor.Inst.MapProcessor.AllCells.ToList();
            var allPlayerCells = GameProcessor.Inst.PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
            tempCells.RemoveAll(p => allPlayerCells.Contains(p));
            
            if (tempCells.Count>0)
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
                enemy.Logic.SetData(data);

                var coms = enemy.Transform.GetComponents<MonoBehaviour>();
                foreach (var com in coms)
                {
                    if (com is IPlayer _com)
                    {
                        _com.SetParent(enemy);
                    }
                }
                enemy.GetComponent<SkillProcessor>().AddSkill(enemy,new SkillData()
                {
                    ID = 10002,
                    Name = "冰冻",
                    CD = 3
                });
                enemy.SetPosition(bornCell,true);
                GameProcessor.Inst.PlayerManager.AddPlayer(enemy);
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
    }
}
