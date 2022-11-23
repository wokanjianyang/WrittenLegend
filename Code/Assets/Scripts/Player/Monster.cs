using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ET;

namespace Game
{
    public class Monster : APlayer
    {
        public long Id;
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
            var box = GameObject.Instantiate(boxPrefab).transform;
            box.SetParent(this.Transform);

            this.Camp = PlayerType.Enemy;
            this.Level = Random.Range(1, 3) == 1 ? 1 : 4;

            this.AttributeBonus = new AttributeBonus();

            int rd = Random.Range(1, 4);
            SetLevelConfigAttr();

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void SetLevelConfigAttr()
        {
            MonsterBase config = MonsterBaseCategory.Instance.Get(this.Level);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAttr);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

            this.Gold = config.Gold;
            this.Exp = config.Exp;
            this.Name = config.Name;
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            Log.Info("Monster ID:" + dead.ToId + " dead");

            //增加经验,金币
            Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.Exp += this.Exp;
            hero.Gold += this.Gold;

            if (hero.Exp >= hero.UpExp)
            {
                hero.EventCenter.Raise(new HeroChangeEvent
                {
                    Type = Hero.HeroChangeType.LevelUp
                });
            }

            //生成装备

        }
    }
}
