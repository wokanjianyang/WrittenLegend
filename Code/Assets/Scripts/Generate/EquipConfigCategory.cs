using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipConfigCategory
    {

    }

    public class EquipHelper
    {
        public static Equip BuildEquip(int ConfigId, int minQuality)
        {
            int RuneConfigId = SkillRuneConfigCategory.Instance.Build();

            Equip equip = new Equip(ConfigId, RuneConfigId);
            equip.ConfigId = ConfigId;

            //生成品质
            equip.Quality = Math.Max(minQuality, RandomHelper.RandomQuality());

            //根据品质,生成随机属性
            for (int i = 0; i < equip.Quality; i++)
            {
                AttrEntryConfigCategory.Instance.Build(ref equip);
            }

            return equip;
        }
    }
}