using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameProcessor : MonoBehaviour
    {
        public static GameProcessor Inst { get; private set; } = null;
        
        public APlayer Hero;
        
        public MapProcessor MapProcessor { get; private set; }
        public PlayerManager PlayerManager { get; private set; }

        private ABattleRule BattleRule;
        public Transform MapRoot { get; private set; }

        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Inst = this;
            //加载地图
            this.LoadMap();
            //            
            this.PlayerManager = this.gameObject.AddComponent<PlayerManager>();
            this.BattleRule = this.gameObject.AddComponent<BattleRule_Normal>();
        }

        // Start is called before the first frame update
        void Start()
        {
            var coms = this.transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IBattleLife _com)
                {
                    _com.OnBattleStart();
                }
            }
        }

        private void LoadMap()
        {
            MapProcessor = GameObject.Find("Canvas").GetComponentInChildren<MapProcessor>();
            
            this.MapRoot = new GameObject().transform;
            this.MapRoot.SetParent(GameObject.Find("Canvas").transform,false);
        }
    }
}
