using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Babel : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button Btn_Close;


    // Start is called before the first frame update
    void Start()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();
        //GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;


    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
