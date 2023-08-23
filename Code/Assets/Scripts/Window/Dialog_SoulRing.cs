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
    List<Item_SoulRing> rings = new List<Item_SoulRing>();
    List<Item_SoulRing_Skill> ring_skills = new List<Item_SoulRing_Skill>();

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

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnBattleStart()
    {
        //GameProcessor.Inst.EventCenter.AddListener<PhantomEvent>(this.OnPhantomEvent);

        rings = this.GetComponentsInChildren<Item_SoulRing>().ToList();
        ring_skills = this.GetComponentsInChildren<Item_SoulRing_Skill>().ToList();

        Init();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        for (int i = 0; i < rings.Count; i++) {
            Item_SoulRing ring = rings[i];
            int position = (int)ring.Type;

            //Item_SoulRing_Skill ring_Skill = ring_skills.Where(m => m.Type == ring.Type).FirstOrDefault();

            if (user.SoulRingData.TryGetValue(0, out MagicData data)) //active
            {
                //ring.SetLevel(data.Data);
            }
            else
            {
                //ring.SetLevel(0);
            }
        }
    }




    public void UpadateItem()
    {
        
    }
    
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
