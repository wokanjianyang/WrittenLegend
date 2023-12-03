using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public class HeroPhatomData
    {

        public long Ticket { get; set; }

        public MagicData Count { get; set; } = new MagicData();

        public HeroPhatomRecord Current = null;

        private int MaxLevel = 10;

        public HeroPhatomRecord GetCurrentRecord()
        {
            if (Current != null && Current.Progress.Data > MaxLevel)
            {
                Current = null;
            }

            if (Current == null && this.Count.Data > 0)
            {
                Current = new HeroPhatomRecord();
                Current.Progress.Data = 0;
                Current.Next();
                this.Count.Data--;
            }

            return Current;
        }

        public void Refresh()
        {
            this.Current = null;
            this.Count.Data = 1;
        }

        public void Complete()
        {
            this.Current = null;
        }
    }

    public class HeroPhatomRecord
    {
        public MagicData Progress { get; set; } = new MagicData();

        public List<int> SkillIdList { get; set; } = new List<int>();

        public int ConfigId { get; set; } = 0;

        public void Next()
        {
            this.Progress.Data++;

            int rd = RandomHelper.RandomNumber(0, 10);

            if (rd < 2)
            {
                this.SkillIdList = RandomSkillList();
                this.ConfigId = 0;
            }
            else
            {
                List<HeroPhatomConfig> allList = HeroPhatomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
                int index = RandomHelper.RandomNumber(0, allList.Count);

                this.ConfigId = allList[index].Id;
                this.SkillIdList = allList[index].SkillList.ToList();
            }
        }

        private List<int> RandomSkillList()
        {
            int role = RandomHelper.RandomNumber(1, 4);

            List<SkillConfig> configs = SkillConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Role == role && m.Id < 4000).ToList();

            //1 随机去掉不能重复的技能
            List<int> repeatIdList = configs.Where(m => m.Repet > 0).Select(m => m.Repet).ToList();

            foreach (int repeatId in repeatIdList)
            {
                List<SkillConfig> repeatList = configs.Where(m => m.Repet == repeatId).ToList();

                if (repeatList.Count > 1)
                {
                    //随机保留一个
                    int index = RandomHelper.RandomNumber(0, repeatList.Count);
                    List<int> removeList = repeatList.Select(m => m.Id).ToList();
                    removeList.RemoveAt(index);

                    configs.RemoveAll(m => removeList.Contains(m.Id));
                }
            }

            //2 保留6个技能
            //最多只去掉一个大于等于7级的技能
            int ImportCount = 0;
            while (configs.Count > 6)
            {
                int index = RandomHelper.RandomNumber(0, configs.Count);

                if (configs[index].Id % 1000 == 5)
                {
                    //如果是盾,不去掉
                    continue;
                }

                if (configs[index].Id % 1000 >= 7)
                {
                    if (ImportCount <= 0)
                    {
                        configs.RemoveAt(index);
                        ImportCount++;
                    }
                    continue;
                }

                configs.RemoveAt(index);
            }

            List<int> skillList = configs.Select(m => m.Id).ToList();

            return skillList;
        }
    }


}
