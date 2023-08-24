using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_SoulRing : MonoBehaviour, IBattleLife
{
    public Text Fee;

    public Toggle Ring1;
    public Toggle Ring2;
    public Toggle Ring3;
    public Toggle Ring4;
    public Toggle Ring5;
    public Toggle Ring6;
    public Toggle Ring7;
    public Toggle Ring8;

    public Button Btn_Active;
    public Button Btn_Strong;

    public StrenthAttrItem Atrr1;
    public StrenthAttrItem Atrr2;
    public StrenthAttrItem Atrr3;
    public StrenthAttrItem Atrr4;
    public StrenthAttrItem Atrr5;

    public Text LockLevel;
    public Text LockMemo;

    public Toggle RingSkill1;
    public Toggle RingSkill2;
    public Toggle RingSkill3;
    public Toggle RingSkill4;
    public Toggle RingSkill5;
    public Toggle RingSkill6;
    public Toggle RingSkill7;
    public Toggle RingSkill8;

    List<Toggle> RingList = new List<Toggle>();
    List<Toggle> RingSkillList = new List<Toggle>();

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowSoulRingEvent>(this.OnShowSoulRingEvent);

        RingList.Add(Ring1);
        RingList.Add(Ring2);
        RingList.Add(Ring3);
        RingList.Add(Ring4);
        RingList.Add(Ring5);
        RingList.Add(Ring6);
        RingList.Add(Ring7);
        RingList.Add(Ring8);

        RingSkillList.Add(RingSkill1);
        RingSkillList.Add(RingSkill2);
        RingSkillList.Add(RingSkill3);
        RingSkillList.Add(RingSkill4);
        RingSkillList.Add(RingSkill5);
        RingSkillList.Add(RingSkill6);
        RingSkillList.Add(RingSkill7);
        RingSkillList.Add(RingSkill8);

        Init();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        for (int i = 0; i < RingList.Count; i++)
        {
            Toggle ring = RingList[i];
            Toggle ring_skill = RingSkillList[i];

            //Item_SoulRing_Skill ring_Skill = ring_skills.Where(m => m.Type == ring.Type).FirstOrDefault();

            if (user.SoulRingData.TryGetValue(0, out MagicData data)) //active
            {
                InitRing(ring, 99);
            }
            else
            {
                InitRing(ring, 0);
                //ring.SetLevel(0);
            }
        }
    }


    private void InitRing(Toggle ring, long level)
    {
        Text[] txtList = ring.GetComponentsInChildren<Text>();

        for (int i = 0; i < txtList.Length; i++)
        {
            if (txtList[i].name == "lb_Name")
            {
                txtList[i].text = "Î´¼¤»î";
            }
            else
            {
                txtList[i].text = level + "";
            }
        }
    }

    private void InitRingSkill() { 

    }

    public void OnShowSoulRingEvent(ShowSoulRingEvent e) {
        this.gameObject.SetActive(true);
    }


    public void OnClick_Close()
    {
        Debug.Log("OnClick_Close");
        this.gameObject.SetActive(false);
    }
}
