using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Buff : MonoBehaviour, IBattleLife
{
    public List<Item_Buff> ItemList;
    public Text Txt_Progress;
    public Button btn_Ok;

    private int Level = 0;
    private int Progress = 0;
    //List<DefendBuffConfig> selectList = new List<DefendBuffConfig>();

    public int Order => (int)ComponentOrder.Dialog;

    void Awake()
    {
        this.btn_Ok.onClick.AddListener(OnClick_OK);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<DefendBuffSelectEvent>(this.OnShow);
    }

    private void OnShow(DefendBuffSelectEvent e)
    {
        //Debug.Log("DefendBuffSelectEvent");

        if (e.Level != this.Level)
        {
            this.Level = e.Level;
            this.Progress = 0;
        }

        if (this.Progress != e.Index)
        {
            User user = GameProcessor.Inst.User;
            //auto select pre
            DefendRecord record = user.DefendData.GetCurrentRecord();
            if (this.Progress > 0 && !record.BuffDict.ContainsKey(this.Progress))
            {
                SelectBuff();
            }

            this.Progress = e.Index;
            this.Txt_Progress.text = "命运" + this.Progress + "层";

            List<int> excludeList = user.DefendData.GetExcludeList();

            List<DefendBuffConfig> list = DefendBuffConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => !excludeList.Contains(m.Id)).ToList();

            for (int i = 0; i < ItemList.Count; i++)
            {
                int k = RandomHelper.RandomNumber(0, list.Count);
                ItemList[i].SetContent(list[k]);
                list.RemoveAt(k);
            }
        }

        this.gameObject.SetActive(true);
    }

    public void OnClick_OK()
    {
        SelectBuff();
        this.gameObject.SetActive(false);
    }

    private void SelectBuff()
    {
        User user = GameProcessor.Inst.User;

        DefendRecord record = user.DefendData.GetCurrentRecord();

        Item_Buff buff = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();

        DefendBuffConfig config = buff.Config;

        record.BuffDict[this.Progress] = config.Id;
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "您选择了 " + config.Name });

        if (config.Type == 1)
        {
            GameProcessor.Inst.PlayerManager.GetHero().EventCenter.Raise(new HeroBuffChangeEvent());
        }
        else
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }

        Log.Debug("您选择了 " + config.Name);
    }
}
