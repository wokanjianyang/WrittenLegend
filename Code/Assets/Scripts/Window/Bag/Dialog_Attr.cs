using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Attr : MonoBehaviour, IBattleLife
{
    public Button btn_Close;

    //List<Item_Card> items = new List<Item_Card>();

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowDialogUserAttrEvent>(this.Show);
    }

    private void Init()
    {

    }

    private void Show(ShowDialogUserAttrEvent e)
    {
        this.gameObject.SetActive(true);
    }

    public int Order => (int)ComponentOrder.Dialog;

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}
