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
        private GameObject BoxDropPrefab = null;
        private GameObject Message_Prefab = null;

        private List<Sprite> BoxImageList = new List<Sprite>();
        private List<Sprite> PlayerList = new List<Sprite>();
        private List<Sprite> ValetList = new List<Sprite>();
        private List<Sprite> MonsterList = new List<Sprite>();
        private List<Sprite> MonsterWorldList = new List<Sprite>();
        private Sprite MonsterDefend = null;

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
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box8"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Box9"));

            BoxSelectPrefab = Resources.Load<GameObject>("Prefab/Window/GameItem/BoxSelect");

            BoxDropPrefab = Resources.Load<GameObject>("Prefab/Window/GameItem/Box_Drop");

            Message_Prefab = Resources.Load<GameObject>("Prefab/Dialog/Msg");

            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box1"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box2"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box3"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box4"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box5"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box6"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box7"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box8"));

            for (int i = 1; i <= 24; i++)
            {
                PlayerList.Add(Resources.Load<Sprite>("UI/Player/Fashion" + i));
            }

            ValetList.Add(Resources.Load<Sprite>("UI/Player/Player_Valet1"));
            ValetList.Add(Resources.Load<Sprite>("UI/Player/Player_Valet2"));
            ValetList.Add(Resources.Load<Sprite>("UI/Player/Player_Valet3"));

            MonsterList.Add(Resources.Load<Sprite>("UI/Player/Player_Monster1"));
            MonsterList.Add(Resources.Load<Sprite>("UI/Player/Player_Monster2"));
            MonsterList.Add(Resources.Load<Sprite>("UI/Player/Player_Monster3"));
            MonsterList.Add(Resources.Load<Sprite>("UI/Player/Player_Monster4"));
            MonsterList.Add(Resources.Load<Sprite>("UI/Player/Player_Monster5"));

            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World1"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World2"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World3"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World4"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World5"));

            MonsterDefend = Resources.Load<Sprite>("UI/Player/Player_Defend");
        }

        public GameObject GetBoxPrefab(int quanlity)
        {
            return ComBoxList[quanlity];
        }

        public Sprite GetBoxImage(int quanlity)
        {
            return BoxImageList[quanlity - 1];
        }


        public Com_Box CreateComBox(BoxItem item)
        {
            var prefab = GetBoxPrefab(item.Item.GetQuality());
            var box = GameObject.Instantiate(prefab);
            Com_Box comItem = box.GetComponent<Com_Box>();

            comItem.SetItem(item);

            return comItem;
        }

        public Box_Select CreateBoxSelect(Transform parent, BoxItem item, ComBoxType type, int cycle)
        {
            var go = GameObject.Instantiate(BoxSelectPrefab);
            Box_Select comItem = go.GetComponent<Box_Select>();
            comItem.SetItem(item, type, cycle);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public Box_Drop CreateBoxDrop(Transform parent, Item item)
        {
            var go = GameObject.Instantiate(BoxDropPrefab);
            Box_Drop comItem = go.GetComponent<Box_Drop>();

            comItem.SetItem(item);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public Box_Drop CreateBoxDrop(Transform parent, string name, int quality, int count)
        {
            var go = GameObject.Instantiate(BoxDropPrefab);
            Box_Drop comItem = go.GetComponent<Box_Drop>();

            comItem.SetItem(name, quality, count);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public GameObject MessagePrefab()
        {
            return this.Message_Prefab;
        }

        public Sprite GetFashion(int id)
        {
            return PlayerList[id - 1];
        }

        public Sprite GetValet(int id)
        {
            return ValetList[id - 1];
        }

        public Sprite GetMonster(int id)
        {
            if (id < 1 || id > 6)
            {
                id = 1;
            }

            return MonsterList[id - 1];
        }

        public Sprite GetMonsterWorld(int id)
        {
            return MonsterWorldList[id - 1];
        }

        public Sprite GetDefend()
        {
            return MonsterDefend;
        }
    }
}