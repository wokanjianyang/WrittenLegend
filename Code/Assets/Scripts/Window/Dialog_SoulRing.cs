using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_SoulRing : MonoBehaviour, IBattleLife
{
    public Text Fee;

    public Button Btn_Full;
    public Button Btn_Active;
    public Button Btn_Strong;

    public Text LockLevel;
    public Text LockMemo;

    public List<Toggle> RingList = new List<Toggle>();
    public List<Toggle> RingSkillList = new List<Toggle>();
    public List<StrenthAttrItem> AttrList = new List<StrenthAttrItem>();

    private int Sid = 0;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);
        Btn_Strong.onClick.AddListener(OnStrong);

        for (int i = 0; i < RingList.Count; i++)
        {
            int index = i + 1;

            RingList[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn) { ShowSoulRing(index); }
            });
        }

        Init();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowSoulRingEvent>(this.OnShowSoulRingEvent);
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        for (int i = 0; i < RingList.Count; i++)
        {
            int sid = i + 1;

            if (user.SoulRingData.TryGetValue(sid, out MagicData data)) //active
            {
                InitRing(sid, data.Data);
            }
            else
            {
                InitRing(sid, 0);
            }
        }
    }


    private void InitRing(int sid, long level)
    {
        Toggle ring = RingList[sid - 1];

        //初始未选中,隐藏具体信息
        for (int i = 0; i < AttrList.Count; i++)
        {
            AttrList[i].gameObject.SetActive(false);
        }
        Fee.gameObject.SetActive(false);
        Btn_Active.gameObject.SetActive(false);
        Btn_Strong.gameObject.SetActive(false);

        Text[] txtList = ring.GetComponentsInChildren<Text>();

        for (int i = 0; i < txtList.Length; i++)
        {
            if (txtList[i].name == "lb_Name")
            {
                if (level <= 0)
                {
                    txtList[i].text = "未激活";
                }
                else
                {
                    SoulRingConfig srConfig = SoulRingConfigCategory.Instance.Get(sid);
                    txtList[i].text = srConfig.Name.Insert(2, "\n");
                }
            }
            else
            {
                txtList[i].text = level + "";
            }
        }

        SoulRingAttrConfig currentConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sid, level);
        Toggle ringAuras = RingSkillList[sid - 1];
        Text aurasName = ringAuras.GetComponentInChildren<Text>();

        if (currentConfig != null && currentConfig.AurasId > 0)
        {
            AurasAttrConfig aurasConfig = AurasAttrConfigCategory.Instance.Get(currentConfig.AurasId);

            //激活Auras
            ringAuras.isOn = true;
            aurasName.text = aurasConfig.Name.Insert(2, "\n");
        }
        else
        {
            ringAuras.isOn = false;
            aurasName.text = "未激活";
        }
    }

    private void ShowSoulRing(int sid)
    {
        this.Sid = sid;

        User user = GameProcessor.Inst.User;

        long currentLevel = 0;

        if (user.SoulRingData.TryGetValue(sid, out MagicData data))
        {
            currentLevel = data.Data;
        }
        InitRing(sid, currentLevel);

        SoulRingAttrConfig currentConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sid, currentLevel);
        SoulRingAttrConfig nextConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sid, currentLevel + 1);


        if (currentConfig == null && nextConfig == null)
        {
            return; //未配置的
        }

        long MaxLevel = user.GetSoulRingLimit();

        if (nextConfig == null || currentLevel >= MaxLevel || currentLevel >= nextConfig.EndLevel)
        {
            //满了
            Btn_Strong.gameObject.SetActive(false);
            Btn_Active.gameObject.SetActive(false);
        }
        else
        {
            SoulRingConfig ringConfig = SoulRingConfigCategory.Instance.Get(sid);

            if (currentLevel == 0)
            {
                Btn_Active.gameObject.SetActive(true);
                Btn_Strong.gameObject.SetActive(false);
            }
            else
            {
                Btn_Active.gameObject.SetActive(false);
                Btn_Strong.gameObject.SetActive(true);
            }
        }

        //Fee
        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_SoulRingShard);
        if (nextConfig != null)
        {
            long fee = nextConfig.GetFee(currentLevel + 1);
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            Fee.gameObject.SetActive(true);
            Fee.text = string.Format("<color={0}>{1}</color>", color, "需要:" + fee + " 魂环碎片");
        }

        SoulRingAttrConfig showConfig = currentConfig == null ? nextConfig : currentConfig;

        long aurasLevel = showConfig.GetAurasLevel(currentLevel);
        long aurasAttr = 0;

        if (currentConfig != null && currentConfig.AurasId > 0)
        {
            AurasAttrConfig aurasAttrConfig = AurasAttrConfigCategory.Instance.GetConfig(currentConfig.AurasId);
            aurasAttr = aurasAttrConfig.GetAttr(aurasLevel);
        }

        LockLevel.text = string.Format(showConfig.LockMemo, aurasLevel);
        LockMemo.text = string.Format(showConfig.AurasMemo, aurasAttr);

        //Attr
        for (int i = 0; i < AttrList.Count; i++)
        {
            StrenthAttrItem attrItem = AttrList[i];

            if (i >= showConfig.AttrIdList.Length)
            {
                attrItem.gameObject.SetActive(false);
            }
            else
            {
                attrItem.gameObject.SetActive(true);

                long attrBase = currentConfig == null ? 0 : currentConfig.GetAttr(i, currentLevel);
                long attrRise = nextConfig == null ? 0 : nextConfig.AttrRiseList[i];

                attrItem.SetContent(showConfig.AttrIdList[i], attrBase, attrRise);
            }
        }
    }

    public void OnShowSoulRingEvent(ShowSoulRingEvent e)
    {
        this.gameObject.SetActive(true);
    }


    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = 0;

        if (user.SoulRingData.TryGetValue(this.Sid, out MagicData data))
        {
            currentLevel = data.Data;
        }

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_SoulRingShard);

        SoulRingAttrConfig currentConfig = SoulRingConfigCategory.Instance.GetAttrConfig(this.Sid, currentLevel);
        SoulRingAttrConfig nextConfig = SoulRingConfigCategory.Instance.GetAttrConfig(this.Sid, currentLevel + 1);

        long fee = nextConfig.GetFee(currentLevel + 1);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (currentLevel == 0)
        {
            user.SoulRingData[Sid] = new MagicData();
        }
        user.SoulRingData[Sid].Data = currentLevel + 1;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_SoulRingShard,
            Quantity = fee
        });

        GameProcessor.Inst.UpdateInfo();

        ShowSoulRing(this.Sid);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
