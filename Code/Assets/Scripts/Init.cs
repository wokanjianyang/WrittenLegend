using System;
using System.Collections;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using ET;

public class Init : MonoBehaviour
{
    [LabelText("战斗模式")]
    public RuleType RuleType = RuleType.Normal;
    // Start is called before the first frame update
    void Start()
    {
        LoadConfig();
        //初始化广告模块
        //初始化Bugly
        //初始化时间戳
        //加载存档
        //加载首页
        this.LoadHome2();
    }

    private void LoadConfig()
    {
        //获取配置数据
        Options.Instance = new Options();
        Options.Instance.Develop = 1;
        Options.Instance.LogLevel = 0;

        Log.ILog = new UnityLogger();

        ET.Game.EventSystem.Add(typeof(LevelConfigCategory).Assembly);

        ResourcesComponent.Instance = new ResourcesComponent();
        ET.Game.Scene.AddComponent<ResourcesComponent>();
        ResourcesComponent.Instance.LoadBundle("config.unity3d");

        ConfigComponent.Instance = new ConfigComponent();
        ET.Game.Scene.AddComponent<ConfigComponent>();
        ConfigComponent.Instance.ConfigLoader = new ConfigLoader();
        ConfigComponent.Instance.Load();
        ResourcesComponent.Instance.UnloadBundle("config.unity3d");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadHome()
    {
        var homePrefab = Resources.Load<GameObject>("Prefab/Window/Home");
        var home = GameObject.Instantiate(homePrefab);
        home.transform.SetParent(GameObject.Find("Canvas").transform,false);
        StartCoroutine(IE_DelayAction(5f, () =>
        {
            home.gameObject.SetActive(false);
            var game = new GameObject("Game");
            var com = game.AddComponent<GameProcessor>();
            com.LoadMap(this.RuleType);
        }));
    }

    private void LoadHome2()
    {
        StartCoroutine(IE_DelayAction(0.1f, () =>
        {
            var game = new GameObject("Game");
            var com = game.AddComponent<GameProcessor>();
            com.LoadMap(this.RuleType);
        }));
    }

    private IEnumerator IE_DelayAction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
