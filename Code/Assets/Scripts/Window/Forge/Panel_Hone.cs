using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Hone : MonoBehaviour
{
    public ScrollRect sr_Panel;

    private List<Item_Forge_Main> mainList = new List<Item_Forge_Main>();

    public List<Item_Hone> honeList = new List<Item_Hone>();

    public Transform Tf_AttrParent;

    public Text Txt_Fee;

    public Button Btn_OK;

    private const int MaxCount = 10; //10件装备

    private Equip SelectEquip;
    private int SelectAttrIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);

        for (int i = 0; i < honeList.Count; i++)
        {
            int index = i;
            honeList[i].AddListener(OnSelectAttr, index);
        }
    }

    // Update is called once per frame
    void Start()
    {

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

        ToggleGroup toggleGroup = Tf_AttrParent.GetComponent<ToggleGroup>();
        for (int i = 0; i < honeList.Count; i++)
        {
            honeList[i].Init(toggleGroup);
        }
    }

    public void Load()
    {
        //把之前的卸载
        this.SelectEquip = null;
        //this.SelectAttrIndex = 0;

        foreach (Item_Forge_Main cb in mainList)
        {
            GameObject.Destroy(cb.gameObject);
        }
        mainList.Clear();

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

            Item_Forge_Main box = this.CreateItem(equip, bagBox, BoxId);
            this.mainList.Add(box);
        }

        this.Tf_AttrParent.gameObject.SetActive(false);

        Txt_Fee.gameObject.SetActive(false);


        this.Btn_OK.gameObject.SetActive(false);
    }

    private Item_Forge_Main CreateItem(Equip equip, Transform parent, int index)
    {
        ToggleGroup toggleGroup = sr_Panel.GetComponent<ToggleGroup>();

        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Forge_Main");

        var go = GameObject.Instantiate(prefab);
        Item_Forge_Main comItem = go.GetComponent<Item_Forge_Main>();
        comItem.Init(equip, toggleGroup);
        comItem.AddListener(OnSelectMain);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void OnSelectMain(Item_Forge_Main item)
    {
        this.SelectEquip = item.GameItem as Equip;
        this.Show();
        this.ShowAttr();
    }

    private void Show()
    {
        this.Btn_OK.gameObject.SetActive(true);

        Tf_AttrParent.gameObject.SetActive(true);

        for (int i = 0; i < SelectEquip.AttrEntryList.Count; i++)
        {
            int attrId = SelectEquip.AttrEntryList[i].Key;
            long attrVal = SelectEquip.AttrEntryList[i].Value;

            int honeLevel = SelectEquip.GetHoneLevel(i);

            honeList[i].SetItem(attrId, attrVal, honeLevel, SelectEquip.Layer);
        }

        int part = SelectEquip.Part;
        int layer = SelectEquip.Layer;

        EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == part && m.Layer == layer).FirstOrDefault();

        if (config == null)
        {
            return;
        }

        Txt_Fee.gameObject.SetActive(true);
        Txt_Fee.text = "0/99";

    }

    private void OnSelectAttr(int index)
    {
        this.SelectAttrIndex = index;
        this.ShowAttr();
    }

    private void ShowAttr()
    {
        if (SelectEquip == null)
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        Item_Hone hone = honeList[SelectAttrIndex];

        int attrId = SelectEquip.AttrEntryList[SelectAttrIndex].Key;
        long attrVal = SelectEquip.AttrEntryList[SelectAttrIndex].Value;

        int MaxLevel = EquipHoneConfigCategory.Instance.GetMaxLevel(attrId, attrVal, SelectEquip.Layer);

        int honeLevel = SelectEquip.GetHoneLevel(SelectAttrIndex);

        int needCount = GetNeedNumber(honeLevel);
        long count = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Red_Stone).Select(m => m.MagicNubmer.Data).Sum();

        string color = "#00FF00";
        if (count < needCount)
        {
            color = "#FF0000";
            //this.check = false;

            Btn_OK.gameObject.SetActive(false);
        }
        else
        {
            Btn_OK.gameObject.SetActive(true);
        }

        if (honeLevel >= MaxLevel)
        {
            Btn_OK.gameObject.SetActive(false);
        }

        Txt_Fee.text = string.Format("<color={0}>({1}/{2})</color>", color, needCount, count);
    }

    private int GetNeedNumber(int honeLevel)
    {
        return EquipHoneConfigCategory.Instance.GetNeedNumber(honeLevel);
    }

    public void OnClickOK()
    {
        Btn_OK.gameObject.SetActive(false);

        int honeLevel = SelectEquip.GetHoneLevel(SelectAttrIndex);

        int needCount = GetNeedNumber(honeLevel);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Red_Stone,
            Quantity = needCount
        });

        SelectEquip.Hone(SelectAttrIndex);

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

        this.Show();
        this.ShowAttr();

        GameProcessor.Inst.SaveData();
    }
}

