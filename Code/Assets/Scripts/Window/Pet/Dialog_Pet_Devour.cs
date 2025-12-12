using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Pet_Devour : MonoBehaviour
{
    public ScrollRect sr_Panel;
    private List<Item_Box_Material> sourceList = new List<Item_Box_Material>();

    public Text TxtCommissionName;
    public Text TxtCommissionCount;

    public Transform Tf_Base;
    private List<Text> AttrBaseList;

    public Transform Tf_Devour;
    private List<Text> AttrDevourList;

    public Transform Tf_Select;
    private List<Item_Pet_Attr_Select> AttrSelectList;
    private int SelectAttrIndex = 0;

    public Button Btn_OK;
    public Button Btn_Close;

    int MinQuality = 7;

    private const int MaxMaterial = 48;

    private bool check = false;

    Item_Box_Material SelectMaterial;

    private Pet MainPet = null;

    int MeterailId = ItemHelper.Specail_Pet_Speical;
    int MeterailCount = 5;

    // Start is called before the first frame update
    void Awake()
    {
        this.Btn_OK.onClick.AddListener(OnClickOK);
        this.Btn_Close.onClick.AddListener(this.OnClick_Close);

        AttrSelectList = Tf_Select.GetComponentsInChildren<Item_Pet_Attr_Select>().ToList();
        AttrBaseList = Tf_Base.GetComponentsInChildren<Text>().ToList();
        AttrDevourList = Tf_Devour.GetComponentsInChildren<Text>().ToList();

        this.Init();
    }
    public void Init()
    {
        var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

        for (var i = 0; i < MaxMaterial; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Panel.content);
            empty.name = "Src_" + i;
        }

        ToggleGroup toggleGroup = Tf_Select.GetComponent<ToggleGroup>();
        for (int i = 0; i < AttrSelectList.Count; i++)
        {
            AttrSelectList[i].Init(toggleGroup);

            int index = i;
            AttrSelectList[i].AddListener(OnSelectAttr, index);
        }
    }

    // Update is called once per frame
    void Start()
    {
        //GameProcessor.Inst.EventCenter.AddListener<BoxSelectEvent>(this.OnBoxSelect);
    }

    public void Open(Pet pet)
    {
        this.MainPet = pet;
        this.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        this.Load();
    }

    private void OnSelectAttr(int index)
    {
        this.SelectAttrIndex = index;

        //Debug.Log("select attr idnex " + index);
    }

    private void Load()
    {
        //把之前的卸载
        this.SelectMaterial = null;

        foreach (Item_Box_Material sb in sourceList)
        {
            GameObject.Destroy(sb.gameObject);
        }
        sourceList.Clear();

        foreach (Text sp in AttrBaseList)
        {
            sp.gameObject.SetActive(false);
        }

        foreach (Item_Pet_Attr_Select item in AttrSelectList)
        {
            item.gameObject.SetActive(false);
        }

        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        TxtCommissionCount.text = string.Format("<color={0}>({1}/{2})</color>", "#FF0000", 0, 0);

        this.ShowMain();

        this.Check();
    }

    private void ShowMain()
    {
        this.Btn_OK.gameObject.SetActive(false);

        foreach (Item_Box_Material sb in sourceList)
        {
            GameObject.Destroy(sb.gameObject);
        }
        sourceList.Clear();

        //long RiseFlairs = (SelectPet.PetLayer.Data - 1) * Pet.LayerRiseAttr;
        Pet pet = MainPet;
        for (int index = 0; index < AttrBaseList.Count; index++)
        {
            Text txt = AttrBaseList[index];
            if (index < pet.Flairs.Count())
            {
                KeyValuePair<int, Game.Data.MagicData> flair = pet.Flairs[index];

                //long tf = flair.Value.Data + RiseFlairs;
                long tf = flair.Value.Data;
                txt.text = StringHelper.FormatAttrValueName(flair.Key) + "：" + tf;
                txt.gameObject.SetActive(true);
            }
            else
            {
                txt.gameObject.SetActive(false);
            }
        }

        for (int index = 0; index < AttrDevourList.Count; index++)
        {
            Text txt = AttrDevourList[index];
            if (index < pet.DevourFlairs.Count())
            {
                KeyValuePair<int, Game.Data.MagicData> flair = pet.DevourFlairs[index];

                //long tf = flair.Value.Data + RiseFlairs;
                long tf = flair.Value.Data;
                txt.text = StringHelper.FormatAttrValueName(flair.Key) + "：" + tf;
                txt.gameObject.SetActive(true);
            }
            else
            {
                txt.gameObject.SetActive(false);
            }
        }

        //选择符合条件的pet
        User user = GameProcessor.Inst.User;

        List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Pet && m.Item.GetQuality() == MinQuality && !m.Item.IsLock).ToList();
        //Debug.Log("pet list:" + list.Count);
        int BoxId = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (BoxId >= MaxMaterial)
            {
                return;
            }

            var bagBox = this.sr_Panel.content.GetChild(BoxId);

            BoxItem item = list[i];
            Pet boxPet = item.Item as Pet;
            if (boxPet.PetLayer.Data > 1 || boxPet.PetLevel.Data > 1)
            {
                continue;
            }

            Item_Box_Material box = this.CreateItem(item, bagBox, BoxId);
            this.sourceList.Add(box);

            BoxId++;
        }

        //默认选中第一个
        if (sourceList.Count > 0)
        {
            this.OnSelectMetial(sourceList[0]);
        }
    }

    private Item_Box_Material CreateItem(BoxItem item, Transform parent, int index)
    {
        ToggleGroup toggleGroup = sr_Panel.GetComponent<ToggleGroup>();

        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Box_Material");

        var go = GameObject.Instantiate(prefab);
        Item_Box_Material comItem = go.GetComponent<Item_Box_Material>();
        comItem.Init(item, toggleGroup);
        comItem.AddListener(OnSelectMetial);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }
    private void OnSelectMetial(Item_Box_Material item)
    {
        //Debug.Log("OnSelectMetial");
        this.SelectMaterial = item;
        Pet pet = SelectMaterial.Box_Item.Item as Pet;

        for (int index = 0; index < AttrSelectList.Count; index++)
        {
            Item_Pet_Attr_Select txt = AttrSelectList[index];
            if (index < pet.Flairs.Count())
            {
                KeyValuePair<int, Game.Data.MagicData> flair = pet.Flairs[index];

                //long tf = flair.Value.Data + RiseFlairs;
                long tf = flair.Value.Data;
                txt.gameObject.SetActive(true);
                txt.SetText(StringHelper.FormatAttrValueName(flair.Key) + "：" + tf);
            }
            else
            {
                txt.gameObject.SetActive(false);
            }
        }

        this.Check();
    }

    private void Check()
    {
        User user = GameProcessor.Inst.User;

        this.check = true;

        long count = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == MeterailId).Select(m => m.MagicNubmer.Data).Sum();

        string color = "#00FF00";

        if (count < MeterailCount)
        {
            color = "#FF0000";
            this.check = false;
            this.Btn_OK.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_OK.gameObject.SetActive(true);
        }

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(MeterailId);

        TxtCommissionName.text = "吞噬消耗：" + itemConfig.Name;
        TxtCommissionCount.text = string.Format("<color={0}>({1}/{2})</color>", color, count, MeterailCount);

    }
    public void OnClickOK()
    {
        this.Btn_OK.gameObject.SetActive(false);

        if (this.SelectMaterial == null || this.SelectAttrIndex < 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请下选择一个宠物，并且选择吞噬属性", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Check();

        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        Pet petMaterial = SelectMaterial.Box_Item.Item as Pet;
        var sp = petMaterial.Flairs[SelectAttrIndex];

        if (!MainPet.IsDevour(sp.Key))
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "不可以吞噬重复属性", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (MainPet.DevourFlairs.Count > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经吞噬过了", ToastType = ToastTypeEnum.Failure });
            return;
        }


        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认吞噬，吞噬之后无法修改，无法退回？", true, () =>
        {
            Devour();
        }, () => { });
    }

    private void Devour()
    {
        Pet petMaterial = SelectMaterial.Box_Item.Item as Pet;
        var sp = petMaterial.Flairs[SelectAttrIndex];

        //材料
        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = MeterailId,
            Quantity = MeterailCount
        });


        //销毁
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent()
        {
            BoxItem = SelectMaterial.Box_Item
        });

        sourceList.Remove(SelectMaterial);//移除包裹
        GameObject.Destroy(SelectMaterial.gameObject); //销毁包裹
        SelectMaterial = null;

        MainPet.Devour(sp.Key, sp.Value.Data);

        this.Load();

        GameProcessor.Inst.SaveData();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}

