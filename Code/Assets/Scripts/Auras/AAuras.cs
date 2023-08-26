using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    abstract public class AAuras
    {
        public APlayer SelfPlayer { get; set; }

        public AurasAttrConfig Config { get; set; }

        public AAuras(APlayer player, AurasAttrConfig config)
        {
            this.SelfPlayer = player;
            this.Config = config;
        }
        abstract public void Do();

        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
