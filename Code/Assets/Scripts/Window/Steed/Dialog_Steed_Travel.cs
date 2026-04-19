using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Steed_Travel : MonoBehaviour
{
    public Text Txt_Content;
    public Text Txt_Time;

    public Button Btn_Close;
    public Button Btn_Ok;
    public Button Btn_Stop;

    private Steed SelectSteed;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnOk);
        Btn_Stop.onClick.AddListener(OnStop);
    }

    private void OnEnable()
    {
        if (SelectSteed != null)
        {
            this.Show();
        }
    }

    public void Open(Steed steed)
    {
        this.gameObject.SetActive(true);
        this.SelectSteed = steed;

        this.Show();
    }

    public void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user.SpiritOfflineLog.Count != 3)
        {
            Btn_Stop.gameObject.SetActive(false);
            Btn_Ok.gameObject.SetActive(false);

            Txt_Content.text = "还没有记录通关副本和时间，请先通关副本";

            return;
        }

        int mapId = user.SpiritOfflineLog[1];
        int time = user.SpiritOfflineLog[2];
        int total = user.SpiritOfflineLog[3];

        SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(mapId);

        Txt_Content.text = string.Format("记录离线副本为：{0}，\n通关时间为{1}秒，通关积分为{2}", config.MapName, time, total);


        if (SelectSteed.RunTime > 0)
        {
            long runTime = TimeHelper.ClientNowSeconds() - SelectSteed.RunTime;
            runTime = Math.Min(runTime, 86400);

            int minTime = time * 3 + 30;
            long count = runTime / minTime;

            Txt_Time.text = "已经打工 " + runTime + " 秒\n（" + minTime + " 秒获取一次奖励，当前可获取 " + count + " 次奖励）";
            Btn_Stop.gameObject.SetActive(true);
            Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            Txt_Time.text = "还没有开始打工";
            Btn_Stop.gameObject.SetActive(false);
            Btn_Ok.gameObject.SetActive(true);
        }
    }



    public void OnOk()
    {
        Btn_Ok.gameObject.SetActive(false);

        SelectSteed.RunTime = TimeHelper.ClientNowSeconds();

        User user = GameProcessor.Inst.User;
        if (user.SpiritOfflineLog == null || user.SpiritOfflineLog.Count != 3)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先通关英灵副本", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SpiritOfflineFlag = true;

        int mapId = user.SpiritOfflineLog[1];

        SelectSteed.RunMapId = mapId;
        SelectSteed.RunTime = TimeHelper.ClientNowSeconds();


        this.Show();
    }

    public void OnStop()
    {
        Btn_Stop.gameObject.SetActive(false);
        Btn_Ok.gameObject.SetActive(false);

        if (this.SelectSteed.RunTime <= 0)
        {
            return;
        }

        //获取奖励
        List<Item> items = new List<Item>();

        User user = GameProcessor.Inst.User;

        //int mapId = user.SpiritOfflineLog[1];
        int time = user.SpiritOfflineLog[2];
        //int total = user.SpiritOfflineLog[3];

        int minTime = time * 3 + 30;
        long runTime = TimeHelper.ClientNowSeconds() - SelectSteed.RunTime;
        runTime = Math.Min(runTime, 86400);

        long count = runTime / minTime;

        this.SelectSteed.RunMapId = 0;
        this.SelectSteed.RunTime = 0;

        if (count >= 1)
        {
            items.AddRange(BuildOfflineSpirit(user, count));

            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

            GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = "坐骑打工奖励", Items = items });
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "还没有任何奖励", ToastType = ToastTypeEnum.Failure });
        }

        this.Show();
    }

    private List<Item> BuildOfflineSpirit(User user, long count)
    {
        MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

        List<Item> itemList = new List<Item>();

        int mapId = user.SpiritOfflineLog[1];
        //int time = user.SpiritOfflineLog[2] + 30;
        int total = user.SpiritOfflineLog[3];

        SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(mapId);


        List<SpiritDropConfig> dropList = SpiritDropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.MapId == mapId).ToList();

        //Debug.Log("spirit drop list :" + dropList.Count);

        IDictionary<int, int> dropDict = new Dictionary<int, int>();

        int rate = 1 + Math.Min(4, total / 500);

        foreach (SpiritDropConfig sdpConfig in dropList)
        {
            long dropCount = sdpConfig.DropRate * rate * count / 100;

            DropConfig dropConfig = DropConfigCategory.Instance.Get(sdpConfig.DropId);

            for (int i = 0; i < dropCount; i++)
            {
                int index = i % dropConfig.ItemIdList.Length;

                int dropId = dropConfig.ItemIdList[index];

                if (!dropDict.ContainsKey(dropId))
                {
                    dropDict[dropId] = 0;
                }

                dropDict[dropId]++;
            }
        }

        dropDict.OrderBy(m => m.Key);

        foreach (var sp in dropDict)
        {
            itemList.Add(ItemHelper.BuildItem(ItemType.Spirit, sp.Key, 0, sp.Value));
        }

        return itemList;
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
