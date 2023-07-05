using DG.Tweening;
using Game.Dialog;
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

        private bool isLoadMap = false;
        private long saveTime = 0;

        void Awake()
        {
            if (Inst != null)
                Destroy(this);
            else
                Inst = this;
        }

        public void OnDestroy()
        {
            if (PlayerManager != null)
            {
                PlayerManager.OnDestroy();
            }

            foreach (var ie in delayActionIEs)
            {
                StopCoroutine(ie);
            }
            delayActionIEs.Clear();
        }

        // Start is called before the first frame update
        void Start()
        {
            this.EventCenter = new EventManager();
            this.PlayerInfo = Canvas.FindObjectOfType<PlayerInfo>(true);

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

            this.EventCenter.AddListener<ShowGameMsgEvent>(ShowGameMsg);
        }

        void Update()
        {
            if (this.IsGameRunning() && isLoadMap)
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
                            user.AddExpAndGold(exp, 0);

                            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                            {
                                Message = BattleMsgHelper.BuildSecondExpMessage(exp)
                            });
                        }
                    }
                }
            }

            //每分钟存档一次
            long ct = TimeHelper.ClientNowSeconds();
            if (saveTime == 0)
            {
                saveTime = ct;
            }
            if (ct - saveTime > 60)
            {
                saveTime = ct;
                UserData.Save();
            }
        }

        public void LoadMap(RuleType ruleType,long currentTimeSecond,Transform map)
        {
            this.PlayerManager = this.gameObject.AddComponent<PlayerManager>();

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

            isLoadMap = true;
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
            //this.PlayerManager.Save();
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

        public void UpdateInfo()
        {
            if (this.User != null)
            {
                this.User.EventCenter.Raise(new UserAttrChangeEvent());
            }

            if (this.PlayerManager != null && this.PlayerManager.GetHero() != null)
            {
                this.PlayerManager.GetHero().EventCenter.Raise(new HeroAttrChangeEvent());
            }
        }

        private void ShowGameMsg(ShowGameMsgEvent e)
        {
            var barragePrefab = Resources.Load<GameObject>("Prefab/Dialog/Msg");

            var msg = GameObject.Instantiate(barragePrefab);

            Transform parent = e.Parent == null ? PlayerRoot : e.Parent;
            msg.transform.SetParent(parent);

            var msgSize = msg.GetComponent<RectTransform>().sizeDelta;

            var msgX = -parent.position.x / 5;
            var msgY = parent.position.y / 5;

            msg.transform.localPosition = new Vector3(msgX, msgY);
            var com = msg.GetComponent<Dialog_Msg>();


            com.tmp_Msg_Content.text = string.Format("<color=#{0}>{1}</color>", "FF0000", e.Content);

            msg.transform.DOLocalMoveY(msgY * 2, 1f).OnComplete(() =>
             {
                 GameObject.Destroy(msg);
             });
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
