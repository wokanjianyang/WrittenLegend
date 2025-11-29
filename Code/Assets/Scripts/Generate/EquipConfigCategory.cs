using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class EquipConfigCategory
    {

    }

    public class EquipHelper
    {
        public static Equip BuildEquip(int configId, int staticQuality, int qualityRate, int seed, RuleType ruleType)
        {
            //if (seed <= 0)
            //{
            //    seed = AppHelper.InitSeed();
            //}

            EquipConfig config = EquipConfigCategory.Instance.Get(configId);

            if (config.Cycle == 5)
            {
                //混沌装备
                User user = GameProcessor.Inst.User;
                if (user != null)
                {
                    int dropLimitId = AppHelper.EquipHundun_MaxDropId;
                    DropData dropData = user.DropDataList.Where(m => m.DropLimitId == dropLimitId).FirstOrDefault();
                    if (dropData == null)
                    {
                        dropData = new DropData(dropLimitId);
                        dropData.Init(user.DeviceId.GetHashCode() + dropLimitId);
                        user.DropDataList.Add(dropData);
                    }
                    //Debug.Log("hundun max number:" + dropData.Number);
                    if (dropData.Number > AppHelper.EquipHundun_MaxCount)
                    {
                        //触发保底
                        if (RandomHelper.RandomResult(AppHelper.EquipHundun_MinRate))
                        {
                            staticQuality = 9;
                            seed = TimeHelper.TodaySeed() + dropData.Seed;
                            //Debug.Log("hundun 保底" + dropData.Seed);

                            dropData.Number = 0;
                            dropData.Seed++;
                        }
                    }
                    else
                    {
                        if (ruleType == RuleType.BossFamily)
                        {
                            dropData.Number += 0;
                        }
                        else
                        {
                            dropData.Number += 3;
                        }
                    }
                }

                return BuildEquipCycle5(config, staticQuality, qualityRate, seed);
            }

            int runeId = config.RuneId;
            int suitId = config.SuitId;
            int quality = config.Quality;

            if (config.Quality == 0)  //随机生成品质
            {
                double rate = ruleType == RuleType.BossFamily ? 7 : 1;
                quality = RandomHelper.RandomEquipQuality(config.LevelRequired, (int)(qualityRate * rate));
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

        public static Equip BuildEquipCycle5(EquipConfig config, int staticQuality, int qualityRate, int seed)
        {

            int quality = staticQuality;
            if (quality <= 0)
            {
                quality = RandomQuanlityCycle5(qualityRate);
            }

            int runeId = 0;
            int suitId = 0;

            if (quality > 2)
            {
                SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.GeEquipRuneCycle5(quality, config.Role, seed);

                runeId = runeConfig.Id;

                suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId, runeConfig.Type).Id;
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
            int start = 0;

            int[] rates = { 1, 4, 15, 50, 500, 5000, 20000, 100000, 500000 };

            //int[] rates = { 1, 10, 200, 300, 400, 500, 600, 7000, 8000, 10000 };

            int r = RandomHelper.RandomNumber(0, rates[8]);

            AppHelper.HundunCount++;
            //Debug.Log("quality start :" + AppHelper.CopyCount + "hundun count :" + AppHelper.HundunCount);

            r = (int)(r / realRate);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    if (i == 0)
                    {
                        //防止SL，给最高品质-1
                        start = AppHelper.GetLossQuality();
                    }

                    return 9 - i - start;
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
                AttrEntryConfig entryConfig = AttrEntryConfigCategory.Instance.GetRedConfig(attrId, config.Cycle);
                AttrEntryList.Add(new KeyValuePair<int, long>(attrId, entryConfig.MaxValue));
            }

            item.AttrEntryList = AttrEntryList;

            item.Count = 1;
            return item;
        }
    }
}