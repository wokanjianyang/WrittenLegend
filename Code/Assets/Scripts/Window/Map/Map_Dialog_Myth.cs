using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Myth : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    private GameObject ItemPrefab;
    List<Map_Pill_Item> items = new List<Map_Pill_Item>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    void OnEnable()
    {
        this.Show();
    }

    private void Show()
    {

    }


    public void OnClick_Start()
    {
        User user = GameProcessor.Inst.User;

        if (user.BabelCount.Data < 5)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "挑战次数不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartBabel();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
