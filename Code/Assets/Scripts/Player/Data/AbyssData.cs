using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class AbyssData
    {

        public MagicData Total { get; set; } = new MagicData();


        public int GetLevel()
        {
            if (Total.Data <= 0)
            {
                return 0;
            }

            int l = (int)Math.Sqrt(Total.Data);

            return Math.Min(l, 100);
        }


    }
}
