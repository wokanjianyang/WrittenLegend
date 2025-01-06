using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PetConfigCategory
    {

        public Pet BuildPet(int configId)
        {
            Pet pet = new Pet();

            IDictionary<int, MagicData> flair = new Dictionary<int, MagicData>();

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;
            pet.Flair = flair;

            return pet;
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
