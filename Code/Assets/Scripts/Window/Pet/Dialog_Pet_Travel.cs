using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pet_Travel : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Text Txt_Info;
    public Toggle toggle_Hide;

    public Button Btn_Close;

    public Transform Tf_Layer;

    private List<Toggle> tgLevelList;

    private int LevelCount = 35; //每个难度多少个
    private int ShowCount = 10; //隐藏的时候显示多少个

    private int MaxLayer = -1;
    private int SelectLayer = -1;

    private Pet SelectPet;

    List<Item_Travel> items = new List<Item_Travel>();

    private void Awake()
    {
        tgLevelList = Tf_Layer.GetComponentsInChildren<Toggle>().ToList();
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Travel");

        Btn_Close.onClick.AddListener(OnClick_Close);

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            int index = i;
            tgLevelList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLevel(index);
            });
        }

        toggle_Hide.onValueChanged.AddListener((isOn) =>
        {
            this.Show();
        });

        this.Init();
    }

    private void Init()
    {
        List<MapConfig> list = MapConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (MapConfig config in list)
        {
            BuildItem(config);
        }
    }

    private void BuildItem(MapConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        Item_Travel com = item.GetComponent<Item_Travel>();

        com.Init(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    private void ChangeLevel(int layer)
    {
        this.SelectLayer = layer;
        this.Show();
    }

    public void Open(Pet pet)
    {
        this.gameObject.SetActive(true);
        this.SelectPet = pet;

        this.Show();
    }

    private void Show()
    {
        if (this.SelectPet.RunMapId > 0)
        {
            MapConfig map = MapConfigCategory.Instance.Get(this.SelectPet.RunMapId);
            Txt_Info.text = "当前巡游地图为：" + map.Name;
        }

        int PetQuality = this.SelectPet.GetQuality();

        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }

        int MapId = GameProcessor.Inst.User.MapId;
        this.MaxLayer = (MapId - ConfigHelper.MapStartId) / 35;

        if (this.SelectLayer < 0)
        {
            this.SelectLayer = Math.Min(this.MaxLayer, PetQuality);
            tgLevelList[SelectLayer].isOn = true;
        }

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            if (i <= MaxLayer && i < PetQuality)
            {
                tgLevelList[i].gameObject.SetActive(true);
            }
            else
            {
                tgLevelList[i].gameObject.SetActive(false);
            }
        }

        int count = MapConfigCategory.Instance.GetAll().Where(m => m.Value.Id <= MapId).Count();

        int startIndex = this.SelectLayer * LevelCount;
        int endIndex = startIndex + Math.Min(LevelCount, count - startIndex) - 1;

        int j = 0;
        for (int i = endIndex; i >= startIndex; i--)
        {
            if (j < ShowCount)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(!toggle_Hide.isOn);
            }
            j++;
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
