using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossInfo : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false); 
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    public int Order => (int)ComponentOrder.Dialog;
    
    private void OnBossInfoEvent(BossInfoEvent e)
    {
        this.gameObject.SetActive(true);
    }
}
