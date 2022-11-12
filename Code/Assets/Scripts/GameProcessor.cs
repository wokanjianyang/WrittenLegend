using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Game
{
    public class GameProcessor : MonoBehaviour
    {
        public static GameProcessor Inst { get; private set; } = null;
        
        public MapProcessor MapProcessor { get; private set; }
        public PlayerManager PlayerManager { get; private set; }

        private ABattleRule BattleRule;
        public Transform MapRoot { get; private set; }

        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Inst = this;
            //加载地图
            // this.LoadMap();
            //            
            
        }

        // Start is called before the first frame update
        void Start()
        {

            var coms = Canvas.FindObjectsOfType<MonoBehaviour>();
            var battleComs = coms.Where(com => com is IBattleLife).Select(com=>com as IBattleLife).ToArray();
            battleComs.Sort((a, b) =>
            {
                if (a.Order < b.Order)
                {
                    return -1;
                }
                else if(a.Order > b.Order)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
            foreach (var com in battleComs)
            {
                com.OnBattleStart();
            }
        }

        public void LoadMap(RuleType ruleType)
        {
            this.PlayerManager = this.gameObject.AddComponent<PlayerManager>();

            MapProcessor = GameObject.Find("Canvas").GetComponentInChildren<MapProcessor>();
            switch (ruleType)
            {
                case RuleType.Normal:
                    this.BattleRule = MapProcessor.gameObject.AddComponent<BattleRule_Normal>();
                    break;
                case RuleType.Survivors:
                    this.BattleRule = MapProcessor.gameObject.AddComponent<BattleRule_Survivors>();
                    break;
            }
            this.MapRoot = new GameObject().transform;
            this.MapRoot.SetParent(GameObject.Find("Canvas").transform,false);
        }

        public void DelayAction(float delay, Action callback)
        {
            StartCoroutine(IE_DelayAction(delay, callback));
        }
        private IEnumerator IE_DelayAction(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
    }
}
