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

            Item_SoulRing_Skill ring_Skill = ring_skills.Where(m => m.Type == ring.Type).FirstOrDefault();

            if (user.SoulRingData.TryGetValue(0, out MagicData data)) //active
            {
                ring.SetLevel(data.Data);
            }
            else
            {
                ring.SetLevel(0);
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
