using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

        public IDictionary<int, Equip> EquipPanel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Item> Bags { get; set; }

        public long LastOut { get; set; }

        private bool isInLevelUp;

        public Hero():base()  {
            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
            this.EventCenter.AddListener<HeroUnUseEquipEvent>(HeroUnUseEquip);
        }

        public override void Load()
        {
            base.Load();

            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform); 
        }

        public void Init() {
            this.Camp = PlayerType.Hero;

            //设置各种属性值
            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 15);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //加载技能
            if (SkillIdList == null)
            {
                SkillIdList = new Dictionary<int, int>();
            }
            if (SkillIdList.Count == 0)
            {
                SkillIdList.Add(1, 1001);  //基础剑术
                SkillIdList.Add(2, 2001);  //火球
                SkillIdList.Add(3, 3001);  //灵魂火符
            }

            if (this.Bags == null)
            {
                this.Bags = new List<Item>();
            }
            if (this.EquipPanel == null)
            {
                this.EquipPanel = new Dictionary<int, Equip>();
            }
        }

        public void UpdatePlayerInfo() {
            GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case HeroChangeType.LevelUp:
                    if(!this.isInLevelUp)
                    {
                        this.isInLevelUp = true;
                        GameProcessor.Inst.StartCoroutine(LevelUp());
                    }
                    break;
            }
        }

        private void HeroUseEquip(HeroUseEquipEvent e)
        {

            this.Equip(e.Position, e.Equip);
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            Bags.Add(e.Equip);
            EquipPanel.Remove(e.Equip.Position % PlayerHelper.MAX_EQUIP_COUNT);

            //替换属性
            foreach (var a in e.Equip.AttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, e.Equip.Position% PlayerHelper.MAX_EQUIP_COUNT * 100 + AttributeFrom.EquipBase, a.Value*-1);
            }

            //显示最新的血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //更新属性面板
            UpdatePlayerInfo();
        }


        private void Equip(int pos, Equip equip)
        {
            //装配包裹的
            Equip old;
            if (EquipPanel.TryGetValue(equip.Position, out old))
            {
                Bags.Add(old);
            }


            Bags.Remove(equip); //old move to bag

            EquipPanel[equip.Position] = equip; //new use to panel

            //替换属性
            foreach (var a in equip.AttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, equip.Position % PlayerHelper.MAX_EQUIP_COUNT * 100 + AttributeFrom.EquipBase, a.Value);
            }

            //显示最新的血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //更新属性面板
            UpdatePlayerInfo();
        }

        private void SetLevelConfigAttr()
        {
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);
            UpExp = config.Exp;
        }

        IEnumerator LevelUp()
        {
            while (this.Exp >= this.UpExp)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetLevelConfigAttr();
                //升级恢复满血量
                SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });

                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;
        }

        public void AddToBags(List<Item> items)
        {
            int num = Mathf.Min( 50 - Bags.Count,items.Count);
            if (num > 0)
            {
                Bags.AddRange(items.GetRange(0, num));
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
