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

        Txt_Floor2.text = progress > 1 ? (progress - 1) + "²ã" : "";
        Txt_Floor1.text = progress > 0 ? progress + "²ã" : "";
        Txt_Floor0.text = nextProgress + "²ã";

        BabelConfig config = BabelConfigCategory.Instance.GetByProgress(nextProgress);

        int ItemId = config.GetItemId(nextProgress);
        int ItemCount = config.GetItemCount(nextProgress);

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(ItemId);

        Txt_Reward.text = "½±Àø:" + itemConfig.Name + "*" + ItemCount;
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
