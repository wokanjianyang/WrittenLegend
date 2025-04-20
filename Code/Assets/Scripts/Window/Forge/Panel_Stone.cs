using Game;
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

    private List<Stone_Item> stoneUsedList = new List<Stone_Item>();

    public List<Item_Metail_Need> metailList;

    public Button Btn_OK;
    public Button Btn_Active;

    private const int MaxCount = 10; //10件装备
    private const int Quality = 7;

    private const int StartPosition = 21;
    private const int MaxLevel = 14;

    private int SelectPosition = 0;
    private int SelectIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        this.items = sr_Panel.content.GetComponentsInChildren<Item_Forge_Item>().ToList();
        this.mainList = tg_Main.GetComponentsInChildren<Stone_Item_Main>().ToList();
        this.stoneList = tg_Stone.GetComponentsInChildren<Stone_Item>().ToList();

        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
        this.Btn_Active.onClick.AddListener(OnClickActive);
    }

    // Update is called once per frame
    void Start()
    {
        this.SelectForgeItem(1);
        this.SelectStoneMain(1);
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

        User user = GameProcessor.Inst.User;
        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int setCount = record.GetSetCount();

        for (int i = 0; i < mainList.Count; i++)
        {
            Stone_Item_Main main = mainList[i];

            if (i <= setCount)
            {
                int stoneId = record.GetStoneId(i);
                int level = record.GetStoneLevel(stoneId);

                //可以镶嵌
                main.toggle.interactable = true;
                main.SetContent(i, stoneId, level);
            }
            else
            {
                main.toggle.interactable = false;
            }
        }

        for (int i = 0; i < stoneList.Count; i++)
        {
            stoneList[i].toggle.interactable = false;
        }

        SelectStoneMain(1);
    }

    private void SelectStoneMain(int index)
    {
        Debug.Log("SelectStoneMain id:" + index);

        this.SelectIndex = index;

        stoneUsedList.Clear();
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_Active.gameObject.SetActive(true);

        User user = GameProcessor.Inst.User;
        StoneRecord record = user.GetStoneRecord(SelectPosition);

        int stoneId = record.GetStoneId(SelectIndex);
        int level = record.GetStoneLevel(stoneId);

        if (stoneId == 0)
        {
            StoneSetConfig setConfig = StoneSetConfigCategory.Instance.Get(SelectPosition);

            for (int i = 0; i < stoneList.Count; i++)
            {
                StoneConfig stoneConfig = StoneConfigCategory.Instance.Get(i + 1);

                if (setConfig.TypeList.Contains(stoneConfig.Type))
                {
                    stoneList[i].toggle.interactable = true;
                    //stoneList[i].gameObject.SetActive(true);

                    stoneUsedList.Add(stoneList[i]);
                }
                else
                {
                    stoneList[i].toggle.interactable = false;
                    //stoneList[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            StoneConfig stoneConfig = StoneConfigCategory.Instance.Get(stoneId);

            for (int i = 0; i < stoneList.Count; i++)
            {
                stoneUsedList.Add(stoneList[i]);
            }
        }


        stoneUsedList[0].toggle.isOn = true;

    }

    private void SelectStone(int itemId)
    {
        Debug.Log("SelectStoneMain id:" + itemId);
    }


    public void Load()
    {
        //把之前的卸载


        this.Btn_OK.gameObject.SetActive(false);
    }

    public void OnClickOK()
    {
    }


    public void OnClickActive()
    {

    }
}

