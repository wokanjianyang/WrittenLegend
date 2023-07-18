using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {

    }

    public class SkillRuneHelper
    {
        public static SkillRuneConfig RandomRune()
        {
            List<SkillRuneConfig> list = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            //User user = GameProcessor.Inst.User;
            //if (user != null && user.SkillList != null)
            //{
            //    List<int> battleSkillList = user.SkillList.Where(m => m.Status == SkillStatus.Equip).Select(m => m.SkillId).ToList();

            //    List<SkillRuneConfig> battleList = list.Where(m => battleSkillList.Contains(m.SkillId)).ToList();
            //    if (battleSkillList.Count > 0)
            //    {
            //        list = battleList;
            //    }
            //}

            int index = RandomHelper.RandomNumber(0, list.Count);
            return list[index];
        }
    }
}
