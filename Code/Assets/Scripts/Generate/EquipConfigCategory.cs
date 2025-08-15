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

            if (config.Cycle == 5)
            {
                //混沌装备
                return BuildEquipNew(config, staticQuality, qualityRate, seed);
            }

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
                SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.RandomRune(seed, -1, config.Role, 1, quality, config.LevelRequired);
                runeId = runeConfig.Id;

                if (suitId == 0 && quality > 3)  //随机生成套装
                {
                    if (quality == 8)
                    {
                        suitId = SkillSuitConfigCategory.Instance.GetSuitIdBySkillLayer(runeConfig.SkillLayer);
                    }
                    else
                    {
                        suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId, runeConfig.Type).Id;
                    }
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

        public static Equip BuildEquipNew(EquipConfig config, int staticQuality, int qualityRate, int seed)
        {

            double realRate = MathHelper.ConvertionDropRate(qualityRate, 100);
            int quality = RandomQuanlityCycle5(realRate);

            int runeId = 0;
            int suitId = 0;

            if (quality > 2)
            {
                SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.GeEquipRune(quality, seed);

                runeId = runeConfig.Id;

                if (quality == 8)
                {
                    suitId = SkillSuitConfigCategory.Instance.GetSuitIdBySkillLayer(runeConfig.SkillLayer);
                }
                else
                {
                    suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId, runeConfig.Type).Id;
                }
        
            }

            Equip equip = new Equip(config.Id, runeId, suitId, quality);
            if (seed < 0)
            {
                seed = AppHelper.InitSeed();
            }
            equip.Init(seed);

            equip.Count = 1;
            return equip;
        }

        private static int RandomQuanlityCycle5(double realRate)
        {
            //int[] rates = { 1, 10, 100, 1000, 10000, 50000, 250000, 1000000, 5000000 };

            int[] rates = { 1, 10, 200, 300, 400, 500, 600, 7000, 8000, 10000 };

            int r = RandomHelper.RandomNumber(0, rates[8]);

            r = (int)(r / realRate);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    return 9 - i;
                }
            }

            return 1;
        }


        public static Equip BuildByPack(int configId)
        {
            GiftPackEquipConfig config = GiftPackEquipConfigCategory.Instance.Get(configId);

            Equip item = new Equip(config.EquipId, config.RuneId, config.SuitId, config.Quality);

            List<KeyValuePair<int, long>> AttrEntryList = new List<KeyValuePair<int, long>>();

            for (int i = 0; i < config.AttrIdList.Length; i++)
            {
                int attrId = config.AttrIdList[i];
                AttrEntryConfig entryConfig = AttrEntryConfigCategory.Instance.GetRedConfig(attrId, config.Quality);
                AttrEntryList.Add(new KeyValuePair<int, long>(attrId, entryConfig.MaxValue));
            }

            item.AttrEntryList = AttrEntryList;

            item.Count = 1;
            return item;
        }
    }
}