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

    public Transform tf_tgs;

    private List<Toggle> toggles;

    public Panel_Relic panel_Relic;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggles = tf_tgs.GetComponentsInChildren<Toggle>().ToList();

        Btn_Full.onClick.AddListener(OnClick_Close);

        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;

            if (ac && index > 1)
            {
                toggles[i].gameObject.SetActive(false);
            }
            else
            {
                toggles[i].onValueChanged.AddListener((isOn) =>
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

    private void ShowPanel(int id)
    {
        panel_Relic.ChangePanel(id);
    }
}
