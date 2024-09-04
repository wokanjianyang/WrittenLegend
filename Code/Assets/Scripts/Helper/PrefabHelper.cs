using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class PrefabHelper
    {
        private List<GameObject> ComBoxList = new List<GameObject>();
        private GameObject BoxSelectPrefab = null;
        private GameObject Message_Prefab = null;

        private static PrefabHelper instance = null;

        public static PrefabHelper Instance()
        {
            if (instance == null)
            {
                instance = new PrefabHelper();
            }

            return instance;
        }

        public PrefabHelper()
        {
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box_White"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box_White"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box_Green"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box_Blue"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box_Pink"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box_Orange"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box6"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box7"));

            BoxSelectPrefab = Resources.Load<GameObject>("Prefab/Window/GameItem/BoxSelect");

            Message_Prefab = Resources.Load<GameObject>("Prefab/Dialog/Msg");
        }

        public GameObject GetBoxPrefab(int quanlity)
        {
            return ComBoxList[quanlity];
        }

        public Box_Select CreateBoxSelect(Transform parent, BoxItem item, ComBoxType type)
        {
            var go = GameObject.Instantiate(BoxSelectPrefab);
            Box_Select comItem = go.GetComponent<Box_Select>();
            comItem.SetItem(item, type);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public Com_Box CreateBoxDrop(Transform parent, Item item)
        {
            GameObject prefab = this.GetBoxPrefab(item.GetQuality());
            var go = GameObject.Instantiate(prefab);
            Com_Box comItem = go.GetComponent<Com_Box>();

            BoxItem boxItem = new BoxItem();
            boxItem.Item = item;
            boxItem.MagicNubmer.Data = item.Count;

            comItem.SetItem(boxItem);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public GameObject MessagePrefab()
        {
            return this.Message_Prefab;
        }
    }
}