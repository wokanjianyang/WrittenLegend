using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Game.Dialog;
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

    [Title("提示")]
    [LabelText("弹幕")]
    public Transform tran_Barrage;

    private GameObject barragePrefab;

    private Vector2 size;

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
    }

    private void OnDestroy()
    {
        this.SelfPlayer.EventCenter.RemoveListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.RemoveListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.RemoveListener<ShowMsgEvent>(OnShowMsgEvent);
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
    
    private void OnShowMsgEvent(ShowMsgEvent e)
    {
        var msg = GameObject.Instantiate(barragePrefab);
        msg.transform.SetParent(this.tran_Barrage);
        var msgSize = msg.GetComponent<RectTransform>().sizeDelta;
        var msgMaxY = (this.size.y - msgSize.y * 0.5f) * 0.5f;
        var msgY = UnityEngine.Random.Range(msgMaxY * -1, msgMaxY);
        msg.transform.localPosition = new Vector3(this.size.x + msgSize.x * 0.5f, msgY);
        var com = msg.GetComponent<Dialog_Msg>();
        com.tmp_Msg_Content.text = e.Content;
        msg.transform.DOLocalMoveX(msgSize.x * 0.5f * -1, 1f).OnComplete(() =>
        {
            GameObject.Destroy(msg);
        });
        
        var effectCom = EffectLoader.CreateEffect(e.Content, false);
        if (effectCom != null)
        {
            var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(e.TargetId);
            effectCom.transform.SetParent(GameProcessor.Inst.EffectRoot);
            effectCom.transform.position = enemy.Transform.position;
        }
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
        // this.SelfPlayer.EventCenter.AddListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.AddListener<ShowMsgEvent>(OnShowMsgEvent);
    }
}
