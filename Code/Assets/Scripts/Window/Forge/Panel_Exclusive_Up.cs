using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Exclusive_Up : MonoBehaviour
{
    public ScrollRect ds_Panel;

    public ScrollRect sr_Panel;

    private List<Box_Select> items = new List<Box_Select>();

    private List<Box_Select> sourceList = new List<Box_Select>();

    public List<Text> runeList;
    public List<Text> suitList;
    public List<Text> levelList;

    public Box_Ready Box_Ready_Main;
    public Box_Ready Box_Ready_Material;

    public Button Btn_OK;

    private const int MaxCount = 6; //10件装备

    ExclusiveItem SelectExclusive;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<BoxSelectEvent>(this.OnBoxSelect);
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
            var empty = GameObject.Instantiate(emptyPrefab, this.ds_Panel.content);
            empty.name = "Des_" + i;
        }


        for (var i = 0; i < ConfigHelper.MaxBagCount; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Panel.content);
            empty.name = "Src_" + i;
        }

        Box_Ready_Main.Init("主专属");
        Box_Ready_Material.Init("材料专属");
    }

    private void Load()
    {
        //把之前的卸载
        this.SelectExclusive = null;

        foreach (Box_Select cb in items)
        {
            GameObject.Destroy(cb.gameObject);
        }
        items.Clear();

        foreach (Box_Select sb in sourceList)
        {
            GameObject.Destroy(sb.gameObject);
        }
        sourceList.Clear();

        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        IDictionary<int, ExclusiveItem> dict = user.ExclusivePanelList[user.ExclusiveIndex];

        for (int BoxId = 0; BoxId < MaxCount; BoxId++)
        {
            int postion = BoxId + 15;

            var bagBox = this.ds_Panel.content.GetChild(BoxId);
            if (bagBox == null || !dict.ContainsKey(postion))
            {
                continue;
            }

            ExclusiveItem exclusive = dict[postion];

            if (exclusive.GetQuality() < 5)
            {
                continue;
            }

            BoxItem boxItem = new BoxItem();
            boxItem.Item = exclusive;
            boxItem.MagicNubmer.Data = 1;

            Box_Select box = PrefabHelper.Instance().CreateBoxSelect(bagBox, boxItem, ComBoxType.Exclusive_Up_Main);
            this.items.Add(box);
        }


        this.Btn_OK.gameObject.SetActive(false);
    }

    private void OnBoxSelect(BoxSelectEvent e)
    {
        if (e.Type == ComBoxType.Exclusive_Up_Main)
        {
            this.SelectExclusive = e.BoxItem.Item as ExclusiveItem;
            Box_Ready_Main.Up(e.BoxItem);

            this.Show();
        }
        else if (e.Type == ComBoxType.Exclusive_Devour_Material)
        {
            Box_Ready_Material.Up(e.BoxItem);
        }
    }

    private void Show()
    {
        this.Btn_OK.gameObject.SetActive(false);

        List<SkillRuneConfig> rcList = new List<SkillRuneConfig>();
        rcList.Add(SkillRuneConfigCategory.Instance.Get(SelectExclusive.RuneConfigId));

        for (int i = 0; i < SelectExclusive.RuneConfigIdList.Count; i++)
        {
            rcList.Add(SkillRuneConfigCategory.Instance.Get(SelectExclusive.RuneConfigIdList[i]));
        }

        for (int i = 0; i < runeList.Count; i++)
        {
            if (i < rcList.Count)
            {
                runeList[i].gameObject.SetActive(true);
                runeList[i].text = rcList[i].Name;
            }
            else
            {
                runeList[i].gameObject.SetActive(false);
            }
        }


        List<SkillSuitConfig> ssList = new List<SkillSuitConfig>();
        ssList.Add(SkillSuitConfigCategory.Instance.Get(SelectExclusive.SuitConfigId));
        for (int i = 0; i < SelectExclusive.RuneConfigIdList.Count; i++)
        {
            ssList.Add(SkillSuitConfigCategory.Instance.Get(SelectExclusive.SuitConfigIdList[i]));
        }

        for (int i = 0; i < suitList.Count; i++)
        {
            if (i < ssList.Count)
            {
                suitList[i].gameObject.SetActive(true);
                suitList[i].text = ssList[i].Name;
            }
            else
            {
                suitList[i].gameObject.SetActive(false);
            }
        }

        List<int> lvs = SelectExclusive.LevelDict.Select(m => m.Key).ToList();
        for (int i = 0; i < levelList.Count; i++)
        {
            if (i < lvs.Count)
            {
                int runeId = lvs[i];
                SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);
                levelList[i].text = config.Name + " * " + SelectExclusive.LevelDict[runeId];
            }
            else
            {
                levelList[i].text = "未升级";
            }

        }

        //选择符合条件的exclusive
        User user = GameProcessor.Inst.User;

        List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() == 5 && !m.Item.IsLock).ToList();
        //Debug.Log("es:" + list.Count);
        int BoxId = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var bagBox = this.sr_Panel.content.GetChild(BoxId);
            if (bagBox == null)
            {
                return;
            }

            BoxItem item = list[i];
            ExclusiveItem exclusive = item.Item as ExclusiveItem;
            if (lvs.Count > 0 && !lvs.Contains(exclusive.RuneConfigId))
            {
                continue;
            }

            Box_Select box = PrefabHelper.Instance().CreateBoxSelect(bagBox, item, ComBoxType.Exclusive_Up_Material);
            this.sourceList.Add(box);

            BoxId++;
        }
    }

    public void OnClickOK()
    {
        //this.SelectExclusive.Refesh(true);

        this.Show();
    }
}

