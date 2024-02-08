using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using SA.Android.App;
using CodeStage.AntiCheat.Detectors;

using TapTap.Bootstrap;
using TapTap.Common;
using System.Text;

public class Init : MonoBehaviour
{
    public enum UILayer
    {
        Top = 0,
        Center,
        Bottom,
    }

    public enum WindowTypeEnum
    {
        //Top
        Loading = -1,

        //Bottom
        View_Map,
        View_Bag,
        View_Skill,
        View_EndlessTower,
        View_Forge,
        View_More,

        //Center
        View_TopStatu,
        View_BottomNavBar,
        Map_EquipCopy,
        Map_Phantom,
        Map_BossFamily,
        Map_AnDian,
        Map_Defend,
        Map_HeroPhantom,

        Select_Dialog,
        Dialog_EquipDetail,
        Dialog_Exclusive_Detail,
        Dialog_OfflineExp,
        Dialog_Settings,
        Dialog_FloatButtons,
        Dialog_SecondaryConfirmation,
        Dialog_SoulRing,
        Dialog_Achievement,
        Dialog_Defend,
        Dialog_Fashion,
        Dialog_Attr,
    }

    [LabelText("战斗模式")]
    public RuleType RuleType = RuleType.Normal;

    [LabelText("加载界面")]
    // public Transform LoadingPage;
    private const string BuglyAppIDForAndroid = "ff5ed4ccb9";

    public GameProcessor Game;

    // public Transform MapRoot;

    public Transform Bottom;
    public Transform Center;
    public Transform Top;

    private Dictionary<UILayer, List<WindowTypeEnum>> allWindows = new Dictionary<UILayer, List<WindowTypeEnum>>()
    {
        {
            UILayer.Bottom, new List<WindowTypeEnum>()
            {
                WindowTypeEnum.View_Map,
                WindowTypeEnum.View_Bag,
                WindowTypeEnum.View_Skill,
                WindowTypeEnum.View_EndlessTower,
                WindowTypeEnum.View_Forge,
                WindowTypeEnum.View_More
            }
        },
        {
            UILayer.Center, new List<WindowTypeEnum>()
            {
                WindowTypeEnum.View_TopStatu,
                WindowTypeEnum.View_BottomNavBar,
                WindowTypeEnum.Select_Dialog,
                WindowTypeEnum.Dialog_EquipDetail,
                WindowTypeEnum.Dialog_Exclusive_Detail,
                WindowTypeEnum.Dialog_Defend,
                //WindowTypeEnum.Dialog_OfflineExp,
                WindowTypeEnum.Dialog_Settings,
                WindowTypeEnum.Map_EquipCopy,
                WindowTypeEnum.Map_Phantom,
                WindowTypeEnum.Map_BossFamily,
                WindowTypeEnum.Map_AnDian,
                WindowTypeEnum.Map_Defend,
                WindowTypeEnum.Map_HeroPhantom,
                WindowTypeEnum.Dialog_SoulRing,
                WindowTypeEnum.Dialog_Achievement,
                WindowTypeEnum.Dialog_Fashion,
                WindowTypeEnum.Dialog_Attr,
            }
        },
        {
            UILayer.Top, new List<WindowTypeEnum>()
            {
                WindowTypeEnum.Dialog_FloatButtons,
                WindowTypeEnum.Loading,
                WindowTypeEnum.Dialog_SecondaryConfirmation,
            }
        }
    };

#if UNITY_EDITOR

    [LabelText("加速")]
    public float TimeScale = 1;

    private void OnValidate()
    {
        //Time.timeScale = this.TimeScale;
        //DOTween.timeScale = 1f / this.TimeScale;
    }
#endif


    void Awake()
    {
        DontDestroyOnLoad(this);
        Log.ILog = new ANLogger();
    }

    // Start is called before the first frame update
    void Start()
    {
        //保持屏幕常亮
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Log.Debug("Demo Start()");

        InitBuglySDK();
        //Log.Debug("Init bugly sdk done");
        //BuglyAgent.SetScene(0);

        InitTapSDK();

        AsyncTapAccount();
    }



    private async Task AsyncStartAsync()
    {
        AN_Preloader.LockScreen("正在获取时间...");

        var timeTaks = TimeCheatingDetector.GetOnlineTimeTask("https://www.baidu.com/");
        await timeTaks;
        long currentTimeSecond = (long)timeTaks.Result.onlineSecondsUtc;
        //Log.Debug("time:" + currentTimeSecond);

        UserData.StartTime = currentTimeSecond;

        AN_Preloader.UnlockScreen();

        this.LoadConfig();

        StartCoroutine(AsyncLoadWindows(currentTimeSecond));
    }

    private void LoadConfig()
    {
        ConfigComponentNew.Load();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator AsyncLoadWindows(long currentTimeSecond)
    {
        GameObject loadingPage = null;
        var layers = Enum.GetValues(typeof(UILayer));
        foreach (UILayer layer in layers)
        {
            allWindows.TryGetValue(layer, out var windowTypes);
            foreach (var winType in windowTypes)
            {
                var request = Resources.LoadAsync<GameObject>($"Prefab/Window/{winType.ToString()}");
                yield return request;
                if (request.asset != null)
                {
                    GameObject win = GameObject.Instantiate(request.asset as GameObject);
                    switch (layer)
                    {
                        case UILayer.Bottom:
                            win.transform.SetParent(Bottom, false);
                            break;
                        case UILayer.Center:
                            win.transform.SetParent(Center, false);
                            break;
                        case UILayer.Top:
                            win.transform.SetParent(Top, false);
                            break;
                    }
                    win.transform.localPosition = Vector3.zero;
                    var isLoading = winType == WindowTypeEnum.Loading;
                    if (isLoading)
                    {
                        loadingPage = win;
                    }
                    win.gameObject.SetActive(isLoading);
                }
                else
                {
                    Log.Error($"窗口：{winType.ToString()}不存在");
                }
            }
        }

        yield return null;
        loadingPage.gameObject.SetActive(false);
        Game.Init(currentTimeSecond);

        yield return null;
        var mapRoot = GameObject.FindObjectOfType<ViewBattleProcessor>();

        yield return new WaitForSeconds(1f);
        Game.LoadMap(RuleType.Normal, mapRoot.transform, null);
    }



    private IEnumerator IE_DelayAction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    void InitBuglySDK()
    {
        // TODO NOT Required. Set the crash reporter type and log to report
        // BuglyAgent.ConfigCrashReporter (1, 2);

        // TODO NOT Required. Enable debug log print, please set false for release version
#if DEBUG
        BuglyAgent.ConfigDebugMode(true);
#endif
        BuglyAgent.ConfigDebugMode(true);
        // TODO NOT Required. Register log callback with 'BuglyAgent.LogCallbackDelegate' to replace the 'Application.RegisterLogCallback(Application.LogCallback)'
        // BuglyAgent.RegisterLogCallback (CallbackDelegate.Instance.OnApplicationLogCallbackHandler);

        // BuglyAgent.ConfigDefault ("Bugly", null, "ronnie", 0);

        BuglyAgent.InitWithAppId(BuglyAppIDForAndroid);

        // TODO Required. If you do not need call 'InitWithAppId(string)' to initialize the sdk(may be you has initialized the sdk it associated Android or iOS project),
        // please call this method to enable c# exception handler only.
        BuglyAgent.EnableExceptionHandler();

        // TODO NOT Required. If you need to report extra data with exception, you can set the extra handler
        //        BuglyAgent.SetLogCallbackExtrasHandler (MyLogCallbackExtrasHandler);

        BuglyAgent.PrintLog(LogSeverity.LogInfo, "Init the bugly sdk");
    }

    private void InitTapSDK()
    {
        var config = new TapConfig.Builder()
            .ClientID("hljkzf86szjm1drye1") // 必须，开发者中心对应 Client ID
            .ClientToken("wmXYPPVmuLxj71r9FJOEafO5XudtQ3Qry6LjMy0W") // 必须，开发者中心对应 Client Token
            .ServerURL("https://hljkzf86.cloud.tds1.tapapis.cn") // 必须，开发者中心 > 你的游戏 > 游戏服务 > 基本信息 > 域名配置 > API
            .RegionType(RegionType.CN) // 非必须，CN 表示中国大陆，IO 表示其他国家或地区
            .ConfigBuilder();
        TapBootstrap.Init(config);


    }

    private async Task AsyncTapAccount()
    {
        var currentUser = await TDSUser.GetCurrent();

        if (null != currentUser)
        {
            UserData.tapAccount = currentUser.ObjectId;
        }

        AsyncStartAsync();
    }
}
