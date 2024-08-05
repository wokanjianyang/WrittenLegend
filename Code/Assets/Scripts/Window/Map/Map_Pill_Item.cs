using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Pill_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Button Btn_Start;


    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }


    private void OnClick_NavigateMap()
    {

    }

    public void SetContent()
    {
    }
}
