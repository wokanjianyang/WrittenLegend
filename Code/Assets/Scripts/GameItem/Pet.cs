using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Pet : Item
    {
        public MagicData PetLevel { get; set; } = new MagicData();
        public MagicData PetLayer { get; set; } = new MagicData();

        public MagicData LevelExp { get; set; } = new MagicData();

        public MagicData LayerExp { get; set; } = new MagicData();
        public List<KeyValuePair<int, MagicData>> Flairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public int Status { get; set; }

        public int Role { get; set; }

        public int RunMapId { get; set; }

        public long RunTime { get; set; }

        public override int GetQuality()
        {
            return Flairs.Count;
        }

        public Pet(int role)
        {
            this.Type = ItemType.Pet;
            this.Role = role;

            this.Name = ConfigHelper.PetName[Role - 1];
        }

        public Dictionary<int, double> GetBaseAttr()
        {
            Dictionary<int, double> attrs = new Dictionary<int, double>();

            for (int i = 0; i < Flairs.Count; i++)
            {
                int attrId = Flairs[i].Key;

                PetConfig config = PetConfigCategory.Instance.GetByAttrId(attrId);

                double attrValue = (Flairs[i].Value.Data * config.AttrValue / 100 * PetLevel.Data);

                if (!attrs.ContainsKey(attrId))
                {
                    attrs[attrId] = 0;
                }

                attrs[attrId] += attrValue;
            }

            return attrs;
        }

        public void AddExp(long exp)
        {
            this.LevelExp.Data += exp;

            int need = 1000;
            if (this.LevelExp.Data >= need)
            {
                this.LevelExp.Data -= need;
                this.PetLevel.Data++;
            }
        }

        public long GetSkillPercent()
        {
            return PetSkillRise[Flairs.Count - 1];
        }

        private int[] PetSkillRise = new int[] { 5, 6, 7, 8, 10, 12, 15 };
    }
}
