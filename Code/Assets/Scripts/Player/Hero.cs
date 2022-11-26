using UnityEngine;
using ET;
using System.Collections.Generic;

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

            this.Camp = PlayerType.Hero;

            //���ø�������ֵ
    
            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 15);

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //���ؼ���
            if (SkillIdList == null)
            {
                SkillIdList = new Dictionary<int, int>();
            }
            if (SkillIdList.Count == 0) {
                SkillIdList.Add(1, 1001);  //��������
                SkillIdList.Add(2, 2001);  //����
                SkillIdList.Add(3, 3001);  //�����
            }

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

        private void SetLevelConfigAttr()
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
                SetLevelConfigAttr();
                //�����ָ���Ѫ��
                SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
            }
        }

        public List<SkillData> GetSelectSkills() {


            return null;
        }
        public enum HeroChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }

}
