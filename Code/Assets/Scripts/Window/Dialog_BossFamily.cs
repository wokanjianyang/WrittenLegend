using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossFamily : MonoBehaviour, IBattleLife
{
    public Toggle toggle_Rate;
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

        User user = GameProcessor.Inst.User;
        if (user.IsDz())
        {
            toggle_Rate.gameObject.SetActive(true);
        }
        else
        {
            toggle_Rate.gameObject.SetActive(false);
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
        int layer = (GameProcessor.Inst.User.MapId - ConfigHelper.MapStartId) / 35;

        for (int i = 0; i < BtnStartList.Count; i++)
        {
            if (i <= layer)
            {
                BtnStartList[i].gameObject.SetActive(true);
            }
            else
            {
                BtnStartList[i].gameObject.SetActive(false);
            }
        }

        this.gameObject.SetActive(true);
    }

    private void OnClick_Start(int index)
    {
        int rate = toggle_Rate.isOn ? 5 : 1;

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartBossFamily(index, rate);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
