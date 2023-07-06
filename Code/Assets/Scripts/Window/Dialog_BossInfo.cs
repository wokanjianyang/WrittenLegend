using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossInfo : MonoBehaviour, IBattleLife
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
        GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);

        User user = GameProcessor.Inst.User;

        Dictionary<int, long> list = user.MapBossTime;
        foreach (int MapId in list.Keys)
        {
            MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);
            //if (mapConfig.LevelRequired <= user.Level)
            //{
                BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);
                BuildItem(mapConfig, bossConfig, list[MapId]);
            //}
        }
    }

    public int Order => (int)ComponentOrder.Dialog;

    private void OnBossInfoEvent(BossInfoEvent e)
    {
        this.gameObject.SetActive(true);

        this.UpadateItem();
    }

    private void BuildItem(MapConfig mapConfig, BossConfig bossConfig, long killTime)
    {
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

        Dictionary<int, long> list = user.MapBossTime;
        foreach (int MapId in list.Keys)
        {
            var item = items.Where(m => m.mapConfig.Id == MapId).FirstOrDefault();
            if (item != null)
            {
                item.SetKillTime(list[MapId]);
            }
        }
    }
}
