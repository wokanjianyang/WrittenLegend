using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pet : MonoBehaviour, IBattleLife
{
    public Transform tf_tgs;
    private List<Toggle> toggles;

    public Panel_Pet panelPet;
    public Panel_Pet_Speical panelSpeical;
    public Panel_Pet1 panelPet1;
    public Panel_Steed panelSteed;

    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        toggles = tf_tgs.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;

            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                this.ShowPanel(index);
            });

        }
    }

    private void Start()
    {
        this.ShowPanel(1);

        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        if (user.Cycle.Data >= 10 && !ac)
        {
            this.toggles[1].gameObject.SetActive(true);
        }
        else
        {
            this.toggles[1].gameObject.SetActive(false);
        }

        if (user.Cycle.Data >= 15 && !ac)
        {
            this.toggles[2].gameObject.SetActive(true);
        }
        else
        {
            this.toggles[2].gameObject.SetActive(false);
        }
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<PetShowEvent>(this.OnShow);
    }

    public void OnShow(PetShowEvent e)
    {
        this.gameObject.SetActive(true);
    }

    private void ShowPanel(int index)
    {
        panelPet.gameObject.SetActive(false);
        panelSpeical.gameObject.SetActive(false);
        panelPet1.gameObject.SetActive(false);
        panelSteed.gameObject.SetActive(false);

        if (index == 1)
        {
            panelPet.gameObject.SetActive(true);
        }
        else if (index == 2)
        {
            panelSpeical.gameObject.SetActive(true);
        }
        else if (index == 3)
        {
            panelPet1.gameObject.SetActive(true);
        }
        else if (index == 4)
        {
            panelSteed.gameObject.SetActive(true);
        }
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
