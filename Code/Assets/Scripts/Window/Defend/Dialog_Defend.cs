using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Defend : MonoBehaviour, IBattleLife
{
    public Transform Tf_Parent;
    public Button btn_FullScreen;

    private List<Item_Defend> ItemList;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        ItemList = Tf_Parent.GetComponentsInChildren<Item_Defend>().ToList();

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
