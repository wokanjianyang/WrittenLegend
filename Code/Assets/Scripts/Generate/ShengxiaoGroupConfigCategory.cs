using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class ShengxiaoGroupConfigCategory
    {

    }

    public class ShengxiaoGroup
    {
        public List<ShengxiaoGroupItem> List;
    }

    public class ShengxiaoGroupItem
    {
        public int Level;

        public int Count;

        public ShengxiaoGroupConfig Config;
    }
}