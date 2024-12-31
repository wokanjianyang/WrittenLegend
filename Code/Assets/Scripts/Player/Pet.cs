using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Pet
    {
        public MagicData Level { get; set; } = new MagicData();
        public MagicData Layer { get; set; } = new MagicData();

        public MagicData LevelExp { get; set; } = new MagicData();

        public MagicData LayerExp { get; set; } = new MagicData();

        public IDictionary<int, MagicData> Flair { get; set; } = new Dictionary<int, MagicData>();

        public string Name { get; set; }

        public int Status { get; set; }
        public int RunMapId { get; set; }



    }
}
