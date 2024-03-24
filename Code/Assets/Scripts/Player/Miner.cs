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

    public MineConfig InlineBuild(long nowSecond)
    {
        if (this.BirthDay >= nowSecond)
        {
            return null;
        }

        List<MineConfig> mines = MineConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        int min = mines.Select(m => m.StartRate).Min();
        int max = mines.Select(m => m.EndRate).Max();

        int ward = RandomHelper.RandomNumber(this.Seed, min, max + 1);
        MineConfig mine = mines.Where(m => m.StartRate <= ward && ward <= m.EndRate).FirstOrDefault();

        this.Seed = AppHelper.RefreshSeed(this.Seed);
        this.BirthDay = nowSecond + RandomHelper.RandomNumber(0, 20);

        return mine;
    }

    public void OfflineBuild(long time, Dictionary<int, long> offlineMetal)
    {
        long count = time / 60;

        for (int i = 0; i < count; i++)
        {
            List<MineConfig> mines = MineConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            int min = mines.Select(m => m.StartRate).Min();
            int max = mines.Select(m => m.EndRate).Max();

            int ward = RandomHelper.RandomNumber(this.Seed, min, max + 1);
            MineConfig mine = mines.Where(m => m.StartRate <= ward && ward <= m.EndRate).FirstOrDefault();
            int key = mine.Id;

            if (!offlineMetal.ContainsKey(key))
            {
                offlineMetal[key] = 0;
            }

            offlineMetal[key]++;
            this.Seed = AppHelper.RefreshSeed(this.Seed);
        }

        this.BirthDay = TimeHelper.ClientNowSeconds() + RandomHelper.RandomNumber(0, 20); ;
    }

}
