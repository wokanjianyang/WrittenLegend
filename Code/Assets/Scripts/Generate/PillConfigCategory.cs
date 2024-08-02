using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PillConfigCategory
    {
        private List<int> last_list = new List<int>();

        public Dictionary<int, long> ParseLevel(long total)
        {
            Dictionary<int, long> dict = new Dictionary<int, long>();

            foreach (PillConfig config in this.list)
            {
                dict[config.AttrId] = 0;
            }

            for (int i = 0; i < total; i++)
            {
                int l = i / 2000;
                int p = i % 2000;

                int id = AllList[p];

                PillConfig config = Get(id);

                int attrId = config.AttrId;
                long attrValue = config.AttrValue * (l + 1);

                dict[attrId] += attrValue;
            }

            return dict;
        }

        public PillConfig GetByLevel(long level)
        {
            int p = (int)(level % 2000);
            int id = this.AllList[p];

            return Get(id);
        }

        public List<int> AllList
        {
            get
            {
                if (last_list.Count == 0)
                {
                    for (int i = 0; i < 2000; i++)
                    {
                        int id = i % 10 + 1;
                        if (id == 10)
                        {
                            id = i / 100 + 10;
                        }

                        last_list.Add(id);
                    }
                }

                return last_list;
            }
        }
    }

    public partial class PillConfig
    {

    }
}
