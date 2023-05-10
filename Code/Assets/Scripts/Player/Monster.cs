using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Monster : APlayer
    {
        public int MonsterId;
        MonsterBase Config { get; set; }
        QualityConfig QualityConfig { get; set; }

        public int Quality { get; set; }

        public int GoldRate;
        public long Gold;
        public int AttTyp;
        public float Att;
        public float Def;
        public long Exp;
        public int range;

        public Monster(int monsterId, int quality) : base()
        {
            this.MonsterId = monsterId;
            this.GroupId = 2;
            this.Quality = quality;

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Config = MonsterBaseCategory.Instance.Get(MonsterId);
            this.QualityConfig = QualityConfigCategory.Instance.Get(Quality);

            this.Camp = PlayerType.Enemy;
            this.Name = Config.Name + QualityConfig.MonsterTitle;
            this.Level = Config.Level;
            this.Exp = Config.Exp * QualityConfig.ExpRate;
            this.Gold = Config.Gold * QualityConfig.GoldRate;


            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Config.HP * QualityConfig.HpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Config.PhyAttr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Config.PhyAttr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Config.PhyAttr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Config.Def * QualityConfig.DefRate);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill() { 
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Monster :" + this.ToString() + " dead");

            User user = GameProcessor.Inst.User;

            long exp = this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100;

            //增加经验,金币

            user.Exp += exp;
            user.Gold += this.Gold;
            user.EventCenter.Raise(new HeroInfoUpdateEvent());
            if (user.Exp >= user.UpExp)
            {
                user.EventCenter.Raise(new HeroChangeEvent
                {
                    Type = User.UserChangeType.LevelUp
                });
            }

            //生成道具奖励
            int mapLevel = user.Level / 10 * 10;
            List<KeyValuePair<int, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(mapLevel);
            List<Item> items = DropHelper.BuildDropItem(dropList, Quality);

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, items)
            });

            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });
            }

            //存档
            UserData.Save();
        }
    }
}
