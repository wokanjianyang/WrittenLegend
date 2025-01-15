using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pet : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;

    private GameObject prefab;
    private List<Item_Pet> items = new List<Item_Pet>();


    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        prefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Pet");
        this.Init();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        foreach (var cb in items)
        {
            GameObject.Destroy(cb.gameObject);
        }
        items.Clear();

        List<Pet> pets = user.PetList;

        for (int i = 0; i < pets.Count; i++)
        {
            Item_Pet item = this.CreateItem(pets[i]);
            this.items.Add(item);
        }
    }

    private Item_Pet CreateItem(Pet pet)
    {
        var go = GameObject.Instantiate(prefab);
        Item_Pet comItem = go.GetComponent<Item_Pet>();
        comItem.Init(pet);

        comItem.transform.SetParent(this.sr_Boss.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }


    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<PetShowEvent>(this.OnShow);
    }

    public void OnShow(PetShowEvent e)
    {
        this.gameObject.SetActive(true);
    }

    private void Init()
    {

    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
