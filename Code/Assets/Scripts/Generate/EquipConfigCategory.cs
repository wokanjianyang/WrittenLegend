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
        public static Equip BuildEquip(int configId, int minQuality)
        {
            EquipConfig config = EquipConfigCategory.Instance.Get(configId);

            int runeId = config.RuneId;
            int suitId = config.SuitId;


            if (runeId == 0) //随机生成词条
            {
                SkillRuneConfig runeConfig = SkillRuneHelper.RandomRune();
                runeId = runeConfig.Id;

                if (suitId == 0)  //随机生成词条
                {
                    suitId = SkillSuitHelper.RandomSuit(runeConfig.SkillId).Id;
                }
            }

            Equip equip = new Equip(configId, runeId, suitId);

            if (config.Quality == 0)  //随机生成品质
            {
                equip.Quality = Math.Max(minQuality, RandomHelper.RandomQuality());
            }
            else
            {
                equip.Quality = config.Quality;
            }

            //根据品质,生成随机属性
            if (config.RandomAttr == 0)
            {
                for (int i = 0; i < equip.Quality; i++)
                {
                    AttrEntryConfigCategory.Instance.Build(ref equip);
                }
            }

            return equip;
        }
    }
}