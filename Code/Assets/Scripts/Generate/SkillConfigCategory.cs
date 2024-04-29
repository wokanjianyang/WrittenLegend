using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillConfigCategory
    {
        private int[] exclusiveList = new int[] { 3004, 3007, 1005, 2005, 3005 };
        public List<int> RandomList(int count)
        {
            List<int> rdList = new List<int>();

            List<int> allList = this.list.Where(m => m.Id < 10001 && !exclusiveList.Contains(m.Id)).Select(m => m.Id).ToList();

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