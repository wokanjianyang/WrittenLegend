using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Grade_Specail : MonoBehaviour
{
    public ScrollRect sr_Panel;

    private List<ItemGrade> items = new List<ItemGrade>();

    public Item_Metail_Need metailFee;

    public Button Btn_OK;

    private const int MaxCount = 4; //4件装备
    //private const int Quality = 6;

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

        IDictionary<int, Equip> dict = user.EquipPanelSpecial;

        for (int BoxId = 0; BoxId < 4; BoxId++)
        {
            int postion = 11 + BoxId;

            var bagBox = this.sr_Panel.content.GetChild(BoxId);
            if (bagBox == null || !dict.ContainsKey(postion))
            {
                continue;
            }

            Equip equip = dict[postion];

            ItemGrade box = this.CreateItem(equip, bagBox);
            this.items.Add(box);
        }

        //metailFee.gameObject.SetActive(false);

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
        if (e.Equip.EquipConfig.Cycle != 0)
        {
            return;
        }

        this.SelectEquip = e.Equip;
        this.Show();
    }

    private void Show()
    {
        Btn_OK.gameObject.SetActive(true);

        int layer = SelectEquip.Layer;
        int itemId = SelectEquip.ConfigId;

        EquipSpeicalConfig config = EquipSpeicalConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ItemId == itemId && m.Layer == layer).FirstOrDefault();

        if (config == null)
        {
            metailFee.SetContent("已经满级");
        }
        else
        {
            metailFee.SetContent(config.FeeItemId, config.Fee);
        }
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
    }

    private bool Grade(Equip equip)
    {
        int layer = equip.Layer;
        int itemId = equip.ConfigId;

        EquipSpeicalConfig config = EquipSpeicalConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ItemId == itemId && m.Layer == layer).FirstOrDefault();

        if (config == null)
        {
            return false;
        }

        User user = GameProcessor.Inst.User;

        int specialId = config.FeeItemId;
        long upCount = config.Fee;

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == specialId).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal < upCount)
        {
            return false;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = specialId,
            Quantity = upCount
        });

        equip.Grade();

        return true;
    }
}

