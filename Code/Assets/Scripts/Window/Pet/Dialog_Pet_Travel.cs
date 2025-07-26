using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pet_Travel : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Text Txt_Info;
    public Toggle toggle_Hide;

    public Button Btn_StopTravel;

    public Button Btn_Close;

    public Transform Tf_Layer;

    private List<Toggle> tgLevelList;

    private int LevelCount = 35; //每个难度多少个
    private int ShowCount = 10; //隐藏的时候显示多少个

    private int MaxLayer = -1;
    private int SelectLayer = -1;

    private Pet SelectPet;

    List<Item_Travel> items = new List<Item_Travel>();

    private void Awake()
    {
        tgLevelList = Tf_Layer.GetComponentsInChildren<Toggle>().ToList();
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Travel");

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_StopTravel.onClick.AddListener(OnStopTravel);

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            int index = i;
            tgLevelList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLevel(index);
            });
        }

        this.Init();

        toggle_Hide.onValueChanged.AddListener((isOn) =>
        {
            this.Show();
        });
    }

    private void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<PetStartTravelEvent>(this.StartTravel);
    }

    private void StartTravel(PetStartTravelEvent e)
    {
        if (SelectPet.RunMapId > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先结束现有的巡游，再开启新的", ToastType = ToastTypeEnum.Failure });
            return;
        }

        SelectPet.RunMapId = e.MapId;
        SelectPet.RunTime = TimeHelper.ClientNowSeconds();

        this.ShowTravelInfo();

    }

    private void OnStopTravel()
    {
        Debug.Log("Pet Stop:" + SelectPet.RunMapId);

        if (this.SelectPet.RunMapId <= 0)
        {
            return;
        }

        long time = TimeHelper.ClientNowSeconds() - SelectPet.RunTime;
        time = Math.Min(time, 86400);
        long count = time / 60;

        int mapId = this.SelectPet.RunMapId;

        this.SelectPet.RunMapId = 0;
        this.SelectPet.RunTime = 0;

        //build reward
        this.SelectPet.AddExp(count);

        this.ShowTravelInfo();

        string message = SelectPet.Name + "打工获得经验：" + count;

        List<Item> items = new List<Item>();

        if (time >= 60)
        {
            User user = GameProcessor.Inst.User;
            long rewardExp = 0;
            long rewardGold = 0;

            items.AddRange(BuildReward(user, time, ref rewardExp, ref rewardGold, ref message, mapId));

            user.AddExpAndGold(rewardExp, rewardGold);

            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });
    }

    private List<Item> BuildReward(User user, long offlineTime, ref long rewardExp, ref long rewardGold, ref string message, int mapId)
    {
        MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

        List<Item> itemList = new List<Item>();

        long killCountFrom = (long)(offlineTime * 2.5);
        //long realKillCount = (long)(killCount * modelConfig.CountRate);

        double lossRate = 2.2; //宠物的系数

        double realRate = user.GetRealDropRate() * modelConfig.DropRate;
        double qualityRate = (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
        double realQualityRate = 1 + Math.Log(qualityRate, 13);
        long soulPercent = user.AttributeBonus.GetTotalAttr(AttributeEnum.SoulPercent);
        //Debug.Log("realRate:" + realRate);
        //Debug.Log("qualityRate:" + qualityRate);
        //Debug.Log("realQualityRate:" + realQualityRate);

        MapConfig mapConfig = MapConfigCategory.Instance.Get(mapId);

        MonsterBase monster = MonsterBaseCategory.Instance.GetByMapId(mapId);

        long burstMul = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
        long killCount = killCountFrom * (100 + burstMul) / 100;

        message += "\n 宠物巡游(" + mapConfig.Name + ")，击杀了" + killCountFrom + "(连爆计算为" + killCount + ")个怪物，获得";

        long gold = (long)(monster.Gold * killCount * modelConfig.RewardRate * ((100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100));
        long exp = (long)(monster.Exp * killCount * modelConfig.RewardRate * ((100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100));

        //Debug.Log("monster:" + monster.Name);

        message += "，金币" + StringHelper.FormatNumber(gold) + "，经验" + StringHelper.FormatNumber(exp);

        rewardExp += exp;
        rewardGold += gold;

        //炼魂
        int soulRise = 0;
        if (soulPercent > 0)
        {
            soulRise = user.SoulRingNumber + user.GetArtifactValue(ArtifactType.SoulStone);
            soulRise = (int)(killCount * soulRise * soulPercent * modelConfig.DropRate / 100);
            if (soulRise > 0)
            {
                itemList.Add(ItemHelper.BuildSoulRingShard(soulRise));
                message += ",炼魂:<color=#FF6600>魂环碎片</color>*" + soulRise;
            }
        }

        int skillBox = 0;

        for (int i = 0; i < mapConfig.DropIdList.Count(); i++)
        {
            int dropId = mapConfig.DropIdList[i];
            DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

            double dropRate = Math.Max(lossRate, mapConfig.DropRateList[i] * lossRate / realRate);

            double killRecord = user.GetKillRecord(dropId);
            int dropCount = MathHelper.CalOfflineDropCount(killRecord, killCount, dropRate);

            if (dropCount > 0)
            {
                if (dropConfig.ItemType == (int)ItemType.Equip)
                {   //Auto Recovery
                    if (dropConfig.Id <= 110)
                    {
                        //四格
                        int layer = dropConfig.Id - 100;
                        int baseQuantity = (int)(Math.Pow(2, layer));
                        int speicaStone = dropCount * baseQuantity;
                        itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Speical_Stone, speicaStone));
                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"四格碎片"}]</color>" + speicaStone + "个";

                        //Debug.Log(dropCount + "个四格->" + speicaStone + "个四格碎片");
                    }
                    else
                    {
                        int refineStone = (int)(dropCount * MathHelper.CalRefineStone(mapConfig.DropLevel, user.StoneNumber + user.GetArtifactValue(ArtifactType.RefineStone)) * realQualityRate);
                        itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, refineStone));
                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"精炼石"}]</color>" + StringHelper.FormatNumber(refineStone) + "个";

                        //Debug.Log(dropCount + "个装备->" + refineStone + "个精炼石");
                    }
                }
                else if (dropConfig.ItemType == (int)ItemType.Exclusive)
                {
                    int exclusiveStone = (int)(dropCount * realQualityRate);
                    itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, exclusiveStone));
                    message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"专属碎片"}]</color>" + dropCount + "个";

                    //Debug.Log(dropCount + "个专属->" + exclusiveStone + "个专属精华");
                }
                else if (dropConfig.ItemType == (int)ItemType.SkillBox)
                {
                    skillBox += dropCount * dropConfig.Level / 50;

                }
                else
                {
                    //道具多次随机
                    Dictionary<int, int> merginDict = new Dictionary<int, int>();
                    for (int d = 0; d < dropCount; d++)
                    {
                        int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);

                        int itemId = dropConfig.ItemIdList[di];

                        if (!merginDict.ContainsKey(itemId))
                        {
                            merginDict[itemId] = 0;
                        }
                        merginDict[itemId]++;
                    }

                    foreach (var sp in merginDict)
                    {
                        itemList.Add(ItemHelper.BuildItem((ItemType)dropConfig.ItemType, sp.Key, 1, sp.Value));


                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(6)}>[{dropConfig.Name}]</color>" + dropCount + "个";
                    }
                }
            }

            user.SaveKillRecord(dropId, killCount);
        }

        //-------书页汇总-----------
        if (skillBox > 0)
        {
            itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Shuye1, skillBox));
            message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"书页"}]</color>" + skillBox + "个";
        }

        List<DropLimitConfig> limits = DropLimitConfigCategory.Instance.GetByMapId((int)DropLimitType.Map, mapId);

        int cardCount = 0;
        int fashionCount = 0;

        string limitMessage = "";
        for (int i = 0; i < limits.Count(); i++)
        {
            DropLimitConfig limitConfig = limits[i];
            int dropId = limitConfig.DropId;
            //Debug.Log("drop Limit Id:" + limitConfig.DropId);

            double dr = limitConfig.ShareRise > 0 ? realRate : 1 * modelConfig.CountRate; //吃爆率用爆率，不吃爆率用数量
            double dropRate = Math.Max(lossRate, (limitConfig.StartRate + limitConfig.Rate) * lossRate / dr);

            //Debug.Log("dropRate:" + dropRate);

            double killRecord = user.GetKillRecord(dropId);
            int dropCount = MathHelper.CalOfflineDropCount(killRecord, killCount, dropRate);

            if (dropCount > 0)
            {
                DropConfig dropConfig = DropConfigCategory.Instance.Get(limitConfig.DropId);

                if (dropConfig.ItemType == (int)ItemType.Equip)
                {   //Auto Recovery
                    //message += "," + dropCount + "个" + limitConfig.Name;

                    for (int d = 0; d < dropCount; d++)
                    {
                        int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                        itemList.Add(ItemHelper.BuildEquip(dropConfig.ItemIdList[di], 0, 1, TimeHelper.TodaySeed()));
                    }
                }
                else
                {
                    //道具多次随机
                    Dictionary<int, int> merginDict = new Dictionary<int, int>();
                    for (int d = 0; d < dropCount; d++)
                    {
                        int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);

                        int itemId = dropConfig.ItemIdList[di];

                        if (!merginDict.ContainsKey(itemId))
                        {
                            merginDict[itemId] = 0;
                        }
                        merginDict[itemId]++;
                    }

                    foreach (var sp in merginDict)
                    {
                        itemList.Add(ItemHelper.BuildItem((ItemType)dropConfig.ItemType, sp.Key, 1, sp.Value));


                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(6)}>[{dropConfig.Name}]</color>" + dropCount + "个";
                    }
                }

                if (dropConfig.ItemType == (int)ItemType.Card)
                {
                    cardCount += dropCount;
                }
                else if (dropConfig.ItemType == (int)ItemType.Fashion)
                {
                    fashionCount += dropCount;
                }
                else
                {
                    int q = limitConfig.Id > 1000 ? 6 : 5;

                    limitMessage += $"，<color=#{QualityConfigHelper.GetQualityColor(q)}>[{limitConfig.Name}]</color>" + dropCount + "个";
                }

                //Debug.Log("drop limit " + killRecord + "-" + (killRecord + killCount) + " 掉落" + dropCount + "个" + limitConfig.Name);
            }

            user.SaveKillRecord(dropId, killCount);
        }

        if (cardCount > 0)
        {
            message += $"，<color=#{QualityConfigHelper.GetQualityColor(4)}>[{"图鉴"}]</color>" + cardCount + "个";
        }
        if (fashionCount > 0)
        {
            message += $"，<color=#{QualityConfigHelper.GetQualityColor(5)}>[{"时装"}]</color>" + fashionCount + "个";
        }

        message += limitMessage + "\n";

        return itemList;
    }

    private void Init()
    {
        List<MapConfig> list = MapConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (MapConfig config in list)
        {
            BuildItem(config);
        }
    }

    private void BuildItem(MapConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        Item_Travel com = item.GetComponent<Item_Travel>();

        com.Init(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    private void ChangeLevel(int layer)
    {
        this.SelectLayer = layer;
        this.Show();
    }

    public void Open(Pet pet)
    {
        this.gameObject.SetActive(true);
        this.SelectPet = pet;

        this.Show();
    }

    private void ShowTravelInfo()
    {
        if (this.SelectPet.RunMapId > 0)
        {
            long time = TimeHelper.ClientNowSeconds() - SelectPet.RunTime;

            long count = time / 60;

            MapConfig map = MapConfigCategory.Instance.Get(this.SelectPet.RunMapId);
            Txt_Info.text = "当前巡游地图为：" + map.Name + "(" + count + "分钟)";

            Btn_StopTravel.gameObject.SetActive(true);
        }
        else
        {
            Txt_Info.text = "空闲中...";
            Btn_StopTravel.gameObject.SetActive(false);
        }
    }

    private void Show()
    {
        this.ShowTravelInfo();

        int PetQuality = this.SelectPet.GetQuality();

        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }

        int MapId = GameProcessor.Inst.User.MapId;
        this.MaxLayer = (MapId - ConfigHelper.MapStartId) / 35;

        if (this.SelectLayer < 0)
        {
            this.SelectLayer = Math.Min(this.MaxLayer, PetQuality);
            this.SelectLayer = Math.Min(this.SelectLayer, tgLevelList.Count - 1);
            tgLevelList[SelectLayer].isOn = true;
        }

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            if (i <= MaxLayer && i < PetQuality)
            {
                tgLevelList[i].gameObject.SetActive(true);
            }
            else
            {
                tgLevelList[i].gameObject.SetActive(false);
            }
        }

        int count = MapConfigCategory.Instance.GetAll().Where(m => m.Value.Id <= MapId).Count();

        int startIndex = this.SelectLayer * LevelCount;
        int endIndex = startIndex + Math.Min(LevelCount, count - startIndex) - 1;

        int j = 0;
        for (int i = endIndex; i >= startIndex; i--)
        {
            if (j < ShowCount)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(!toggle_Hide.isOn);
            }
            j++;
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
