using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Grade : MonoBehaviour
{
    public ScrollRect sr_Panel;

    private List<ItemGrade> items = new List<ItemGrade>();

    public List<Item_Metail_Need> metailList;

    public Button Btn_OK;
    public Button Btn_Batch;
    public Button Btn_Batch_Restore;

    private const int MaxCount = 10; //10件装备
    private const int Quality = 6;

    Equip SelectEquip;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
        this.Btn_Batch.onClick.AddListener(OnClickBatch);
        this.Btn_Batch_Restore.onClick.AddListener(OnClickBatchRestore);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<GradeSelectEvent>(this.OnSelect);
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

    public void Load()
    {
        //把之前的卸载
        this.SelectEquip = null;

        foreach (ItemGrade cb in items)
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

            ItemGrade box = this.CreateItem(equip, bagBox);
            this.items.Add(box);
        }


        foreach (var item in metailList)
        {
            item.gameObject.SetActive(false);
        }

        this.Btn_OK.gameObject.SetActive(false);
    }

    private ItemGrade CreateItem(Equip equip, Transform parent)
    {
        ToggleGroup toggleGroup = sr_Panel.GetComponent<ToggleGroup>();

        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Grade");

        var go = GameObject.Instantiate(prefab);
        ItemGrade comItem = go.GetComponent<ItemGrade>();
        comItem.Init(equip, toggleGroup);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }


    private void OnSelect(GradeSelectEvent e)
    {
        this.SelectEquip = e.Equip;
        this.Show();
    }

    private void Show()
    {
        Btn_OK.gameObject.SetActive(true);

        int part = SelectEquip.Part;
        int layer = SelectEquip.Layer;

        EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == part && m.Layer == layer && m.Quanlity == Quality).FirstOrDefault();

        if (config == null)
        {
            return;
        }

        metailList[0].gameObject.SetActive(true);
        metailList[0].SetContent(config.MetailId, config.MetailCount);

        metailList[1].gameObject.SetActive(true);
        metailList[1].SetContent(config.MetailId1, config.MetailCount1);
    }

    public void OnClickOK()
    {
        bool grade = Grade(SelectEquip);

        if (!grade)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的升阶材料不足", ToastType = ToastTypeEnum.Failure });
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "升阶成功", ToastType = ToastTypeEnum.Success });
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
            this.Load();
        }

        //int part = SelectEquip.Part;
        //int layer = SelectEquip.Layer;
        //EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == part && m.Layer == layer && m.Quanlity == Quality).FirstOrDefault();

        //if (config == null)
        //{
        //    return;
        //}

        //User user = GameProcessor.Inst.User;

        //int[] idList = { config.MetailId, config.MetailId1 };
        //int[] countList = { config.MetailCount, config.MetailCount1 };

        //for (int i = 0; i < idList.Length; i++)
        //{
        //    int specialId = idList[i];
        //    int upCount = countList[i];

        //    long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();
        //    if (stoneTotal < upCount)
        //    {
        //        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的升阶材料不足", ToastType = ToastTypeEnum.Failure });
        //        return;
        //    }
        //}

        //for (int i = 0; i < idList.Length; i++)
        //{
        //    int specialId = idList[i];
        //    int upCount = countList[i];

        //    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        //    {
        //        Type = ItemType.Material,
        //        ItemId = specialId,
        //        Quantity = upCount
        //    });
        //}

        //this.SelectEquip.Grade();

        //GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

        //this.Load();

        //GameProcessor.Inst.SaveData();
    }

    private bool Grade(Equip equip)
    {
        int part = equip.Part;
        int layer = equip.Layer;
        EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == part && m.Layer == layer && m.Quanlity == Quality).FirstOrDefault();

        if (config == null)
        {
            return false;
        }

        User user = GameProcessor.Inst.User;

        int[] idList = { config.MetailId, config.MetailId1 };
        int[] countList = { config.MetailCount, config.MetailCount1 };

        for (int i = 0; i < idList.Length; i++)
        {
            int specialId = idList[i];
            int upCount = countList[i];

            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();
            if (stoneTotal < upCount)
            {
                return false;
            }
        }

        for (int i = 0; i < idList.Length; i++)
        {
            int specialId = idList[i];
            int upCount = countList[i];

            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = specialId,
                Quantity = upCount
            });
        }

        equip.Grade();

        return true;
    }

    public void OnClickBatch()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("一键进阶消耗10京金币。是否确认？", true,
        () =>
        {
            BatchGrade();
        }, () =>
        {

        });
    }

    private void BatchGrade()
    {
        User user = GameProcessor.Inst.User;

        if (user.MagicGold.Data <= ConfigHelper.RestoreGold * 20)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足10京", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(ConfigHelper.RestoreGold * 20);

        IDictionary<int, Equip> dict = user.EquipPanelList[user.EquipPanelIndex];

        foreach (Equip equip in dict.Values)
        {
            for (int i = 0; i <= 14; i++)
            {
                Grade(equip);
            }
        }

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        this.Load();
    }


    public void OnClickBatchRestore()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("一键重生消耗10京金币。是否确认？", true,
        () =>
        {
            BatchRestore();
        }, () =>
        {

        });
    }

    private void BatchRestore()
    {
        User user = GameProcessor.Inst.User;

        if (user.MagicGold.Data <= ConfigHelper.RestoreGold * 20)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足10京", ToastType = ToastTypeEnum.Failure });
            return;
        }

        IDictionary<int, Equip> dict = user.EquipPanelList[user.EquipPanelIndex];

        Dictionary<int, int> mlist = new Dictionary<int, int>();

        foreach (Equip equip in dict.Values)
        {
            equip.GetRestoreItems(mlist);
        }

        int haveCount = user.GetBagIdleCount(4);
        if (haveCount < mlist.Count)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留" + mlist.Count + "个包裹空额", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(ConfigHelper.RestoreGold * 20);

        foreach (Equip equip in dict.Values)
        {
            equip.Layer = 1;
            equip.HoneList = new Dictionary<int, int>();
        }

        List<Item> newList = new List<Item>();
        foreach (var kp in mlist)
        {
            Item item = ItemHelper.BuildMaterial(kp.Key, kp.Value);
            newList.Add(item);
        }

        user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        this.Load();
    }
}

