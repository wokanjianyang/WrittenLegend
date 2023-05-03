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

            //����Ʒ��
            equip.Quality = Math.Max(minQuality, RandomHelper.RandomQuality());

            //����Ʒ��,�����������
            for (int i = 0; i < equip.Quality; i++)
            {
                AttrEntryConfigCategory.Instance.Build(ref equip);
            }

            return equip;
        }
    }
}