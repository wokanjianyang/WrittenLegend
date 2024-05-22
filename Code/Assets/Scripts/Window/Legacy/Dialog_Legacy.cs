using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Legacy : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button btn_Close;

    private List<Item_Copy_Legacy> items = new List<Item_Copy_Legacy>();

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Legacy/Item_Legacy");


        this.Init();
    }

    public void Init()
    {
        this.gameObject.SetActive(true);

        List<CardConfig> configs = CardConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Copy_Legacy>();

            com.SetContent(configs[i]);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }

        this.Show();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void OnClick_Start(int level)
    {
        AppHelper.DefendLevel = level;

        User user = GameProcessor.Inst.User;
        user.DefendData.BuildCurrent();
        DefendRecord record = user.DefendData.GetCurrentRecord();

        if (record == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有了挑战次数", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.gameObject.SetActive(false);

        record.Count.Data--;
        GameProcessor.Inst.EventCenter.Raise(new CloseViewMoreEvent());
        GameProcessor.Inst.EventCenter.Raise(new DefendStartEvent());
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
