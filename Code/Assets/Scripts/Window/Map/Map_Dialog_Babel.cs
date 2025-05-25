using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json;
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

    public Text Txt_Rank;

    public Toggle toggle_Auto;

    public Button Btn_Start;
    public Button Btn_Close;

    private bool IsNet = false;

    // Start is called before the first frame update
    void Start()
    {
        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.Babel_Auto = isOn;
        });

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

        if (user.Account != "" && ConfigHelper.Channel != ConfigHelper.Channel_Tap)
        {
            IsNet = true;
            Txt_Rank.gameObject.SetActive(true);
        }
        else
        {
            Txt_Rank.gameObject.SetActive(false);
        }

        if (progress == 0 && user.BabelCount.Data == 0)
        {
            user.BabelCount.Data = ConfigHelper.BabelCount * 2;
        }

        long nextProgress = progress + 1;

        Txt_Floor2.text = progress > 1 ? (progress - 1) + "层" : "";
        Txt_Floor1.text = progress > 0 ? progress + "层" : "";
        Txt_Floor0.text = nextProgress + "层";

        Txt_Progress.text = "挑战层数:" + nextProgress + "";
        Txt_Count.text = "今日挑战次数:" + user.BabelCount.Data;

        if (nextProgress > ConfigHelper.BabelMax)
        {
            Txt_Reward.text = "已通关，等待开发上限";
        }
        else
        {
            BabelConfig rewardConfig = BabelConfigCategory.Instance.GetByProgress(nextProgress);
            Item item = rewardConfig.BuildItem(nextProgress);
            Txt_Reward.text = "通过奖励:" + item.Name + "*" + item.Count;
        }

        if (IsNet)
        {
            try
            {
                //再存储新档
                StartCoroutine(NetworkHelper.GetRank("babel",
                        (WebResultWrapper result) =>
                        {
                            if (result.Code == StatusMessage.OK)
                            {
                                string name = result.Data["name"];
                                string time = result.Data["time"];
                                string rank = result.Data["rank"];

                                this.Txt_Rank.text = "最高纪录 " + name + " " + rank + "层" + " (" + time + ")";
                                AppHelper.BabelRecord = int.Parse(rank);
                            }
                            else
                            {
                                this.Txt_Rank.text = "读取失败.";
                            }
                        },
                        () =>
                        {
                            this.Txt_Rank.text = "读取失败.";
                        }
                        ));
            }
            catch (Exception ex)
            {
                this.Txt_Rank.text = "读取失败，请稍等一会重试...";
            }
        }
    }


    public void OnClick_Start()
    {
        User user = GameProcessor.Inst.User;

        if (user.BabelCount.Data <= 0)
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
