using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public partial class SystemConfigCategory
    {
        private static SystemConfigCategory instance; // 单例实例

        public Dictionary<SystemEnum, int> SystemDict = new Dictionary<SystemEnum, int>();

        private SystemConfigCategory()
        {
            SystemDict.Add(SystemEnum.MonsterQuanlity, 5);
            SystemDict.Add(SystemEnum.SoulRing, 40);
        }

        public static SystemConfigCategory Instance
        {
            get
            {
                // 如果实例还未创建，则创建一个新实例
                if (instance == null)
                {
                    instance = new SystemConfigCategory();
                }

                return instance;
            }
        }
    }

    public static class SystemConfigHelper
    {
        public static bool CheckRequireLevel(SystemEnum SystemId)
        {
            long level = GameProcessor.Inst.User.MagicLevel.Data;
            var dict = SystemConfigCategory.Instance.SystemDict;

            if (dict.ContainsKey(SystemId) && level < dict[SystemId])
            {
                return false;
            }

            return true;
        }
    }

    public enum SystemEnum
    {
        MonsterQuanlity = 1,
        SoulRing = 2,
    }
}
