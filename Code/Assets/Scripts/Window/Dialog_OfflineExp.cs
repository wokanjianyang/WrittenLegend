using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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


        public void ShowOffline()
        {

            //0AF588B5A9 Self 905A621CD2 gs
            //BBEFBA0DDF RS
            //C8A92C5388 豆浆
            //7B97AC4A45 搅拌
            //Debug.Log("sc:" + CodeConfigCategory.Instance.BuildSpecicalCode("io&zkd153", "C8A92C5388"));
            //Debug.Log("sc:" + CodeConfigCategory.Instance.BuildSpecicalCode("!xyfubent050", "0AF588B5A9"));

            User user = GameProcessor.Inst.User;

            long currentTick = TimeHelper.ClientNowSeconds();
            long offlineTime = currentTick - user.SecondExpTick;

            long offlineFloor = 0;
            long rewardExp = 0;
            long rewardGold = 0;
            long tmepFloor = user.MagicTowerFloor.Data;

            long tempTime = Math.Min(offlineTime, ConfigHelper.MaxOfflineTime);
            long mineTime = tempTime;

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

            List<Item> items = new List<Item>();
            for (int i = 0; i < offlineFloor; i++)
            {
                long fl = user.MagicTowerFloor.Data + i;

                int equipLevel = Math.Max(10, (user.MapId - ConfigHelper.MapStartId) * 10);

                items.AddRange(DropHelper.TowerEquip(fl, equipLevel));
            }

            //items.Add(new Equip(22005701, 23, 15, 5));
            //items.Add(new Equip(22005702, 23, 15, 5));
            //items.Add(new Equip(22005703, 23, 15, 5));
            //items.Add(new Equip(22005704, 23, 15, 5));
            //items.Add(new Equip(22005705, 16, 10037, 5));
            //items.Add(new Equip(22005705, 16, 10037, 5));
            //items.Add(new Equip(22005707, 16, 10037, 5));
            //items.Add(new Equip(22005707, 16, 10037, 5));
            //items.Add(new Equip(22005709, 23, 15, 5));
            //items.Add(new Equip(22005710, 23, 15, 5));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Copy_Ticket, 125000));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 12500));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Wing_Stone, 999));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, 1000));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Heart, 100));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 999999999));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Red_Stone, 87));

            //items.Add(ItemHelper.BuildItem(ItemType.Card, 2000010, 10, 5));

            //user.SaveArtifactLevel(180005, 10);

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 13, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 14, 1, 1));

            //items.AddRange(AddRedEquip1());
            //items.AddRange(AddExclusive1());

            long newFloor = user.MagicTowerFloor.Data + offlineFloor;

            user.MagicTowerFloor.Data = Math.Min(newFloor, ConfigHelper.Max_Floor);

            long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp) * (offlineTime / 5) + rewardExp;
            long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold) * (offlineTime / 5) + rewardGold;

            user.AddExpAndGold(exp, gold);
            user.SecondExpTick = currentTick;

            foreach (var item in items)
            {
                BoxItem boxItem = new BoxItem();
                boxItem.Item = item;
                boxItem.MagicNubmer.Data = Math.Max(1, item.Count);
                boxItem.BoxId = -1;
                user.Bags.Add(boxItem);
            }

            string OfflineMessage = BattleMsgHelper.BuildOfflineMessage(offlineTime, offlineFloor, exp, gold, items.Count);
            //Debug.Log(OfflineMessage);

            //检查
            DateTime saveDate = new DateTime(user.DataDate);
            if (saveDate.Day < DateTime.Now.Day || saveDate.Month < DateTime.Now.Month || saveDate.Year < DateTime.Now.Year)
            {
                user.DefendData.Refresh();
                user.HeroPhatomData.Refresh();

                user.DataDate = DateTime.Now.Ticks;
                //保存到Tap
            }

            //miner
            Dictionary<int, long> offlineMetal = new Dictionary<int, long>();
            foreach (var miner in user.MinerList)
            {
                miner.OfflineBuild(mineTime, offlineMetal);
            }

            var sortedDict = offlineMetal.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


            OfflineMessage += $"\n";
            foreach (var kp in sortedDict)
            {
                var md = user.MetalData;
                int key = kp.Key;
                if (!md.ContainsKey(key))
                {
                    md[key] = new Game.Data.MagicData();
                }

                md[key].Data += kp.Value;

                MetalConfig metalConfig = MetalConfigCategory.Instance.Get(key);

                OfflineMessage += $"<color=#{QualityConfigHelper.GetQualityColor(metalConfig.Quality)}>[{metalConfig.Name}]</color>" + kp.Value + "个";
            }

            UserData.Save();

            this.gameObject.SetActive(true);

            this.Txt_Msg.text = OfflineMessage;

            //Time.timeScale = 0;
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
            Equip equip1 = new Equip(22105801, 10047, 10021, 6);
            List<KeyValuePair<int, long>> AttrEntryList1 = new List<KeyValuePair<int, long>>();
            AttrEntryList1.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList1.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            AttrEntryList1.Add(new KeyValuePair<int, long>(7, 8));
            equip1.AttrEntryList = AttrEntryList1;
            equip1.Layer = 2;
            list.Add(equip1);

            //项链  倍率，幸运4，爆裂
            Equip equip2 = new Equip(22105803, 3, 10021, 6);
            List<KeyValuePair<int, long>> AttrEntryList2 = new List<KeyValuePair<int, long>>();
            AttrEntryList2.Add(new KeyValuePair<int, long>(2001, 3));
            AttrEntryList2.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            AttrEntryList2.Add(new KeyValuePair<int, long>(7, 8));
            equip2.AttrEntryList = AttrEntryList2;
            equip2.Layer = 2;
            list.Add(equip2);

            //衣服 爆裂
            Equip equip3 = new Equip(22105802, 3, 10022, 6);
            List<KeyValuePair<int, long>> AttrEntryList3 = new List<KeyValuePair<int, long>>();
            AttrEntryList3.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList3.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList3.Add(new KeyValuePair<int, long>(2011, 1));
            AttrEntryList3.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList3.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList3.Add(new KeyValuePair<int, long>(2001, 3));
            equip3.AttrEntryList = AttrEntryList3;
            equip3.Layer = 2;
            list.Add(equip3);

            //头盔 爆裂
            Equip equip4 = new Equip(22105804, 3, 10022, 6);
            List<KeyValuePair<int, long>> AttrEntryList4 = new List<KeyValuePair<int, long>>();
            AttrEntryList4.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList4.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList4.Add(new KeyValuePair<int, long>(2011, 1));
            AttrEntryList4.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList4.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList4.Add(new KeyValuePair<int, long>(2001, 3));
            equip4.AttrEntryList = AttrEntryList4;
            equip4.Layer = 2;
            list.Add(equip4);

            //手镯 法力精通
            Equip equip5 = new Equip(22105805, 10049, 8, 6);
            List<KeyValuePair<int, long>> AttrEntryList5 = new List<KeyValuePair<int, long>>();
            AttrEntryList5.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList5.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList5.Add(new KeyValuePair<int, long>(2011, 1));
            AttrEntryList5.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList5.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList5.Add(new KeyValuePair<int, long>(2001, 3));
            equip5.AttrEntryList = AttrEntryList5;
            equip5.Layer = 2;
            list.Add(equip5);

            //手镯 法力精通
            Equip equip6 = new Equip(22105805, 10049, 8, 6);
            List<KeyValuePair<int, long>> AttrEntryList6 = new List<KeyValuePair<int, long>>();
            AttrEntryList6.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList6.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList6.Add(new KeyValuePair<int, long>(2011, 1));
            AttrEntryList6.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList6.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList6.Add(new KeyValuePair<int, long>(2001, 3));
            equip6.AttrEntryList = AttrEntryList6;
            equip6.Layer = 2;
            list.Add(equip6);


            //戒指 魔法盾
            Equip equip7 = new Equip(22105807, 15, 10023, 6);
            List<KeyValuePair<int, long>> AttrEntryList7 = new List<KeyValuePair<int, long>>();
            AttrEntryList7.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList7.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList7.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList7.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList7.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList7.Add(new KeyValuePair<int, long>(2001, 3));
            equip7.AttrEntryList = AttrEntryList7;
            equip7.Layer = 2;
            list.Add(equip7);

            //戒指 魔法盾
            Equip equip8 = new Equip(22105807, 15, 10023, 6);
            List<KeyValuePair<int, long>> AttrEntryList8 = new List<KeyValuePair<int, long>>();
            AttrEntryList8.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList8.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList8.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList8.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList8.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList8.Add(new KeyValuePair<int, long>(2001, 3));
            equip8.AttrEntryList = AttrEntryList8;
            equip8.Layer = 2;
            list.Add(equip8);


            //腰带 魔法盾
            Equip equip9 = new Equip(22105809, 15, 10024, 6);
            List<KeyValuePair<int, long>> AttrEntryList9 = new List<KeyValuePair<int, long>>();
            AttrEntryList9.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList9.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList9.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList9.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList9.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList9.Add(new KeyValuePair<int, long>(2001, 3));
            equip9.AttrEntryList = AttrEntryList9;
            equip9.Layer = 2;
            list.Add(equip9);

            //鞋子 魔法盾
            Equip equip10 = new Equip(22105810, 10048, 10024, 6);
            List<KeyValuePair<int, long>> AttrEntryList10 = new List<KeyValuePair<int, long>>();
            AttrEntryList10.Add(new KeyValuePair<int, long>(34, 10));
            AttrEntryList10.Add(new KeyValuePair<int, long>(2002, 3));
            AttrEntryList10.Add(new KeyValuePair<int, long>(2011, 1));
            AttrEntryList10.Add(new KeyValuePair<int, long>(2005, 3));
            AttrEntryList10.Add(new KeyValuePair<int, long>(2003, 3));
            AttrEntryList10.Add(new KeyValuePair<int, long>(2001, 3));
            equip10.AttrEntryList = AttrEntryList10;
            equip10.Layer = 2;
            list.Add(equip10);

            return list;
        }

        private List<ExclusiveItem> AddExclusive1()
        {
            //定制红
            List<ExclusiveItem> list = new List<ExclusiveItem>();

            //雷电
            ExclusiveItem exclusive1 = new ExclusiveItem(1, 8, 10017, 5, 1);
            exclusive1.RuneConfigIdList.Add(8);
            exclusive1.RuneConfigIdList.Add(8);
            exclusive1.SuitConfigIdList.Add(10017);
            exclusive1.SuitConfigIdList.Add(10017);
            exclusive1.Count = 1;
            list.Add(exclusive1);

            //雷电
            ExclusiveItem exclusive2 = new ExclusiveItem(2, 12, 10018, 5, 1);
            exclusive2.RuneConfigIdList.Add(12);
            exclusive2.RuneConfigIdList.Add(12);
            exclusive2.SuitConfigIdList.Add(10018);
            exclusive2.SuitConfigIdList.Add(10018);
            exclusive2.Count = 1;
            list.Add(exclusive2);

            //冰咆哮
            ExclusiveItem exclusive3 = new ExclusiveItem(3, 5, 6, 5, 1);
            exclusive3.RuneConfigIdList.Add(5);
            exclusive3.RuneConfigIdList.Add(5);
            exclusive3.SuitConfigIdList.Add(6);
            exclusive3.SuitConfigIdList.Add(11);
            exclusive3.Count = 1;
            list.Add(exclusive3);

            //冰咆哮+隐身
            ExclusiveItem exclusive4 = new ExclusiveItem(4, 18, 11, 5, 3);
            exclusive4.RuneConfigIdList.Add(10015);
            exclusive4.RuneConfigIdList.Add(10015);
            exclusive4.SuitConfigIdList.Add(10005);
            exclusive4.SuitConfigIdList.Add(10005);
            exclusive4.Count = 1;
            list.Add(exclusive4);

            //隐身+ 瞬移
            ExclusiveItem exclusive5 = new ExclusiveItem(5, 23, 15, 5, 3);
            exclusive5.RuneConfigIdList.Add(23);
            exclusive5.RuneConfigIdList.Add(22);
            exclusive5.SuitConfigIdList.Add(15);
            exclusive5.SuitConfigIdList.Add(14);
            exclusive5.Count = 1;
            list.Add(exclusive5);

            //烈火
            ExclusiveItem exclusive6 = new ExclusiveItem(6, 22, 14, 5, 3);
            exclusive6.RuneConfigIdList.Add(22);
            exclusive6.RuneConfigIdList.Add(22);
            exclusive6.SuitConfigIdList.Add(14);
            exclusive6.SuitConfigIdList.Add(14);
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

            Debug.Log(" Test Over ");
        }
    }
}
