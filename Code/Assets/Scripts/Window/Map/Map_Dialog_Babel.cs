using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Babel : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public Text Txt_Current;
    public Text Txt_Next;
    public Text Txt_Preview;

    public Text Txt_Reward;

    public Button Btn_Start;
    public Button Btn_Close;


    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_Start);
        Btn_Close.onClick.AddListener(OnClick_Close);


        this.Init();
        //GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        long progress = user.BabelData.Data;

        Txt_Preview.text = progress > 1 ? (progress - 1) + "²ã" : "";
        Txt_Current.text = progress > 0 ? progress + "²ã" : "";
        Txt_Next.text = (progress + 1) + "²ã";

        Txt_Reward.text = "½±Àø:" + "XXXXX";

    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;


    }


    public void OnClick_Start()
    {
        User user = GameProcessor.Inst.User;

        if (user.PillTime.Time.Data < 5)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "ÌôÕ½´ÎÊý²»×ã", ToastType = ToastTypeEnum.Failure });
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
