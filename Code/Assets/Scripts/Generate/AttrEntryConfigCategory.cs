using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AttrEntryConfigCategory
    {
        public void Build(ref Equip equip)
        {
            int part = equip.Part;

            List<AttrEntryConfig> configs = list.FindAll(m => m.PartList.Contains(part));

            if (configs.Count <= 0)
            {
                return;
            }

            int rd = RandomHelper.RandomNumber(0, configs.Count);

            if (equip.AttrEntryList == null)
            {
                equip.AttrEntryList = new List<KeyValuePair<int, long>>();
            }

            AttrEntryConfig config = configs[rd];

            long attrValue = 0;

            if (config.Type == 1)
            {  //按基础属性计算
                int level = equip.Level;
                long baseValue = EquipConfigCategory.Instance.GetAll().Where(m => m.Value.LevelRequired == level && m.Value.Part == 1).First().Value.AttributeBase[0];
                attrValue = baseValue * config.Min / 100;
            }
            else if (config.Type == 2)
            {
                attrValue = RandomHelper.RandomNumber(config.Min, config.Max + 1);
            }


            equip.AttrEntryList.Add(new KeyValuePair<int, long>(config.AttrId, attrValue));
        }
    }
}
