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

        public List<SkillBook> SkillPanel { get; set; }

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
            this.EventCenter.AddListener<HeroUseSkillBookEvent>(HeroUseSkillBook);
            this.GroupId = 1;
        }

        public override void Load()
        {
            base.Load();

            //var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            //var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            //box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform); 
        }

        public void Init() {
            this.Camp = PlayerType.Hero;

            //设置各种属性值
            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 15);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            if (this.Bags == null)
            {
                this.Bags = new List<Item>();
            }
            else
            {
                foreach(var item in this.Bags)
                {
                    if(item.Type == ItemType.SkillBox)
                    {
                        var book = item as SkillBook;
                        var bookconfig = SkillConfigCategory.Instance.Get(book.ConfigId);
                        book.BookConfig = bookconfig;
                        var config = ItemConfigCategory.Instance.Get(book.ConfigId);
                        book.Config = config;
                    }
                    
                }
            }
            if (this.EquipPanel == null)
            {
                this.EquipPanel = new Dictionary<int, Equip>();
            }
            if(this.SkillPanel == null)
            {
                this.SkillPanel = new List<SkillBook>();
            }
            else
            {
                foreach (var book in this.SkillPanel)
                {
                    var bookconfig = SkillConfigCategory.Instance.Get(book.ConfigId);
                    book.BookConfig = bookconfig;
                    var config = ItemConfigCategory.Instance.Get(book.ConfigId);
                    book.Config = config;

                }
            }
        }

        /// <summary>
        /// 加载面板已选择技能放到SkillIdList
        /// </summary>
        public void InitPanelSkill()
        {
            SkillIdList = new Dictionary<int, int>();

            List<SkillBook> list = SkillPanel.FindAll(m => m.SkillType == SkillBookType.Equip);

            for (int i = 0; i < list.Count; i++)
            {
                SkillIdList.Add(i, list[i].ConfigId);
            }

            base.LoadSkill();
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

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {

            Bags.Remove(e.Book);
            if (e.IsLearn)
            {
                this.SkillPanel.Add(e.Book);
                e.Book.SkillType = SkillBookType.Learn;
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    Book = e.Book
                });
            }
            else
            {
                var book = this.SkillPanel.Find(b => b.ConfigId == e.Book.ConfigId);
                book.AddExp(10);
                this.EventCenter.Raise(new HeroUpdateSkillEvent() { 
                    Book = book
                });
            }
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
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
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
