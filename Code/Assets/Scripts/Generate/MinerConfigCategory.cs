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

            User user = GameProcessor.Inst.User;

            int levelN = user.GetLimitMineCount();

            for (int i = 0; i < levelN * count; i++)
            {
                seed = AppHelper.RefreshSeed(seed);

                int metalId = RandomMetal(0, levelN, seed);

                if (!drops.ContainsKey(metalId))
                {
                    drops[metalId] = 0;
                }
                drops[metalId]++;
            }

            int levelS = user.GetLimitMineCount2();
            for (int i = 0; i < levelS * count; i++)
            {
                seed = AppHelper.RefreshSeed(seed);

                int metalId = MineConfigCategory.Instance.RandomMetal(1, levelS, seed);

                if (!drops.ContainsKey(metalId))
                {
                    drops[metalId] = 0;
                }
                drops[metalId]++;
            }

            return drops;
        }

        public int RandomMetal(int type, int level)
        {
            return RandomMetal(type, level, 0);
        }

        public int RandomMetal(int type, int level, int seed)
        {
            List<MineConfig> mines = this.list.Where(m => (m.Type == 0 || m.Type == type) && m.RequireLevel <= level).ToList();

            int max = mines.Select(m => m.Rate).Sum();

            int rd = RandomHelper.RandomNumber(seed, 0, max);

            int endRate = 0;
            for (int i = 0; i < mines.Count; i++)
            {
                endRate += mines[i].Rate;

                if (rd <= endRate)
                {
                    return mines[i].Id;
                }
            }

            return 0;
        }
    }
}
