using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ET;

namespace Game
{
    public class Monster : APlayer
    {
        public long Id;
        public int Level;
        public int GoldRate;
        public long Gold;
        public int AttTyp;
        public float Att;
        public float Def;
        public long Hp;
        public long Exp;
        public int range;

        public override void Load()
        {
            base.Load();
            
            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/MonsterBox");
            var box = GameObject.Instantiate(boxPrefab).transform;
            box.SetParent(this.Transform);
            
            this.Camp = PlayerType.Enemy;
            this.Level = 1;

            MonsterBase config = MonsterBaseCategory.Instance.Get(this.Level);

            this.Gold = config.Gold;
            this.Exp = config.Exp;
            this.Name = config.Name;

            this.AttributeBonus = new AttributeBonus();

            AttributeBonus.SetAttr(AttributeEnum.HP, 1, 100);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, 1, 1);
            AttributeBonus.SetAttr(AttributeEnum.Def, 1, 1);
         
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Monster ID:" + dead.ToId + " dead");

            //增加经验,金币
            Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.Exp += this.Exp;
            hero.Gold += this.Gold;

            if (hero.Exp >= hero.UpExp) {
                hero.EventCenter.Raise(new HeroChangeEvent{ 
                    Type = Hero.HeroChangeType.LevelUp
                });
            }

            //生成装备

        }
    }
}
