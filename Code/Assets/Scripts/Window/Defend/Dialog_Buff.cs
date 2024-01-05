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

    private int Progress = 0;
    private int SelectIndex = 0;
    List<DefendBuffConfig> selectList = new List<DefendBuffConfig>();

    public int Order => (int)ComponentOrder.Dialog;

    void Awake()
    {
        this.btn_Ok.onClick.AddListener(OnClick_OK);

        for (int i = 0; i < ItemList.Count; i++)
        {
            int si = i;
            ItemList[i].onValueChanged.AddListener((on) =>
            {
                if (on)
                {
                    SelectItem(si);
                }
            });
        }

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<DefendBuffSelectEvent>(this.OnShow);
    }

    private void SelectItem(int index)
    {
        this.SelectIndex = index;
    }

    private void OnShow(DefendBuffSelectEvent e)
    {
        //Debug.Log("DefendBuffSelectEvent");

        if (this.Progress < e.Index)
        {
            User user = GameProcessor.Inst.User;
            //auto select pre
            DefendRecord record = user.DefendData.GetCurrentRecord();
            if (!record.BuffDict.ContainsKey(this.Progress))
            {
                record.BuffDict[this.Progress] = selectList[SelectIndex].Id;
            }

            this.Progress = e.Index;
            this.Txt_Progress.text = "ÃüÔË" + this.Progress + "²ã";

            List<int> excludeList = user.DefendData.GetExcludeList();

            List<DefendBuffConfig> list = DefendBuffConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => !excludeList.Contains(m.Id)).ToList();

            selectList.Clear();
            for (int i = 0; i < ItemList.Count; i++)
            {
                int k = RandomHelper.RandomNumber(0, list.Count);
                selectList.Add(list[k]);
                list.RemoveAt(k);
            }

            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].SetContent(selectList[i]);
            }
        }

        this.gameObject.SetActive(true);
    }

    public void OnClick_OK()
    {
        User user = GameProcessor.Inst.User;

        DefendRecord record = user.DefendData.GetCurrentRecord();

        record.BuffDict[this.Progress] = selectList[SelectIndex].Id;

        if (selectList[SelectIndex].Type == 1)
        {
            GameProcessor.Inst.UpdateInfo();
        }
        else
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }

        this.gameObject.SetActive(false);
    }
}
