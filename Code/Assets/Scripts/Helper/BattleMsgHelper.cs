using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class BattleMsgHelper
    {
        public static string BuildMonsterDeadMessage(APlayer monster, long exp, long gold, List<Item> Drops)
        {
            string drops = "";
            if (exp > 0)
            {
                drops += ",经验增加:" + StringHelper.FormatNumber(exp);
            }

            if (gold > 0)
            {
                drops += ",金币增加:" + StringHelper.FormatNumber(gold);
            }

            if (Drops != null && Drops.Count > 0)
            {
                drops += ",掉落";
                foreach (var drop in Drops)
                {
                    drops += $"<color=#{QualityConfigHelper.GetColor(drop)}>[{drop.Name}]</color>";
                }
            }

            string message = $"<color=#{QualityConfigHelper.GetQualityColor(monster.Quality)}>[{monster.Name}]</color><color=white>死亡{drops}</color>";

            return message;
        }

        public static string BuildBossDeadMessage(APlayer monster, long exp, long gold, List<Item> Drops)
        {
            string drops = "";

            if (exp > 0)
            {
                drops += ",经验增加:" + StringHelper.FormatNumber(exp);
            }

            if (gold > 0)
            {
                drops += ",金币增加:" + StringHelper.FormatNumber(gold);
            }

            if (Drops != null && Drops.Count > 0)
            {
                drops += ",掉落";
                foreach (var drop in Drops)
                {
                    string qt = "";
                    if (drop.Count > 1)
                    {
                        qt = "*" + drop.Count;
                    }

                    drops += $"<color=#{QualityConfigHelper.GetColor(drop)}>[{drop.Name}]</color>" + qt;
                }
            }

            string message = $"<color=#FFD700>[{monster.Name}]</color><color=white>死亡{drops}</color>";

            return message;
        }

        public static string BuildRewardMessage(string src, long exp, long gold, List<Item> Drops)
        {
            string drops = src + "";

            if (exp > 0)
            {
                drops += ",经验增加:" + StringHelper.FormatNumber(exp);
            }

            if (gold > 0)
            {
                drops += ",金币增加:" + StringHelper.FormatNumber(gold);
            }

            if (Drops != null && Drops.Count > 0)
            {
                drops += ",掉落";
                foreach (var drop in Drops)
                {
                    string qt = "";
                    if (drop.Count > 1)
                    {
                        qt = "*" + drop.Count + " ";
                    }

                    drops += $"<color=#{QualityConfigHelper.GetColor(drop)}>[{drop.Name}]</color>" + qt;
                }
            }

            return drops;
        }

        public static string BuildAutoRecoveryMessage(int equipQuantity, List<Item> itemList, long gold)
        {
            string message = "回收" + equipQuantity + "件装备，获得";

            foreach (Item item in itemList)
            {
                message += item.Count + "个" + item.ItemConfig.Name + "，";
            }

            if (gold > 0)
            {
                message += StringHelper.FormatNumber(gold) + "金币";
            }

            return message;
        }

        public static string BuildGiftPackMessage(string src, long exp, long gold, List<Item> items)
        {
            string message = $"<color=#{QualityConfigHelper.GetQualityColor(4)}> {src}";
            if (exp > 0)
            {
                message += $"经验{StringHelper.FormatNumber(exp)}";
            }
            if (gold > 0)
            {
                message += $"金币{StringHelper.FormatNumber(gold)}";
            }

            if (items != null)
            {
                foreach (var item in items)
                {
                    message += $",{item.Name}*{item.Count}";
                }
            }

            return message + "</color>";
        }


        public static string BuildSecondExpMessage(long exp, long gold)
        {
            return $"获得经验收益{StringHelper.FormatNumber(exp)}，金币收益{StringHelper.FormatNumber(gold)}";
        }

        public static string BuildOfflineMessage(long time, long floor, long exp, long gold, long itemCount)
        {
            return $"离线时间{time}S,闯关{floor}层 \n获得总经验{StringHelper.FormatNumber(exp)}，总金币{StringHelper.FormatNumber(gold)} \n装备{itemCount}件,金币经验收益提升";
        }

        public static string BuildTowerSuccessMessage(long riseExp, long riseGold, long exp, long gold, long floor, List<Item> items)
        {
            string message = $"闯关成功,奖励";

            foreach (var drop in items)
            {
                message += $"<color=#{QualityConfigHelper.GetColor(drop)}>[{drop.Name}]</color>";
            }
            return message + $",经验:{ StringHelper.FormatNumber(exp)},金币奖励: { StringHelper.FormatNumber(gold)},收益提升";
        }

        public static string BuildCopySuccessMessage()
        {
            return $"<color=white>挑战副本成功,已自动解锁下一个副本</color>";
        }


        public static string BuildGiftPackMessage(List<Item> items)
        {
            string message = "礼包获取:";
            foreach (var drop in items)
            {
                message += $"<color=#{QualityConfigHelper.GetColor(drop)}>[{drop.Name}]</color>";
            }

            return message;
        }

        public static string BuildTimeErrorMessage()
        {
            string message = $"时间不正确,没有收益，请校准自己的时间 ";
            return message;
        }

        public static string BuildMinerMessage(Miner miner, MetalConfig config, long level)
        {
            string message = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") + " 矿工" + " 挖到了";
            message += $"<color=#{QualityConfigHelper.GetQualityColor(config.Quality)}>[{config.Name}]</color>";
            message += ",矿物等级为:" + level;

            return message;
        }
    }
}