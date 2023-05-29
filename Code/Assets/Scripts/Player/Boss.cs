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
        public int Quality { get;  }

        public int GoldRate;
        public long Gold;
        public float Att;
        public float Def;
        public long Exp;

        public Boss(int bossId,int mapId) : base()
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


            this.SetAttr();  //��������ֵ
            this.SetSkill(); //���ü���

            base.Load();
            this.Logic.SetData(null); //����UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, Config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, Config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, Config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, Config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, Config.Def);

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill() {
            //���ؼ���
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //����Ĭ�ϼ���

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

            //���Ӿ���,���

            user.Exp += exp;
            user.Gold += this.Gold;
            user.EventCenter.Raise(new HeroInfoUpdateEvent());
            if (user.Exp >= user.UpExp)
            {
                user.EventCenter.Raise(new HeroChangeEvent
                {
                    Type = UserChangeType.LevelUp
                });
            }

            //���ɵ��߽���
            int mapLevel = user.Level / 10 * 10;
            List<KeyValuePair<int, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(mapLevel);
            List<Item> items = DropHelper.BuildDropItem(dropList,0);

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildBossDeadMessage(this, items)
            });

            //�Զ�����
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });
            }

            //��¼��ɱʱ��
            user.MapBossTime[MapId] = TimeHelper.ClientNowSeconds();

            //�浵
            UserData.Save();
        }
    }
}