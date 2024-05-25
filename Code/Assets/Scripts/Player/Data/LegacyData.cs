using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class LegacyData
    {
        public Dictionary<int, List<int>> DropIdList = new Dictionary<int, List<int>>();

        public Dictionary<int, List<int>> DropLayerList = new Dictionary<int, List<int>>();

        public int GetDropId(int role)
        {
            if (!DropIdList.ContainsKey(role))
            {
                DropIdList[role] = new List<int>();
            }

            List<int> dropList = DropIdList[role];
            if (dropList.Count < 50)
            {
                for (int i = dropList.Count; i < 100; i++)
                {
                    int dropId = LegacyConfigCategory.Instance.GetDropItem(role).Id;
                    dropList.Add(dropId);
                }
            }

            int rs = dropList[0];
            dropList.RemoveAt(0);

            return rs;
        }

        public int GetDropLayer(int role, int layer)
        {
            if (!DropLayerList.ContainsKey(role))
            {
                DropLayerList[role] = new List<int>();
            }

            List<int> dropList = DropLayerList[role];
            if (dropList.Count < 50)
            {
                for (int i = dropList.Count; i < 100; i++)
                {
                    int dropId = LegacyConfigCategory.Instance.GetDropLayer();
                    dropList.Add(dropId);
                }
            }

            int rs = dropList[0];
            dropList.RemoveAt(0);

            return Math.Max(1, layer + rs - 3);
        }
    }
}
