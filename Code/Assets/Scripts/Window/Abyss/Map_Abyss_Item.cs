using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Abyss_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Button Btn_Start;

    private AbyssCopyConfig Config;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }


    private void OnClick_NavigateMap()
    {
        var dialog = this.GetComponentInParent<Map_Dialog_Abyss>();
        dialog.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartAbyss(Config.Id);
    }



    public void SetContent(AbyssCopyConfig config)
    {
        this.Config = config;

        Txt_Name.text = config.MapName;
    }

    public void Show(int cycle)
    {
        if (Config.Cycle == cycle)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
