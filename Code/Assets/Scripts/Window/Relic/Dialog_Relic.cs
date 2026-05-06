using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Relic : MonoBehaviour, IBattleLife
{
    public Button Btn_Full;

    public Transform tf_Cycle;
    public Transform tf_Role1;
    public Transform tf_Role2;

    private List<Toggle> toggles_Cyle;

    private List<Toggle> toggles_Role1;
    private List<Toggle> toggles_Role2;

    public Panel_Relic panel_Relic;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggles_Cyle = tf_Cycle.GetComponentsInChildren<Toggle>().ToList();
        toggles_Role1 = tf_Role1.GetComponentsInChildren<Toggle>().ToList();
        toggles_Role2 = tf_Role2.GetComponentsInChildren<Toggle>().ToList();

        Btn_Full.onClick.AddListener(OnClick_Close);

        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        for (int i = 0; i < toggles_Cyle.Count; i++)
        {
            int index = i + 1;

            toggles_Cyle[i].onValueChanged.AddListener((isOn) =>
            {
                this.ShowCycle(index);
            });
        }


        for (int i = 0; i < toggles_Role1.Count; i++)
        {
            int index = i + 1;

            if (ac && index > 1)
            {
                toggles_Role1[i].gameObject.SetActive(false);
            }
            else
            {
                toggles_Role1[i].onValueChanged.AddListener((isOn) =>
                {
                    this.ShowPanel(index);
                });
            }
        }

        for (int i = 0; i < toggles_Role2.Count; i++)
        {
            int index = i + 6;

            if (ac && index > 1)
            {
                toggles_Role2[i].gameObject.SetActive(false);
            }
            else
            {
                toggles_Role2[i].onValueChanged.AddListener((isOn) =>
                {
                    this.ShowPanel(index);
                });
            }
        }

        this.ShowPanel(1);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<RelicShowEvent>(this.OnShow);
    }

    public void OnShow(RelicShowEvent e)
    {
        this.gameObject.SetActive(true);
    }
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


    private void ShowCycle(int cycle)
    {
        if (cycle == 1)
        {
            tf_Role1.gameObject.SetActive(true);
            tf_Role2.gameObject.SetActive(false);

            this.ShowPanel(1);
        }
        else
        {
            tf_Role1.gameObject.SetActive(false);
            tf_Role2.gameObject.SetActive(true);

            this.ShowPanel(6);
        }
    }

    private void ShowPanel(int id)
    {
        panel_Relic.ChangePanel(id);
    }
}
