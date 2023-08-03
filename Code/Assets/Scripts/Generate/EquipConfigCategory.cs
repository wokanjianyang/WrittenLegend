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
        public static Equip BuildEquip(int configId, int qualityRate)
        {
            EquipConfig config = EquipConfigCategory.Instance.Get(configId);

            int runeId = config.RuneId;
            int suitId = config.SuitId;
            int quality = config.Quality;

            if (config.Quality == 0)  //随机生成品质
            {
                quality = RandomHelper.RandomEquipQuality(qualityRate);
            }

            if (runeId == 0 && quality > 2) //随机生成词条
            {
                SkillRuneConfig runeConfig = SkillRuneHelper.RandomRune(config.Role);
                runeId = runeConfig.Id;

                if (suitId == 0 && quality > 3)  //随机生成词条
                {
                    suitId = SkillSuitHelper.RandomSuit(runeConfig.SkillId).Id;
                }
            }

            Equip equip = new Equip(configId, runeId, suitId, quality);

            //Equip equip = new Equip(configId, 7, 2, 4); //Test

            return equip;
        }
    }
}