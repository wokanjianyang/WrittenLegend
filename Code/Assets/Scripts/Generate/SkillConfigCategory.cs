using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillConfigCategory
    {
        private int[] exclusiveList = new int[] { 3004, 3007, 1005, 2005, 3005, 3008, 3009, 3012 };
        public List<int> RandomList(int count, int progress)
        {
            int layer = 11;
            if (progress >= 4000)
            {
                layer = 12;
            }


            List<int> rdList = new List<int>();

            List<int> allList = this.list.Where(m => m.Id < 4000 && m.SkillLayer <= layer && !exclusiveList.Contains(m.Id)).Select(m => m.Id).ToList();

            for (int i = 0; i < count; i++)
            {
                int index = RandomHelper.RandomNumber(0, allList.Count);
                rdList.Add(allList[index]);
                allList.RemoveAt(index);
            }

            return rdList;
        }
    }

    //public partial class SkillConfig
    //{
    //    public long GetMaxLevel(long level)
    //    {
    //        return this.MaxLevel + this.RiseMaxLevel * level;
    //    }
    //}
}