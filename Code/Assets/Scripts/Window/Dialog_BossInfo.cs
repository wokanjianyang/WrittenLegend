using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossInfo : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_BossInfo");
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    public int Order => (int)ComponentOrder.Dialog;
    
    private void OnBossInfoEvent(BossInfoEvent e)
    {
        this.gameObject.SetActive(true);

        //
        for (int i = 0; i < 10; i++) {
            AddItem();
        }

    }

    private void AddItem() {
        var emptyBook = GameObject.Instantiate(ItemPrefab);
        var com = emptyBook.GetComponent<Com_BossInfoItem>();
        com.SetContent("TestBoss");

        emptyBook.transform.SetParent(this.sr_Boss.content);
        emptyBook.transform.localScale = Vector3.one;
    }
}
