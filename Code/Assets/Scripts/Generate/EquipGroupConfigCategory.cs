using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipGroupConfigCategory
    {
        public EquipGroupConfig GetByLevelAndPart(int level, int part)
        {
            EquipGroupConfig config = GetAll().Select(m => m.Value).Where(m => m.Level == level && m.Position == part).FirstOrDefault();
            return config;
        }
    }

    public class EquipSuitItem
    {
        public EquipSuitItem(int id, string name, bool active)
        {
            this.Id = id;
            this.Name = name;
            this.Active = active;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }
    }

    public class EquipSuit
    {
        public bool Active { get; set; } = false;

        public EquipGroupConfig Config { get; set; }
        public EquipSuitItem Self { get; set; }

        public List<EquipSuitItem> ItemList = new List<EquipSuitItem>();
    }

    public class EquipRedSuit
    {
        public List<EquipRedItem> List;
    }

    public class EquipRedItem
    {
        public int Level;

        public int Count;

        public EquipRedConfig Config;
    }
}