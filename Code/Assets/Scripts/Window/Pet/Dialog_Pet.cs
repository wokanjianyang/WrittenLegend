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
    private List<Item_Pet> PetItems = new List<Item_Pet>();

    public Dialog_Pet_Forge DialogPetForge;

    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        prefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Pet");
        this.Init();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<PetShowEvent>(this.OnShow);
        GameProcessor.Inst.EventCenter.AddListener<PetBattleDownEvent>(this.PetBattleDown);
        GameProcessor.Inst.EventCenter.AddListener<PetForgeEvent>(this.PetForge);
    }

    private void PetBattleDown(PetBattleDownEvent e)
    {
        User user = GameProcessor.Inst.User;

        Item_Pet item = e.Item;

        Pet pet = item.pet;

        PetItems.Remove(item);
        GameObject.Destroy(item.gameObject);

        user.PetList.Remove(pet);

        List<Item> items = new List<Item>();
        items.Add(pet);
        if (items.Count > 0)
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }
    }

    private void PetForge(PetForgeEvent e)
    {
        DialogPetForge.Open(e.Item.pet);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        foreach (var cb in PetItems)
        {
            GameObject.Destroy(cb.gameObject);
        }
        PetItems.Clear();

        List<Pet> pets = user.PetList;

        for (int i = 0; i < pets.Count; i++)
        {
            Item_Pet item = this.CreateItem(pets[i], i);
            this.PetItems.Add(item);
        }
    }

    private Item_Pet CreateItem(Pet pet, int position)
    {
        var go = GameObject.Instantiate(prefab);
        Item_Pet comItem = go.GetComponent<Item_Pet>();
        comItem.Init(pet);

        comItem.transform.SetParent(this.sr_Boss.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
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
