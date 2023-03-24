using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Monster : APlayer
    {
        public int MonsterId;
        public int GoldRate;
        public long Gold;
        public int AttTyp;
        public float Att;
        public float Def;
        public long Exp;
        public int range;

        public Monster() : base()
        {
            this.GroupId = 2;
        }

        public override void Load()
        {
            base.Load();

            //var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/MonsterBox");
            //var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            //box.SetParent(this.Transform);

            this.Camp = PlayerType.Enemy;
            this.Level = Random.Range(1, 4);

            this.AttributeBonus = new AttributeBonus();

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        public void SetLevelConfigAttr(MonsterBase config)
        {
            //TODO 测试增加被动属性
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, 1);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, 0);

            this.MonsterId = config.Id;
            this.Gold = config.Gold;
            this.Exp = config.Exp;
            this.Name = config.Name;
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Monster :" + this.ToString() + " dead");

            Hero hero = GameProcessor.Inst.PlayerManager.GetHero();

            long exp = this.Exp * (100 + hero.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100;

            //增加经验,金币

            hero.Exp += exp;
            hero.Gold += this.Gold;
            hero.EventCenter.Raise(new HeroInfoUpdateEvent());
            if (hero.Exp >= hero.UpExp)
            {
                hero.EventCenter.Raise(new HeroChangeEvent
                {
                    Type = Hero.HeroChangeType.LevelUp
                });
            }

            //生成道具奖励
            int mapLevel = hero.Level / 10 * 10;
            List<KeyValuePair<int, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(mapLevel);
            List<Item> items = DropHelper.BuildDropItem(dropList);

            if (items.Count > 0)
            {
                hero.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                RoundNum = hero.RoundCounter,
                MonsterId = this.MonsterId,
                Exp = exp,
                Gold = this.Gold,
                Drops = items
            });

            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() {});
            }

            //存档
            UserData.Save();
        }
    }
}
