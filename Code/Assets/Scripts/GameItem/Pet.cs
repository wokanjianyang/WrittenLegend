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

        public int SkillId { get; set; }

        public int RunMapId { get; set; }

        public override int GetQuality()
        {
            return Flairs.Count;
        }

        public Pet()
        {
            this.Type = ItemType.Pet;
            this.Name = "³èÎï";

        }

        public Dictionary<int, long> GetBaseAttr()
        {
            Dictionary<int, long> attrs = new Dictionary<int, long>();

            for (int i = 0; i < Flairs.Count; i++)
            {
                int attrId = Flairs[i].Key;
                long attrValue = Flairs[i].Value.Data * PetLevel.Data;

                if (!attrs.ContainsKey(attrId))
                {
                    attrs[attrId] = 0;
                }

                attrs[attrId] += attrValue;
            }

            return attrs;
        }

        public long GetSkillPercent()
        {
            return (long)(PetLevel.Data * 1);
        }
    }
}
