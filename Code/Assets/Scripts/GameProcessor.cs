using SDD.Events;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameProcessor : MonoBehaviour
    {
        public static GameProcessor Inst { get; private set; } = null;
        
        public MapDrawHelper MapProcessor { get; private set; }
        public PlayerManager PlayerManager { get; private set; }

        private ABattleRule BattleRule;
        public Transform PlayerRoot { get; private set; }

        public Transform EffectRoot { get; private set; }

        public EventManager EventCenter { get; private set; }

        private int PauseCounter = -1;


        void Awake()
        {
            if (Inst != null)
                Destroy(this);
            else
                Inst = this;
        }

        void OnDestroy()
        {
            if (Inst == this)
                Inst = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            var coms = Canvas.FindObjectsOfType<MonoBehaviour>();
            var battleComs = coms.Where(com => com is IBattleLife).Select(com=>com as IBattleLife).ToList();
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

        void Update()
        {
            if(this.IsGameRunning())
            {
                this.BattleRule?.OnUpdate();
            }
        }

        public void LoadMap(RuleType ruleType)
        {
            this.PlayerManager = this.gameObject.AddComponent<PlayerManager>();

            MapProcessor = GameObject.Find("Canvas").GetComponentInChildren<MapDrawHelper>();
            switch (ruleType)
            {
                case RuleType.Normal:
                    this.BattleRule = new BattleRule_Normal();
                    break;
                case RuleType.Survivors:
                    this.BattleRule = new BattleRule_Survivors();
                    break;
            }

            //加载档案
            this.PlayerManager.hero = UserData.Load();

            this.PlayerRoot = new GameObject("[PlayerRoot]").transform;
            this.PlayerRoot.SetParent(MapProcessor.transform,false);
            
            this.EffectRoot = new GameObject("[EffectRoot]").transform;
            this.EffectRoot.SetParent(MapProcessor.transform,false);

            this.EventCenter = new EventManager();
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

        public void Pause()
        {
            this.PauseCounter--;
            this.PlayerManager.Save();
        }

        public void Resume()
        {
            this.PauseCounter++;
        }

        public bool IsGameRunning()
        {
            return this.PauseCounter == 0;
        }


        void OnApplicationPause(bool isPaused)
        {
            if(isPaused)
            {
                this.Pause();
            }
            else
            {
                this.Resume();
            }
        }

        void OnApplicationQuit()
        {
            //TODO 存档
            UserData.Save(PlayerManager.hero);
        }
    }
}
