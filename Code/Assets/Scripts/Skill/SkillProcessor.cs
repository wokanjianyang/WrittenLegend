using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using SDD.Events;
using UnityEngine;

public class SkillProcessor : MonoBehaviour,IPlayer
{
    private List<SkillState> allSkills;

    private void Awake()
    {
        this.allSkills = new List<SkillState>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSkill(APlayer player,SkillData data)
    {
        this.allSkills.Add(new SkillState(player,new SkillData()
        {
            ID = 0,
            CD = 0,
            Name = "普攻"
        }));
        this.allSkills.Add(new SkillState(player,data));
    }

    public void UseSkill(int tid)
    {
        var skills = this.allSkills.FindAll(s => s.IsCanUse());
        var skill = skills?.LastOrDefault();
        skill?.Do(tid);
    }

    public APlayer SelfPlayer { get; set; }
    public void SetParent(APlayer player)
    {
        this.SelfPlayer = player;
    }
}
