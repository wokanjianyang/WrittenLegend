using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossFamily : MonoBehaviour, IBattleLife
{
    public Toggle toggle_Auto;

    public Button btn_Start;

    public Button btn_FullScreen;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.EquipBossFamily_Auto = isOn;
        });

        btn_Start.onClick.AddListener(this.OnClick_Start);
        btn_FullScreen.onClick.AddListener(this.OnClick_Close);
    }

    void Update()
    {

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenBossFamilyEvent>(this.OnOpenBossFamily);
    }


    private void OnOpenBossFamily(OpenBossFamilyEvent e)
    {
        this.gameObject.SetActive(true);
    }

    private void OnClick_Start()
    {
        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartBossFamily();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
