using DG.Tweening;
using Game.Dialog;
using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SA.CrossPlatform.UI;
using UnityEngine;
using zFramework.Internal;

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
        private string OfflineMessage = "";

        public bool RefreshSkill = false; //是否要刷新技能

        private PocketAD.AdStateCallBack adStateCallBack;
        
        private bool isGameOver { get; set; } = true;
        public PlayerType winCamp { get; private set; }

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

            //计算离线
            OfflineReward();

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
            
            adStateCallBack += OnAdStateCallBack;
        }

        private void OfflineReward()
        {
            long currentTick = TimeHelper.ClientNowSeconds();
            long offlineTime = currentTick - User.SecondExpTick;

            long offlineFloor = 0;
            long rewardExp = 0;
            long rewardGold = 0;

            long tempTime = Math.Min(offlineTime, 12 * 3600);
            while (tempTime > 0)
            {
                long tmepFloor = User.TowerFloor + offlineFloor + 100;
                TowerConfig config = TowerConfigCategory.Instance.GetByFloor(tmepFloor); //quick

                AttributeBonus offlineHero = User.AttributeBonus;
                AttributeBonus offlineTower = MonsterTowerHelper.BuildOffline(tmepFloor);

                SkillPanel sp = new SkillPanel(new SkillData(9001, (int)SkillPosition.Default), new List<SkillRune>(), new List<SkillSuit>());

                int roundHeroToTower = DamageHelper.CalcAttackRound(offlineHero, offlineTower, sp);
                int roundTowerToHero = DamageHelper.CalcAttackRound(offlineTower, offlineHero, sp);

                if (roundHeroToTower > roundTowerToHero)
                {
                    //fail
                    tempTime = 0;
                }
                else
                {
                    long floorTime = roundHeroToTower + 5; //5s find monster
                    long maxFloor = Math.Min(tempTime / floorTime, 100);

                    offlineFloor += maxFloor;
                    rewardExp += maxFloor * config.Exp;
                    rewardGold += maxFloor * config.Gold;
                    tempTime -= Math.Max(maxFloor, 1) * floorTime;
                }
            }

            User.TowerFloor += offlineFloor;

            long exp = User.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp) * (offlineTime / 5);
            long gold = User.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold) * (offlineTime / 5);

            User.AddExpAndGold(rewardExp + exp, rewardGold + gold);
            User.SecondExpTick = currentTick;

            OfflineMessage = BattleMsgHelper.BuildOfflineMessage(offlineTime, offlineFloor, rewardExp, rewardGold, exp, gold);
            Debug.Log(OfflineMessage);

            UserData.Save();
        }

        void Update()
        {
            if (this.IsGameRunning())
            {
                return;
            }
            if (isLoadMap)
            {
                this.BattleRule?.OnUpdate();
            }

            if (OfflineMessage != "")
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = OfflineMessage
                });
                OfflineMessage = "";
            }

            //计算泡点经验
            if (User != null)
            {
                int interval = 5;
                if (User.SecondExpTick == 0)
                {
                    User.SecondExpTick = TimeHelper.ClientNowSeconds();
                }
                else
                {
                    long calTk = (TimeHelper.ClientNowSeconds() - User.SecondExpTick) / interval;
                    if (calTk >= 1)
                    {  //5秒计算一次经验,金币
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

        public void LoadMap(RuleType ruleType, long currentTimeSecond, int mapId, Transform map)
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
                    this.BattleRule = new BattleRule_Tower(mapId);
                    break;
            }
            this.PlayerRoot = MapData.transform.parent.Find("[PlayerRoot]").transform;

            this.EffectRoot = MapData.transform.parent.Find("[EffectRoot]").transform;

            if (currentTimeSecond > 0)
            {
                this.CurrentTimeSecond = currentTimeSecond;
            }

            this.PlayerManager.LoadHero();

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
        
        public void HeroDie()
        {
            string title = "显示广告";
            string message = "激励视频广告测试";
            var builder = new UM_NativeDialogBuilder(title, message);
            builder.SetPositiveButton("看广告复活", () => {
                Log.Debug("看广告复活");
                PocketAD.Inst.ShowAD("ResurrectionHero", adStateCallBack);
            });
            builder.SetNegativeButton("取消", () =>
            {
                Log.Debug("不复活");
            });
            var dialog = builder.Build();
            dialog.Show();
        }
        
        
        public async void OnAdStateCallBack(int rv, AdStateEnum state, AdTypeEnum adType)
        {
            switch (state)
            {
                case AdStateEnum.Click:
                    Log.Debug("点击广告");
                    break;
                case AdStateEnum.Close:
                    Log.Debug("关闭广告");
                    // 到主线程执行
                    await Loom.ToMainThread;
                    PlayerManager.GetHero().Resurrection();
                    this.StartGame();
                    break;
                case AdStateEnum.Reward:
                    Log.Debug("发放奖励");

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

                    break;
            }

        }

        public void SetGameOver(PlayerType winCamp)
        {
            this.isGameOver = true;
            this.winCamp = winCamp;
        }

        public void StartGame()
        {
            this.isGameOver = false;
        }
    }
}
