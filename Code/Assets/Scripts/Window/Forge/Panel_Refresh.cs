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

    public Button Btn_Auto;

    public Button Btn_Setting;
    public Button Btn_Setting_OK;
    public Transform Tf_Setting;
    public Transform Tf_Setting_Item_List;
    private List<Refresh_Setting_Item> ItemList = new List<Refresh_Setting_Item>();

    public Toggle Toggle_Auto;

    private int RefreshCount = 0;

    private const int MaxCount = 10; //10件装备

    Equip SelectEquip;

    //自动强化
    private bool Auto = false;
    private int AutoTotal = 0;
    private int AutoFrequency = 20;
    private Dictionary<int, int> AutoAttrDict = new Dictionary<int, int>();

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_Refesh.onClick.AddListener(OnClickReferesh);
        this.Btn_OK.onClick.AddListener(OnClickOK);
        this.Btn_Cancle.onClick.AddListener(OnClickCancle);

        this.Btn_Setting.onClick.AddListener(OnOpenSetting);
        this.Btn_Setting_OK.onClick.AddListener(OnClickSettingOK);
    }

    // Update is called once per frame
    void Start()
    {
        this.Toggle_Auto.gameObject.SetActive(false);

        Toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                this.Btn_Refesh.gameObject.SetActive(false);
            }
            else
            {
                this.Btn_Refesh.gameObject.SetActive(true);
            }

            this.Auto = isOn;
        });

        GameProcessor.Inst.EventCenter.AddListener<RefershSelectEvent>(this.OnSelect);
    }

    void OnEnable()
    {
        this.Load();
    }

    private void Update()
    {
        if (Auto && SelectEquip != null)
        {
            AutoTotal++;
            if (AutoTotal % AutoFrequency == 0)
            {
                //自动洗练
                if (!DoFrefresh(5))
                {
                    Auto = false;
                }

                //check
                bool check = true;

                List<KeyValuePair<int, long>> keyValues = SelectEquip.Data.GetAttrList();
                foreach (var kv in AutoAttrDict)
                {
                    int n = keyValues.Where(m => m.Key == kv.Key).Count();
                    if (n < kv.Value)
                    {
                        check = false;
                        break;
                    }
                }

                if (check)
                {
                    //success
                    Toggle_Auto.isOn = false;
                    Auto = false;

                    this.Btn_Refesh.gameObject.SetActive(false);
                    this.Btn_OK.gameObject.SetActive(true);
                    this.Btn_Cancle.gameObject.SetActive(true);
                }
                else
                {
                    this.SelectEquip.Refesh(false);
                }
            }
        }
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
        this.Auto = false;

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
        this.Btn_Refesh.gameObject.SetActive(false);
        this.Btn_OK.gameObject.SetActive(true);
        this.Btn_Cancle.gameObject.SetActive(true);

        DoFrefresh(1);
    }


    private bool DoFrefresh(int type)
    {
        int specialId = ItemHelper.SpecailEquipRefreshId;
        int upCount = GetUpCount();

        User user = GameProcessor.Inst.User;

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal < upCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的洗练石不足" + upCount + "个", ToastType = ToastTypeEnum.Failure });
            return false;
        }

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
        if (RefreshCount > 7 * type)
        {
            RefreshCount = 0;
            GameProcessor.Inst.SaveData();
        }

        if ((user.RedRefreshCount.Data + 1) % (200 * type) == 0)
        {

            GameProcessor.Inst.SaveNetData();
        }

        return true;
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

    private void OnOpenSetting()
    {
        if (ItemList.Count == 0)
        {
            var itemPrefab = Resources.Load<GameObject>("Prefab/Window/Forge/Refresh_Setting_Item");

            List<AttrEntryConfig> attrs = AttrEntryConfigCategory.Instance.GetRedAttrList();
            for (int i = 0; i < attrs.Count; i++)
            {
                var item = GameObject.Instantiate(itemPrefab);
                item.transform.SetParent(Tf_Setting_Item_List);
                item.transform.localScale = Vector3.one;
                item.gameObject.SetActive(true);


                var com = item.GetComponent<Refresh_Setting_Item>();
                com.SetItem(attrs[i].AttrId);

                ItemList.Add(com);
            }
        }

        this.Tf_Setting.gameObject.SetActive(true);
    }

    private void OnClickSettingOK()
    {
        int total = 0;

        for (int i = 0; i < ItemList.Count; i++)
        {
            int count = ItemList[i].GetCount();

            if (count > 0)
            {
                AutoAttrDict[ItemList[i].AttrId] = count;
                total += count;
            }
        }

        if (total > 6 || total <= 3)
        {
            AutoAttrDict.Clear();
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "设置错误,保留词条总和，必须是4-6之间" });
            return;
        }

        Toggle_Auto.isOn = false;
        this.Toggle_Auto.gameObject.SetActive(true);
        this.Tf_Setting.gameObject.SetActive(false);
    }
}

