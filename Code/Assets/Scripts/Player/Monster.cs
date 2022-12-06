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

        public override void Load()
        {
            base.Load();

            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/MonsterBox");
            var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            box.SetParent(this.Transform);

            this.Camp = PlayerType.Enemy;
            this.Level = Random.Range(1, 4) ;

            this.AttributeBonus = new AttributeBonus();

            int rd = Random.Range(1, 4);
            SetLevelConfigAttr();

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void SetLevelConfigAttr()
        {
            MonsterBase config = MonsterBaseCategory.Instance.Get(1000+this.Level);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

            this.MonsterId = config.Id;
            this.Gold = config.Gold;
            this.Exp = config.Exp;
            this.Name = config.Name;
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Monster :" + this.ToString() + " dead");

            //���Ӿ���,���
            Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.Exp += this.Exp;
            hero.Gold += this.Gold;
            hero.EventCenter.Raise(new HeroInfoUpdateEvent());
            if (hero.Exp >= hero.UpExp)
            {
                hero.EventCenter.Raise(new HeroChangeEvent
                {
                    Type = Hero.HeroChangeType.LevelUp
                });
            }

            //����װ��
            DropConfig drop = DropConfigCategory.Instance.GetByMonsterId(this.MonsterId);
            List<Equip> equips = EquipHelper.DropEquip(drop);

            if (equips.Count > 0)
            {
                hero.AddToBags(equips);

                foreach (Equip equip in equips) {
                    Debug.Log("drop equip :" + JsonConvert.SerializeObject(equip));
                }
                hero.EventCenter.Raise(new HeroBagUpdateEvent());
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                MonsterId = this.MonsterId,
                Exp = this.Exp,
                Gold = this.Gold,
                Drops = equips
            });
            //�浵
            UserData.Save(hero);
        }
    }
}
