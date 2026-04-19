using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class SteedConfigCategory
    {
        //public Steed BuildByPack(int configId)
        //{

        //    GiftPackPet packPet = GiftPackPetCategory.Instance.Get(configId);

        //    Pet pet = new Pet(packPet.ItemId, packPet.Role);

        //    pet.PetLevel.Data = 1;
        //    pet.PetLayer.Data = 1;


        //    for (int i = 0; i < packPet.AttrIdList.Length; i++)
        //    {
        //        int attrId = packPet.AttrIdList[i];
        //        MagicData attrValue = new MagicData();
        //        attrValue.Data = packPet.AttrValueList[i];

        //        pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
        //    }

        //    return pet;
        //}

        public Steed BuildSteed(int configId)
        {
            int role = RandomHelper.RandomNumber(1, 4);
            Steed steed = new Steed(configId, role);

            steed.PetLevel.Data = 1;
            steed.PetLayer.Data = 1;

            List<KeyValuePair<int, int>> flairs = BuildAttr(configId, role);

            foreach (var flair in flairs)
            {
                int attrId = flair.Key;
                MagicData attrValue = new MagicData();
                attrValue.Data = flair.Value;

                steed.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            return steed;
        }

        private List<KeyValuePair<int, int>> BuildAttr(int configId, int role)
        {
            List<KeyValuePair<int, int>> flairs = new List<KeyValuePair<int, int>>();

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(configId);

            int quality = itemConfig.Quality;

            int total = 9 * 45;
            int tempTotal = 0;

            for (int i = 1; i <= quality; i++)
            {
                SteedConfig config = this.list.Where(m => m.StartQuality <= i && i <= m.EndQuality && (role == m.Role || m.Role == 0)).FirstOrDefault();

                int avg = (total - tempTotal) / (quality - i + 1);

                //Debug.Log("avg:" + avg);

                int attrValue = RandomHelper.RandomNumber(Math.Max(20, avg - 22), Math.Min(70, avg + 23));

                flairs.Add(new KeyValuePair<int, int>(config.AttrId, Math.Min(70, attrValue)));

                tempTotal += attrValue;
            }

            //Debug.Log("tempTotal:" + tempTotal);

            return flairs;
        }

        public SteedConfig GetByAttrId(int attrId)
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


        public int GetLayerFee(long layer)
        {
            return (int)Math.Min((4 + layer), 10);
        }

        public int GetLayerFeeTotal(long layer)
        {
            int total = 0;

            for (int i = 1; i < layer; i++)
            {
                total += GetLayerFee(i);
            }
            return total;
        }
    }


}
