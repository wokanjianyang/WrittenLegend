using SDD.Events;
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
        
        public MapData MapData { get; private set; }

        public User User { get; private set; }

        public PlayerManager PlayerManager { get; private set; }

        private ABattleRule BattleRule;
        public Transform PlayerRoot { get; private set; }

        public Transform EffectRoot { get; private set; }

        public EventManager EventCenter { get; private set; }

        private int PauseCounter = -1;

        public PlayerInfo PlayerInfo { get; set; }

        public long CurrentTimeSecond { get; private set; }

        private List<Coroutine> delayActionIEs = new List<Coroutine>();

        void Awake()
        {
            if (Inst != null)
                Destroy(this);
            else
                Inst = this;
        }

        public void OnDestroy()
        {
            PlayerManager.OnDestroy();
            foreach(var ie in delayActionIEs)
            {
                StopCoroutine(ie);
            }
            delayActionIEs.Clear();
        }

        // Start is called before the first frame update
        void Start()
        {
            this.EventCenter = new EventManager();

            //启动就加载用户存档
            this.User = UserData.Load();
            this.User.Init();
            //加载礼包奖励
            GameProcessor.Inst.User.BuildReword();

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
            if (this.IsGameRunning())
            {
                this.BattleRule?.OnUpdate();
            }

            //计算泡点经验
            User user = GameProcessor.Inst.User;
            if (user != null)
            {
                int interval = 5;
                if (user.SecondExpTick == 0)
                {
                    user.SecondExpTick = TimeHelper.ClientNowSeconds();
                }
                else
                {
                    long calTk = (TimeHelper.ClientNowSeconds() - user.SecondExpTick) / interval;
                    if (calTk >= 1)
                    {  //5秒计算一次经验
                        user.SecondExpTick += interval * calTk;
                        long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp) * calTk;
                        if (exp > 0)
                        {
                            user.Exp += exp;
                            Debug.Log("经验:" + user.Exp);

                            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                            {
                                Message = BattleMsgHelper.BuildSecondExpMessage(exp)
                            });
                            user.EventCenter.Raise(new HeroInfoUpdateEvent());

                            if (user.Exp >= user.UpExp)
                            {
                                user.EventCenter.Raise(new HeroChangeEvent
                                {
                                    Type = User.UserChangeType.LevelUp
                                });
                            }
                        }
                    }
                }
            }
        }

        public void LoadMap(RuleType ruleType,long currentTimeSecond,Transform map)
        {
            this.PlayerManager = this.gameObject.AddComponent<PlayerManager>();
            PlayerInfo = Canvas.FindObjectOfType<PlayerInfo>(true);

            MapData = map.GetComponentInChildren<MapData>();
            switch (ruleType)
            {
                case RuleType.Normal:
                    this.BattleRule = new BattleRule_Normal();
                    break;
                case RuleType.Survivors:
                    this.BattleRule = new BattleRule_Survivors();
                    break;
                case RuleType.Tower:
                    this.BattleRule = new BattleRule_Tower();
                    break;
            }
            this.PlayerRoot = MapData.transform.parent.Find("[PlayerRoot]").transform;
            
            this.EffectRoot = MapData.transform.parent.Find("[EffectRoot]").transform;


            this.CurrentTimeSecond = currentTimeSecond;

            this.PlayerManager.LoadHero();
        }

        public void DelayAction(float delay, Action callback)
        {
            var ie = StartCoroutine(IE_DelayAction(delay, callback));
            delayActionIEs.Add(ie);
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
