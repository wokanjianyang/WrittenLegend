using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillBook : Item
    {
        public SkillBook(int configId) : base(configId)
        {
            this.Type = ItemType.SkillBox;

            SkillConfig = SkillConfigCategory.Instance.Get(ConfigId);
        }

        public long Exp { get; set; }


        [JsonIgnore]
        public SkillConfig SkillConfig { get; set; }

        public int EquipBoxId { get; set; }

    }
}
