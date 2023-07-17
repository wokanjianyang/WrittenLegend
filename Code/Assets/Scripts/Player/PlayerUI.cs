using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Game.Dialog;
using SDD.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour,IPlayer
{
    [LabelText("背景图片")]
    public Image image_Background;

    public Sprite[] list_Backgrounds;

    [Title("信息")]
    [LabelText("信息")]
    public Transform tran_Info;
    
    [LabelText("名称")]
    public Text tmp_Info_Name;
    
    [LabelText("等级")]
    public Text tmp_Info_Level;

    [Title("提示")]
    [LabelText("弹幕")]
    public Transform tran_Barrage;

    [LabelText("攻击标识")]
    public Transform tran_Attack;

    [LabelText("血条")]
    public Com_Progress com_Progress;

    private GameObject barragePrefab;

    private Vector2 size;

    public Vector3Int Cell;

    // Start is called before the first frame update
    void Start()
    {
        this.tran_Info.gameObject.SetActive(true);
        this.barragePrefab = Resources.Load<GameObject>("Prefab/Dialog/Msg");
        this.size = this.transform.GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.SelfPlayer!=null)
        {
            this.Cell = this.SelfPlayer.Cell;
        }
    }

    public void OnDestroy()
    {
        this.SelfPlayer.EventCenter.RemoveListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.RemoveListener<ShowMsgEvent>(OnShowMsgEvent);
        GameProcessor.Inst.EventCenter.RemoveListener<HideAttackIcon>(OnHideAttackIcon); 

        this.com_Progress = null;
    }
    
    private void OnSetBackgroundColorEvent(SetBackgroundColorEvent e)
    {
        this.image_Background.color = e.Color;
    }

    private void OnSetNameEvent(SetPlayerNameEvent e)
    {
        this.tmp_Info_Name.text = e.Name;
        switch (SelfPlayer.Camp)
        {
            case PlayerType.Hero:
                this.image_Background.sprite = list_Backgrounds[0];
                break;
            case PlayerType.Valet:
                this.image_Background.sprite = list_Backgrounds[1];
                break;
            case PlayerType.Enemy:
                if (this.SelfPlayer.ModelType == MondelType.Boss)
                {
                    this.image_Background.sprite = list_Backgrounds[3];
                }
                else
                {
                    this.image_Background.sprite = list_Backgrounds[2];
                }
                break;
        }
    }
    
    private void OnSetPlayerLevelEvent(SetPlayerLevelEvent e)
    {
        this.tmp_Info_Level.text = "Lv." + e.Level;
    }

    private void OnSetPlayerHPEvent(SetPlayerHPEvent e)
    {
        if (this.com_Progress != null)
        {
            this.com_Progress.SetProgress(this.SelfPlayer.HP, SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        //this.com_Progress?.SetProgress(this.SelfPlayer.HP, this.SelfPlayer.Logic.GetMaxHP()); TODO
        //GameProcessor.Inst.DelayAction(0.5f, () =>
        //{
  
        //});
    }

    private void OnShowMsgEvent(ShowMsgEvent e)
    {
        if (barragePrefab == null)
        {
            return;
        }
        var msg = GameObject.Instantiate(barragePrefab);
        msg.transform.SetParent(this.tran_Barrage);
        var msgSize = msg.GetComponent<RectTransform>().sizeDelta;
        var msgMaxY = (this.size.y - msgSize.y * 0.5f) * 0.5f;
        var msgY = UnityEngine.Random.Range(msgMaxY * -1, msgMaxY);
        msg.transform.localPosition = new Vector3(this.size.x + msgSize.x * 0.5f, msgY);
        var com = msg.GetComponent<Dialog_Msg>();

        var msgColor = QualityConfigHelper.GetMsgColor(e.Type);
        com.tmp_Msg_Content.text = string.Format("<color=#{0}>{1}</color>", msgColor, e.Content);

        msg.transform.DOLocalMoveX(msgSize.x * 0.5f * -1, 1f).OnComplete(() =>
        {
            GameObject.Destroy(msg);
        });

    }

    private void OnShowAttackIcon(ShowAttackIcon e)
    {
        this.tran_Attack.localScale = Vector3.one ;
    }

    private void OnHideAttackIcon(HideAttackIcon e)
    {
        switch (e.RoundType)
        {
            case RoundType.Hero:
                if (this.SelfPlayer.Camp == PlayerType.Enemy)
                {
                    this.tran_Attack.localScale = Vector3.zero;
                }
                break;
            case RoundType.Monster:
                if (this.SelfPlayer.Camp != PlayerType.Enemy)
                {
                    this.tran_Attack.localScale = Vector3.zero;
                }
                break;
        }
    }

    public APlayer SelfPlayer { get; set; }


    public void SetParent(APlayer player)
    {
        this.SelfPlayer = player;
        // this.SelfPlayer.EventCenter.AddListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.AddListener<ShowMsgEvent>(OnShowMsgEvent); 
        this.SelfPlayer.EventCenter.AddListener<ShowAttackIcon>(OnShowAttackIcon); 
        GameProcessor.Inst.EventCenter.AddListener<HideAttackIcon>(OnHideAttackIcon); 
    }
}
