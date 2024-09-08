using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Defend : MonoBehaviour, IBattleLife
{
    public List<Item_Defend> ItemList;

    public Button btn_FullScreen;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        User user = GameProcessor.Inst.User;
        user.DefendData.BuildCurrent();

        long progess = user.GetAchievementProgeress(AchievementSourceType.Defend);

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetContent(i);
        }
        btn_FullScreen.onClick.AddListener(this.OnClick_Close);
    }

    void Update()
    {

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDefendEvent>(this.OnOpenDefend);
    }


    private void OnOpenDefend(OpenDefendEvent e)
    {
        this.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
