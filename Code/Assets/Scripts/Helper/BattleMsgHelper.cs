using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class BattleMsgHelper
    {
        public static string BuildMonsterDeadMessage(Monster monster, List<Item> Drops)
        {
            string drops = "";
            if (Drops != null && Drops.Count > 0)
            {
                drops = ",掉落";
                foreach (var drop in Drops)
                {
                    drops += $"<color=#{QualityConfigHelper.GetColor(drop.GetQuality())}>[{drop.Name}]</color>";
                }
            }

            string message = $"<color=#{QualityConfigHelper.GetColor(monster.Quality)}>[{monster.Name}]</color><color=white>死亡,经验增加:{monster.Exp},金币增加:{monster.Gold}{drops}</color>";

            return message;
        }

        public static string BuildBossDeadMessage(Boss monster, List<Item> Drops)
        {
            string drops = "";
            if (Drops != null && Drops.Count > 0)
            {
                drops = ",掉落";
                foreach (var drop in Drops)
                {
                    string qt = "";
                    if (drop.Quantity > 1)
                    {
                        qt = "*" + drop.Quantity;
                    }

                    drops += $"<color=#{QualityConfigHelper.GetColor(drop.GetQuality())}>[{drop.Name}]</color>" + qt;
                }
            }

            string message = $"<color=#FFD700>[{monster.Name}]</color><color=white>死亡,经验增加:{monster.Exp},金币增加:{monster.Gold}{drops}</color>";

            return message;
        }

        public static string BuildAutoRecoveryMessage(int equipQuantity, int refineStone, long gold)
        {
            string message = "回收" + equipQuantity + "件装备，获得" + refineStone + "个装备精炼石，" + gold + "金币";
            return message;
        }


        public static string BuildSecondExpMessage(long exp, long gold)
        {
            return $"获得经验收益{exp}，金币收益{gold}";
        }

        public static string BuildTowerSuccessMessage(long riseExp, long riseGold, long exp, long gold, int floor)
        {
            return $"<color=white>闯关成功,获得经验奖励:{exp},金币奖励:{gold} ,提升经验收益:{riseExp},金币收益:{riseGold},进入第{floor}层</color>";
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
                message += $"<color=#{QualityConfigHelper.GetColor(drop.GetQuality())}>[{drop.Name}]</color>";
            }

            return message;
        }

    }
}