using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewSkillProcessor : AViewPage
    {
        [Title("技能信息")]
        [LabelText("已学习技能")]
        public ScrollRect sr_AllSkill;

        [LabelText("已装配技能")]
        public Transform tran_EquipSkills;

        private List<Item_Skill> learnSkills;
        private List<Com_Skill> equipSkills;
        private GameObject bookPrefab;

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.learnSkills = new List<Item_Skill>();
            this.equipSkills = new List<Com_Skill>();

            var hero = GameProcessor.Inst.PlayerManager.hero;
            hero.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateSkillEvent);
            bookPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Skill");

            foreach(var book in hero.SkillPanel)
            {
                if(book.SkillType == SkillBookType.Learn)
                {
                    this.CreateBook(book);
                }
            }
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Skill;
        }

        private void OnHeroUpdateSkillEvent(HeroUpdateSkillEvent e)
        {
            if(e.Book.SkillType == SkillBookType.Learn)
            {
                this.equipSkills.RemoveAll(b=>b.Book.ConfigId == e.Book.ConfigId);
                var learn = this.learnSkills.Find(s => s.Book.ConfigId == e.Book.ConfigId);
                if (learn != null)
                {
                    learn.SetItem(e.Book);
                }
                else
                {
                    this.CreateBook(e.Book);
                }
            }
            else
            {
                var learn = this.learnSkills.Find(s => s.Book.ConfigId == e.Book.ConfigId);
                if (learn != null)
                {
                    this.learnSkills.Remove(learn);
                    GameObject.Destroy(learn.gameObject);
                }

                var equip = this.equipSkills.Find(s => s.Book.ConfigId == e.Book.ConfigId);
                if (equip == null)
                {
                    var com = this.tran_EquipSkills.GetChild(this.equipSkills.Count).GetComponent<Com_Skill>();
                    com.SetItem(e.Book);
                    this.equipSkills.Add(com);
                }
            }
        }

        private void CreateBook(SkillBook book)
        {
            var emptyBook = GameObject.Instantiate(bookPrefab);
            var com = emptyBook.GetComponent<Item_Skill>();
            com.SetItem(book);
            emptyBook.transform.SetParent(this.sr_AllSkill.content);
            emptyBook.transform.localScale = Vector3.one;
            this.learnSkills.Add(com);
        }
    }
}
