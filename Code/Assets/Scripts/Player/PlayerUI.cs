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

public class PlayerUI : MonoBehaviour, IPlayer, IPointerClickHandler
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

    [LabelText("魂环")]
    public Transform SourRingEffect;

    private GameObject barragePrefab;

    private float doTime = 0;

    private float effectTime = 0;

    private float damageTime = 0;

    private float restoreTime = 0;

    public APlayer SelfPlayer { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.tran_Info.gameObject.SetActive(true);
        this.barragePrefab = Resources.Load<GameObject>("Prefab/Dialog/Msg");
        //this.size = this.transform.GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        this.doTime -= Time.unscaledDeltaTime;
        this.effectTime += Time.unscaledDeltaTime;
        this.damageTime += Time.unscaledDeltaTime;
        this.restoreTime += Time.unscaledDeltaTime;

        if (this.SelfPlayer == null)
        {
            return;
        }

        if (doTime <= 0)
        {
            this.doTime = this.SelfPlayer.DoEvent();
        }

        if (effectTime > 0.2f)
        {
            this.SelfPlayer.DoEffect(effectTime);
            effectTime = 0;
        }

        if (damageTime > 0.1f)
        {
            this.damageTime = 0;
            this.ShowNextToast();
        }

        if (this.restoreTime >= 1f)
        {
            this.restoreTime = 0;
            this.SelfPlayer.AutoRestore();
        }
    }

    public void SetParent(APlayer player)
    {
        this.SelfPlayer = player;
        // this.SelfPlayer.EventCenter.AddListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerNameEvent>(OnSetNameEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
        this.SelfPlayer.EventCenter.AddListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        this.SelfPlayer.EventCenter.AddListener<ShowMsgEvent>(OnShowMsgEvent);
        this.SelfPlayer.EventCenter.AddListener<ShowAttackIcon>(OnShowAttackIcon);

        this.SelfPlayer.EventCenter.AddListener<ShowHideEvent>(OnShowHide);
    }

    public void OnDestroy()
    {
        if (this.SelfPlayer != null)
        {
            this.SelfPlayer.EventCenter.RemoveListener<SetBackgroundColorEvent>(OnSetBackgroundColorEvent);
            this.SelfPlayer.EventCenter.RemoveListener<SetPlayerNameEvent>(OnSetNameEvent);
            this.SelfPlayer.EventCenter.RemoveListener<SetPlayerLevelEvent>(OnSetPlayerLevelEvent);
            this.SelfPlayer.EventCenter.RemoveListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
            this.SelfPlayer.EventCenter.RemoveListener<ShowMsgEvent>(OnShowMsgEvent);
            this.SelfPlayer.EventCenter.RemoveListener<ShowHideEvent>(OnShowHide);
        }
        this.com_Progress = null;
    }

    private void OnSetBackgroundColorEvent(SetBackgroundColorEvent e)
    {
        this.image_Background.color = e.Color;
    }

    private void OnSetNameEvent(SetPlayerNameEvent e)
    {
        if (SelfPlayer.Quality > 1 && SelfPlayer.Quality < 5)
        {
            string color = QualityConfigHelper.GetQualityColor(SelfPlayer.Quality);
            this.tmp_Info_Name.text = string.Format("<color=#{0}>{1}</color>", color, e.Name);
        }
        else
        {
            this.tmp_Info_Name.text = e.Name;
        }

        switch (SelfPlayer.Camp)
        {
            case PlayerType.Hero:
            case PlayerType.HeroPhatom:
                this.image_Background.sprite = list_Backgrounds[0];
                if (SelfPlayer.RingType > 0)
                {
                    SourRingEffect.gameObject.SetActive(true);
                }
                break;
            case PlayerType.Valet:
                if (this.SelfPlayer.ModelType == MondelType.YueLing)
                {
                    this.image_Background.sprite = list_Backgrounds[5];
                }
                else
                {
                    this.image_Background.sprite = list_Backgrounds[1];
                }
                break;
            case PlayerType.Defend:
                this.image_Background.sprite = list_Backgrounds[4];
                break;
            case PlayerType.Enemy:
                if (this.SelfPlayer.ModelType == MondelType.Boss || this.SelfPlayer.Quality == 5)
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
        string title = this.SelfPlayer.Title;
        title = title == null ? "" : "(" + title + ")";

        if (SelfPlayer is Monster_Phantom)
        {
            this.tmp_Info_Level.text = e.Level + "转";
        }
        else
        {
            this.tmp_Info_Level.text = "Lv." + e.Level + title;
        }


    }

    private void OnSetPlayerHPEvent(SetPlayerHPEvent e)
    {
        if (this.com_Progress != null)
        {
            this.com_Progress.SetProgress(this.SelfPlayer.HP, SelfPlayer.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }
    }

    private void OnShowMsgEvent(ShowMsgEvent e)
    {

        if (barragePrefab == null)
        {
            return;
        }
        msgTaskList.Add(e);
    }

    private List<ShowMsgEvent> msgTaskList = new List<ShowMsgEvent>();
    private void ShowNextToast()
    {
        if (msgTaskList.Count > 10)
        {
            this.damageTime = 1 / msgTaskList.Count;
        }

        if (msgTaskList.Count > 0)
        {
            var e = msgTaskList[0];
            msgTaskList.RemoveAt(0);

            var msg = GameObject.Instantiate(barragePrefab);
            msg.transform.SetParent(this.tran_Barrage);

            var msgSize = msg.GetComponent<RectTransform>().sizeDelta;
            var msgMaxY = 60;
            var msgMinY = -120;

            var msgX = 0;

            msg.transform.localPosition = new Vector3(msgX, msgMinY);
            var com = msg.GetComponent<Dialog_Msg>();

            var msgColor = QualityConfigHelper.GetMsgColor(e.Type);
            com.tmp_Msg_Content.text = string.Format("<color=#{0}>{1}</color>", msgColor, e.Content);

            Vector3 scale = new Vector3(1.3f, 1.3f, 0);

            //首先要创建一个DOTween队列
            Sequence seq = DOTween.Sequence();

            //seq.Append  里面是让主相机振动的临时试验代码
            seq.Append(msg.transform.DOLocalMoveY(msgMaxY, 1.5f));
            seq.Join(msg.transform.DOScale(scale, 1.5f));

            seq.AppendCallback(() =>
            {
                GameObject.Destroy(msg);
            });
        }
    }

    private void OnShowAttackIcon(ShowAttackIcon e)
    {
        if (this.tran_Attack != null)
        {
            this.tran_Attack.localScale = e.NeedShow ? Vector3.one : Vector3.zero;
        }
    }
    private void OnShowHide(ShowHideEvent e)
    {
        if (this.image_Background != null)
        {
            Color temp = this.image_Background.color;
            temp.a = e.IsHide ? 0.3f : 1f;
            this.image_Background.color = temp;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Hero hero = GameProcessor.Inst.PlayerManager.GetHero();

        if (this.SelfPlayer.GroupId != hero.GroupId)
        {
            if (hero.Enemy != null)
            {
                hero.Enemy.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });
            }
            hero.UpdateEnemy(this.SelfPlayer);
            this.SelfPlayer.EventCenter.Raise(new ShowAttackIcon { NeedShow = true });
        }
    }
}
