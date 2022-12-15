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
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSkill(APlayer player)
    {
        this.allSkills = new List<SkillState>();
        foreach (var data in player.SelectSkillList)
        {
            this.allSkills.Add(data);
        }
    }

    public void UseSkill(int targetId)
    {
        var skills = this.allSkills.FindAll(s => s.IsCanUse());
        var skill = skills?.FirstOrDefault();
        skill?.Do(targetId);
    }

    public APlayer SelfPlayer { get; set; }
    public void SetParent(APlayer player)
    {
        this.SelfPlayer = player;
    }
}
