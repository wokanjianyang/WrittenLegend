using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pill : MonoBehaviour
{
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;

    public Panel_Pill pp;
    public Panel_Pill2 pp2;
    public Panel_Pill3 pp3;

    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;



    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        toggle1.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(1);
        });

        toggle2.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(2);
        });

        toggle3.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(3);
        });
    }



    private void ShowPanel(int index)
    {

        if (index == 1)
        {
            pp.gameObject.SetActive(true);
            pp2.gameObject.SetActive(false);
            pp3.gameObject.SetActive(false);
        }
        else if (index == 2)
        {
            pp.gameObject.SetActive(false);
            pp2.gameObject.SetActive(true);
            pp3.gameObject.SetActive(false);
        }
        else if (index == 3) {
            pp.gameObject.SetActive(false);
            pp2.gameObject.SetActive(false);
            pp3.gameObject.SetActive(true);
        }

    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
