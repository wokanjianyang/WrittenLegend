using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Fashion : MonoBehaviour, IBattleLife
{
    public Button Btn_Close;

    public Transform Tf_Nav;
    private List<Toggle> toggles;

    public Panel_Fashion PanelFashion;
    public Panel_Fashion_Special PanelFashionSpecial;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        toggles = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                ChangePanel(index);
            });
        }

        this.ChangePanel(0);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenFashionDialogEvent>(this.OnShow);
    }

    public void OnShow(OpenFashionDialogEvent e)
    {
        this.gameObject.SetActive(true);
    }

    private void ChangePanel(int index) {

        if (index == 2)
        {
            PanelFashion.gameObject.SetActive(false);
            PanelFashionSpecial.Show();
        }
        else {
            PanelFashion.Show(index);
            PanelFashionSpecial.gameObject.SetActive(false);
        }

    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
