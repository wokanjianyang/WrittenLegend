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
        
        public MapData MapData { get; private set; }
        public PlayerManager PlayerManager { get; private set; }

        private ABattleRule BattleRule;
        public Transform PlayerRoot { get; private set; }

        public Transform EffectRoot { get; private set; }

        public EventManager EventCenter { get; private set; }

        private int PauseCounter = -1;

        public PlayerInfo PlayerInfo { get; set; }

        public long CurrentTimeSecond { get; private set; }

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
            var coms = Canvas.FindObjectsOfType<MonoBehaviour>(true);
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

        public void LoadMap(RuleType ruleType,long currentTimeSecond)
        {
            this.PlayerManager = this.gameObject.AddComponent<PlayerManager>();
            PlayerInfo = Canvas.FindObjectOfType<PlayerInfo>();

            MapData = GameObject.Find("Canvas").GetComponentInChildren<MapData>();
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
            this.PlayerManager.hero.Init();
            this.PlayerManager.hero.UpdatePlayerInfo();

            this.PlayerRoot = MapData.transform.parent.Find("[PlayerRoot]").transform;
            this.PlayerRoot.SetParent(MapData.transform.parent,false);
            
            this.EffectRoot = MapData.transform.parent.Find("[EffectRoot]").transform;
            this.EffectRoot.SetParent(MapData.transform.parent, false);

            this.EventCenter = new EventManager();

            this.CurrentTimeSecond = currentTimeSecond;

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
            //return this.PauseCounter == 0;
            return true;
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
            UserData.Save();
        }
    }
}
