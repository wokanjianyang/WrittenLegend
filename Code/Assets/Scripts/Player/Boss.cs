using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Boss : APlayer
    {
        public int BossId;
        public int MapId;
        BossConfig Config { get; set; }
        public int Quality { get; }

        public int GoldRate;
        public long Gold;
        public float Att;
        public float Def;
        public long Exp;

        public Boss(int bossId, int mapId) : base()
        {
            this.BossId = bossId;
            this.MapId = mapId;
            this.GroupId = 2;
            this.Quality = 4;

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Config = BossConfigCategory.Instance.Get(BossId);

            this.Camp = PlayerType.Enemy;
            this.ModelType = MondelType.Boss;
            this.Name = Config.Name;
            this.Level = Config.Level;
            this.Exp = Config.Exp;
            this.Gold = Config.Gold;


            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Config.Def);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Boss :" + this.ToString() + " dead");

            User user = GameProcessor.Inst.User;

            long exp = this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100;
            long gold = this.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100;

            //增加经验,金币
            user.AddExpAndGold(exp, gold);

            //地图道具奖励
            List<KeyValuePair<int, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(user.MapId, 10) ;
  
            //自身掉落
            if (Config.DropIdList != null && Config.DropIdList.Length > 0)
            {
                for (int i = 0; i < Config.DropIdList.Length; i++)
                {
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(Config.DropIdList[i]);

                    dropList.Add(new KeyValuePair<int, DropConfig>(Config.DropRateList[i], dropConfig));
                }
            }

            List<Item> items = DropHelper.BuildDropItem(dropList, 4);

            if (SystemConfigHelper.CheckRequireLevel(SystemEnum.SoulRing))
            {
                items.Add(ItemHelper.BuildSoulRingShard(Level));
            }

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildBossDeadMessage(this, items)
            });


            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });
            }

            //记录击杀时间
            user.MapBossTime[MapId] = TimeHelper.ClientNowSeconds();

            //存档
            //UserData.Save();
        }
    }
}
