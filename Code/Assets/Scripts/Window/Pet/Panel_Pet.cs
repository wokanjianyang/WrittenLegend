using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pet : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;

    private GameObject prefab;
    private List<Item_Pet> PetItems = new List<Item_Pet>();

    public Dialog_Pet_Forge DialogPetForge;
    public Dialog_Pet_Travel DialogPetTravel;
    public Dialog_Pet_Devour DialogPetDevour;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        prefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Pet");
        this.Init();
    }

    private void OnEnable()
    {
        this.Show();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<PetBattleDownEvent>(this.PetBattleDown);
        GameProcessor.Inst.EventCenter.AddListener<OpenPetForgeEvent>(this.OpenPetForge);
    }

    private void PetBattleDown(PetBattleDownEvent e)
    {
        User user = GameProcessor.Inst.User;

        //判断空格
        int ic = GameProcessor.Inst.User.GetBagIdleCount(3);
        if (ic < 10)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
            return;
        }

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

        user.EventCenter.Raise(new HeroUnUseEquipEvent() { });
    }

    private void OpenPetForge(OpenPetForgeEvent e)
    {
        if (e.Type == 1)
        {
            DialogPetForge.Open(e.Item.pet);
        }
        else if (e.Type == 2)
        {
            DialogPetTravel.Open(e.Item.pet);
        }
        else if (e.Type == 3) {
            DialogPetDevour.Open(e.Item.pet);
        }
    }



    // Start is called before the first frame update
    public void Show()
    {
        //Debug.Log("pet onable");

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

    private void Init()
    {

    }
}
