using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class MineConfigCategory
    {
        public Dictionary<int, int> BuildMetal(ref int seed, long count)
        {
            //Debug.Log("BuildMetal seed:" + seed);
            Dictionary<int, int> drops = new Dictionary<int, int>();

            RandomMetal0(drops, count, seed);

            RandomMetal1(drops, count, seed);

            return drops;
        }

        public void RandomMetal0(Dictionary<int, int> drops, long count, int seed)
        {
            User user = GameProcessor.Inst.User;

            int levelN = user.GetLimitMineCount();

            List<MineConfig> mines = this.list.Where(m => m.Type == 0 && m.RequireLevel <= levelN).ToList();
            int max = mines.Select(m => m.Rate).Sum();

            for (int i = 0; i < levelN * count; i++)
            {
                seed = AppHelper.RefreshSeed(seed);

                int rd = RandomHelper.RandomNumber(seed, 0, max);

                int metalId = 0;
                int endRate = 0;
                for (int j = 0; j < mines.Count; j++)
                {
                    endRate += mines[j].Rate;

                    if (rd <= endRate)
                    {
                        metalId = mines[j].Id;
                        break;
                    }
                }

                if (metalId > 0)
                {
                    if (!drops.ContainsKey(metalId))
                    {
                        drops[metalId] = 0;
                    }
                    drops[metalId]++;
                }
            }
        }

        public void RandomMetal1(Dictionary<int, int> drops, long count, int seed)
        {
            User user = GameProcessor.Inst.User;

            int levelS = user.GetLimitMineCount2();

            List<MineConfig> mines = this.list.Where(m => m.RequireLevel <= levelS).ToList();
            int max = mines.Select(m => m.Rate + m.RiseRate).Sum();

            for (int i = 0; i < levelS * count; i++)
            {
                seed = AppHelper.RefreshSeed(seed);

                int rd = RandomHelper.RandomNumber(seed, 0, max);

                int metalId = 0;
                int endRate = 0;
                for (int j = 0; j < mines.Count; j++)
                {
                    endRate += mines[j].Rate + mines[j].RiseRate;

                    if (rd <= endRate)
                    {
                        metalId = mines[j].Id;
                        break;
                    }
                }

                if (metalId > 0)
                {
                    if (!drops.ContainsKey(metalId))
                    {
                        drops[metalId] = 0;
                    }
                    drops[metalId]++;
                }
            }
        }


        //public int RandomMetal(int type, int level, int seed)
        //{
        //    List<MineConfig> mines = this.list.Where(m => (m.Type == 0 || m.Type == type) && m.RequireLevel <= level).ToList();

        //    int max = 0;
        //    if (type == 0)
        //    {
        //        max = mines.Select(m => m.Rate).Sum();
        //    }
        //    else
        //    {
        //        max = mines.Select(m => m.Rate + m.RiseRate).Sum();
        //    }

        //    int rd = RandomHelper.RandomNumber(seed, 0, max);

        //    int endRate = 0;
        //    for (int i = 0; i < mines.Count; i++)
        //    {
        //        endRate += mines[i].Rate;
        //        if (type == 1)
        //        {
        //            endRate += mines[i].RiseRate;
        //        }

        //        if (rd <= endRate)
        //        {
        //            return mines[i].Id;
        //        }
        //    }

        //    return 0;
        //}
    }
}
