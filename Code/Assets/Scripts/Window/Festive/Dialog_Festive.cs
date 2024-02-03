using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Festive : MonoBehaviour
{
    public Text Txt_Total;

    public ScrollRect sr_Panel;
    private GameObject ItemPrefab;

    public Button Btn_Close;

    // Start is called before the first frame update
    void Start()
    {
        this.Init();
        this.Btn_Close.onClick.AddListener(OnClose);

        GameProcessor.Inst.EventCenter.AddListener<FestiveUIFreshEvent>(this.OnFestiveUIFresh);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Festive");

        List<FestiveConfig> list = FestiveConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (var config in list)
        {
            var Item = GameObject.Instantiate(ItemPrefab);
            Item.transform.SetParent(sr_Panel.content);
            Item.gameObject.SetActive(true);

            Item_Festive com = Item.GetComponent<Item_Festive>();
            com.SetData(config);
        }
    }

    private void OnFestiveUIFresh(FestiveUIFreshEvent e)
    {
        long count = GameProcessor.Inst.User.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();
        this.Txt_Total.text = count + " ¸ö";
    }

    public void Open()
    {
        long count = GameProcessor.Inst.User.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();
        this.Txt_Total.text = count + " ¸ö";

        this.gameObject.SetActive(true);
    }

    private void OnClose()
    {
        this.gameObject.SetActive(false);
    }
}

