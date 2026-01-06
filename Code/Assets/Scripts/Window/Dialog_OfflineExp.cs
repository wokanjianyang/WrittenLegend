using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Dialog_OfflineExp : MonoBehaviour
    {
        [LabelText("离线奖励提示")]
        public Text Txt_Msg;

        [LabelText("领取按钮")]
        public Button Btn_OK;

        public Button Btn_Close;

        private string[] bl = new string[] { "1E88210276", "1F0EA58EE1", "7291392C1C", "B8A8BEA0E6", "FCDE5D0871", "E5DF3740F6"
            , "8B5735A9E7", "7BF9F93802", "57248E8144", "568AA4F817", "4DA09FE954", "4BC052AD8E", "", "" };

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_OK.onClick.AddListener(this.OnClick_OK);
            this.Btn_Close.onClick.AddListener(this.OnClick_OK);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;


        private void OnClick_OK()
        {
            this.gameObject.SetActive(false);
            //Time.timeScale = 1;
        }

        private void TestSend(User user)
        {
            List<Item> items = new List<Item>();

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Legacy_Ticket, 300)); //传世卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Copy_Ticket, 6000)); //装备卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 300)); //BOSS卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Level_Stone, 20000));  //羽毛
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Wing_Stone, 100));  //幻境劵

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Shuye3, 100)); //专属碎片
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Heart, 100)); //专属之心
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 1999999999)); //四格碎片

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 999999999)); //精炼石
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Red_Stone, 999)); //红装精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Golden_Stone, 999)); //金色精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Dark_Stone, 999)); //暗金精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Hundun, 999)); //混沌精华

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Golden, 999)); //传奇精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Dark, 999)); //不朽精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Stone_Set, 999)); //开孔石

            //items.Add(ItemHelper.BuildMaterial(50000106, 10000)); //十六阶装备升阶石

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecailEquipRefreshId, 99999)); //橙装精华

            //items.Add(ItemHelper.BuildItem(ItemType.Card, 2000010, 10, 5));

            //items.Add(ItemHelper.BuildItem(ItemType.SkillBox, 1010, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.SkillBox, 3010, 1, 1));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Pill, 6000000));

            //user.SaveArtifactLevel(180001, 1); //boss杀手
            //user.SaveArtifactLevel(180019, 1); //副本杀手

            //user.SaveArtifactLevel(180005, 10); //卖身契
            //user.SaveArtifactLevel(180006, 10); //万界图
            //user.SaveArtifactLevel(180007, 1); //破限
            //user.SaveArtifactLevel(180008, 1); //魔法
            //user.SaveArtifactLevel(180009, 1); //圣者
            //user.SaveArtifactLevel(180011, 1); //锤子
            //user.SaveArtifactLevel(180013, 1); //金蛟剪
            //user.SaveArtifactLevel(180015, 1); //财富契约
            //user.SaveArtifactLevel(180020, 1); //传世之源
            //user.SaveArtifactLevel(180021, 1); //神戒之源
            //user.SaveArtifactLevel(180030, 45); //极。卖身契
            //user.SaveArtifactLevel(180033, 45); //极。BOSS
            //user.SaveArtifactLevel(180035, 45); //极。魔法

            //user.Record.AddRecord(RecordType.AdReal, -800);
            //user.MagicGold.Data = 100000000000000000L;//10京金币

            //user.SaveItemMeterialCount(1999994, 100000);
            //user.SaveItemMeterialCount(ItemHelper.SpecialId_Card_Stone, 10000000);
            //user.Cycle.Data = 2;
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 22, 1, 1)); //特戒
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 32, 1, 1)); //普通金宠包
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 33, 1, 1)); //满资质金宠包
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 41, 1, 8)); //神器自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 95, 1, 6)); //传奇自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 90, 1, 6)); //不朽自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 36, 1, 1)); //宝石自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 43, 1, 1)); //生肖自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 44, 1, 1)); //极戒自选

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 26, 1, 1));  //神技

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 28, 1, 6));  //魂骨
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 29, 1, 3));  //11技能
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 42, 1, 1)); //12技能自选

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 106, 1, 3)); //白银
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 107, 1, 1)); //黄金
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 108, 1, 1)); //钻石

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 109, 1, 5));  //战士经验
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 110, 1, 1)); //法师经验
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 111, 1, 1)); //道士经验

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 112, 1, 10));  //战士输出
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 113, 1, 1)); //法师输出
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 114, 1, 1)); //道士输出

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 127, 1, 1)); //金装自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 128, 1, 1)); //暗金自选

            //items.AddRange(AddGoldenEquip());
            //items.AddRange(AddExclusiveDaoshi());
            //items.AddRange(AddRedEquip1());

            //user.SaveItemMeterialCount(2100001, 11125); //英灵关羽
            //user.SaveItemMeterialCount(2100002, 11125); //英灵刘备
            //user.SaveItemMeterialCount(2100003, 11125); //英灵诸葛
            //user.SaveItemMeterialCount(2100004, 11125); //英灵关羽
            //user.SaveItemMeterialCount(2100005, 11125); //英灵关羽
            //user.SaveItemMeterialCount(2100006, 11125); //英灵关羽

            foreach (var item in items)
            {
                BoxItem boxItem = new BoxItem();
                boxItem.Item = item;
                boxItem.MagicNubmer.Data = Math.Max(1, item.Count);
                boxItem.BoxId = -1;
                user.Bags.Add(boxItem);
            }
        }

        //items.Add(ItemHelper.BuildMaterial(50000105, 10000)); //十五阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000106, 10000)); //十六阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000107, 10000)); //十七阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000108, 10000)); //十八阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000109, 10000)); //十九阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000110, 10000)); //二十阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000111, 10000)); //二一阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000112, 10000)); //二二阶装备升阶石

        //user.SaveItemMeterialCount(1998001, 5); //暗金图鉴青龙
        //user.SaveItemMeterialCount(1998002, 5); //暗金图鉴白虎
        //user.SaveItemMeterialCount(1998003, 5); //暗金图鉴朱雀
        //user.SaveItemMeterialCount(1998004, 5); //暗金图鉴玄武
        //user.SaveItemMeterialCount(1998005, 5); //暗金图鉴麒麟

        //for (int i = 1; i <= 8; i++)
        //{
        //    items.Add(ItemHelper.BuildEquip(21205800 + i, 7, 0, 0));
        //}

        //items.Add(new Equip(21205805, 11010, 11110, 7));
        //items.Add(new Equip(21205807, 11010, 11110, 7));

        //items.Add(new Equip(21105801, 9, 10, 6));
        //items.Add(new Equip(21105802, 9, 10, 6));
        //items.Add(new Equip(21105803, 9, 10, 6));

        //items.Add(ItemHelper.BuildEquip(21105803, 6,0,0));
        //items.Add(ItemHelper.BuildEquip(21105804, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105805, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105805, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105807, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105807, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105809, 6, 0, 0));
        //items.Add(new Equip(22005707, 16, 10037, 5));
        //items.Add(new Equip(22005709, 23, 15, 5));
        //items.Add(new Equip(22005710, 23, 15, 5));

        //items.Add(ItemHelper.BuildMaterial(50000009, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000011, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000014, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000015, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000016, 30)); //3介升阶红石头

        //int rc = 1;
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190001, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190002, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190003, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190004, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190005, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190006, 1, rc));

        //int ic = 1;
        //items.Add(ItemHelper.BuildMaterial(8001, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8002, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8003, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8004, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8005, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8006, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8007, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8008, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8009, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8010, ic)); //神技

        //int ic = 1;
        //items.Add(ItemHelper.BuildMaterial(8101, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8102, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8103, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8104, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8105, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8106, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8107, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8108, ic)); //魂骨

        //int ic = 10000;
        //items.Add(ItemHelper.BuildMaterial(60000001, ic)); //攻击宝石
        //items.Add(ItemHelper.BuildMaterial(60000002, ic)); //防御宝石
        //items.Add(ItemHelper.BuildMaterial(60000003, ic)); //生命宝石
        //items.Add(ItemHelper.BuildMaterial(60000004, ic)); //命中宝石
        //items.Add(ItemHelper.BuildMaterial(60000005, ic)); //闪避宝石
        //items.Add(ItemHelper.BuildMaterial(60000006, ic)); //物理宝石
        //items.Add(ItemHelper.BuildMaterial(60000007, ic)); //魔法宝石
        //items.Add(ItemHelper.BuildMaterial(60000008, ic)); //道术宝石
        //items.Add(ItemHelper.BuildMaterial(60000009, ic)); //增伤宝石
        //items.Add(ItemHelper.BuildMaterial(60000010, ic)); //韧性宝石

        private List<Item> TestShengxiao(User user)
        {

            List<Item> dropList = new List<Item>();
            for (int i = 0; i < 3600 * 5 * 24 * 10; i++)
            {
                int quality = BuildQuality();
                int configId = RandomHelper.RandomNumber(7, 25) % 12 + 1;

                dropList.AddRange(BuildReword(4, quality, configId));
            }

            Debug.Log("dropList Count:" + dropList.Count);

            //Debug.Log("shengxiao metail Count:" + AppHelper.TempRecord);

            for (int i = 1; i <= 9; i++)
            {
                int count = dropList.Where(m => m.GetQuality() == i).Count();

                Debug.Log("Quality" + i + " Count:" + count);
            }

            List<Item> recoveryList = user.CheckRecovery(dropList, out long recoveryGold, out int recoveryCount);

            //dropList.AddRange(recoveryList);

            return dropList;
        }

        private double[] DropRateList = { 1, 2, 4, 10, 40 };
        private double[] QualityRateList = { 1, 1.1, 1.2, 1.5, 2 };
        private List<Item> BuildReword(int mapId, int Quality, int NameId)
        {
            MonsterShengxiaoConfig config = MonsterShengxiaoConfigCategory.Instance.Get(mapId);

            int maxQuality = config.Id + 5;

            User user = GameProcessor.Inst.User;

            double exp = (config.Exp * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
            double gold = (config.Gold * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

            double dropRate = 400.0 / (user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea) / 750000.0 + 1) / DropRateList[Quality - 1];
            double qualityRate = (user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea) / 750000.0 + 1) * QualityRateList[Quality - 1];

            //生肖掉落
            List<Item> items = new List<Item>();
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao, Quality * config.Id));

            //Debug.Log("this dropRate :" + dropRate + "  qualityRate:" + qualityRate + " nameId:" + NameId + " - " + Name + " quality:" + Quality);

            if (RandomHelper.RandomResult(dropRate))
            {
                //AppHelper.TempRecord++;
                //Debug.Log("shengxiao count:" + AppHelper.TempRecord);
                //生肖
                items.Add(ShengxiaoConfigCategory.Instance.Build(NameId, qualityRate, maxQuality, 0));
            }

            double rs = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            int itemCount = MathHelper.RandomBurstMul(rs);

            if (itemCount > 0)
            {
                exp += exp * itemCount;
                gold += gold * itemCount;
                items.AddRange(ItemHelper.BurstMul(items, itemCount, qualityRate, RuleType.Normal, maxQuality));
            }

            AppHelper.TempRecord += Quality * config.Id * (1 + itemCount);

            return items;
        }

        private int BuildQuality()
        {
            int rd = RandomHelper.RandomNumber(1, 501);
            if (rd > 499)
            {
                return 5;
            }
            else if (rd > 495)
            {
                return 4;
            }
            else if (rd > 480)
            {
                return 3;
            }
            else if (rd > 400)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public void TestInfinityDrop()
        {

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < 20; i++)
            {
                user.InfiniteData.GetDropId(10);
                List<int> drops = user.InfiniteData.DropList[0];
                user.InfiniteData.DropList.RemoveAt(0);

                int countT = drops.Where(m => m >= 180032 && m <= 180034).Count();
                if (countT > 0)
                {
                    Debug.Log(i + " drop Talent :" + countT);
                }
            }
        }

        public void ShowOffline()
        {
            User user = GameProcessor.Inst.User;

            string deviceId = user.DeviceId;
            if (bl.Contains(deviceId))
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

            long currentTick = TimeHelper.ClientNowSeconds();
            long offlineTime = currentTick - user.SecondExpTick;

            long tempTime = Math.Min(offlineTime, ConfigHelper.MaxOfflineTime);

            List<Item> items = new List<Item>();
            long rewardExp = 0;
            long rewardGold = 0;

            string OfflineMessage = "离线时间" + offlineTime + "S";
            if (tempTime < offlineTime)
            {
                OfflineMessage += "，实际计算" + tempTime + "S)";
            }
            OfflineMessage += "\n";

            if (user.SpiritOfflineFlag)
            {
                items.AddRange(BuildOfflineSpirit(user, tempTime, ref rewardExp, ref rewardGold, ref OfflineMessage));
            }
            else if (user.OffLineMapId > 0)
            {
                //离线暗殿
                items.AddRange(BuildOfflineAndian(user, tempTime, ref rewardExp, ref rewardGold, ref OfflineMessage));
            }
            else
            {
                //离线闯关
                items.AddRange(BuildOfflineTower(user, tempTime, ref rewardExp, ref rewardGold, ref OfflineMessage));
            }

            //离线经验，金币
            long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp) * (offlineTime / 5);
            long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold) * (offlineTime / 5);

            OfflineMessage += "\n离线秒收金币" + StringHelper.FormatNumber(gold) + "，经验" + StringHelper.FormatNumber(exp) + "\n";

            //离线挖矿
            this.BuildOfflineMine(user, tempTime, ref OfflineMessage);

            //测试道具
            this.TestSend(user);

            user.AddExpAndGold(exp + rewardExp, gold + rewardGold);
            user.SecondExpTick = currentTick;
            user.MinerTime = currentTick;

            foreach (var item in items)
            {
                if (item.Type == ItemType.Card || item.Type == ItemType.Fashion || item.Type == ItemType.Spirit || (item.Type == ItemType.Material && item.ConfigId == ItemHelper.SpecialId_Card_Stone))
                {
                    user.SaveItemMeterialCount(item.ConfigId, item.Count);
                }
                else
                {
                    BoxItem boxItem = user.Bags.Find(m => !m.IsFull() && m.Item.Type == item.Type && m.Item.ConfigId == item.ConfigId);  //ͬ

                    if (boxItem != null)
                    {
                        boxItem.AddStack(item.Count);
                    }
                    else
                    {
                        boxItem = new BoxItem();
                        boxItem.Item = item;
                        boxItem.MagicNubmer.Data = Math.Max(1, item.Count);
                        boxItem.BoxId = -1;
                        user.Bags.Add(boxItem);
                    }
                }
            }

            //检查
            DateTime saveDate = new DateTime(user.DataDate);
            if (saveDate.Day < DateTime.Now.Day || saveDate.Month < DateTime.Now.Month || saveDate.Year < DateTime.Now.Year)
            {
                user.DefendData.Refresh();
                user.HeroPhatomData.Refresh();
                user.PillTime.Check(user.Cycle.Data);
                user.RedRefreshCount.Data = 0;

                int BabelCount = ConfigHelper.BabelCount;
                if (user.BabelData.Data < 10000)
                {
                    BabelCount = ConfigHelper.BabelCount + 200;
                }
                else if (user.BabelData.Data < 20000)
                {
                    BabelCount = ConfigHelper.BabelCount + 100;
                }

                user.BabelCount.Data = BabelCount;

                user.DataDate = DateTime.Now.Ticks;
                //保存到Tap
            }
            user.WorldData.Check();
            GameProcessor.Inst.SaveData();

            this.gameObject.SetActive(true);

            this.Txt_Msg.text = OfflineMessage;

            //Time.timeScale = 0;
        }

        private List<Item> BuildOfflineTower(User user, long tempTime, ref long totalExp, ref long totalGold, ref string message)
        {
            List<Item> itemList = new List<Item>();

            long offlineFloor = 0;
            long tmepFloor = user.MagicTowerFloor.Data;
            long rewardExp = 0;
            long rewardGold = 0;

            while (tempTime > 0 && tmepFloor < ConfigHelper.Max_Floor)
            {
                tmepFloor = user.MagicTowerFloor.Data + offlineFloor + 100;

                TowerConfig config = TowerConfigCategory.Instance.GetByFloor(tmepFloor); //quick

                AttributeBonus offlineHero = user.AttributeBonus;
                AttributeBonus offlineTower = MonsterTowerHelper.BuildOffline(tmepFloor);

                SkillPanel sp = new SkillPanel(new SkillData(9001, (int)SkillPosition.Default), new List<SkillRune>(), new List<SkillSuit>(), false);

                int roundHeroToTower = DamageHelper.CalcAttackRound(offlineHero, offlineTower, sp);
                int roundTowerToHero = DamageHelper.CalcAttackRound(offlineTower, offlineHero, sp);

                if (roundHeroToTower > roundTowerToHero)
                {
                    //fail
                    tempTime = 0;
                }
                else
                {
                    long floorTime = roundHeroToTower + (5 - user.TowerNumber); //5s find monster - achievement time
                    floorTime = Math.Max(floorTime, 1);
                    long maxFloor = Math.Min(tempTime / floorTime, 100);

                    offlineFloor += maxFloor;
                    rewardExp += maxFloor * config.Exp;
                    rewardGold += maxFloor * config.Gold;
                    tempTime -= Math.Max(maxFloor, 1) * floorTime;
                }
            }

            int floorRate = ConfigHelper.GetFloorRate(tmepFloor) * user.GetDzRate();
            offlineFloor = offlineFloor * floorRate;

            for (int i = 0; i < offlineFloor; i++)
            {
                long fl = user.MagicTowerFloor.Data + i;

                int equipLevel = Math.Max(10, (user.MapId - ConfigHelper.MapStartId) * 10);

                itemList.AddRange(DropHelper.TowerEquip(fl, equipLevel));
            }

            long newFloor = user.MagicTowerFloor.Data + offlineFloor;
            user.MagicTowerFloor.Data = Math.Min(newFloor, ConfigHelper.Max_Floor);

            message += "\n离线闯关了" + offlineFloor + "层";
            message += "，获得装备" + itemList.Count + "件，金币" + StringHelper.FormatNumber(rewardGold) + "，经验" + StringHelper.FormatNumber(rewardExp);
            message += "\n";

            totalExp += rewardExp;
            totalGold += rewardGold;

            return itemList;
        }


        private List<Item> BuildOfflineSpirit(User user, long offlineTime, ref long rewardExp, ref long rewardGold, ref string message)
        {
            MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

            List<Item> itemList = new List<Item>();

            int mapId = user.SpiritOfflineLog[1];
            int time = user.SpiritOfflineLog[2] + 30;
            int total = user.SpiritOfflineLog[3];

            long count = offlineTime / time;

            SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(mapId);

            message += "\n离线英灵副本(" + config.MapName + ")，计算为通关次数：" + count + "，获得";

            List<SpiritDropConfig> dropList = SpiritDropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.MapId == mapId).ToList();

            //Debug.Log("spirit drop list :" + dropList.Count);

            IDictionary<int, int> dropDict = new Dictionary<int, int>();

            int rate = 1 + Math.Min(4, total / 500);

            foreach (SpiritDropConfig sdpConfig in dropList)
            {
                long dropCount = sdpConfig.DropRate * rate * count / 100;

                DropConfig dropConfig = DropConfigCategory.Instance.Get(sdpConfig.DropId);

                for (int i = 0; i < dropCount; i++)
                {
                    int index = i % dropConfig.ItemIdList.Length;

                    int dropId = dropConfig.ItemIdList[index];

                    if (!dropDict.ContainsKey(dropId))
                    {
                        dropDict[dropId] = 0;
                    }

                    dropDict[dropId]++;
                }
            }

            dropDict.OrderBy(m => m.Key);

            foreach (var sp in dropDict)
            {
                itemList.Add(ItemHelper.BuildItem(ItemType.Spirit, sp.Key, 0, sp.Value));

                ItemConfig spiritConfig = ItemConfigCategory.Instance.Get(sp.Key);

                message += $"，<color=#{QualityConfigHelper.GetQualityColor(spiritConfig.Quality)}>[" + spiritConfig.Name + "]</color>" + sp.Value + "个";
            }

            return itemList;
        }

        private List<Item> BuildOfflineAndian(User user, long offlineTime, ref long rewardExp, ref long rewardGold, ref string message)
        {
            MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

            List<Item> itemList = new List<Item>();

            long killCountFrom = (long)(offlineTime * 2.5);
            //long realKillCount = (long)(killCount * modelConfig.CountRate);

            double lossRate = 1.1; //损失系数,只有 1/1.1 倍的掉落收益

            double realRate = user.GetRealDropRate() * modelConfig.DropRate;
            double qualityRate = (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
            double realQualityRate = 1 + Math.Log(qualityRate, 13);
            long soulPercent = user.AttributeBonus.GetTotalAttr(AttributeEnum.SoulPercent);
            //Debug.Log("realRate:" + realRate);
            //Debug.Log("qualityRate:" + qualityRate);
            //Debug.Log("realQualityRate:" + realQualityRate);

            int mapId = Math.Max(MapConfigCategory.Instance.GetMinMapId(), user.OffLineMapId);
            mapId = Math.Min(MapConfigCategory.Instance.GetMaxMapId(), mapId);

            MapConfig mapConfig = MapConfigCategory.Instance.Get(mapId);

            MonsterBase monster = MonsterBaseCategory.Instance.GetByMapId(mapId);

            long burstMul = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            long killCount = killCountFrom * (100 + burstMul) / 100;

            message += "\n离线未知暗殿(" + mapConfig.Name + ")，击杀了" + killCountFrom + "(连爆计算为" + killCount + ")个怪物，获得";

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
                        for (int d = 0; d < dropCount; d++)
                        {
                            int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                            itemList.Add(ItemHelper.BuildItem((ItemType)dropConfig.ItemType, dropConfig.ItemIdList[di], 1, 1));
                        }
                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(6)}>[{dropConfig.Name}]</color>" + dropCount + "个";
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
                        for (int d = 0; d < dropCount; d++)
                        {
                            int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                            itemList.Add(ItemHelper.BuildItem((ItemType)dropConfig.ItemType, dropConfig.ItemIdList[di], 1, 1));
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

        private void BuildOfflineMine(User user, long mineTime, ref string message)
        {
            long runTime = (long)(ConfigHelper.Mine_Time * 100 / (100 + user.AttributeBonus.GetBaseAttr(AttributeEnum.MetailFinal)));

            runTime = Math.Max(runTime, 6);

            long count = mineTime / runTime;

            if (count <= 0)
            {
                return;
            }

            //miner
            Dictionary<int, int> offlineMetal = MineConfigCategory.Instance.BuildMetal(ref user.MinerSeed, count);

            var sortedDict = offlineMetal.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            message += $"\n离线挖矿收益";
            foreach (var kp in sortedDict)
            {
                var md = user.MetalData;
                int key = kp.Key;
                if (!md.ContainsKey(key))
                {
                    md[key] = new MagicData();
                }

                md[key].Data += kp.Value;

                MetalConfig metalConfig = MetalConfigCategory.Instance.Get(key);

                message += $"<color=#{QualityConfigHelper.GetQualityColor(metalConfig.Quality)}>[{metalConfig.Name}]</color>" + kp.Value + "个";
            }
        }

        private List<Equip> AddGoldenEquip()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //list.Add(ItemHelper.BuildEquip(22205801, 6, 1, 0)); //红色战
            //list.Add(ItemHelper.BuildEquip(22205801, 6, 1, 0)); //红色法
            //list.Add(ItemHelper.BuildEquip(22205801, 6, 1, 0)); //红色道

            //list.Add(ItemHelper.BuildEquip(22205802, 7, 1, 0)); //金色战
            //list.Add(ItemHelper.BuildEquip(22205802, 7, 1, 0)); //金色法
            //list.Add(ItemHelper.BuildEquip(22205802, 7, 1, 0)); //金色道

            //list.Add(ItemHelper.BuildEquip(22205802, 8, 1, 0)); //暗金战
            //list.Add(ItemHelper.BuildEquip(22205802, 8, 1, 0)); //暗金法
            //list.Add(ItemHelper.BuildEquip(22205802, 8, 1, 0)); //暗金道

            //list.Add(ItemHelper.BuildEquip(22205802, 9, 1, 0)); //混沌战
            //list.Add(ItemHelper.BuildEquip(22205802, 9, 1, 0)); //混沌法
            //list.Add(ItemHelper.BuildEquip(22205802, 9, 1, 0)); //混沌道

            int role = 3; //战士1，法师2，道士3
            int quality = 8; //6红，7金，8暗金，9混沌

            //金色
            for (int i = 0; i < 1; i++)
            {
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5801, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5802, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5803, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5804, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5805, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5805, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5807, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5807, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5809, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5810, quality, 1, 0)); //
            }


            return list;
        }

        private List<ExclusiveItem> AddExclusiveZhanshi()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //刺杀
            //ExclusiveItem exclusive1 = new ExclusiveItem(1, 9, 10, 5, 2);
            //exclusive1.RuneConfigIdList.Add(9);
            //exclusive1.RuneConfigIdList.Add(9);
            //exclusive1.RuneConfigIdList.Add(27);
            //exclusive1.SuitConfigIdList.Add(10);
            //exclusive1.SuitConfigIdList.Add(10003);
            //exclusive1.SuitConfigIdList.Add(10003);
            //exclusive1.LevelDict[10008] = 99;
            //exclusive1.Count = 1;
            //list.Add(exclusive1);

            //武力盾
            //ExclusiveItem exclusive2 = new ExclusiveItem(2, 27, 10009, 5, 2);
            //exclusive2.RuneConfigIdList.Add(14);
            //exclusive2.RuneConfigIdList.Add(14);
            //exclusive2.RuneConfigIdList.Add(14);
            //exclusive2.SuitConfigIdList.Add(10009);
            //exclusive2.SuitConfigIdList.Add(10010);
            //exclusive2.SuitConfigIdList.Add(10010);
            //exclusive2.LevelDict[10009] = 99;
            //exclusive2.Count = 1;
            //list.Add(exclusive2);

            ////冰咆哮
            //ExclusiveItem exclusive3 = new ExclusiveItem(3, 5, 6, 5, 2);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.RuneConfigIdList.Add(18);
            //exclusive3.SuitConfigIdList.Add(6);
            //exclusive3.SuitConfigIdList.Add(11);
            //exclusive3.SuitConfigIdList.Add(11);
            //exclusive3.LevelDict[10010] = 99;
            //exclusive3.Count = 1;
            //list.Add(exclusive3);

            ////护体神盾
            //ExclusiveItem exclusive4 = new ExclusiveItem(4, 21, 10029, 5, 2);
            //exclusive4.RuneConfigIdList.Add(21);
            //exclusive4.RuneConfigIdList.Add(21);
            //exclusive4.RuneConfigIdList.Add(21);
            //exclusive4.SuitConfigIdList.Add(10029);
            //exclusive4.SuitConfigIdList.Add(10030);
            //exclusive4.SuitConfigIdList.Add(10030);
            //exclusive4.LevelDict[24] = 99;
            //exclusive4.Count = 1;
            //list.Add(exclusive4);

            ////流行+回复
            //ExclusiveItem exclusive5 = new ExclusiveItem(5, 26, 3, 5, 2);
            //exclusive5.RuneConfigIdList.Add(26);
            //exclusive5.RuneConfigIdList.Add(26);
            //exclusive5.RuneConfigIdList.Add(23);
            //exclusive5.SuitConfigIdList.Add(3);
            //exclusive5.SuitConfigIdList.Add(17);
            //exclusive5.SuitConfigIdList.Add(17);
            //exclusive5.LevelDict[25] = 99;
            //exclusive5.Count = 1;
            //list.Add(exclusive5);

            //月灵+隐身
            //ExclusiveItem exclusive6 = new ExclusiveItem(6, 23, 18, 5, 2);
            //exclusive6.RuneConfigIdList.Add(20);
            //exclusive6.RuneConfigIdList.Add(20);
            //exclusive6.RuneConfigIdList.Add(20);
            //exclusive6.SuitConfigIdList.Add(18);
            //exclusive6.SuitConfigIdList.Add(15);
            //exclusive6.SuitConfigIdList.Add(15);
            //exclusive6.LevelDict[29] = 99;
            //exclusive6.Count = 1;
            //list.Add(exclusive6);

            return list;
        }

        private List<ExclusiveItem> AddExclusiveHuoXing()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //瞬移+法术
            ExclusiveItem exclusive1 = new ExclusiveItem(1, 22, 14, 5, 2);
            exclusive1.RuneConfigIdList.Add(22);
            exclusive1.RuneConfigIdList.Add(22);
            exclusive1.RuneConfigIdList.Add(22);
            exclusive1.SuitConfigIdList.Add(14);
            exclusive1.SuitConfigIdList.Add(14);
            exclusive1.SuitConfigIdList.Add(14);
            exclusive1.LevelDict[10033] = 99;
            exclusive1.Count = 1;
            list.Add(exclusive1);

            //瞬移+护体+鹰眼
            ExclusiveItem exclusive2 = new ExclusiveItem(2, 10049, 8, 5, 2);
            exclusive2.RuneConfigIdList.Add(10049);
            exclusive2.RuneConfigIdList.Add(12);
            exclusive2.RuneConfigIdList.Add(21);
            exclusive2.SuitConfigIdList.Add(8);
            exclusive2.SuitConfigIdList.Add(10018);
            exclusive2.SuitConfigIdList.Add(13);
            exclusive2.LevelDict[10035] = 99;
            exclusive2.Count = 1;
            list.Add(exclusive2);

            //多重+武力
            ExclusiveItem exclusive3 = new ExclusiveItem(3, 21, 13, 5, 1);
            exclusive3.RuneConfigIdList.Add(8);
            exclusive3.RuneConfigIdList.Add(8);
            exclusive3.RuneConfigIdList.Add(8);
            exclusive3.SuitConfigIdList.Add(10018);
            exclusive3.SuitConfigIdList.Add(10017);
            exclusive3.SuitConfigIdList.Add(10017);
            exclusive3.LevelDict[10036] = 99;
            exclusive3.Count = 1;
            list.Add(exclusive3);

            //治疗
            ExclusiveItem exclusive4 = new ExclusiveItem(4, 10055, 10029, 5, 2);
            exclusive4.RuneConfigIdList.Add(10055);
            exclusive4.RuneConfigIdList.Add(10055);
            exclusive4.RuneConfigIdList.Add(10055);
            exclusive4.SuitConfigIdList.Add(10029);
            exclusive4.SuitConfigIdList.Add(3);
            exclusive4.SuitConfigIdList.Add(3);
            exclusive4.LevelDict[10049] = 99;
            exclusive4.Count = 1;
            list.Add(exclusive4);

            //冰咆哮
            ExclusiveItem exclusive5 = new ExclusiveItem(5, 5, 6, 5, 2);
            exclusive5.RuneConfigIdList.Add(5);
            exclusive5.RuneConfigIdList.Add(5);
            exclusive5.RuneConfigIdList.Add(10054);
            exclusive5.SuitConfigIdList.Add(6);
            exclusive5.SuitConfigIdList.Add(11);
            exclusive5.SuitConfigIdList.Add(11);
            exclusive5.LevelDict[10037] = 99;
            exclusive5.Count = 1;
            list.Add(exclusive5);

            //武力盾
            ExclusiveItem exclusive6 = new ExclusiveItem(6, 14, 10009, 5, 2);
            exclusive6.RuneConfigIdList.Add(14);
            exclusive6.RuneConfigIdList.Add(14);
            exclusive6.RuneConfigIdList.Add(14);
            exclusive6.SuitConfigIdList.Add(10009);
            exclusive6.SuitConfigIdList.Add(10010);
            exclusive6.SuitConfigIdList.Add(10010);
            exclusive6.LevelDict[10021] = 99;
            exclusive6.Count = 1;
            list.Add(exclusive6);

            return list;
        }

        private List<ExclusiveItem> AddExclusiveFashi()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //雷电
            ExclusiveItem exclusive1 = new ExclusiveItem(1, 8, 10017, 5, 2);
            exclusive1.RuneConfigIdList.Add(8);
            exclusive1.RuneConfigIdList.Add(8);
            exclusive1.RuneConfigIdList.Add(3);
            exclusive1.SuitConfigIdList.Add(10017);
            exclusive1.SuitConfigIdList.Add(10018);
            exclusive1.SuitConfigIdList.Add(10018);
            exclusive1.LevelDict[10033] = 99;
            exclusive1.Count = 1;
            list.Add(exclusive1);

            //爆裂
            ExclusiveItem exclusive2 = new ExclusiveItem(2, 12, 10021, 5, 2);
            exclusive2.RuneConfigIdList.Add(12);
            exclusive2.RuneConfigIdList.Add(12);
            exclusive2.RuneConfigIdList.Add(3);
            exclusive2.SuitConfigIdList.Add(10021);
            exclusive2.SuitConfigIdList.Add(10022);
            exclusive2.SuitConfigIdList.Add(10022);
            exclusive2.LevelDict[10035] = 99;
            exclusive2.Count = 1;
            list.Add(exclusive2);

            //冰咆哮
            ExclusiveItem exclusive3 = new ExclusiveItem(3, 5, 6, 5, 2);
            exclusive3.RuneConfigIdList.Add(5);
            exclusive3.RuneConfigIdList.Add(5);
            exclusive3.RuneConfigIdList.Add(18);
            exclusive3.SuitConfigIdList.Add(6);
            exclusive3.SuitConfigIdList.Add(11);
            exclusive3.SuitConfigIdList.Add(11);
            exclusive3.LevelDict[10036] = 99;
            exclusive3.Count = 1;
            list.Add(exclusive3);

            //瞬移
            ExclusiveItem exclusive4 = new ExclusiveItem(4, 22, 14, 5, 2);
            exclusive4.RuneConfigIdList.Add(22);
            exclusive4.RuneConfigIdList.Add(22);
            exclusive4.RuneConfigIdList.Add(3);
            exclusive4.SuitConfigIdList.Add(14);
            exclusive4.SuitConfigIdList.Add(8);
            exclusive4.SuitConfigIdList.Add(8);
            exclusive4.LevelDict[10037] = 99;
            exclusive4.Count = 1;
            list.Add(exclusive4);

            //盾
            ExclusiveItem exclusive5 = new ExclusiveItem(5, 15, 10023, 5, 2);
            exclusive5.RuneConfigIdList.Add(15);
            exclusive5.RuneConfigIdList.Add(15);
            exclusive5.RuneConfigIdList.Add(18);
            exclusive5.SuitConfigIdList.Add(10023);
            exclusive5.SuitConfigIdList.Add(10024);
            exclusive5.SuitConfigIdList.Add(10024);
            exclusive5.LevelDict[10046] = 99;
            exclusive5.Count = 1;
            list.Add(exclusive5);

            //精通
            ExclusiveItem exclusive6 = new ExclusiveItem(6, 5, 8, 5, 2);
            exclusive6.RuneConfigIdList.Add(5);
            exclusive6.RuneConfigIdList.Add(23);
            exclusive6.RuneConfigIdList.Add(23);
            exclusive6.SuitConfigIdList.Add(8);
            exclusive6.SuitConfigIdList.Add(15);
            exclusive6.SuitConfigIdList.Add(15);
            exclusive6.LevelDict[10047] = 99;
            exclusive6.Count = 1;
            list.Add(exclusive6);

            return list;
        }

        private List<ExclusiveItem> AddExclusiveDaoshi()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //火符
            //ExclusiveItem exclusive1 = new ExclusiveItem(1, 20, 10031, 5, 2);
            //exclusive1.RuneConfigIdList.Add(20);
            //exclusive1.RuneConfigIdList.Add(20);
            //exclusive1.RuneConfigIdList.Add(20);
            //exclusive1.SuitConfigIdList.Add(10031);
            //exclusive1.SuitConfigIdList.Add(10032);
            //exclusive1.SuitConfigIdList.Add(10032);
            //exclusive1.LevelDict[24] = 99;
            //exclusive1.Count = 1;
            //list.Add(exclusive1);

            ////盾
            //ExclusiveItem exclusive2 = new ExclusiveItem(2, 16, 10037, 5, 2);
            //exclusive2.RuneConfigIdList.Add(16);
            //exclusive2.RuneConfigIdList.Add(16);
            //exclusive2.RuneConfigIdList.Add(16);
            //exclusive2.SuitConfigIdList.Add(10037);
            //exclusive2.SuitConfigIdList.Add(10038);
            //exclusive2.SuitConfigIdList.Add(10038);
            //exclusive2.LevelDict[25] = 99;
            //exclusive2.Count = 1;
            //list.Add(exclusive2);

            ////精通
            //ExclusiveItem exclusive3 = new ExclusiveItem(3, 20, 4, 5, 1);
            //exclusive3.RuneConfigIdList.Add(20);
            //exclusive3.RuneConfigIdList.Add(20);
            //exclusive3.RuneConfigIdList.Add(20);
            //exclusive3.SuitConfigIdList.Add(4);
            //exclusive3.SuitConfigIdList.Add(19);
            //exclusive3.SuitConfigIdList.Add(19);
            //exclusive3.LevelDict[29] = 99;
            //exclusive3.Count = 1;
            //list.Add(exclusive3);

            ////隐身
            //ExclusiveItem exclusive4 = new ExclusiveItem(4, 23, 15, 5, 1);
            //exclusive4.RuneConfigIdList.Add(23);
            //exclusive4.RuneConfigIdList.Add(23);
            //exclusive4.RuneConfigIdList.Add(23);
            //exclusive4.SuitConfigIdList.Add(15);
            //exclusive4.SuitConfigIdList.Add(15);
            //exclusive4.SuitConfigIdList.Add(15);
            //exclusive4.LevelDict[10056] = 99;
            //exclusive4.Count = 1;
            //list.Add(exclusive4);

            ////月灵
            //ExclusiveItem exclusive5 = new ExclusiveItem(5, 28, 18, 5, 2);
            //exclusive5.RuneConfigIdList.Add(28);
            //exclusive5.RuneConfigIdList.Add(9);
            //exclusive5.RuneConfigIdList.Add(9);
            //exclusive5.SuitConfigIdList.Add(18);
            //exclusive5.SuitConfigIdList.Add(10);
            //exclusive5.SuitConfigIdList.Add(10);
            //exclusive5.LevelDict[10063] = 99;
            //exclusive5.Count = 1;
            //list.Add(exclusive5);

            ////治疗
            //ExclusiveItem exclusive6 = new ExclusiveItem(6, 5, 10029, 5, 2);
            //exclusive6.RuneConfigIdList.Add(5);
            //exclusive6.RuneConfigIdList.Add(23);
            //exclusive6.RuneConfigIdList.Add(23);
            //exclusive6.SuitConfigIdList.Add(10029);
            //exclusive6.SuitConfigIdList.Add(10030);
            //exclusive6.SuitConfigIdList.Add(10030);
            //exclusive6.LevelDict[10064] = 99;
            //exclusive6.Count = 1;
            //list.Add(exclusive6);

            return list;
        }

        private List<ExclusiveItem> AddExclusiveHaimian()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //火符
            //ExclusiveItem exclusive1 = new ExclusiveItem(1, 20, 10031, 5, 2);
            //exclusive1.RuneConfigIdList.Add(20);
            //exclusive1.RuneConfigIdList.Add(20);
            //exclusive1.RuneConfigIdList.Add(10056);
            //exclusive1.SuitConfigIdList.Add(10031);
            //exclusive1.SuitConfigIdList.Add(10032);
            //exclusive1.SuitConfigIdList.Add(10032);
            ////exclusive1.LevelDict[24] = 99;
            //exclusive1.Count = 1;
            //list.Add(exclusive1);



            ////精通
            //ExclusiveItem exclusive3 = new ExclusiveItem(2, 5, 6, 5, 1);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.RuneConfigIdList.Add(18);
            //exclusive3.SuitConfigIdList.Add(6);
            //exclusive3.SuitConfigIdList.Add(11);
            //exclusive3.SuitConfigIdList.Add(11);
            ////exclusive3.LevelDict[29] = 99;
            //exclusive3.Count = 1;
            //list.Add(exclusive3);

            ////隐身
            //ExclusiveItem exclusive4 = new ExclusiveItem(3, 23, 15, 5, 1);
            //exclusive4.RuneConfigIdList.Add(23);
            //exclusive4.RuneConfigIdList.Add(28);
            //exclusive4.RuneConfigIdList.Add(28);
            //exclusive4.SuitConfigIdList.Add(15);
            //exclusive4.SuitConfigIdList.Add(18);
            //exclusive4.SuitConfigIdList.Add(18);
            ////exclusive4.LevelDict[10056] = 99;
            //exclusive4.Count = 1;
            //list.Add(exclusive4);


            ////月灵
            //ExclusiveItem exclusive5 = new ExclusiveItem(4, 10064, 9, 5, 2);
            //exclusive5.RuneConfigIdList.Add(10064);
            //exclusive5.RuneConfigIdList.Add(23);
            //exclusive5.RuneConfigIdList.Add(23);
            //exclusive5.SuitConfigIdList.Add(9);
            //exclusive5.SuitConfigIdList.Add(4);
            //exclusive5.SuitConfigIdList.Add(4);
            ////exclusive5.LevelDict[10063] = 99;
            //exclusive5.Count = 1;
            //list.Add(exclusive5);

            ////盾
            //ExclusiveItem exclusive2 = new ExclusiveItem(6, 16, 10037, 5, 2);
            //exclusive2.RuneConfigIdList.Add(16);
            //exclusive2.RuneConfigIdList.Add(16);
            //exclusive2.RuneConfigIdList.Add(16);
            //exclusive2.SuitConfigIdList.Add(10037);
            //exclusive2.SuitConfigIdList.Add(10038);
            //exclusive2.SuitConfigIdList.Add(10038);
            ////exclusive2.LevelDict[25] = 99;
            //exclusive2.Count = 1;
            //list.Add(exclusive2);

            //治疗
            //ExclusiveItem exclusive4 = new ExclusiveItem(5, 10055, 10029, 5, 2);
            //exclusive4.RuneConfigIdList.Add(10055);
            //exclusive4.RuneConfigIdList.Add(10055);
            //exclusive4.RuneConfigIdList.Add(10055);
            //exclusive4.SuitConfigIdList.Add(10029);
            //exclusive4.SuitConfigIdList.Add(3);
            //exclusive4.SuitConfigIdList.Add(3);
            ////exclusive4.LevelDict[10049] = 99;
            //exclusive4.Count = 1;
            //list.Add(exclusive4);




            return list;
        }

        private List<ExclusiveItem> AddExclusive()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //刺杀
            //ExclusiveItem exclusive1 = new ExclusiveItem(1, 9, 10, 5, 1);
            //exclusive1.RuneConfigIdList.Add(10010);
            //exclusive1.RuneConfigIdList.Add(10010);
            //exclusive1.SuitConfigIdList.Add(10003);
            //exclusive1.SuitConfigIdList.Add(10003);
            //exclusive1.Count = 1;
            //list.Add(exclusive1);

            ////刺杀
            //ExclusiveItem exclusive2 = new ExclusiveItem(2, 9, 10, 5, 1);
            //exclusive2.RuneConfigIdList.Add(10010);
            //exclusive2.RuneConfigIdList.Add(10010);
            //exclusive2.SuitConfigIdList.Add(10004);
            //exclusive2.SuitConfigIdList.Add(10004);
            //exclusive2.Count = 1;
            //list.Add(exclusive2);

            ////冰咆哮
            //ExclusiveItem exclusive3 = new ExclusiveItem(3, 5, 6, 5, 1);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.SuitConfigIdList.Add(6);
            //exclusive3.SuitConfigIdList.Add(11);
            //exclusive3.Count = 1;
            //list.Add(exclusive3);

            ////冰咆哮+半月
            //ExclusiveItem exclusive4 = new ExclusiveItem(4, 18, 11, 5, 1);
            //exclusive4.RuneConfigIdList.Add(10015);
            //exclusive4.RuneConfigIdList.Add(10015);
            //exclusive4.SuitConfigIdList.Add(10005);
            //exclusive4.SuitConfigIdList.Add(10005);
            //exclusive4.Count = 1;
            //list.Add(exclusive4);

            ////半月+ 烈火
            //ExclusiveItem exclusive5 = new ExclusiveItem(5, 10027, 10013, 5, 1);
            //exclusive5.RuneConfigIdList.Add(10015);
            //exclusive5.RuneConfigIdList.Add(10015);
            //exclusive5.SuitConfigIdList.Add(10006);
            //exclusive5.SuitConfigIdList.Add(10006);
            //exclusive5.Count = 1;
            //list.Add(exclusive5);

            //烈火
            ExclusiveItem exclusive6 = new ExclusiveItem(6, 19, 10013, 5, 1);
            exclusive6.RuneConfigIdList.Add(19);
            exclusive6.RuneConfigIdList.Add(19);
            exclusive6.SuitConfigIdList.Add(10014);
            exclusive6.SuitConfigIdList.Add(10014);
            exclusive6.Count = 1;
            list.Add(exclusive6);

            return list;
        }

        private void DoRedEquip(Equip equip1)
        {
            List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            //AttrEntryList1.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(24, 50));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(24, 50));

            AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(19, 50));


            equip1.AttrEntryList = AttrEntryList1;
        }

        private List<Equip> AddRedEquip()
        {
            //定制红
            List<Equip> list = new List<Equip>();


            //武器 品质1，幸运5,护体
            Equip equip1 = new Equip(21105801, 21, 13, 6);
            DoRedEquip(equip1);
            list.Add(equip1);

            //项链  品质1，幸运5，护体
            Equip equip2 = new Equip(21105802, 21, 13, 6);
            List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            DoRedEquip(equip2);
            list.Add(equip2);

            //衣服 护体
            Equip equip3 = new Equip(21105803, 21, 13, 6);
            DoRedEquip(equip3);
            list.Add(equip3);

            //头盔 护体
            Equip equip4 = new Equip(21105804, 21, 13, 6);
            DoRedEquip(equip4);
            list.Add(equip4);

            //手镯 武力精通
            Equip equip5 = new Equip(21105805, 10022, 7, 6);
            DoRedEquip(equip5);
            list.Add(equip5);

            //手镯 武力精通
            Equip equip6 = new Equip(21105805, 10022, 7, 6);
            DoRedEquip(equip6);
            list.Add(equip6);


            //戒指 武力盾
            Equip equip7 = new Equip(21105807, 14, 10009, 6);
            DoRedEquip(equip7);
            list.Add(equip7);

            //戒指 武力盾
            Equip equip8 = new Equip(21105807, 14, 10009, 6);
            DoRedEquip(equip8);
            list.Add(equip8);


            //腰带 武力盾
            Equip equip9 = new Equip(21105809, 14, 10010, 6);
            DoRedEquip(equip9);
            list.Add(equip9);

            //鞋子 武力盾
            Equip equip10 = new Equip(21105810, 10021, 10010, 6);
            DoRedEquip(equip10);
            list.Add(equip10);

            return list;
        }

        private List<Equip> AddRedEquip1()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //武器 倍率，幸运4,爆裂
            Equip equip1 = new Equip(23105801, 20, 4, 6);
            DoRedEquip(equip1);
            list.Add(equip1);

            //项链  倍率，幸运4，爆裂
            Equip equip2 = new Equip(23105803, 20, 4, 6);
            DoRedEquip(equip2);
            list.Add(equip2);

            //衣服 爆裂
            Equip equip3 = new Equip(23105802, 16, 10037, 6);
            DoRedEquip(equip3);
            list.Add(equip3);

            //头盔 爆裂
            Equip equip4 = new Equip(23105804, 16, 10037, 6);
            DoRedEquip(equip4);
            list.Add(equip4);

            //手镯 法力精通
            Equip equip5 = new Equip(23105805, 10064, 9, 6);
            DoRedEquip(equip5);
            list.Add(equip5);

            //手镯 法力精通
            Equip equip6 = new Equip(23105805, 10064, 9, 6);
            DoRedEquip(equip6);
            list.Add(equip6);


            //戒指 魔法盾
            Equip equip7 = new Equip(23105807, 20, 15, 6);
            DoRedEquip(equip7);
            list.Add(equip7);

            //戒指 魔法盾
            Equip equip8 = new Equip(23105807, 20, 15, 6);
            DoRedEquip(equip8);
            list.Add(equip8);


            //腰带 魔法盾
            Equip equip9 = new Equip(23105809, 28, 18, 6);
            DoRedEquip(equip9);
            list.Add(equip9);

            //鞋子 魔法盾
            Equip equip10 = new Equip(23105810, 28, 18, 6);
            DoRedEquip(equip10);
            list.Add(equip10);

            return list;
        }

        private List<Equip> AddRedEquipDaoshi()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //武器 倍率，幸运4,爆裂
            Equip equip1 = new Equip(23105801, 20, 10031, 6);
            List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            equip1.AttrEntryList = AttrEntryList1;
            equip1.Layer = 1;
            list.Add(equip1);

            //项链  倍率，幸运4，爆裂
            Equip equip2 = new Equip(23105803, 20, 10031, 6);
            List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            equip2.AttrEntryList = AttrEntryList2;
            equip2.Layer = 1;
            list.Add(equip2);

            //衣服 爆裂
            Equip equip3 = new Equip(23105802, 20, 10031, 6);
            List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            equip3.AttrEntryList = AttrEntryList3;
            equip3.Layer = 1;
            list.Add(equip3);

            //头盔 爆裂
            Equip equip4 = new Equip(23105804, 24, 10032, 6);
            List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            equip4.AttrEntryList = AttrEntryList4;
            equip4.Layer = 1;
            list.Add(equip4);

            //手镯 法力精通
            Equip equip5 = new Equip(23105805, 24, 10032, 6);
            List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            equip5.AttrEntryList = AttrEntryList5;
            equip5.Layer = 1;
            list.Add(equip5);

            //手镯 法力精通
            Equip equip6 = new Equip(23105805, 24, 10032, 6);
            List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            equip6.AttrEntryList = AttrEntryList6;
            equip6.Layer = 1;
            list.Add(equip6);


            //戒指 魔法盾
            Equip equip7 = new Equip(23105807, 23, 15, 6);
            List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            equip7.AttrEntryList = AttrEntryList7;
            equip7.Layer = 1;
            list.Add(equip7);

            //戒指 魔法盾
            Equip equip8 = new Equip(23105807, 23, 15, 6);
            List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            equip8.AttrEntryList = AttrEntryList8;
            equip8.Layer = 1;
            list.Add(equip8);


            //腰带 魔法盾
            Equip equip9 = new Equip(23105809, 28, 18, 6);
            List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            equip9.AttrEntryList = AttrEntryList9;
            equip9.Layer = 1;
            list.Add(equip9);

            //鞋子 魔法盾
            Equip equip10 = new Equip(23105810, 28, 18, 6);
            List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            equip10.AttrEntryList = AttrEntryList10;
            equip10.Layer = 1;
            list.Add(equip10);

            return list;
        }

        private List<Equip> AddRedEquipDaoshi1()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //武器 倍率，幸运4,爆裂
            Equip equip1 = new Equip(23105801, 20, 4, 6);
            List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            equip1.AttrEntryList = AttrEntryList1;
            equip1.Layer = 1;
            list.Add(equip1);

            //项链  倍率，幸运4，爆裂
            Equip equip2 = new Equip(23105802, 20, 4, 6);
            List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            equip2.AttrEntryList = AttrEntryList2;
            equip2.Layer = 1;
            list.Add(equip2);

            //衣服 爆裂
            Equip equip3 = new Equip(23105803, 16, 10037, 6);
            List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            equip3.AttrEntryList = AttrEntryList3;
            equip3.Layer = 1;
            list.Add(equip3);

            //头盔 爆裂
            Equip equip4 = new Equip(23105804, 16, 10037, 6);
            List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            equip4.AttrEntryList = AttrEntryList4;
            equip4.Layer = 1;
            list.Add(equip4);

            //手镯 法力精通
            Equip equip5 = new Equip(23105805, 10064, 9, 6);
            List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            equip5.AttrEntryList = AttrEntryList5;
            equip5.Layer = 1;
            list.Add(equip5);

            //手镯 法力精通
            Equip equip6 = new Equip(23105805, 10064, 9, 6);
            List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            equip6.AttrEntryList = AttrEntryList6;
            equip6.Layer = 1;
            list.Add(equip6);


            //戒指 魔法盾
            Equip equip7 = new Equip(23105807, 23, 15, 6);
            List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            equip7.AttrEntryList = AttrEntryList7;
            equip7.Layer = 1;
            list.Add(equip7);

            //戒指 魔法盾
            Equip equip8 = new Equip(23105807, 23, 15, 6);
            List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            equip8.AttrEntryList = AttrEntryList8;
            equip8.Layer = 1;
            list.Add(equip8);


            //腰带 魔法盾
            Equip equip9 = new Equip(23105809, 28, 18, 6);
            List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            equip9.AttrEntryList = AttrEntryList9;
            equip9.Layer = 1;
            list.Add(equip9);

            //鞋子 魔法盾
            Equip equip10 = new Equip(23105810, 28, 18, 6);
            List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            equip10.AttrEntryList = AttrEntryList10;
            equip10.Layer = 1;
            list.Add(equip10);

            return list;
        }

        private List<Equip> AddRedEquipFashi()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //武器 倍率，幸运4,爆裂
            Equip equip1 = new Equip(22105801, 5, 6, 6);
            List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            AttrEntryList1.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList1.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            equip1.AttrEntryList = AttrEntryList1;
            equip1.Layer = 1;
            list.Add(equip1);

            //项链  倍率，幸运4，爆裂
            Equip equip2 = new Equip(22105802, 5, 6, 6);
            List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            AttrEntryList2.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList2.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            equip2.AttrEntryList = AttrEntryList2;
            equip2.Layer = 1;
            list.Add(equip2);

            //衣服 爆裂
            Equip equip3 = new Equip(22105803, 5, 11, 6);
            List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
            AttrEntryList3.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList3.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            equip3.AttrEntryList = AttrEntryList3;
            equip3.Layer = 1;
            list.Add(equip3);

            //头盔 爆裂
            Equip equip4 = new Equip(22105804, 5, 11, 6);
            List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
            AttrEntryList4.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList4.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            equip4.AttrEntryList = AttrEntryList4;
            equip4.Layer = 1;
            list.Add(equip4);

            //手镯 法力精通
            Equip equip5 = new Equip(22105805, 22, 14, 6);
            List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
            AttrEntryList5.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList5.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            equip5.AttrEntryList = AttrEntryList5;
            equip5.Layer = 1;
            list.Add(equip5);

            //手镯 法力精通
            Equip equip6 = new Equip(22105805, 22, 14, 6);
            List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
            AttrEntryList6.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList6.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            equip6.AttrEntryList = AttrEntryList6;
            equip6.Layer = 1;
            list.Add(equip6);


            //戒指 魔法盾
            Equip equip7 = new Equip(22105807, 22, 14, 6);
            List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
            AttrEntryList7.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList7.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            equip7.AttrEntryList = AttrEntryList7;
            equip7.Layer = 1;
            list.Add(equip7);

            //戒指 魔法盾
            Equip equip8 = new Equip(22105807, 22, 14, 6);
            List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
            AttrEntryList8.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList8.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            equip8.AttrEntryList = AttrEntryList8;
            equip8.Layer = 1;
            list.Add(equip8);


            //腰带 魔法盾
            Equip equip9 = new Equip(22105809, 12, 10018, 6);
            List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
            AttrEntryList9.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList9.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            equip9.AttrEntryList = AttrEntryList9;
            equip9.Layer = 1;
            list.Add(equip9);

            //鞋子 魔法盾
            Equip equip10 = new Equip(22105810, 12, 10018, 6);
            List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
            AttrEntryList10.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList10.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            equip10.AttrEntryList = AttrEntryList10;
            equip10.Layer = 1;
            list.Add(equip10);

            return list;
        }

        private List<ExclusiveItem> AddExclusive1()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            for (int i = 1; i <= 3; i++)
            {
                ExclusiveItem exclusive1 = new ExclusiveItem(i, 3, 10021, 5, 1);
                exclusive1.Count = 1;
                list.Add(exclusive1);
            }

            for (int i = 4; i <= 6; i++)
            {
                ExclusiveItem exclusive1 = new ExclusiveItem(i, 3, 10022, 5, 1);
                exclusive1.Count = 1;
                list.Add(exclusive1);
            }

            ////雷电
            //ExclusiveItem exclusive1 = new ExclusiveItem(1, 8, 10017, 5, 1);
            //exclusive1.RuneConfigIdList.Add(8);
            //exclusive1.RuneConfigIdList.Add(8);
            //exclusive1.SuitConfigIdList.Add(10017);
            //exclusive1.SuitConfigIdList.Add(10017);
            //exclusive1.Count = 1;
            //list.Add(exclusive1);

            ////雷电
            //ExclusiveItem exclusive2 = new ExclusiveItem(2, 12, 10018, 5, 1);
            //exclusive2.RuneConfigIdList.Add(12);
            //exclusive2.RuneConfigIdList.Add(12);
            //exclusive2.SuitConfigIdList.Add(10018);
            //exclusive2.SuitConfigIdList.Add(10018);
            //exclusive2.Count = 1;
            //list.Add(exclusive2);

            //冰咆哮
            //ExclusiveItem exclusive3 = new ExclusiveItem(3, 5, 6, 5, 1);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.RuneConfigIdList.Add(5);
            //exclusive3.SuitConfigIdList.Add(6);
            //exclusive3.SuitConfigIdList.Add(10027);
            //exclusive3.Count = 1;
            //list.Add(exclusive3);

            //冰咆哮+隐身
            //ExclusiveItem exclusive4 = new ExclusiveItem(4, 18, 11, 5, 2);
            //exclusive4.RuneConfigIdList.Add(18);
            //exclusive4.RuneConfigIdList.Add(18);
            //exclusive4.SuitConfigIdList.Add(11);
            //exclusive4.SuitConfigIdList.Add(10027);
            //exclusive4.Count = 1;
            //list.Add(exclusive4);

            ////隐身+ 瞬移
            //ExclusiveItem exclusive5 = new ExclusiveItem(5, 23, 15, 5, 2);
            //exclusive5.RuneConfigIdList.Add(23);
            //exclusive5.RuneConfigIdList.Add(22);
            //exclusive5.SuitConfigIdList.Add(15);
            //exclusive5.SuitConfigIdList.Add(14);
            //exclusive5.Count = 1;
            //list.Add(exclusive5);

            ////烈火
            //ExclusiveItem exclusive6 = new ExclusiveItem(6, 22, 14, 5, 2);
            //exclusive6.RuneConfigIdList.Add(22);
            //exclusive6.RuneConfigIdList.Add(22);
            //exclusive6.SuitConfigIdList.Add(14);
            //exclusive6.SuitConfigIdList.Add(14);
            //exclusive6.Count = 1;
            //list.Add(exclusive6);

            return list;
        }

        private List<ExclusiveItem> AddExclusive2()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //刺杀
            ExclusiveItem exclusive1 = new ExclusiveItem(1, 10010, 10, 5, 1);
            exclusive1.Count = 1;
            list.Add(exclusive1);

            //刺杀
            ExclusiveItem exclusive2 = new ExclusiveItem(2, 10010, 10, 5, 1);
            exclusive2.Count = 1;
            list.Add(exclusive2);

            ////刺杀
            ExclusiveItem exclusive3 = new ExclusiveItem(3, 10010, 10, 5, 1);
            exclusive3.Count = 1;
            list.Add(exclusive3);

            //盾
            ExclusiveItem exclusive4 = new ExclusiveItem(4, 14, 10009, 5, 2);
            exclusive4.Count = 1;
            list.Add(exclusive4);

            //盾
            ExclusiveItem exclusive5 = new ExclusiveItem(5, 14, 10009, 5, 2);
            exclusive5.Count = 1;
            list.Add(exclusive5);

            //盾
            ExclusiveItem exclusive6 = new ExclusiveItem(6, 14, 10009, 5, 2);
            exclusive6.Count = 1;
            list.Add(exclusive6);

            return list;
        }

        private void Test(User user)
        {
            for (int count = 1; count <= 10; count++)
            {
                if (user.InfiniteData.DropList.Count > 0)
                {
                    user.InfiniteData.DropList.RemoveAt(0);
                }
                user.InfiniteData.GetDropId(1);

                List<int> dropList = user.InfiniteData.DropList[0];

                for (int i = 1; i < dropList.Count; i++)
                {
                    if (dropList[i - 1] == 4002)
                    {
                        //Debug.Log(count + "次 - " + i + "层 掉落魂骨");
                    }
                    else if (dropList[i - 1] >= 180001 && dropList[i - 1] <= 180101)
                    {
                        Debug.Log(count + "次 - " + i + "层 掉落法宝");
                    }
                }
            }
        }
    }

    public enum OfflineType
    {
        Tower = 1,
        Andian = 2,
    }
}
