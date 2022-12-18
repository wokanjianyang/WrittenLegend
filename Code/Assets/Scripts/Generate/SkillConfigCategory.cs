using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillConfigCategory
    {
    }

    public class SkillHelper
    {
        public static SkillBook BuildItem(int ConfigId)
        {
            SkillConfig skillconfig = SkillConfigCategory.Instance.Get(ConfigId);
            ItemConfig config = ItemConfigCategory.Instance.Get(ConfigId);

            SkillBook item = new SkillBook();

            item.Id = IdGenerater.Instance.GenerateId();
            item.ConfigId = ConfigId;
            item.Type = (ItemType)config.Type;
            item.Name = config.Name;
            item.Des = config.Name;
            item.Level = config.LevelRequired;
            item.Gold = config.Price;
            item.Quality = config.Quality;
            item.MaxNum = config.MaxNum;
            item.BookConfig = skillconfig;
            item.Config = config;
            return item;
        }
    }
}