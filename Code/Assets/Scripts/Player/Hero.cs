using UnityEngine;
using ET;
namespace Game
{
    public class Hero : APlayer
    {
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

            //jia

            this.Camp = PlayerType.Hero;
            this.Level = 1;
            this.Exp = 0;

            //���ø�������ֵ
            this.AttributeBonus = new AttributeBonus();
            SetLevelConfigAttr(1);
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case HeroChangeType.LevelUp:
                    LevelUp();
                    break;
            }
        }

        private void SetLevelConfigAttr(int level)
        {
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);
            UpExp = config.Exp;
        }

        private void LevelUp()
        {
            if (this.Exp >= this.UpExp)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetLevelConfigAttr(Level);
                //�����ָ���Ѫ��
                SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
            }
        }

        public enum HeroChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }

}
