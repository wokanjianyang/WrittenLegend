using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class SpiritData
    {

        public MagicData Level { get; set; } = new MagicData();

        public MagicData Layer { get; set; } = new MagicData();


    }
}
