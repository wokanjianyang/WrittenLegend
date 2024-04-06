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

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Copy_Ticket, 3600));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 450));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Wing_Stone, 300));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, 1000));
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Heart, 100));
          

            //items.Add(ItemHelper.BuildItem(ItemType.Card, 2000010, 10, 5));

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 13, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 14, 1, 1));

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
    }
}
