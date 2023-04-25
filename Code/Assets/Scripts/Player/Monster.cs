using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Monster : APlayer
    {
        public int MonsterId;

        public int Quality { get; set; }

        public int GoldRate;
        public long Gold;
        public int AttTyp;
        public float Att;
        public float Def;
        public long Exp;
        public int range;

        public Monster(int monsterId,int quality) : base()
        {
            this.MonsterId = monsterId;
            this.GroupId = 2;
            this.Quality = quality;

            this.Load();
            this.SetLevelConfigAttr();
            this.Logic.SetData(null);
        }

        public override void Load()
        {
            base.Load();

            //var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/MonsterBox");
            //var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            //box.SetParent(this.Transform);

            this.Camp = PlayerType.Enemy;


            MonsterBase config = MonsterBaseCategory.Instance.Get(MonsterId);

            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(Quality);

            this.Name = config.Name + qualityConfig.MonsterTitle;
            this.Level = config.Level;
            this.Exp = config.Exp * qualityConfig.ExpRate;
            this.Gold = config.Gold * qualityConfig.GoldRate;


            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP* qualityConfig.HpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAttr* qualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAttr * qualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAttr * qualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def * qualityConfig.DefRate);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        public void SetLevelConfigAttr()
        {
        }



        virtual protected void MakeReward(DeadRewarddEvent dead)
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
            List<Item> items = DropHelper.BuildDropItem(dropList,Quality);

            if (items.Count > 0)
            {
                hero.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, items)
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
