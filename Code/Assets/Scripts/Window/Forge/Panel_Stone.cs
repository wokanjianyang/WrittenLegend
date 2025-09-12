using Game;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Stone : MonoBehaviour
{
    public ScrollRect sr_Panel;

    private List<Item_Forge_Item> items = new List<Item_Forge_Item>();

    public ToggleGroup tg_Main;
    private List<Stone_Item_Main> mainList = new List<Stone_Item_Main>();

    public ToggleGroup tg_Stone;
    private List<Stone_Item> stoneList = new List<Stone_Item>();

    public StrenthAttrItem AttrItem;
    public Text Txt_Fee;
    public Text Txt_Fee_Set;


    public Button Btn_Active;
    public Button Btn_Restore;
    public Button Btn_OK;
    public Button Btn_OK_Batch;

    private const int MaxCount = 10; //10件装备
    private const int Quality = 7;

    private const int StartPosition = 21;
    private const int MaxLevel = 14;
    private const int BatchCount = 5;

    private int SelectPosition = 0;
    private int MainIndex = 0;
    private int StoneId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        this.items = sr_Panel.content.GetComponentsInChildren<Item_Forge_Item>().ToList();
        this.mainList = tg_Main.GetComponentsInChildren<Stone_Item_Main>().ToList();
        this.stoneList = tg_Stone.GetComponentsInChildren<Stone_Item>().ToList();

        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
        this.Btn_Active.onClick.AddListener(OnClickActive);
        this.Btn_OK_Batch.onClick.AddListener(OnClickOKBatch);
        this.Btn_Restore.onClick.AddListener(OnRestore);
    }

    // Update is called once per frame
    void Start()
    {
        this.SelectPosition = 1;
        this.MainIndex = 1;
        this.StoneId = 0;

        ShowForgeItem();
        ShowStoneMain();
        ShowStone();
    }

    public void Init()
    {
        ToggleGroup ItemGroup = sr_Panel.GetComponent<ToggleGroup>();
        var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Forge_Item");

        for (var i = 1; i <= MaxCount; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Panel.content);
            empty.name = "Box_" + i;

            Item_Forge_Item item = empty.GetComponent<Item_Forge_Item>();
            item.toggle.group = ItemGroup;
            item.SetContent(i, i);

            item.AddListener(SelectForgeItem);

            items.Add(item);
        }

        for (int i = 0; i < mainList.Count; i++)
        {
            mainList[i].toggle.group = tg_Main;
            mainList[i].AddListener(SelectStoneMain);
        }

        List<StoneConfig> stoneConfigs = StoneConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
        for (int i = 0; i < stoneConfigs.Count; i++)
        {
            stoneList[i].toggle.group = tg_Stone;
            stoneList[i].SetContent(stoneConfigs[i]);

            stoneList[i].AddListener(SelectStone);
        }
    }

    private void SelectForgeItem(int id)
    {
        Debug.Log("SelectForgeItem id:" + id);

        this.SelectPosition = id;
        this.MainIndex = 1;
        this.StoneId = 0;

        ShowForgeItem();

        mainList[MainIndex - 1].toggle.isOn = true;
        ShowStoneMain();
    }

    private void ShowForgeItem()
    {
        User user = GameProcessor.Inst.User;
        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int setCount = record.GetSetCount();

        for (int i = 0; i < mainList.Count; i++)
        {
            Stone_Item_Main main = mainList[i];

            if (i <= setCount)
            {
                //Debug.Log("SelectForgeItem-stoneId:" + stoneId);

                int level = record.GetStoneLevel(i + 1);
                int stoneId = record.GetStoneId(i + 1);

                //可以镶嵌
                main.toggle.interactable = true;
                main.SetContent(i + 1, stoneId, level);
            }
            else
            {
                main.toggle.interactable = false;
            }
        }

        for (int i = 0; i < stoneList.Count; i++)
        {
            stoneList[i].toggle.interactable = false;
            stoneList[i].Show();
        }

        if (record.GetSetCount() < 2)
        {
            long fee = setCount + 1;
            long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Stone_Set);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(ItemHelper.SpecialId_Stone_Set);

            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            Txt_Fee_Set.text = string.Format("<color={0}>{1}</color>", color, itemConfig.Name + ":" + materialCount + "/ " + fee);

            Txt_Fee_Set.gameObject.SetActive(true);
            Btn_Active.gameObject.SetActive(true);
        }
        else
        {
            Txt_Fee_Set.gameObject.SetActive(false);
            Btn_Active.gameObject.SetActive(false);
        }

        if (record.List.Count > 0)
        {
            this.Btn_Restore.gameObject.SetActive(true);
        }
        else
        {
            this.Btn_Restore.gameObject.SetActive(false);
        }
    }

    private void SelectStoneMain(int index)
    {
        Debug.Log("SelectMain Index:" + index);

        this.MainIndex = index;

        ShowStoneMain();
        ShowStone();
    }

    private void ShowStoneMain()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_OK_Batch.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;
        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int stoneId = record.GetStoneId(MainIndex);

        if (stoneId == 0)
        {
            StoneSetConfig setConfig = StoneSetConfigCategory.Instance.Get(SelectPosition);

            List<int> excludeList = record.GetExcludeStoneId(MainIndex);

            Debug.Log("excludeList:" + JsonConvert.SerializeObject(excludeList));

            for (int i = 0; i < stoneList.Count; i++)
            {
                StoneConfig stoneConfig = StoneConfigCategory.Instance.Get(i + 1);

                if (setConfig.TypeList.Contains(stoneConfig.Type) && !excludeList.Contains(stoneConfig.Id))
                {
                    stoneList[i].toggle.interactable = true;
                    if (stoneId == 0)
                    {
                        stoneId = stoneConfig.Id;
                        stoneList[i].toggle.isOn = true;
                    }
                }
                else
                {
                    stoneList[i].toggle.interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < stoneList.Count; i++)
            {
                if (stoneId == i + 1)
                {
                    stoneList[i].toggle.interactable = true;
                    stoneList[i].toggle.isOn = true;
                }
                else
                {
                    stoneList[i].toggle.interactable = false;
                }
            }
        }

        if (StoneId == 0)
        {
            StoneId = stoneId;
        }
    }

    private void SelectStone(int stoneId)
    {
        Debug.Log("SelectStone id:" + stoneId);
        this.StoneId = stoneId;

        ShowStone();
    }

    private void ShowStone()
    {
        //显示属性，费用
        User user = GameProcessor.Inst.User;
        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int level = record.GetStoneLevel(MainIndex);

        StoneConfig config = StoneConfigCategory.Instance.Get(StoneId);

        int fee = config.GetFee(level + 1);

        long materialCount = user.GetMaterialCount(config.ItemId);
        string color = materialCount >= fee ? "#FFFF00" : "#FF0000";
        Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, config.Name + ":" + materialCount + "/ " + fee);

        if (materialCount >= fee)
        {
            this.Btn_OK.gameObject.SetActive(true);
            this.Btn_OK_Batch.gameObject.SetActive(true);
        }

        int currentAttr = config.GetAttr(level);

        int nextAtt = config.GetAttr(level + 1);

        AttrItem.SetContent(config.AttrId, currentAttr, nextAtt - currentAttr);
    }

    public void OnClickOKBatch()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_OK_Batch.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        StoneRecord record = user.GetStoneRecord(SelectPosition);

        for (int i = 0; i < BatchCount; i++)
        {
            int level = record.GetStoneLevel(MainIndex);

            StoneConfig config = StoneConfigCategory.Instance.Get(StoneId);

            int fee = config.GetFee(level + 1);

            long materialCount = user.GetMaterialCount(config.ItemId);

            if (materialCount < fee)
            {
                break;
            }

            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = config.ItemId,
                Quantity = fee
            });

            record.AddLevel(MainIndex, StoneId);
        }

        GameProcessor.Inst.UpdateInfo();

        ShowForgeItem();
        ShowStoneMain();
        ShowStone();
    }

    public void OnClickOK()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_OK_Batch.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int level = record.GetStoneLevel(MainIndex);

        StoneConfig config = StoneConfigCategory.Instance.Get(StoneId);

        int fee = config.GetFee(level + 1);

        long materialCount = user.GetMaterialCount(config.ItemId);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = config.ItemId,
            Quantity = fee
        });

        record.AddLevel(MainIndex, StoneId);

        GameProcessor.Inst.UpdateInfo();

        ShowForgeItem();
        ShowStoneMain();
        ShowStone();
    }

    public void OnClickActive()
    {
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int setCount = record.GetSetCount();

        long fee = setCount + 1;
        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Stone_Set);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Stone_Set,
            Quantity = fee
        });

        record.AddCount();

        ShowForgeItem();
        ShowStoneMain();
        ShowStone();
    }

    public void OnRestore()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重置消耗100京金币。是否确认？", true,
        () =>
        {
            Restore();
        }, () =>
        {

        });
    }

    private void Restore()
    {
        this.Btn_Restore.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        if (user.MagicGold.Data <= ConfigHelper.RestoreGold * 200)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足100京", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(ConfigHelper.RestoreGold * 200);

        StoneRecord record = user.GetStoneRecord(SelectPosition);

        List<Item> newList = new List<Item>();

        foreach (var sp in record.List)
        {
            StoneSet set = sp.Value;

            StoneConfig config = StoneConfigCategory.Instance.Get(set.StoneId);

            int totalFee = config.GetTotalFee(set.StoneLevel.Data);

            Item item = ItemHelper.BuildMaterial(config.ItemId, totalFee);
            newList.Add(item);

        }

        record.List.Clear();
        user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

        ShowForgeItem();
    }
}

