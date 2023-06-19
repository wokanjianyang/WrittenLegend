using System;
using System.Collections;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using CodeStage.AntiCheat.Detectors;
using System.Threading.Tasks;
using SA.Android.Utilities;
using SA.Android.App;
using Newtonsoft.Json;

public class Init : MonoBehaviour
{
    [LabelText("战斗模式")]
    public RuleType RuleType = RuleType.Normal;

    [LabelText("加载界面")]
    public Transform LoadingPage;

    private const string BuglyAppIDForAndroid = "ff5ed4ccb9";

    private long currentTimeSecond = -1;

#if UNITY_EDITOR

    [LabelText("加速")]
    public float TimeScale = 1;

    private void OnValidate()
    {
        Time.timeScale = this.TimeScale;
        DOTween.timeScale = this.TimeScale;
    }
#endif


    void Awake()
    {
        DontDestroyOnLoad(this);
        Log.ILog = new ANLogger();

        BuglyAgent.DebugLog("Demo.Awake()", "Screen: {0} x {1}", Screen.width, Screen.height);

    }

    // Start is called before the first frame update
    void Start()
    {
        BuglyAgent.PrintLog(LogSeverity.LogInfo, "Demo Start()");

        InitBuglySDK();

        BuglyAgent.PrintLog(LogSeverity.LogWarning, "Init bugly sdk done");

        BuglyAgent.SetScene(0);


        AsyncStartAsync();
    }

    private async Task AsyncStartAsync()
    {
        AN_Preloader.LockScreen("正在获取时间...");

        var timeTaks = TimeCheatingDetector.GetOnlineTimeTask("https://www.baidu.com/");
        await timeTaks;
        this.currentTimeSecond = (long)timeTaks.Result.onlineSecondsUtc;
        Log.Debug("time:" + this.currentTimeSecond);
        
        AN_Preloader.UnlockScreen();

        this.LoadConfig();
        //初始化广告模块
        //初始化Bugly
        //初始化时间戳
        //加载存档
        //加载首页
        this.LoadHome2();

        Debug.Log(JsonConvert.SerializeObject(SynthesisConfigCategory.Instance.GetList()));
    }

    private void LoadConfig()
    {
        ConfigComponentNew.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void LoadHome()
    //{
    //    var homePrefab = Resources.Load<GameObject>("Prefab/Window/Home");
    //    var home = GameObject.Instantiate(homePrefab);
    //    home.transform.SetParent(GameObject.Find("Canvas").transform,false);
    //    StartCoroutine(IE_DelayAction(5f, () =>
    //    {
    //        home.gameObject.SetActive(false);
    //        var game = new GameObject("Game");
    //        var com = game.AddComponent<GameProcessor>();
    //        com.LoadMap(this.RuleType);
    //    }));
    //}

    private void LoadHome2()
    {
        var com = this.gameObject.AddComponent<GameProcessor>();
        var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>().transform;
        this.LoadingPage.gameObject.SetActive(false);

        StartCoroutine(IE_DelayAction(1f, () =>
        {
            com.LoadMap(this.RuleType,this.currentTimeSecond, map);
        }));
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
}
