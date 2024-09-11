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
using System.Text;

public class Init : MonoBehaviour
{
    public enum UILayer
    {
        Top = 0,
        Center,
        Bottom,
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


    private Dictionary<UILayer, List<string>> allWindows = new Dictionary<UILayer, List<string>>()
    {
        {
            UILayer.Bottom, new List<string>()
            {
                "Window/View_Map",
                "Window/View_Bag",
                "Window/View_Skill",
                "Window/View_EndlessTower",
                "Window/View_Forge",
                "Window/View_More"
            }
        },
        {
            UILayer.Center, new List<string>()
            {
                "Window/View_TopStatu",
                "Window/View_BottomNavBar",

                "Window/Map/Map_EquipCopy",
                "Window/Map/Map_Phantom",
                "Window/Map/Map_BossFamily",
                "Window/Map/Map_AnDian",
                "Window/Map/Map_Defend",
                "Window/Map/Map_HeroPhantom",
                "Window/Map/Map_Infinite",
                "Window/Map/Map_Legacy",
                "Window/Map/Map_Pill",
                "Window/Map/Map_Babel",

                "Window/Dialog_Detail_Select",
                "Window/Dialog_Detail",
                "Window/Dialog_EquipDetail",
                "Window/Dialog_Exclusive_Detail",
                "Window/Defend/Dialog_Defend",
                "Window/Dialog_OfflineExp",
                "Window/Dialog_Settings",

                "Window/SoulRing/Dialog_SoulRing",
                "Window/Dialog_Achievement",
                "Window/Dialog_Fashion",
                "Window/Dialog_Attr",
                "Window/Legacy/Dialog_Legacy",

                "Window/Skill/Dialog_Divine",
            }
        },
        {
            UILayer.Top,  new List<string>()
            {
                "Window/Dialog_FloatButtons",
                "Window/Loading",
                "Window/Dialog_Drop",
                "Window/Dialog_SecondaryConfirmation",
            }
        }
    };

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

        //InitTapSDK();

        //AsyncTapAccount();

        AsyncStartAsync();
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
                var request = Resources.LoadAsync<GameObject>($"Prefab/{winType}");
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
                    var isLoading = winType == "Window/Loading";
                    if (isLoading)
                    {
                        loadingPage = win;
                    }
                    win.gameObject.SetActive(isLoading);
                }
                else
                {
                    Log.Error($"窗口：{winType}不存在");
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

        Game.LoadMin();
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

    //private void InitTapSDK()
    //{
    //    var config = new TapConfig.Builder()
    //        .ClientID("hljkzf86szjm1drye1") // 必须，开发者中心对应 Client ID
    //        .ClientToken("wmXYPPVmuLxj71r9FJOEafO5XudtQ3Qry6LjMy0W") // 必须，开发者中心对应 Client Token
    //        .ServerURL("https://hljkzf86.cloud.tds1.tapapis.cn") // 必须，开发者中心 > 你的游戏 > 游戏服务 > 基本信息 > 域名配置 > API
    //        .RegionType(RegionType.CN) // 非必须，CN 表示中国大陆，IO 表示其他国家或地区
    //        .ConfigBuilder();
    //    TapBootstrap.Init(config);


    //}

    //private async Task AsyncTapAccount()
    //{
    //    var currentUser = await TDSUser.GetCurrent();

    //    if (null != currentUser)
    //    {
    //        UserData.tapAccount = currentUser.ObjectId;
    //    }

    //    AsyncStartAsync();
    //}
}
