using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Shengxiao : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    public Transform Tf_Type;
    private List<Toggle> tgTypeList;

    public Toggle toggle_Auto;

    private GameObject ItemPrefab;
    List<Map_Shengxiao_Item> items = new List<Map_Shengxiao_Item>();

    private int SelectLayer = 1;

    // Start is called before the first frame update
    void Awake()
    {
        tgTypeList = Tf_Type.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.Shengxiao_Auto = isOn;
        });

        for (int i = 0; i < tgTypeList.Count; i++)
        {
            int index = i + 1;
            tgTypeList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeType(index);
            });
        }

    }

    private void OnEnable()
    {
        this.ShowItemMax();
    }


    private void ChangeType(int layer)
    {
        this.SelectLayer = layer;

        ShowItemMax();
    }

    private void ShowItemMax()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        if (user.Cycle.Data < 20)
        {
            tgTypeList[1].gameObject.SetActive(false);
        }

        if (user.Cycle.Data < 25)
        {
            tgTypeList[2].gameObject.SetActive(false);
        }

        int max = GetMaxLayer(SelectLayer);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetMax(SelectLayer, max + 1);
        }
    }

    private int GetMaxLayer(int cycle)
    {
        User user = GameProcessor.Inst.User;

        int startKey = (cycle - 1) * 12 + 1;
        int endKey = (cycle - 1) * 12 + 12;

        List<Shengxiao> equips = user.ShengxiaoList.Where(m => m.Key >= startKey && m.Key <= endKey).Select(m => m.Value).ToList();

        for (int i = 0; i < 3; i++)
        {
            int quality = 8 - i;
            int count = equips.Where(m => m.GetQuality() >= quality).Count();

            if (count >= 11)
            {
                return 3 - i;
            }
        }

        return 0;
    }


    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Map/Map_Shengxiao_Item");

        List<ShengxiaoCopyConfig> list = ShengxiaoCopyConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
        }

        this.ShowItemMax();
    }

    private void BuildItem(ShengxiaoCopyConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Shengxiao_Item>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
