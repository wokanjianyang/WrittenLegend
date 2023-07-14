using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AttrEntryConfigCategory
    {
        public KeyValuePair<int, long> Build(int part, int level)
        {
            List<AttrEntryConfig> configs = list.FindAll(m => m.PartList.Contains(part));

            if (configs.Count <= 0)
            {
                return new KeyValuePair<int, long>(0, 0);
            }

            int rd = RandomHelper.RandomNumber(0, configs.Count);

            AttrEntryConfig config = configs[rd];

            long attrValue = 0;

            if (config.Type == 1)
            {  //���������Լ���
                long baseValue = EquipConfigCategory.Instance.GetAll().Where(m => m.Value.LevelRequired == level && m.Value.Part == 1).First().Value.AttributeBase[0];
                attrValue = baseValue * config.Min / 100;
            }
            else if (config.Type == 2)
            {
                attrValue = RandomHelper.RandomNumber(config.Min, config.Max + 1);
            }

            return new KeyValuePair<int, long>(config.AttrId, attrValue);
        }
    }
}
