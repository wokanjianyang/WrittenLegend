using System;
using System.Collections;
using Game;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //初始化广告模块
        //初始化Bugly
        //初始化时间戳
        //加载存档
        //加载首页
        this.LoadHome();
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
        StartCoroutine(IE_DelayAction(1f, () =>
        {
            home.gameObject.SetActive(false);
            var game = new GameObject("Game");
            game.AddComponent<GameProcessor>();
        }));
    }

    private IEnumerator IE_DelayAction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
