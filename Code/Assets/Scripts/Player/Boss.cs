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
        MonsterModelConfig ModelConfig { get; set; }

        public int GoldRate;
        public long Gold;
        public float Att;
        public float Def;
        public long Exp;

        private int CopyType = 1;
        private int RewarCount = 1;

        public Boss(int bossId, int mapId, int copyType, int rewarCount, int modelId) : base()
        {
            this.BossId = bossId;
            this.MapId = mapId;
            this.GroupId = 2;
            this.Quality = 5;

            this.CopyType = copyType;
            this.RewarCount = rewarCount;

            this.Config = BossConfigCategory.Instance.Get(BossId);
            if (modelId > 0)
            {
                ModelConfig = MonsterModelConfigCategory.Instance.Get(modelId);
            }

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.ModelType = MondelType.Boss;

            string modelName = ModelConfig?.Name + "��";

            this.Name = modelName + Config.Name;
            this.Level = (Config.MapId - 999) * 100;
            this.Exp = Config.Exp;
            this.Gold = Config.Gold;


            this.SetAttr();  //��������ֵ
            this.SetSkill(); //���ü���

            base.Load();
            this.Logic.SetData(null); //����UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hpModelRate = ModelConfig == null ? 1 : ModelConfig.HpRate;
            double attrModelRate = ModelConfig == null ? 1 : ModelConfig.AttrRate;
            double defModelRate = ModelConfig == null ? 1 : ModelConfig.DefRate;

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (long)(Config.HP * hpModelRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (long)(Config.Def * defModelRate));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //���ؼ���
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
                        list.Add(new SkillData(model.SkillList[i], i)); //����Ĭ�ϼ���
                    }
                }

                this.Name = model.Name + "��" + Config.Name;
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //����Ĭ�ϼ���

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
            //Log.Info("Boss :" + this.ToString() + " dead");

            for (int i = 0; i < RewarCount; i++)
            {
                BuildReword();
            }
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            double rewardModelRate = ModelConfig == null ? 1 : ModelConfig.RewardRate;
            double dropModelRate = ModelConfig == null ? 1 : ModelConfig.DropRate;

            long exp = (long)(this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100 * rewardModelRate);
            long gold = (long)(this.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100 * rewardModelRate);

            //���Ӿ���,���
            user.AddExpAndGold(exp, gold);

            //��ͼ���߽���
            List<KeyValuePair<int, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, 10 * dropModelRate);

            //�������
            if (Config.DropIdList != null && Config.DropIdList.Length > 0)
            {
                for (int i = 0; i < Config.DropIdList.Length; i++)
                {
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(Config.DropIdList[i]);

                    dropList.Add(new KeyValuePair<int, DropConfig>(Config.DropRateList[i], dropConfig));
                }
            }

            //��ʱ����
            dropList.AddRange(DropLimitHelper.Build(10 * dropModelRate));

            int qualityRate = 250 * (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;

            List<Item> items = DropHelper.BuildDropItem(dropList, 250);

            int mapIndex = Config.MapId - ConfigHelper.MapStartId;
            int quantity = mapIndex / 10 + 1 + user.SoulRingNumber;

            items.Add(ItemHelper.BuildSoulRingShard(quantity * 2));

            //����BOSS֮����Ʊ
            //if (CopyType == 1)
            //{
            //    if (RandomHelper.RandomNumber(1, 25) <= 1)
            //    {
            //        items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 1));
            //    }
            //}

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildBossDeadMessage(this, exp, gold, items)
            });


            //�Զ�����
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });
            }
        }
    }
}
