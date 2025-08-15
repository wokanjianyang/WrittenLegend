using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Talent : MonoBehaviour, IBattleLife
{
    public Dialog_Talent_Detail DialogDetail;

    public Text Txt_Total;
    public Text Txt_Enable;
    public Text Txt_Used;

    public Transform Tf_Layer;
    private List<Toggle> tgLevelList;

    public HP_Progress ExpProgress;

    public List<Transform> tfs;
    private List<Item_Talent> ItemList = new List<Item_Talent>();
    public Button Btn_Close;
    public Button Btn_Reset;

    private const int LevelExp = 10000;

    private int SelectLayer = 0;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        tgLevelList = Tf_Layer.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Reset.onClick.AddListener(OnClick_Reset);



        for (int i = 0; i < tgLevelList.Count; i++)
        {
            int index = i;
            tgLevelList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLevel(index);
            });
        }
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<TalentShowEvent>(this.OnShowEvent);
        GameProcessor.Inst.EventCenter.AddListener<TalentDetailShowEvent>(this.OnShowDetailEvent);
    }

    private void Start()
    {
        this.ChangeLevel(0);

        this.Show();
    }


    private void ChangeLevel(int layer)
    {
        this.SelectLayer = layer;

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            Transform tf = tfs[i];
            if (i == SelectLayer)
            {
                tf.gameObject.SetActive(true);
                ItemList = tf.GetComponentsInChildren<Item_Talent>().ToList();
            }
            else
            {
                tf.gameObject.SetActive(false);
            }
        }

        this.Show();
    }

    private void OnShowEvent(TalentShowEvent e)
    {
        this.gameObject.SetActive(true);
        this.Refresh();
    }

    private void OnShowDetailEvent(TalentDetailShowEvent e)
    {
        DialogDetail.Open(e.Tid);
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        long total = user.TalentExp.Data / LevelExp;
        long used = user.TalentPoint;
        long enabled = total - used;

        long exp = user.TalentExp.Data % LevelExp;
        this.ExpProgress.SetProgress(exp, LevelExp);

        Txt_Total.text = "天赋等级：Lv" + total;
        Txt_Enable.text = "剩余天赋点：" + enabled;
        Txt_Used.text = "已分配天赋点：" + used;

        int startId = SelectLayer * 100;
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetContent(i + 1 + startId);
        }
    }

    public void Refresh()
    {
        User user = GameProcessor.Inst.User;

        long total = user.TalentExp.Data / LevelExp;
        long used = user.TalentPoint;
        long enabled = total - used;

        long exp = user.TalentExp.Data % LevelExp;
        this.ExpProgress.SetProgress(exp, LevelExp);

        Txt_Total.text = "天赋等级：Lv" + total;
        Txt_Enable.text = "剩余天赋点：" + enabled;
        Txt_Used.text = "已分配天赋点：" + used;

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].Show();
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_Reset()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重置天赋消耗1垓金币。是否确认？", true,
         () =>
         {
             ResetTalent();
         }, () =>
         {

         });
    }

    private void ResetTalent()
    {
        User user = GameProcessor.Inst.User;

        if (user.MagicGold.Data <= ConfigHelper.RestoreGold * 20000.0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足1垓", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(ConfigHelper.RestoreGold * 20000.0);

        user.TalentData.Clear();
        user.TalentPoint = 0;

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        this.Refresh();
    }
}
