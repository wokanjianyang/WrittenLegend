using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Refresh : MonoBehaviour
{
    public ScrollRect sr_Panel;

    private List<ItemRefresh> items = new List<ItemRefresh>();

    public RefreshAttr AttrOld;
    public RefreshAttr AttrNew;

    public Text txt_Total;
    public Text TxtCostName;
    public Text TxtCostCount;

    public Button Btn_Refesh;
    public Button Btn_OK;
    public Button Btn_Cancle;

    private int RefreshCount = 0;

    private const int MaxCount = 10; //10件装备

    Equip SelectEquip;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_Refesh.onClick.AddListener(OnClickReferesh);
        this.Btn_OK.onClick.AddListener(OnClickOK);
        this.Btn_Cancle.onClick.AddListener(OnClickCancle);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<RefershSelectEvent>(this.OnSelect);
    }

    void OnEnable()
    {
        this.Load();
    }

    public void Init()
    {
        var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

        for (var i = 0; i < MaxCount; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Panel.content);
            empty.name = "Box_" + i;
        }
    }

    private void Load()
    {
        //把之前的卸载
        this.SelectEquip = null;

        foreach (ItemRefresh cb in items)
        {
            GameObject.Destroy(cb.gameObject);
        }
        items.Clear();

        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        IDictionary<int, Equip> dict = user.EquipPanelList[user.EquipPanelIndex];

        for (int BoxId = 0; BoxId < MaxCount; BoxId++)
        {
            int postion = BoxId + 1;

            var bagBox = this.sr_Panel.content.GetChild(BoxId);
            if (bagBox == null || !dict.ContainsKey(postion))
            {
                continue;
            }

            Equip equip = dict[postion];

            if (equip.GetQuality() < 6)
            {
                continue;
            }

            ItemRefresh box = this.CreateBox(equip, bagBox);
            this.items.Add(box);
        }

        AttrOld.gameObject.SetActive(false);
        AttrNew.gameObject.SetActive(false);

        this.Btn_Refesh.gameObject.SetActive(false);
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_Cancle.gameObject.SetActive(false);
    }

    private ItemRefresh CreateBox(Equip equip, Transform parent)
    {
        ToggleGroup toggleGroup = sr_Panel.GetComponent<ToggleGroup>();

        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Refresh");

        var go = GameObject.Instantiate(prefab);
        ItemRefresh comItem = go.GetComponent<ItemRefresh>();
        comItem.Init(equip, toggleGroup);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void OnSelect(RefershSelectEvent e)
    {
        this.SelectEquip = e.Equip;

        this.Show();
    }

    private void Show()
    {
        AttrOld.gameObject.SetActive(true);
        AttrOld.Show(SelectEquip.AttrEntryList, SelectEquip.RuneConfigId, SelectEquip.SuitConfigId);
        AttrNew.gameObject.SetActive(false);

        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_Cancle.gameObject.SetActive(false);

        SelectEquip.CheckReFreshCount();
        if (SelectEquip.AttrEntryList.Count > 0) //SelectEquip.RefreshCount > 0 && 
        {
            this.Btn_Refesh.gameObject.SetActive(true);
        }
        else
        {
            this.Btn_Refesh.gameObject.SetActive(false);
        }

        User user = GameProcessor.Inst.User;
        this.txt_Total.text = "今日洗练总次数：" + user.RedRefreshCount.Data + "次";
        int specialId = ItemHelper.SpecailEquipRefreshId;
        int upCount = GetUpCount();

        ItemConfig refreshConfig = ItemConfigCategory.Instance.Get(specialId);
        this.TxtCostName.text = refreshConfig.Name;


        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();

        string color = stoneTotal >= upCount ? "#11FF11" : "#FF0000";
        this.TxtCostCount.text = string.Format("<color={0}>{1}/{2}</color>", color, stoneTotal, upCount);
    }

    private int GetUpCount()
    {
        long total = GameProcessor.Inst.User.RedRefreshCount.Data;
        long layer = Math.Min(total / 200 + 1, 10);

        return (int)(layer * 10);
    }


    public void OnClickReferesh()
    {
        int specialId = ItemHelper.SpecailEquipRefreshId;
        int upCount = GetUpCount();

        User user = GameProcessor.Inst.User;

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal < upCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的洗练石不足" + upCount + "个", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Btn_Refesh.gameObject.SetActive(false);
        this.Btn_OK.gameObject.SetActive(true);
        this.Btn_Cancle.gameObject.SetActive(true);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = specialId,
            Quantity = upCount
        });

        List<KeyValuePair<int, long>> keyValues = SelectEquip.Data.GetAttrList();

        int runeId = SelectEquip.Data.GetRuneId();

        int suitId = SelectEquip.Data.GetSuitId();

        AttrNew.gameObject.SetActive(true);
        AttrNew.Show(keyValues, runeId, suitId);

        user.RedRefreshCount.Data++;
        this.txt_Total.text = "今日洗练总次数：" + user.RedRefreshCount.Data + "次";


        stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();
        string color = stoneTotal >= upCount ? "#11FF11" : "#FF0000";
        this.TxtCostCount.text = string.Format("<color={0}>{1}/{2}</color>", color, stoneTotal, upCount);

        RefreshCount++;
        if (RefreshCount > 7)
        {
            RefreshCount = 0;
            GameProcessor.Inst.SaveData();
        }

        if (user.RedRefreshCount.Data % 200 == 199) {

            GameProcessor.Inst.SaveNetData();
        }
    }

    public void OnClickOK()
    {
        this.SelectEquip.Refesh(true);

        this.Show();
    }

    public void OnClickCancle()
    {
        this.SelectEquip.Refesh(false);

        this.Show();
    }
}

