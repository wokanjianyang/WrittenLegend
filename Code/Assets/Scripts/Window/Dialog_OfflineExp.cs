﻿using Sirenix.OdinInspector;
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

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Legacy_Ticket, 2000)); //传世卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Copy_Ticket, 100000)); //装备卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 5000)); //BOSS卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Wing_Stone, 1200));  //羽毛
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Pill_Ticket, 100));  //幻境劵

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, 1000)); //专属碎片
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Heart, 100)); //专属之心
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 1999999999)); //四格碎片

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 999999999)); //精炼石
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Red_Stone, 999)); //红装精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Golden_Stone, 999)); //红装精华

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecailEquipRefreshId, 99999)); //橙装精华

            //items.Add(ItemHelper.BuildItem(ItemType.Card, 2000010, 10, 5));

            //items.Add(ItemHelper.BuildItem(ItemType.SkillBox, 1010, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.SkillBox, 3010, 1, 1));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Pill, 6000000));

            //user.SaveArtifactLevel(180001, 1); //boss杀手
            //user.SaveArtifactLevel(180005, 10); //卖身契
            //user.SaveArtifactLevel(180007, 1); //破限
            //user.SaveArtifactLevel(180009, 1); //圣者
            //user.SaveArtifactLevel(180019, 1); //副本杀手
            //user.SaveArtifactLevel(180030, 45); //极。卖身契

            //user.Record.AddRecord(RecordType.AdReal, -800);
            //user.MagicGold.Data = 100000000000000000L;//10京金币

            //user.SaveItemMeterialCount(1999994, 100000);
            //user.SaveItemMeterialCount(ItemHelper.SpecialId_Card_Stone, 10000000);
            //user.Cycle.Data = 2;
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 13, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 18, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 19, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 20, 1, 1));

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 26, 1, 1));  //神技

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 28, 1, 6));  //魂骨
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 29, 1, 3));  //11技能

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 106, 1, 2));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 107, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 108, 1, 1));

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 109, 1, 5));  //战士经验
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 110, 1, 1)); //法师经验
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 111, 1, 1)); //道士经验

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 112, 1, 10));  //战士输出
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 113, 1, 1)); //法师输出
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 114, 1, 1)); //道士输出

            //items.AddRange(AddRedEquipDaoshi());
            //items.AddRange(AddExclusive1());

            foreach (var item in items)
            {
                BoxItem boxItem = new BoxItem();
                boxItem.Item = item;
                boxItem.MagicNubmer.Data = Math.Max(1, item.Count);
                boxItem.BoxId = -1;
                user.Bags.Add(boxItem);
            }
        }

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

        public void ShowOffline()
        {
            User user = GameProcessor.Inst.User;

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

            if (user.OffLineMapId > 0)
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
                if (item.Type == ItemType.Card || item.Type == ItemType.Fashion || (item.Type == ItemType.Material && item.ConfigId == ItemHelper.SpecialId_Card_Stone))
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
                user.BabelCount.Data = ConfigHelper.BabelCount;

                user.DataDate = DateTime.Now.Ticks;
                //保存到Tap
            }

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

        private List<Item> BuildOfflineAndian(User user, long offlineTime, ref long rewardExp, ref long rewardGold, ref string message)
        {
            MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

            List<Item> itemList = new List<Item>();

            long killCount = (long)(offlineTime * 2.5);
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

            message += "\n离线未知暗殿(" + mapConfig.Name + ")，击杀了" + killCount + "个怪物，获得";
            long burstMul = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            killCount = killCount * (100 + burstMul) / 100;

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
                itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Moon_Cake, skillBox));
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
            long count = mineTime / 60;

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

        private List<Equip> AddRedEquip()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //武器 品质1，幸运5,护体
            //Equip equip1 = new Equip(21105801, 21, 13, 6);
            //List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            //AttrEntryList1.Add(new KeyValuePair<int, long>(2001, 3));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            //AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            //equip1.AttrEntryList = AttrEntryList1;
            //equip1.Layer = 2;
            //list.Add(equip1);

            ////项链  品质1，幸运5，护体
            //Equip equip2 = new Equip(21105803, 21, 13, 6);
            //List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            //AttrEntryList2.Add(new KeyValuePair<int, long>(2001, 3));
            //AttrEntryList2.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            //AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            //AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            //AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            //equip2.AttrEntryList = AttrEntryList2;
            //equip2.Layer = 2;
            //list.Add(equip2);

            ////衣服 护体
            //Equip equip3 = new Equip(21105802, 21, 13, 6);
            //List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
            //AttrEntryList3.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList3.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList3.Add(new KeyValuePair<int, long>(2011, 1));
            //AttrEntryList3.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList3.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList3.Add(new KeyValuePair<int, long>(2001, 3));
            //equip3.AttrEntryList = AttrEntryList3;
            //equip3.Layer = 2;
            //list.Add(equip3);

            ////头盔 护体
            //Equip equip4 = new Equip(21105804, 21, 13, 6);
            //List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
            //AttrEntryList4.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList4.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList4.Add(new KeyValuePair<int, long>(2011, 1));
            //AttrEntryList4.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList4.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList4.Add(new KeyValuePair<int, long>(2001, 3));
            //equip4.AttrEntryList = AttrEntryList4;
            //equip4.Layer = 2;
            //list.Add(equip4);

            ////手镯 武力精通
            //Equip equip5 = new Equip(21105805, 10022, 7, 6);
            //List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
            //AttrEntryList5.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList5.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList5.Add(new KeyValuePair<int, long>(2011, 1));
            //AttrEntryList5.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList5.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList5.Add(new KeyValuePair<int, long>(2001, 3));
            //equip5.AttrEntryList = AttrEntryList5;
            //equip5.Layer = 2;
            //list.Add(equip5);

            ////手镯 武力精通
            //Equip equip6 = new Equip(21105805, 10022, 7, 6);
            //List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
            //AttrEntryList6.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList6.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList6.Add(new KeyValuePair<int, long>(2011, 1));
            //AttrEntryList6.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList6.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList6.Add(new KeyValuePair<int, long>(2001, 3));
            //equip6.AttrEntryList = AttrEntryList6;
            //equip6.Layer = 2;
            //list.Add(equip6);


            ////戒指 武力盾
            //Equip equip7 = new Equip(21105807, 14, 10009, 6);
            //List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
            //AttrEntryList7.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList7.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList7.Add(new KeyValuePair<int, long>(2002, 3));
            //AttrEntryList7.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList7.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList7.Add(new KeyValuePair<int, long>(2001, 3));
            //equip7.AttrEntryList = AttrEntryList7;
            //equip7.Layer = 2;
            //list.Add(equip7);

            ////戒指 武力盾
            //Equip equip8 = new Equip(21105807, 14, 10009, 6);
            //List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
            //AttrEntryList8.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList8.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList8.Add(new KeyValuePair<int, long>(2002, 3));
            //AttrEntryList8.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList8.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList8.Add(new KeyValuePair<int, long>(2001, 3));
            //equip8.AttrEntryList = AttrEntryList8;
            //equip8.Layer = 2;
            //list.Add(equip8);


            ////腰带 武力盾
            //Equip equip9 = new Equip(21105809, 14, 10010, 6);
            //List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
            //AttrEntryList9.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList9.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList9.Add(new KeyValuePair<int, long>(2002, 3));
            //AttrEntryList9.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList9.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList9.Add(new KeyValuePair<int, long>(2001, 3));
            //equip9.AttrEntryList = AttrEntryList9;
            //equip9.Layer = 2;
            //list.Add(equip9);

            ////鞋子 武力盾
            //Equip equip10 = new Equip(21105810, 10021, 10010, 6);
            //List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
            //AttrEntryList10.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList10.Add(new KeyValuePair<int, long>(33, 10));
            //AttrEntryList10.Add(new KeyValuePair<int, long>(2011, 1));
            //AttrEntryList10.Add(new KeyValuePair<int, long>(2004, 3));
            //AttrEntryList10.Add(new KeyValuePair<int, long>(2003, 3));
            //AttrEntryList10.Add(new KeyValuePair<int, long>(2001, 3));
            //equip10.AttrEntryList = AttrEntryList10;
            //equip10.Layer = 2;
            //list.Add(equip10);

            return list;
        }

        private List<Equip> AddRedEquip1()
        {
            //定制红
            List<Equip> list = new List<Equip>();

            //武器 倍率，幸运4,爆裂
            Equip equip1 = new Equip(23105801, 10047, 10021, 6);
            List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList1.Add(new KeyValuePair<int, long>(18, 50));
            equip1.AttrEntryList = AttrEntryList1;
            list.Add(equip1);

            //项链  倍率，幸运4，爆裂
            Equip equip2 = new Equip(23105803, 3, 10021, 6);
            List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList2.Add(new KeyValuePair<int, long>(18, 50));
            equip2.AttrEntryList = AttrEntryList2;
            list.Add(equip2);

            //衣服 爆裂
            Equip equip3 = new Equip(23105802, 3, 10022, 6);
            List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList3.Add(new KeyValuePair<int, long>(18, 50));
            equip3.AttrEntryList = AttrEntryList3;
            list.Add(equip3);

            //头盔 爆裂
            Equip equip4 = new Equip(23105804, 3, 10022, 6);
            List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList4.Add(new KeyValuePair<int, long>(18, 50));
            equip4.AttrEntryList = AttrEntryList4;
            list.Add(equip4);

            //手镯 法力精通
            Equip equip5 = new Equip(23105805, 10049, 8, 6);
            List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList5.Add(new KeyValuePair<int, long>(18, 50));
            equip5.AttrEntryList = AttrEntryList5;
            list.Add(equip5);

            //手镯 法力精通
            Equip equip6 = new Equip(23105805, 10049, 8, 6);
            List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList6.Add(new KeyValuePair<int, long>(18, 50));
            equip6.AttrEntryList = AttrEntryList6;
            list.Add(equip6);


            //戒指 魔法盾
            Equip equip7 = new Equip(23105807, 15, 10023, 6);
            List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList7.Add(new KeyValuePair<int, long>(18, 50));
            equip7.AttrEntryList = AttrEntryList7;
            list.Add(equip7);

            //戒指 魔法盾
            Equip equip8 = new Equip(23105807, 15, 10023, 6);
            List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList8.Add(new KeyValuePair<int, long>(18, 50));
            equip8.AttrEntryList = AttrEntryList8;
            list.Add(equip8);


            //腰带 魔法盾
            Equip equip9 = new Equip(23105809, 15, 10024, 6);
            List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList9.Add(new KeyValuePair<int, long>(18, 50));
            equip9.AttrEntryList = AttrEntryList9;
            list.Add(equip9);

            //鞋子 魔法盾
            Equip equip10 = new Equip(23105810, 10048, 10024, 6);
            List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            AttrEntryList10.Add(new KeyValuePair<int, long>(18, 50));
            equip10.AttrEntryList = AttrEntryList10;
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

        //private List<Equip> AddRedEquip1()
        //{
        //    //定制红
        //    List<Equip> list = new List<Equip>();

        //    //武器 倍率，幸运4,爆裂
        //    Equip equip1 = new Equip(22105801, 10047, 10021, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList1.Add(new KeyValuePair<int, long>(2001, 3));
        //    AttrEntryList1.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
        //    AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
        //    AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
        //    AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
        //    equip1.AttrEntryList = AttrEntryList1;
        //    equip1.Layer = 2;
        //    list.Add(equip1);

        //    //项链  倍率，幸运4，爆裂
        //    Equip equip2 = new Equip(22105803, 3, 10021, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList2.Add(new KeyValuePair<int, long>(2001, 3));
        //    AttrEntryList2.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
        //    AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
        //    AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
        //    AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
        //    equip2.AttrEntryList = AttrEntryList2;
        //    equip2.Layer = 2;
        //    list.Add(equip2);

        //    //衣服 爆裂
        //    Equip equip3 = new Equip(22105802, 3, 10022, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList3.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList3.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList3.Add(new KeyValuePair<int, long>(2011, 1));
        //    AttrEntryList3.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList3.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList3.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip3.AttrEntryList = AttrEntryList3;
        //    equip3.Layer = 2;
        //    list.Add(equip3);

        //    //头盔 爆裂
        //    Equip equip4 = new Equip(22105804, 3, 10022, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList4.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList4.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList4.Add(new KeyValuePair<int, long>(2011, 1));
        //    AttrEntryList4.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList4.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList4.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip4.AttrEntryList = AttrEntryList4;
        //    equip4.Layer = 2;
        //    list.Add(equip4);

        //    //手镯 法力精通
        //    Equip equip5 = new Equip(22105805, 10049, 8, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList5.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList5.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList5.Add(new KeyValuePair<int, long>(2011, 1));
        //    AttrEntryList5.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList5.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList5.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip5.AttrEntryList = AttrEntryList5;
        //    equip5.Layer = 2;
        //    list.Add(equip5);

        //    //手镯 法力精通
        //    Equip equip6 = new Equip(22105805, 10049, 8, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList6.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList6.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList6.Add(new KeyValuePair<int, long>(2011, 1));
        //    AttrEntryList6.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList6.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList6.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip6.AttrEntryList = AttrEntryList6;
        //    equip6.Layer = 2;
        //    list.Add(equip6);


        //    //戒指 魔法盾
        //    Equip equip7 = new Equip(22105807, 15, 10023, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList7.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList7.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList7.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList7.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList7.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList7.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip7.AttrEntryList = AttrEntryList7;
        //    equip7.Layer = 2;
        //    list.Add(equip7);

        //    //戒指 魔法盾
        //    Equip equip8 = new Equip(22105807, 15, 10023, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList8.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList8.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList8.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList8.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList8.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList8.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip8.AttrEntryList = AttrEntryList8;
        //    equip8.Layer = 2;
        //    list.Add(equip8);


        //    //腰带 魔法盾
        //    Equip equip9 = new Equip(22105809, 15, 10024, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList9.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList9.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList9.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList9.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList9.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList9.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip9.AttrEntryList = AttrEntryList9;
        //    equip9.Layer = 2;
        //    list.Add(equip9);

        //    //鞋子 魔法盾
        //    Equip equip10 = new Equip(22105810, 10048, 10024, 6);
        //    List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
        //    AttrEntryList10.Add(new KeyValuePair<int, long>(34, 10));
        //    AttrEntryList10.Add(new KeyValuePair<int, long>(2002, 3));
        //    AttrEntryList10.Add(new KeyValuePair<int, long>(2011, 1));
        //    AttrEntryList10.Add(new KeyValuePair<int, long>(2005, 3));
        //    AttrEntryList10.Add(new KeyValuePair<int, long>(2003, 3));
        //    AttrEntryList10.Add(new KeyValuePair<int, long>(2001, 3));
        //    equip10.AttrEntryList = AttrEntryList10;
        //    equip10.Layer = 2;
        //    list.Add(equip10);

        //    return list;
        //}

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

        private void Test()
        {
            for (int i = 1; i <= 501; i++)
            {
                EquipRefineConfig oldConfig = EquipRefineConfigCategory.Instance.GetByLevel(i);
            }

            //Debug.Log(" Test Over ");
        }
    }

    public enum OfflineType
    {
        Tower = 1,
        Andian = 2,
    }
}
