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

    public int Order => (int)ComponentOrder.Dialog;

    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowDialogUserAttrEvent>(this.Show);
    }

    private void Show(ShowDialogUserAttrEvent e)
    {
        this.gameObject.SetActive(true);

        Item_Attr[] items = this.GetComponentsInChildren<Item_Attr>();

        Debug.Log("Item_Attr :" + items.Length);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
