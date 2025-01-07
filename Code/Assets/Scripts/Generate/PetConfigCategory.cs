using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PetConfigCategory
    {

        public Pet BuildPet(int configId, List<KeyValuePair<int, int>> flairs)
        {
            Pet pet = new Pet();

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;

            foreach (var flair in flairs)
            {
                int attrId = flair.Key;
                MagicData attrValue = new MagicData();
                attrValue.Data = flair.Value;

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            pet.Name = "³èÎï";
            pet.Flairs = new List<KeyValuePair<int, MagicData>>(); ;

            return pet;
        }

        public List<KeyValuePair<int, int>> BuildPetAttr(int quality)
        {
            List<KeyValuePair<int, int>> flairs = new List<KeyValuePair<int, int>>();

            for (int i = 1; i <= quality; i++)
            {
                List<PetConfig> temps = this.list.Where(m => m.StartQuality == quality).ToList();
                int index = RandomHelper.RandomNumber(1, temps.Count + 1);

                PetConfig config = temps[index - 1];
                int attrValue = RandomHelper.RandomNumber(config.MinValue, config.MaxCount + 1);

                flairs.Add(new KeyValuePair<int, int>(config.AttrId, attrValue));
            }

            return flairs;
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
