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

    private const int MaxCount = 10; //10件装备

    Equip SelectEquip;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
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

            if (equip.GetQuality() < 6 || equip.Layer >= 7)
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
        if (SelectEquip.Layer >= 7)
        {
            this.Btn_OK.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_OK.gameObject.SetActive(true);
        }

        //
        int part = SelectEquip.Part;
        int layer = SelectEquip.Layer;

        EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == part && m.Layer == layer).FirstOrDefault();

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
        int part = SelectEquip.Part;
        int layer = SelectEquip.Layer;
        EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == part && m.Layer == layer).FirstOrDefault();

        if (config == null)
        {
            return;
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
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您的升阶材料不足", ToastType = ToastTypeEnum.Failure });
                return;
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

        this.SelectEquip.Grade();
        this.Load();
    }

}

