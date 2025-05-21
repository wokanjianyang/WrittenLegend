using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PillConfig2Category
    {
        private List<int> last_list = new List<int>();

        public Dictionary<int, double> ParseLevel(long total)
        {
            Dictionary<int, double> dict = new Dictionary<int, double>();

            foreach (PillConfig2 config in this.list)
            {
                dict[config.AttrId] = 0;
            }

            for (int i = 0; i < total; i++)
            {
                int l = i / 2000;
                int p = i % 2000;

                int id = AllList[p];

                PillConfig2 config = Get(id);

                int attrId = config.AttrId;
                double attrValue = config.AttrValue;

                dict[attrId] += attrValue;
            }

            return dict;
        }

        public PillConfig2 GetByLevel(long level)
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

    public partial class PillConfig2
    {
        public double GetAttr(long layer)
        {
            return this.AttrValue * layer;
        }

        public long GetFee(long layer)
        {
            return (long)(this.Fee * (1 + layer * 0.2));
        }
    }

}
