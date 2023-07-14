using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBaseCategory
    {

    }

    public class MonsterHelper
    {
        public static Monster BuildMonster(MapConfig mapConfig)
        {
            int minLevel = mapConfig.MonsterLevelMin;
            int maxLevel = mapConfig.MonsterLevelMax;
            int quality = 1;

            if (SystemConfigHelper.CheckRequireLevel(SystemEnum.MonsterQuanlity))
            {
                quality = RandomHelper.RandomMonsterQuality();  //超过5级开始有精英怪
            }
            else
            {
                maxLevel = GameProcessor.Inst.User.Level; //低于5级，只刷低于自身等级的怪
            }

            List<MonsterBase> list = MonsterBaseCategory.Instance.GetAll().Where(m => m.Value.Level >= minLevel && m.Value.Level <= maxLevel).Select(m => m.Value).ToList();

            int rd = RandomHelper.RandomNumber(1, list.Count + 1);
            MonsterBase config = list[rd - 1];

            Monster enemy = new Monster(config.Id, quality);
            return enemy;
        }
    }
}