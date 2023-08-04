using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Phantom : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    List<Com_BossInfoItem> items = new List<Com_BossInfoItem>();

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_BossInfo");

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<PhantomEvent>(this.OnPhantomEvent);

    }

    public int Order => (int)ComponentOrder.Dialog;

    private void OnPhantomEvent(PhantomEvent e)
    {
        this.gameObject.SetActive(true);

        this.UpadateItem();
    }

    private void BuildItem(int id, long killTime)
    {
        MapConfig mapConfig = MapConfigCategory.Instance.Get(1001);
        BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);

        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Com_BossInfoItem>();

        com.SetContent(mapConfig, bossConfig, killTime);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void UpadateItem()
    {
        User user = GameProcessor.Inst.User;

        Dictionary<int, int> recordList = user.PhantomRecord;

        List<PhantomConfig> configs = PhantomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (PhantomConfig config in configs)
        {
            //
            int lv = 1;
            recordList.TryGetValue(config.Id, out lv);
            BuildItem(config.Id, lv);
        }
    }
    
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
