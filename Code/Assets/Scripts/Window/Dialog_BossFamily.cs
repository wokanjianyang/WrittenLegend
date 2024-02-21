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

    public List<Button> BtnStartList;

    public Button btn_FullScreen;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.EquipBossFamily_Auto = isOn;
        });

        for (int i = 0; i < BtnStartList.Count; i++)
        {
            int index = i + 1;
            BtnStartList[i].onClick.AddListener(() => this.OnClick_Start(index));
        }


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

    private void OnClick_Start(int index)
    {
        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartBossFamily(index);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
