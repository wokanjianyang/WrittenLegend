using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameProcessor : MonoBehaviour
    {
        public static GameProcessor Inst { get; private set; } = null;
        
        public APlayer Hero;
        
        public MapProcessor MapProcessor { get; private set; }
        public PlayerManager PlayerManager { get; private set; }
        public Transform MapRoot { get; private set; }


        private bool isGameOver = false;

        private int roundNum = 1;

        private PlayerType winCamp;

        private Vector3Int enemyBornCell = new Vector3Int(8, 15, 0);
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Inst = this;
            //加载地图
            this.LoadMap();
            //            
            PlayerManager = this.gameObject.AddComponent<PlayerManager>();

        }

        // Start is called before the first frame update
        void Start()
        {
            //创建主角
            this.CreateHero();

            StartCoroutine(this.DoLogic());
        }

        private void LoadMap()
        {
            MapProcessor = GameObject.Find("Canvas").GetComponentInChildren<MapProcessor>();
            
            this.MapRoot = new GameObject().transform;
            this.MapRoot.SetParent(GameObject.Find("Canvas").transform,false);
        }

        private void CreateHero()
        {
            this.Hero = new Hero();
            this.Hero.SetPosition(new Vector3(0,0),true);
            PlayerManager.AddPlayer(this.Hero);
            
            var data  = new Dictionary<AttributeEnum, object>();
            data[AttributeEnum.Color] = Color.white;
            data[AttributeEnum.Name] = "传奇";
            data[AttributeEnum.Level] = 1;
            data[AttributeEnum.HP] = 100;
            data[AttributeEnum.Atk] = 2;
            this.Hero.Logic.SetData(data);
        }

        IEnumerator DoLogic()
        {
            while (!this.isGameOver)
            {
                switch (this.roundNum%2)
                {
                    case 0:
                        this.Hero.DoEvent();
                        break;
                    case 1:
                        var enemys = PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
                        enemys.Sort((a, b) =>
                        {
                            var distance = a.Cell - this.Hero.Cell;
                            var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);
            
                            distance = b.Cell - this.Hero.Cell;
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

                        var tempCells = MapProcessor.allCells.ToList();
                        var allPlayerCells = PlayerManager.GetAllPlayers().Select(p => p.Cell).ToList();
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
                            var enemy = new Monster();
                            enemy.SetPosition(bornCell,true);
                            PlayerManager.AddPlayer(enemy);
                            
                            
                            var data  = new Dictionary<AttributeEnum, object>();
                            data[AttributeEnum.Color] = Color.red;
                            data[AttributeEnum.Name] = "小怪" + (enemys.Count + 1);
                            data[AttributeEnum.Level] = 1;
                            data[AttributeEnum.HP] = 100f;
                            data[AttributeEnum.Atk] = 1f;
                            enemy.Logic.SetData(data);
                        }
                        break;
                }
            
                this.roundNum++;
                this.isGameOver = this.CheckGameResult();

                yield return new WaitForSeconds(1f);
            }
            
            Debug.Log($"{(this.winCamp == PlayerType.Hero?"英雄":"怪物")}队伍获胜！！！");
        }

        private bool CheckGameResult()
        {
            var heroCamp = PlayerManager.GetPlayersByCamp(PlayerType.Hero);
            if (heroCamp.Count == 0)
            {
                this.winCamp = PlayerType.Enemy;
                return true;
            }
            var enemyCamp = PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
            if (enemyCamp.Count == 0)
            {
                this.winCamp = PlayerType.Hero;
                return true;
            }

            return false;
        }

    }
}
