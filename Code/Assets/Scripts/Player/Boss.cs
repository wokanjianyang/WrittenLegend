using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    public class Boss : APlayer
    {
        public int BossId;
        public int MapId;
        BossConfig Config { get; set; }

        public int GoldRate;
        public long Gold;
        public float Att;
        public float Def;
        public long Exp;

        private int CopyType = 1;
        private int Rate = 1;

        public Boss(int bossId, int mapId, int copyType, int rate) : base()
        {
            this.BossId = bossId;
            this.MapId = mapId;
            this.GroupId = 2;
            this.Quality = 5;

            this.CopyType = copyType;
            this.Rate = rate;

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Config = BossConfigCategory.Instance.Get(BossId);

            this.Camp = PlayerType.Enemy;
            this.ModelType = MondelType.Boss;
            this.Name = Config.Name;
            this.Level = (Config.MapId - 999) * 100;
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

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();

            if (Config.ModelType == 0)
            {
                //random model
                List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).ToList();
                int index = RandomHelper.RandomNumber(0, models.Count);
                PlayerModel model = models[index];

                if (model.SkillList != null)
                {
                    for (int i = 0; i < model.SkillList.Length; i++)
                    {
                        list.Add(new SkillData(model.SkillList[i], i)); //增加默认技能
                    }
                }

                this.Name = model.Name + "·" + Config.Name;
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Boss :" + this.ToString() + " dead");

            for (int i = 0; i < Rate; i++)
            {
                BuildReword();
            }

            //存档
            //UserData.Save();
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            long exp = this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100;
            long gold = this.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100;

            //增加经验,金币
            user.AddExpAndGold(exp, gold);

            //地图道具奖励
            List<KeyValuePair<int, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, 10);

            //自身掉落
            if (Config.DropIdList != null && Config.DropIdList.Length > 0)
            {
                for (int i = 0; i < Config.DropIdList.Length; i++)
                {
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(Config.DropIdList[i]);

                    dropList.Add(new KeyValuePair<int, DropConfig>(Config.DropRateList[i], dropConfig));
                }
            }

            int qualityRate = 250 * (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;

            List<Item> items = DropHelper.BuildDropItem(dropList, 250);


            int mapIndex = Config.MapId - ConfigHelper.MapStartId;
            int quantity = mapIndex / 10 + 1 + user.SoulRingNumber;

            items.Add(ItemHelper.BuildSoulRingShard(quantity * 2));

            //限时奖励
            items.AddRange(DropLimitHelper.RandomItem(1));

            //掉落BOSS之家门票
            if (CopyType == 1)
            {
                if (RandomHelper.RandomNumber(1, 25) <= 1)
                {
                    items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 1));
                }
            }

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildBossDeadMessage(this, exp, gold, items)
            });


            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });
            }
        }
    }
}
