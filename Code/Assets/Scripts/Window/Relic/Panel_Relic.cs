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
    void Awake()
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

        Debug.Log("show panel :" + id);
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

    private void SelectItem(int id)
    {
        //Debug.Log("select item:" + i);

        RelicConfig config = RelicConfigCategory.Instance.Get(id);

        for (int i = 0; i < AttrList.Count; i++)
        {
            if (i >= config.AttrId.Length)
            {
                AttrList[i].gameObject.SetActive(false);
            }
            else
            {
                AttrList[i].gameObject.SetActive(true);

                int attrValue = config.AttrValue[i];
                AttrList[i].SetContent(config.AttrId[i], attrValue, config.AttrRise[i]);
            }
        }

    }


    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;


    }
}
