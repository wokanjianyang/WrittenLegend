using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Miner
{
    public int Seed { get; set; }

    public string Name { get; set; }

    public long BirthDay = 0;

    public Miner()
    {

    }

    public void Init(string name)
    {
        this.BirthDay = DateTime.Now.Second;
        this.Seed = RandomHelper.RandomNumber(1, 1000000);
        this.Name = name;

        Debug.Log("init seed :" + Seed);
    }

    public void InlineBuild()
    {
        System.Random random = new System.Random(this.Seed);

        List<MineConfig> mines = MineConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        int min = mines.Select(m => m.EndRate).Min();
        int max = mines.Select(m => m.EndRate).Max();

        int ns = random.Next(min, max + 1);

        this.Seed = ns;

        MineConfig mine = mines.Where(m => m.StartRate <= ns && ns <= m.EndRate).FirstOrDefault();

        string message = "¿ó¹¤" + Name + " ÍÚµ½ÁË" + mine.Name;
        Debug.Log("seed :" + Seed + "mine:" + mine.Name);
    }

    public void OfflineBuild()
    {

    }

}
