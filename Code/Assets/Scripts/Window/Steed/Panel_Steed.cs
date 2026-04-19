using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Steed : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;

    private GameObject prefab;
    private List<Item_Steed> SteedItems = new List<Item_Steed>();

    public Dialog_Steed_Forge DialogForge;
    public Dialog_Steed_Travel DialogTravel;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        prefab = Resources.Load<GameObject>("Prefab/Window/Steed/Item_Steed");
        this.Init();
    }

    private void OnEnable()
    {
        this.Show();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<SteedBattleDownEvent>(this.BattleDown);
        GameProcessor.Inst.EventCenter.AddListener<SteedOpenForgeEvent>(this.OpenForge);
    }

    private void BattleDown(SteedBattleDownEvent e)
    {
        User user = GameProcessor.Inst.User;

        //判断空格
        int ic = GameProcessor.Inst.User.GetBagIdleCount(3);
        if (ic < 10)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
            return;
        }

        Item_Steed item = e.Item;

        Steed steed = item.steed;

        SteedItems.Remove(item);
        GameObject.Destroy(item.gameObject);

        user.SteedDict.Remove(steed.Role);

        List<Item> items = new List<Item>();
        items.Add(steed);
        if (items.Count > 0)
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        user.EventCenter.Raise(new HeroUnUseEquipEvent() { });
    }

    private void OpenForge(SteedOpenForgeEvent e)
    {
        if (e.Type == 1)
        {
            DialogForge.Open(e.Item.steed);
        }
        else if (e.Type == 2)
        {
            DialogTravel.Open(e.Item.steed);
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

        foreach (var cb in SteedItems)
        {
            GameObject.Destroy(cb.gameObject);
        }
        SteedItems.Clear();

        List<Steed> steeds = user.SteedDict.Select(m => m.Value).ToList(); ;

        for (int i = 0; i < steeds.Count; i++)
        {
            Item_Steed item = this.CreateItem(steeds[i], i);
            this.SteedItems.Add(item);
        }
    }

    private Item_Steed CreateItem(Steed steed, int position)
    {
        var go = GameObject.Instantiate(prefab);
        Item_Steed comItem = go.GetComponent<Item_Steed>();
        comItem.Init(steed);

        comItem.transform.SetParent(this.sr_Boss.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void Init()
    {

    }
}
