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

        public static string BuildSecondExpMessage(long exp)
        {
            return $"增加泡点经验{exp}";
        }

        public static string BuildTowerSuccessMessage(long exp, int floor)
        {
            return $"<color=white>提升泡点经验:{exp},进入第{floor}层</color>";
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