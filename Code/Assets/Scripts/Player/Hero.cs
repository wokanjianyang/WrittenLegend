using UnityEngine;
using ET;
namespace Game
{
    public class Hero : APlayer
    {
        public int Level { get; set; }

        public long Exp { get; set; }

        public long UpExp { get; set; }

        public long Gold { get; set; }

        public long Essence { get; set; }

        public int MapId { get; set; }

        public int LastCityId { get; set; }

        public long Power { get; set; }

        public long LastOut { get; set; }
        public override void Load()
        {
            base.Load();
            
            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            var box = GameObject.Instantiate(boxPrefab).transform;
            box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform);
            
            this.Camp = PlayerType.Hero;
            this.Level = 1;
            this.Exp = 0;

            //设置各种属性值
            this.AttributeBonus = new AttributeBonus();

            AttributeBonus.SetAttr(AttributeEnum.HP, 1, 1000);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, 1, 40);
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, 1, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, 1, 5);

            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);

            SetHp(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void HeroChange(HeroChangeEvent e) {
            switch (e.Type){
                case HeroChangeType.LevelUp:
                    LevelUp();
                    break;


            }

        }

        private void LevelUp() {
            if (this.Exp >= this.UpExp) {
                Exp =  Exp - UpExp;
                Level++;

                //Add Base Attr
                LevelConfig config = LevelConfigCategory.Instance.Get(Level);
                AttributeBonus.SetAttr(AttributeEnum.HP, 1, config.Hp);
                AttributeBonus.SetAttr(AttributeEnum.PhyAtt, 1, config.PhyAtt);
                AttributeBonus.SetAttr(AttributeEnum.Def, 1, config.Def);

                SetHp(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

                UpExp = config.Exp;

                EventCenter.Raise(new SetPlayerLevelEvent
                {
                    Level = Level.ToString()
                });
            }
        }

 
        public enum HeroChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }
    
}
