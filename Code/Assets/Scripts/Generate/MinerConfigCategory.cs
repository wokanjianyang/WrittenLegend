using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MineConfigCategory
    {
        public Dictionary<int, int> BuildMetal(bool inline, long count)
        {
            Dictionary<int, int> drops = new Dictionary<int, int>();

            User user = GameProcessor.Inst.User;

            int levelN = user.GetLimitMineCount();

            RandomRecord record = inline ? null : user.RandomRecord;

            for (int i = 0; i < levelN * count; i++)
            {
                int metalId = MineConfigCategory.Instance.RandomMetal(0, levelN, record);

                if (!drops.ContainsKey(metalId))
                {
                    drops[metalId] = 0;
                }
                drops[metalId]++;
            }

            int levelS = user.GetLimitMineCount2();
            for (int i = 0; i < levelS * count; i++)
            {
                int metalId = MineConfigCategory.Instance.RandomMetal(1, levelS, record);

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
            return RandomMetal(type, level, null);
        }

        public int RandomMetal(int type, int level, RandomRecord record)
        {
            List<MineConfig> mines = this.list.Where(m => (m.Type == 0 || m.Type == type) && m.RequireLevel <= level).ToList();

            int max = mines.Select(m => m.Rate).Sum();

            int rd = 0;
            if (record == null)
            {
                rd = RandomHelper.RandomNumber(0, max);
            }
            else
            {
                rd = RandomTableHelper.Instance().Random(0, max, record);
            }

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
