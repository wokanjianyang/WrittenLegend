using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public partial class SystemConfigCategory
    {
        public static SystemConfigCategory Instance;

        public Dictionary<SystemEnum, int> SystemDict = new Dictionary<SystemEnum, int>();

        public SystemConfigCategory()
        {
            Instance = this;

            SystemDict.Add(SystemEnum.MonsterQuanlity, 5);
            SystemDict.Add(SystemEnum.SoulRing, 40);
        }


    }

    public static class SystemConfigHelper
    {
        public static bool CheckRequireLevel(SystemEnum SystemId)
        {
            int level = GameProcessor.Inst.User.Level;
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
