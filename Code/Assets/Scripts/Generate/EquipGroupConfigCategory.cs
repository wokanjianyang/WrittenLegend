using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipGroupConfigCategory
    {
        public EquipGroupConfig GetByLevelAndPart(int level, int part)
        {
            EquipGroupConfig config = GetAll().Select(m => m.Value).Where(m => m.Level == level && m.Position.Contains(part)).FirstOrDefault();
            return config;
        }
    }

    public class EquipGroup
    {
        public EquipGroup(int id, string name, bool have)
        {
            this.Id = id;
            this.Name = name;
            this.Have = have;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public bool Have { get; set; }
    }
}