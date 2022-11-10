using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using SDD.Events;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour,IPlayer
{
    [LabelText("背景图片")]
    public Image image_Background;

    [Title("信息")]
    [LabelText("信息")]
    public Transform tran_Info;
    
    [LabelText("名称")]
    public TextMeshProUGUI tmp_Info_Name;
    
    [LabelText("等级")]
    public TextMeshProUGUI tmp_Info_Level;
    
    [LabelText("血量")]
    public TextMeshProUGUI tmp_Info_HP;

    [Title("技能")]
    [LabelText("技能")]
    public Transform tran_Skill;
    
    [LabelText("名称")]
    public TextMeshProUGUI tmp_Skill_Name;

    // Start is called before the first frame update
    void Start()
    {
        this.tran_Info.gameObject.SetActive(true);
        this.tran_Skill.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        this.SelfPlayer.EventCenter.RemoveListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.RemoveListener<ShowSkillEvent>(OnShowSkillEvent);
    }
    
    private void OnSetBackgroundColorEvent(SetBackgroundColorEvent e)
    {
        this.image_Background.color = e.Color;
    }

    private void OnSetNameEvent(SetPlayerNameEvent e)
    {
        this.tmp_Info_Name.text = "名称:" + e.Name;
    }
    
    private void OnSetPlayerLevelEvent(SetPlayerLevelEvent e)
    {
        this.tmp_Info_Level.text = "等级:" + e.Level;
    }
    
    private void OnSetPlayerHPEvent(SetPlayerHPEvent e)
    {
        this.tmp_Info_HP.text = "血量:" + e.HP;
    }
    
    private void OnShowSkillEvent(ShowSkillEvent e)
    {
        this.tran_Info.gameObject.SetActive(false);
        this.tran_Skill.gameObject.SetActive(true);
        this.tmp_Skill_Name.text = e.Name;
        this.tmp_Skill_Name.autoSizeTextContainer = true;
        StartCoroutine(IE_Delay(1f, () =>
        {
            this.tran_Info.gameObject.SetActive(true);
            this.tran_Skill.gameObject.SetActive(false);
        }));
    }

    IEnumerator IE_Delay(float delay,Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    public APlayer SelfPlayer { get; set; }
    public void SetParent(APlayer player)
    {
        this.SelfPlayer = player;
        this.SelfPlayer.EventCenter.AddListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.AddListener<ShowSkillEvent>(OnShowSkillEvent);
    }
}
