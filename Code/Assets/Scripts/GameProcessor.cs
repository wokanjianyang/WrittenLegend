using DG.Tweening;
using Game.Dialog;
using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SA.CrossPlatform.UI;
using UnityEngine;
using zFramework.Internal;
using Newtonsoft.Json;

namespace Game
{
    public class GameProcessor : MonoBehaviour
    {
        public static GameProcessor Inst { get; private set; } = null;

        public MapData MapData { get; private set; }

        public User User { get; private set; }

        public PlayerManager PlayerManager;

        private ABattleRule BattleRule;

        private BattleRule_Mine MineRule;

        public Transform PlayerRoot { get; private set; }

        public Transform EffectRoot { get; private set; }
        public Transform UIRoot_Top { get; private set; }

        public EventManager EventCenter { get; private set; }

        public PlayerInfo PlayerInfo { get; set; }

        public long CurrentTimeSecond { get; private set; }

        private List<Coroutine> delayActionIEs = new List<Coroutine>();

        private bool isLoadMap = false;
        private long saveTime = 0;
        public string OfflineMessage = "";

        // public bool RefreshSkill = false; //是否要刷新技能
        public bool isTimeError = false;
        public bool isCheckError = false;
        public bool isVersionError = false;

        private bool isGameOver { get; set; } = true;
        public PlayerType winCamp { get; private set; }

        public delegate void ShowDialog(string msg, bool showButton, Action doneAction, Action cancleAction);

        public ShowDialog ShowSecondaryConfirmationDialog;

        public delegate void ShowAd(string des, string action, Action<int> adResult);

        public ShowAd ShowVideoAd;

        private float currentToastShowTime = 0f;
        private List<ShowGameMsgEvent> toastTaskList = new List<ShowGameMsgEvent>();
        private GameObject barragePrefab;

        private Coroutine ie_autoExitKey = null;
        private Coroutine ie_autoStartCopy = null;
        private Coroutine ie_autoBossFamily = null;

        //副本临时设置
        public bool EquipCopySetting_Rate = false;
        public bool EquipCopySetting_Auto = false;
        public bool EquipBossFamily_Auto = false;

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

        }


        public bool LoadInit(string json)
        {
            string str_json = EncryptionHelper.AesDecrypt(json);
            //Debug.Log(str_json);

            //反序列化
            User user = JsonConvert.DeserializeObject<User>(str_json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (user != null)
            {
                this.User = user;
                this.User.LoadTicketTime = TimeHelper.ClientNowSeconds();
                this.User.DataDate = DateTime.Now.Ticks;

                return true;
            }

            return false;
        }

        public void Init(long currentTimeSecond)
        {
            if (currentTimeSecond > 0)
            {
                this.CurrentTimeSecond = currentTimeSecond;
            }

            this.EventCenter = new EventManager();
            this.PlayerInfo = Canvas.FindObjectOfType<PlayerInfo>(true);

            //启动就加载用户存档
            this.User = UserData.Load();

            this.User.Init();


            //判断是否非法时间
            if (UserData.StartTime < ConfigHelper.PackTime)
            {
                //load时间小于打包时间,必定是往前修改了时间
                isTimeError = true;
            }

            if (UserData.StartTime > ConfigHelper.PackEndTime)
            {
                //load时间大于结束时间,必须要更新
                isTimeError = true;
            }

            if (this.User.SecondExpTick > UserData.StartTime)
            {
                //收益时间已经大于启动时间了，必然是往后改了
                isTimeError = true;
            }

            if (!this.User.Record.Check())
            {
                isCheckError = true;
            }

            int MaxVersion = User.VersionLog.Select(m => m.Key).Max();
            if (ConfigHelper.Version < MaxVersion)
            {
                isVersionError = true;
            }

            long currentTick = TimeHelper.ClientNowSeconds();

            if (Math.Abs(UserData.StartTime - currentTick) > 60 * 2)
            {
                //终端时间和网络时间差2分钟
                isTimeError = true;
            }

            if (isTimeError)
            {
                OfflineMessage = BattleMsgHelper.BuildTimeErrorMessage();
                return;
            }


            Dialog_OfflineExp offlineExp = Canvas.FindObjectOfType<Dialog_OfflineExp>(true);

            offlineExp.ShowOffline();
            ////计算离线
            //if (User.SecondExpTick > 0)
            //{
            //    OfflineReward();
            //}

            this.Run();
        }

        public void Run()
        {
            var coms = Canvas.FindObjectsOfType<MonoBehaviour>(true);
            var battleComs = coms.Where(com => com is IBattleLife).Select(com => com as IBattleLife).ToList();
            battleComs.Sort((a, b) =>
            {
                if (a.Order < b.Order)
                {
                    return -1;
                }
                else if (a.Order > b.Order)
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
            this.EventCenter.AddListener<EndCopyEvent>(this.OnEndCopy);
            this.EventCenter.AddListener<BossFamilyEndEvent>(this.OnEndBossFamily);
            this.EventCenter.AddListener<CheckGameCheatEvent>(CheckGameCheat);


            ShowVideoAd += OnShowVideoAd;

            this.UIRoot_Top = GameObject.Find("Canvas/UIRoot/Top").transform;
            this.barragePrefab = Resources.Load<GameObject>("Prefab/Dialog/Toast");

            if (isTimeError || isCheckError || isVersionError)
            {
                StartCoroutine(this.AutoExitApp(true));
                return;
            }

            if (User.GameCheat)
            {
                //StartCoroutine(this.AutoExitApp(false));
                //return;
            }

            this.User.AdData.Check();
        }

        private void SecondRewarod()
        {
            if (User == null)
            {
                return;
            }

            if (isTimeError)
            {
                return;
            }

            int interval = 5;
            if (User.SecondExpTick == 0)
            {
                if (!isTimeError)
                {
                    User.SecondExpTick = TimeHelper.ClientNowSeconds();
                }
            }
            else
            {
                if (User.isError)
                {
                    if (TimeHelper.ClientNowSeconds() - UserData.StartTime > 60 * 3)
                    {
                        if (RandomHelper.RandomNumber(1, 10) > 8)
                        {
                            Application.Quit();
                            this.User = null;
                        }
                    }
                }

                if (TimeHelper.ClientNowSeconds() < (User.SecondExpTick - 60 * 2))
                {
                    isTimeError = true;
                    return;
                }

                long tempTime = Math.Min(TimeHelper.ClientNowSeconds() - User.SecondExpTick, ConfigHelper.MaxOfflineTime);
                long calTk = (tempTime) / interval;
                if (calTk >= 1)
                {
                    //5秒计算一次经验,金币
                    User.SecondExpTick += interval * calTk;
                    long exp = User.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp) * calTk;
                    long gold = User.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold) * calTk;
                    if (exp > 0 || gold > 0)
                    {
                        User.AddExpAndGold(exp, gold);

                        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                        {
                            Message = BattleMsgHelper.BuildSecondExpMessage(exp, gold)
                        });
                    }
                }
            }

        }

        void Update()
        {
            this.ShowNextToast();

            if (this.IsGameOver())
            {
                return;
            }
            if (isLoadMap)
            {
                this.BattleRule?.OnUpdate();
                this.MineRule?.OnUpdate();
            }

            //计算泡点经验
            SecondRewarod();

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

        public void LoadMin()
        {
            this.MineRule = new BattleRule_Mine();
        }

        public void LoadMap(RuleType ruleType, Transform map, Dictionary<string, object> param)
        {
            MapData = map.GetComponentInChildren<MapData>();
            MapData.Clear();

            this.PlayerRoot = MapData.transform.parent.Find("[PlayerRoot]").transform;

            this.EffectRoot = MapData.transform.parent.Find("[EffectRoot]").transform;

            bool autoHero = true;

            switch (ruleType)
            {
                case RuleType.Normal:
                    this.BattleRule = new BattleRule_Normal();
                    break;
                case RuleType.Survivors:
                    this.BattleRule = new BattleRule_Survivors();
                    break;
                case RuleType.EquipCopy:
                    this.BattleRule = new BattleRule_EquipCopy(param);
                    break;
                case RuleType.Phantom:
                    this.BattleRule = new BattleRule_Phantom(param);
                    break;
                case RuleType.BossFamily:
                    this.BattleRule = new Battle_BossFamily(param);
                    break;
                case RuleType.AnDian:
                    this.BattleRule = new Battle_AnDian(param);
                    break;
                case RuleType.Defend:
                    this.BattleRule = new Battle_Defend(param);
                    break;
                case RuleType.HeroPhantom:
                    autoHero = false;
                    this.BattleRule = new BattleRule_HeroPhantom(param);
                    break;
            }

            if (autoHero)
            {
                this.PlayerManager.LoadHero(ruleType);
            }

            isLoadMap = true;

            this.StartGame();
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

        //public void Pause()
        //{
        //    this.PauseCounter--;
        //    //this.PlayerManager.Save();
        //}

        //public void Resume()
        //{
        //    this.PauseCounter++;
        //}

        public bool IsGameOver()
        {
            //return this.PauseCounter == 0;
            return this.isGameOver;
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
            if (barragePrefab == null)
            {
                return;
            }
            toastTaskList.Add(e);
            this.ShowNextToast();
        }

        private void CheckGameCheat(CheckGameCheatEvent e)
        {
            if (User != null)
            {
                User.GameCheat = true;
            }
            StartCoroutine(this.AutoExitApp(false));
        }

        private void OnEndCopy(EndCopyEvent e)
        {
            if (ie_autoExitKey != null)
            {
                StopCoroutine(ie_autoExitKey);
            }
            ie_autoExitKey = null;
        }
        private void OnEndBossFamily(BossFamilyEndEvent e)
        {
            if (ie_autoExitKey != null)
            {
                StopCoroutine(ie_autoExitKey);
            }
            ie_autoExitKey = null;
        }

        private void ShowNextToast()
        {
            if (Time.realtimeSinceStartup - currentToastShowTime > 0.5f)
            {
                if (toastTaskList.Count > 0)
                {
                    var toast = toastTaskList[0];
                    toastTaskList.RemoveAt(0);

                    currentToastShowTime = Time.realtimeSinceStartup;

                    var msg = GameObject.Instantiate(barragePrefab);

                    msg.transform.SetParent(this.UIRoot_Top);


                    var msgX = 0;
                    var msgY = 200;

                    msg.transform.localPosition = new Vector3(msgX, msgY);
                    var com = msg.GetComponent<Dialog_Msg>();

                    var color = "FFFFFF";
                    switch (toast.ToastType)
                    {
                        case ToastTypeEnum.Normal:
                            color = "FFFFFF";
                            break;
                        case ToastTypeEnum.Failure:
                            color = "FF0000";
                            break;
                        case ToastTypeEnum.Success:
                            color = "CBFFC1";
                            break;
                        default:
                            color = "FFFFFF";
                            break;
                    }

                    com.tmp_Msg_Content.text = string.Format("<color=#{0}>{1}</color>", color, toast.Content);

                    //首先要创建一个DOTween队列
                    Sequence seq = DOTween.Sequence();

                    //seq.Append  里面是让主相机振动的临时试验代码
                    seq.Append(msg.transform.DOLocalMoveY(msgY + 100, 1f));

                    //seq.AppendInterval 传入的是一个时间数值，是在队列上一句代码执行完毕隔多长时间执行下一句代码
                    float delayedTimer = 0.5f;
                    seq.AppendInterval(delayedTimer);

                    seq.AppendCallback(() =>
                    {
                        GameObject.Destroy(msg);
                    });
                }

            }
        }

        void OnApplicationPause(bool isPaused)
        {
            //if(isPaused)
            //{
            //    this.Pause();
            //}
            //else
            //{
            //    this.Resume();
            //}
        }

        void OnApplicationQuit()
        {
            //TODO 存档
            UserData.Save();
        }

        public void HeroDie(RuleType ruleType, long time)
        {
            switch (ruleType)
            {
                case RuleType.EquipCopy:
                case RuleType.Phantom:
                case RuleType.BossFamily:
                case RuleType.HeroPhantom:
                    ie_autoExitKey = StartCoroutine(this.AutoExitMap(ruleType, time));
                    break;
                default:
                    StartCoroutine(this.AutoResurrection());
                    break;
            }
        }

        public void CloseBattle(RuleType ruleType, long time)
        {
            ie_autoExitKey = StartCoroutine(this.AutoExitMap(ruleType, time));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="des"></param>
        /// <param name="action"></param>
        /// <param name="adResult"></param>
        public void OnShowVideoAd(string des, string action, Action<int> adResult)
        {
            string title = "友情支持";
            string message = des;
            var builder = new UM_NativeDialogBuilder(title, message);
            builder.SetPositiveButton(des, () =>
            {
                Log.Debug(des);
                PocketAD.Inst.ShowAD(action, async delegate (int rv, AdStateEnum state, AdTypeEnum type)
                {
                    //var ret = false;
                    switch (state)
                    {
                        case AdStateEnum.Click:
                            Log.Debug("点击广告");
                            break;
                        case AdStateEnum.Close:
                            Log.Debug("关闭广告");
                            break;
                        case AdStateEnum.Reward:
                            Log.Debug("发放奖励");
                            //ret = true;
                            break;
                        case AdStateEnum.Show:
                            Log.Debug("广告显示");
                            break;
                        case AdStateEnum.LoadFail:
                            Log.Debug("广告加载失败");
                            break;
                        case AdStateEnum.NotSupport:
                            Log.Debug("不支持广告");
                            break;
                        case AdStateEnum.SkippedVideo:
                            Log.Debug("跳过广告");
                            break;
                        case AdStateEnum.VideoComplete:
                            Log.Debug("广告播放完毕");
                            //ret = true;
                            break;
                    }
                    // 到主线程执行
                    await Loom.ToMainThread;
                    adResult?.Invoke((int)(state));
                });
            });
            builder.SetNegativeButton("取消", async () =>
            {
                // 到主线程执行
                await Loom.ToMainThread;
                adResult?.Invoke(-1);
            });
            var dialog = builder.Build();
            dialog.Show();
        }

        public void SetGameOver(PlayerType winCamp)
        {
            this.isGameOver = true;
            this.winCamp = winCamp;
        }

        public void StartGame()
        {
            this.isGameOver = false;

            //Debug.Log("StartGame");

            if (GameProcessor.Inst.OfflineMessage != "")
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = GameProcessor.Inst.OfflineMessage
                });
                GameProcessor.Inst.OfflineMessage = "";
            }
        }

        private IEnumerator AutoResurrection()
        {
            for (int i = 0; i < 10; i++)
            {
                PlayerManager.GetHero().EventCenter.Raise(new ShowMsgEvent()
                {
                    Type = MsgType.Normal,
                    Content = $"{(10 - i)}秒后复活"
                });
                yield return new WaitForSeconds(1f);
            }
            PlayerManager.GetHero().Resurrection();
            this.StartGame();
        }

        private IEnumerator AutoExitMap(RuleType ruleType, long time)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerManager.GetHero().EventCenter.Raise(new ShowMsgEvent()
                {
                    Type = MsgType.Normal,
                    Content = $"{(5 - i)}秒后退出"
                });
                yield return new WaitForSeconds(1f);
            }
            this.EventCenter.Raise(new BattleLoseEvent() { Type = ruleType, Time = time });

            if (ruleType == RuleType.EquipCopy)
            {
                int rate = this.EquipCopySetting_Rate ? 5 : 1;

                //判断是否自动挑战
                if (EquipCopySetting_Auto && GameProcessor.Inst.User.MagicCopyTikerCount.Data >= rate)
                {
                    this.AutoEquipCopy();
                }
            }
            else if (ruleType == RuleType.BossFamily)
            {
                long bossTicket = GameProcessor.Inst.User.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);
                if (EquipBossFamily_Auto && bossTicket > 0)
                {
                    this.AutoBossFamily();
                }
            }
        }

        private void AutoEquipCopy()
        {
            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("5S后自动挑战装备副本", true,
            () =>
            {
                StopCoroutine(ie_autoStartCopy);
                AutoStartCopy();
            }, () =>
            {
                StopCoroutine(ie_autoStartCopy);
            });

            ie_autoStartCopy = StartCoroutine(this.ShowAutoStartCopy());
        }
        private IEnumerator ShowAutoStartCopy()
        {
            for (int i = 0; i < 5; i++)
            {
                this.EventCenter.Raise(new SecondaryConfirmTextEvent() { Text = $"{(5 - i)}秒后自动挑战副本" });
                yield return new WaitForSeconds(1f);
            }

            this.EventCenter.Raise(new SecondaryConfirmCloseEvent());

            AutoStartCopy();
        }

        private void AutoStartCopy()
        {
            this.EventCenter.Raise(new CopyViewCloseEvent());
            this.EventCenter.Raise(new AutoStartCopyEvent());
        }

        private void AutoBossFamily()
        {
            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("5S后自动挑战BOSS之家", true,
            () =>
            {
                StopCoroutine(ie_autoBossFamily);
                AutoStartBossFamily();
            }, () =>
            {
                StopCoroutine(ie_autoBossFamily);
            });

            ie_autoBossFamily = StartCoroutine(this.ShowAutoStartBossFamily());
        }

        private IEnumerator ShowAutoStartBossFamily()
        {
            for (int i = 0; i < 5; i++)
            {
                this.EventCenter.Raise(new SecondaryConfirmTextEvent() { Text = $"{(5 - i)}秒后自动挑战BOSS之家" });
                yield return new WaitForSeconds(1f);
            }

            this.EventCenter.Raise(new SecondaryConfirmCloseEvent());

            AutoStartBossFamily();
        }
        private void AutoStartBossFamily()
        {
            this.EventCenter.Raise(new CopyViewCloseEvent());
            this.EventCenter.Raise(new AutoStartBossFamily());
        }


        private IEnumerator AutoExitApp(bool type)
        {
            string text = "后自动关闭游戏,请更新";

            if (!type)
            {
                text = "后自动关闭游戏,请不要作弊";
            }

            if (isVersionError)
            {
                text = "后自动关闭游戏,请不要降版本";
            }

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("5S" + text, false, null, null);

            for (int i = 0; i < 5; i++)
            {
                this.EventCenter.Raise(new SecondaryConfirmTextEvent() { Text = $"{(5 - i)}S" + text });
                yield return new WaitForSeconds(1f);
            }

            Application.Quit();
        }
    }
}
