using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Steed : Item
    {
        public const int LayerRiseAttr = 5;
        public const int LayerRiseSkill = 3;

        public MagicData SteedLevel { get; set; } = new MagicData();
        public MagicData SteedLayer { get; set; } = new MagicData();

        public MagicData LevelExp { get; set; } = new MagicData();

        //public MagicData LayerExp { get; set; } = new MagicData();
        public List<KeyValuePair<int, MagicData>> Flairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public int Role { get; set; }

        public int RunMapId { get; set; }

        public long RunTime { get; set; }

        public override int GetQuality()
        {
            return Flairs.Count;
        }

        public Steed(int configId, int role)
        {
            this.Type = ItemType.Steed;
            this.Role = role;
            this.ConfigId = configId;
            this.Name = ConfigHelper.SteedName[Role - 1];

        }

        public Dictionary<int, long> GetTotalFlairs()
        {
            Dictionary<int, long> flairs = new Dictionary<int, long>();

            long layer = SteedLayer.Data;

            for (int i = 0; i < Flairs.Count; i++)
            {
                int attrId = Flairs[i].Key;

                long flair = Flairs[i].Value.Data * (100) / 100 + (layer - 1) * LayerRiseAttr;

                flairs[attrId] = flair;
            }

            return flairs;
        }

        public Dictionary<int, double> GetBaseAttr()
        {
            Dictionary<int, double> attrs = new Dictionary<int, double>();

            long level = SteedLevel.Data;
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

            long fee = 0;
            if (this.GetQuality() == 9)
            {
                fee = PetConfigCategory.Instance.GetPetFee1(SteedLevel.Data);
            }
            else
            {
                fee = PetConfigCategory.Instance.GetPetFee(SteedLevel.Data);
            }

            if (this.LevelExp.Data >= fee)
            {
                this.LevelExp.Data -= fee;
                this.SteedLevel.Data++;
            }
        }


        public long GetSkillPercent()
        {
            return PetSkillRise[Flairs.Count - 1] + (SteedLayer.Data - 1) * LayerRiseSkill;
        }

        private int[] PetSkillRise = new int[] { 5, 6, 7, 8, 10, 15, 20, 25, 35 };
    }
}
