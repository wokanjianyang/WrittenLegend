using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Relic : MonoBehaviour
{
    public Text txt_Fee;
    public Button Btn_Active;

    public Transform Tf_Ring;
    public Transform Tf_Attr;

    private List<Item_Relic> ItemList;
    private List<StrenthAttrItem> AttrList;

    private int Rid = 0;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Active.onClick.AddListener(OnStrong);

        ItemList = Tf_Ring.GetComponentsInChildren<Item_Relic>().ToList();

        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();

        foreach (Item_Relic item in ItemList)
        {
            item.AddListener(SelectItem);
        }
    }

    public void Show(int id)
    {
        this.Rid = id;

        this.Init();
    }

    private void Init()
    {
        Btn_Active.gameObject.SetActive(false);

        List<RelicConfig> list = RelicConfigCategory.Instance.GetListByType(Rid);

        for (int i = 0; i < list.Count; i++)
        {
            ItemList[i].SetContent(list[i]);
        }
    }

    private void SelectItem(int i)
    {
        Debug.Log("select item:" + i);
    }


    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;


    }
}
