using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Halidom : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button btn_Close;

    private int SelectStage = 1;
    public List<Toggle> toggleStageList = new List<Toggle>();

    private List<Item_Halidom> items = new List<Item_Halidom>();

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Halidom");

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i + 1;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        Init();

        this.ChangePanel(SelectStage);
    }

    private void Init()
    {
        List<HalidomConfig> configs = HalidomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            Item_Halidom com = item.GetComponentInChildren<Item_Halidom>();

            long level = user.GetHalidomLevel(configs[i].Id);
            com.SetContent(configs[i], level);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }

    private void ChangePanel(int index)
    {
        this.SelectStage = index;
        this.Show();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < items.Count; i++)
        {
            if (this.SelectStage == items[i].Config.Layer)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
