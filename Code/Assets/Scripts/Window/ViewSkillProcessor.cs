using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewSkillProcessor : AViewPage
    {
        [Title("技能面板")]
        [LabelText("所有技能")]
        public ScrollRect sr_AllSkill;

        [LabelText("装载技能")]
        public Transform tran_EquipSkills;

        public List<Button> PlanList;

        private Com_Skill[] AllEquipSkills;

        private List<Item_Skill> learnSkills;
        private List<Com_Skill> equipSkills;
        private GameObject bookPrefab;

        void Awake()
        {
            for (int i = 0; i < PlanList.Count; i++)
            {
                int index = i;
                PlanList[i].onClick.AddListener(() =>
                {
                    ChangePlan(index);
                });
            }
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.learnSkills = new List<Item_Skill>();
            this.equipSkills = new List<Com_Skill>();

            var user = GameProcessor.Inst.User;

            user.EventCenter.AddListener<SkillShowEvent>(OnSkillShow);
            user.EventCenter.AddListener<SkillUpEvent>(OnSkillUp);
            user.EventCenter.AddListener<SkillDownEvent>(OnSkillDown);
            user.EventCenter.AddListener<SkillChangePlanEvent>(OnSkillChangePlan);
            

            bookPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Skill");

            this.AllEquipSkills = this.tran_EquipSkills.GetComponentsInChildren<Com_Skill>();

            this.ShowSkillPanel();
            this.ShowSkillBattle();
        }

        private void ChangePlan(int index)
        {
            GameProcessor.Inst.User.SkillPanelIndex = index;
            this.ShowSkillBattle();
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }

        private void ShowSkillPanel()
        {
            User user = GameProcessor.Inst.User;

            user.SkillList.Sort((a, b) =>
            {
                return a.SkillConfig.Id.CompareTo(b.SkillConfig.Id);
            });

            foreach (var skill in user.SkillList)
            {
                SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId), true);
                ShowSkillPanelItem(skillPanel);
            }
        }
        private void ShowSkillPanelItem(SkillPanel skill)
        {
            if (skill == null)
            {
                return;
            }

            Item_Skill learn = this.learnSkills.Find(s => s.SkillPanel.SkillId == skill.SkillId);
            if (learn != null)
            {
                learn.SetItem(skill);
            }
            else
            {
                var emptyBook = GameObject.Instantiate(bookPrefab);
                var com = emptyBook.GetComponent<Item_Skill>();
                com.SetItem(skill);
                emptyBook.transform.SetParent(this.sr_AllSkill.content);
                emptyBook.transform.localScale = Vector3.one;

                this.learnSkills.Add(com);
            }
        }

        private void ShowSkillBattle()
        {
            var user = GameProcessor.Inst.User;

            for (int i = 0; i < AllEquipSkills.Length; i++)
            {
                AllEquipSkills[i].Clear();
                if (i < user.SkillNumber)
                {
                    AllEquipSkills[i].gameObject.SetActive(true);
                }
                else
                {
                    AllEquipSkills[i].gameObject.SetActive(false);
                }
            }

            List<int> skills = user.GetCurrentSkillList();

            for (int i = 0; i < skills.Count; i++)
            {
                SkillData skillData = user.SkillList.Where(m => m.SkillId == skills[i]).FirstOrDefault();

                AllEquipSkills[i].SetItem(skillData);
            }
        }

        private void OnSkillShow(SkillShowEvent e)
        {
            this.ShowSkillPanel();
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }

        private void OnSkillUp(SkillUpEvent e)
        {
            this.ShowSkillBattle();
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }

        private void OnSkillDown(SkillDownEvent e)
        {
            this.ShowSkillBattle();
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }
        private void OnSkillChangePlan(SkillChangePlanEvent e)
        {
            this.ShowSkillBattle();
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent());
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Skill;
        }

        public override void OnOpen()
        {
            base.OnOpen();
        }
    }
}
