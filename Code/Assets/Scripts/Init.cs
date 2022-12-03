using System;
using System.Collections;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

public class Init : MonoBehaviour
{
    [LabelText("战斗模式")]
    public RuleType RuleType = RuleType.Normal;

    void Awake()
    {
         DontDestroyOnLoad(this);
    }

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
        Log.ILog = new UnityLogger();
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
        StartCoroutine(IE_DelayAction(0.1f, () =>
        {
            var com = this.gameObject.AddComponent<GameProcessor>();
            com.LoadMap(this.RuleType);
        }));
    }

    private IEnumerator IE_DelayAction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
