using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class PetConfigCategory
    {
        public Pet BuildByPack(int configId)
        {

            GiftPackPet packPet = GiftPackPetCategory.Instance.Get(configId);

            Pet pet = new Pet(packPet.Role);

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;


            for (int i = 0; i < packPet.AttrIdList.Length; i++)
            {
                int attrId = packPet.AttrIdList[i];
                MagicData attrValue = new MagicData();
                attrValue.Data = packPet.AttrValueList[i];

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            return pet;
        }

        public Pet BuildPet(int configId)
        {

            int role = RandomHelper.RandomNumber(1, 4);
            Pet pet = new Pet(role);

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;

            List<KeyValuePair<int, int>> flairs = BuildPetAttr(configId, role);

            foreach (var flair in flairs)
            {
                int attrId = flair.Key;
                MagicData attrValue = new MagicData();
                attrValue.Data = flair.Value;

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            return pet;
        }

        private List<KeyValuePair<int, int>> BuildPetAttr(int configId, int role)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(configId);

            int quality = itemConfig.Quality;

            List<KeyValuePair<int, int>> flairs = new List<KeyValuePair<int, int>>();

            int total = quality * 30;

            int tempTotal = 0;

            for (int i = 1; i <= quality; i++)
            {
                List<PetConfig> temps = this.list.Where(m => m.StartQuality <= i && i <= m.EndQuality && (role == m.Role || m.Role == 0)).ToList();
                int index = RandomHelper.RandomNumber(1, temps.Count + 1);

                PetConfig config = temps[index - 1];

                int avg = (total - tempTotal) / (quality - i + 1);

                int attrValue = RandomHelper.RandomNumber(Math.Max(10, avg - 15), Math.Min(50, avg + 15));

                flairs.Add(new KeyValuePair<int, int>(config.AttrId, Math.Min(50, attrValue + quality)));

                tempTotal += attrValue;
            }

            return flairs;
        }

        public PetConfig GetByAttrId(int attrId)
        {
            return this.list.Where(m => m.AttrId == attrId).FirstOrDefault();
        }

        public long GetPetFee(long level)
        {
            return 1000 + (level - 1) * 100;
        }

        public long GetFeeTotal(long level)
        {
            long total = 0;
            for (int i = 1; i < level; i++)
            {
                total += GetPetFee(i);
            }
            return total;
        }

        public int GetPetLayerFee(long layer)
        {
            return (int)Math.Min((4 + layer), 10);
        }

        public long GetPetTotalFee(long layer)
        {
            long total = 0;

            for (int i = 2; i <= layer; i++)
            {
                int fee = PetConfigCategory.Instance.GetPetLayerFee(i - 1);
                total += fee;
            }

            return total;
        }

        public int GetPetLayerFeeTotal(long layer)
        {
            int total = 0;

            for (int i = 1; i < layer; i++)
            {
                total += GetPetLayerFee(i);
            }
            return total;
        }
    }

    public partial class PetConfig
    {
        public long GetAttr(long layer)
        {
            return 0;
        }

        public long GetFee(long layer)
        {
            return 0;
        }


    }

}
