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
        public static Equip BuildEquip(int configId, int staticQuality, int qualityRate, int seed)
        {
            //if (seed <= 0)
            //{
            //    seed = AppHelper.InitSeed();
            //}

            EquipConfig config = EquipConfigCategory.Instance.Get(configId);

            int runeId = config.RuneId;
            int suitId = config.SuitId;
            int quality = config.Quality;

            if (config.Quality == 0)  //随机生成品质
            {
                quality = RandomHelper.RandomEquipQuality(config.LevelRequired, qualityRate);
            }
            if (staticQuality > 0)
            {
                quality = staticQuality;
            }

            if (runeId == 0 && quality > 2) //随机生成词条
            {
                SkillRuneConfig runeConfig = SkillRuneHelper.RandomRune(seed, -1, config.Role, 1, quality, config.LevelRequired);
                runeId = runeConfig.Id;

                if (suitId == 0 && quality > 3)  //随机生成套装
                {
                    suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId).Id;
                }
            }

            Equip equip = new Equip(configId, runeId, suitId, quality);
            if (seed < 0)
            {
                seed = AppHelper.InitSeed();
            }
            equip.Init(seed);

            equip.Count = 1;
            return equip;
        }
    }
}