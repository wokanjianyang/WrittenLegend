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

            long layer = (total / 2000);
            long layerRate = MathHelper.GetSequence1(layer);

            if (layerRate > 0)
            {
                foreach (PillConfig config in this.list)
                {
                    long attrValue = config.AttrTotal * layerRate;
                    dict[config.AttrId] += attrValue;
                }
            }

            long ct = total % 2000;
            foreach (PillConfig config in this.list)
            {
                long level = 0;
                if (config.Type == 1)
                {
                    level = ct / 10;
                    long position = ct % 10;
                    if (position < config.Position)
                    {
                        level += 1;
                    }
                }
                else
                {
                    long position = Math.Min(ct - config.Position, 100);
                    level = position / 100;
                }

                long currentLayerRate = MathHelper.GetSequence1(layer + 1);
                long attrValue = currentLayerRate * level * config.AttrValue;

                dict[config.AttrId] += attrValue;
            }

            return dict;
        }

        public PillConfig GetByLevel(long level)
        {
            long p = level % 2000;
            int type = p % 10 == 0 ? 2 : 1;
            long position = p / 100 + 1;

            var config = this.list.Where(m => m.Type == type && m.Position == position).FirstOrDefault();
            return config;
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
