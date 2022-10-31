using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public APlayer Hero;
    
    public Transform MapRoot { get; set; }

    private bool isGameOver = false;

    private int roundNum = 0;

    private PlayerType winCamp;
    
    // Start is called before the first frame update
    void Start()
    {
        this.MapRoot = GameObject.Find("Map").transform;
        
        var prefab = Resources.Load<GameObject>("Prefab/Level/Level_0");
        var level = GameObject.Instantiate(prefab, this.MapRoot);
        
        this.Hero = new Hero();
        this.Hero.Transform.position = new Vector3(0, 0);
        this.Hero.SetPosition(new Vector3(0, 0));
        this.Hero.Transform.SetParent(this.MapRoot);
        PlayerManager.Inst.AddPlayer(this.Hero);

        for (var i = 0; i < 5; i++)
        {
            var enemy = new Monster();
            enemy.SetPosition(new Vector3(1, i));
            enemy.Transform.SetParent(this.MapRoot);
            PlayerManager.Inst.AddPlayer(enemy);
        }

        StartCoroutine(this.DoLogic());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DoLogic()
    {
        while (!this.isGameOver)
        {
            this.isGameOver = this.CheckGameResult();
            if (!this.isGameOver)
            {
                switch (this.roundNum%2)
                {
                    case 0:
                        this.Hero.DoEvent();
                        break;
                    case 1:
                        var enemys = PlayerManager.Inst.GetPlayersByCamp(PlayerType.Enemy);
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
                        break;
                }
            
                this.roundNum++;

                yield return new WaitForSeconds(1f);
            }
        }
        
        Debug.Log($"{(this.winCamp == PlayerType.Hero?"英雄":"怪物")}队伍获胜！！！");
    }

    private bool CheckGameResult()
    {
        var heroCamp = PlayerManager.Inst.GetPlayersByCamp(PlayerType.Hero);
        if (heroCamp.Count == 0)
        {
            this.winCamp = PlayerType.Enemy;
            return true;
        }
        var enemyCamp = PlayerManager.Inst.GetPlayersByCamp(PlayerType.Enemy);
        if (enemyCamp.Count == 0)
        {
            this.winCamp = PlayerType.Hero;
            return true;
        }

        return false;
    }
}
