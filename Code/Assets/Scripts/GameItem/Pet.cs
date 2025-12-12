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
        public const int LayerRiseAttr = 3;
        public const int LayerRiseSkill = 2;

        public MagicData PetLevel { get; set; } = new MagicData();
        public MagicData PetLayer { get; set; } = new MagicData();

        public MagicData LevelExp { get; set; } = new MagicData();

        //public MagicData LayerExp { get; set; } = new MagicData();
        public List<KeyValuePair<int, MagicData>> Flairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<KeyValuePair<int, MagicData>> DevourFlairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

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

        public int GetDevourCount()
        {
            if (PetLayer.Data >= 10)
            {
                return DevourFlairs.Count;
            }

            return 0;
        }

        public Dictionary<int, long> GetTotalFlairs()
        {
            Dictionary<int, long> flairs = new Dictionary<int, long>();

            long layer = PetLayer.Data;

            int fc = GetDevourCount();

            for (int i = 0; i < Flairs.Count; i++)
            {
                int attrId = Flairs[i].Key;

                long flair = Flairs[i].Value.Data * (100 + fc * 20) / 100 + (layer - 1) * LayerRiseAttr;

                flairs[attrId] = flair;
            }

            for (int i = 0; i < fc; i++)
            {
                int attrId = DevourFlairs[i].Key;

                long flair = DevourFlairs[i].Value.Data * (100 + fc * 20) / 100 + (layer - 1) * LayerRiseAttr;

                flairs[attrId] = flair;
            }

            return flairs;
        }

        public Dictionary<int, double> GetBaseAttr()
        {
            Dictionary<int, double> attrs = new Dictionary<int, double>();

            long level = PetLevel.Data;
            long rise = level / 10;
            double riseRate = (1 + rise * 0.05);

            Dictionary<int, long> flairs = this.GetTotalFlairs();

            foreach (KeyValuePair<int, long> sp in flairs)
            {
                int attrId = sp.Key;

                PetConfig config = PetConfigCategory.Instance.GetByAttrId(attrId);

                double attrValue = (sp.Value * config.AttrValue / 100 * level) * riseRate;

                attrs[attrId] = attrValue;
            }

            return attrs;
        }

        public void AddExp(long exp)
        {
            this.LevelExp.Data += exp;

            long fee = PetConfigCategory.Instance.GetPetFee(PetLevel.Data);

            if (this.LevelExp.Data >= fee)
            {
                this.LevelExp.Data -= fee;
                this.PetLevel.Data++;
            }
        }

        public void Devour(int attrId, long attrValue)
        {
            MagicData far = new MagicData();
            far.Data = attrValue;

            DevourFlairs.Add(new KeyValuePair<int, MagicData>(attrId, far));
        }

        public bool IsDevour(int attrId)
        {
            if (Flairs.Select(m => m.Key).Contains(attrId))
            {
                return false;
            }
            if (DevourFlairs.Select(m => m.Key).Contains(attrId))
            {
                return false;
            }

            return true;
        }

        public long GetSkillPercent()
        {
            int fc = GetDevourCount();
            return PetSkillRise[Flairs.Count - 1] + (PetLayer.Data - 1) * LayerRiseSkill + fc * 4;
        }

        private int[] PetSkillRise = new int[] { 5, 6, 7, 8, 10, 12, 15 };
    }
}
