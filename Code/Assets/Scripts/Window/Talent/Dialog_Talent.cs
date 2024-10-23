using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Talent : MonoBehaviour, IBattleLife
{
    private List<Item_Talent> ItemList = new List<Item_Talent>();
    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        ItemList = this.GetComponentsInChildren<Item_Talent>().ToList();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<TalentShowEvent>(this.OnShowEvent);
        GameProcessor.Inst.EventCenter.AddListener<TalentDetailShowEvent>(this.OnShowDetailEvent);
    }

    private void Start()
    {
        this.Show();
    }

    private void OnShowEvent(TalentShowEvent e)
    {
        this.gameObject.SetActive(true);
    }

    private void OnShowDetailEvent(TalentDetailShowEvent e)
    {

    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetContent(i + 1);
        }

    }



    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
