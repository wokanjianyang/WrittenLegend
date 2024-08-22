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

    public Text Txt_Floor2;
    public Text Txt_Floor1;
    public Text Txt_Floor0;

    public Text Txt_Count;
    public Text Txt_Progress;
    public Text Txt_Reward;

    public Button Btn_Start;
    public Button Btn_Close;


    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_Start);
        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    void OnEnable()
    {
        this.Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;
        long progress = user.BabelData.Data;

        long nextProgress = progress + 1;

        Txt_Floor2.text = progress > 1 ? (progress - 1) + "��" : "";
        Txt_Floor1.text = progress > 0 ? progress + "��" : "";
        Txt_Floor0.text = nextProgress + "��";

        BabelConfig rewardConfig = BabelConfigCategory.Instance.GetByProgress(nextProgress);
        Item item = rewardConfig.BuildItem(nextProgress);

        Txt_Progress.text = "��ս����:" + nextProgress + "";
        Txt_Reward.text = "ͨ������:" + item.Name + "*" + item.Count;
        Txt_Count.text = "������ս����:" + user.BabelCount.Data;
    }


    public void OnClick_Start()
    {
        User user = GameProcessor.Inst.User;

        if (user.BabelCount.Data < 5)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "��ս��������", ToastType = ToastTypeEnum.Failure });
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
