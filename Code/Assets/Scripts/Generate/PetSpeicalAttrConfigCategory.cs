using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class PetSpeicalAttrConfigCategory
    {
        public List<PetSpeicalAttrConfig> GetList(int petId, int petLayer)
        {
            return this.list.Where(m => m.PetId == petId && m.StartLayer <= petLayer).ToList();
        }


    }

}
