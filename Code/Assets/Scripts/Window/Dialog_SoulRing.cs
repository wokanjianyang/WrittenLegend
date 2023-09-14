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

    public Toggle Ring1;
    public Toggle Ring2;
    public Toggle Ring3;
    public Toggle Ring4;
    public Toggle Ring5;
    public Toggle Ring6;
    public Toggle Ring7;
    public Toggle Ring8;

    public Button Btn_Full;
    public Button Btn_Active;
    public Button Btn_Strong;

    public StrenthAttrItem Atrr1;
    public StrenthAttrItem Atrr2;
    public StrenthAttrItem Atrr3;
    public StrenthAttrItem Atrr4;
    public StrenthAttrItem Atrr5;

    public Text LockLevel;
    public Text LockMemo;

    public Toggle RingSkill1;
    public Toggle RingSkill2;
    public Toggle RingSkill3;
    public Toggle RingSkill4;
    public Toggle RingSkill5;
    public Toggle RingSkill6;
    public Toggle RingSkill7;
    public Toggle RingSkill8;

    List<Toggle> RingList = new List<Toggle>();
    List<Toggle> RingSkillList = new List<Toggle>();
    List<StrenthAttrItem> AttrList = new List<StrenthAttrItem>();

    private int Sid = 0;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);
        Btn_Strong.onClick.AddListener(OnStrong);

        RingList.Add(Ring1);
        RingList.Add(Ring2);
        RingList.Add(Ring3);
        RingList.Add(Ring4);
        RingList.Add(Ring5);
        RingList.Add(Ring6);
        RingList.Add(Ring7);
        RingList.Add(Ring8);

        RingSkillList.Add(RingSkill1);
        RingSkillList.Add(RingSkill2);
        RingSkillList.Add(RingSkill3);
        RingSkillList.Add(RingSkill4);
        RingSkillList.Add(RingSkill5);
        RingSkillList.Add(RingSkill6);
        RingSkillList.Add(RingSkill7);
        RingSkillList.Add(RingSkill8);

        AttrList.Add(Atrr1);
        AttrList.Add(Atrr2);
        AttrList.Add(Atrr3);
        AttrList.Add(Atrr4);
        AttrList.Add(Atrr5);

        Ring1.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(1); }
        });

        Ring2.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(2); }
        });

        Ring3.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(3); }
        });

        Ring4.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(4); }
        });

        Ring5.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(5); }
        });

        Ring6.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(6); }
        });

        Ring7.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(7); }
        });

        Ring8.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) { ShowSoulRing(8); }
        });

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
        LockLevel.gameObject.SetActive(false);
        LockMemo.gameObject.SetActive(false);

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
            AurasConfig aurasConfig = AurasConfigCategory.Instance.Get(currentConfig.AurasId);

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

    private void InitRingSkill() { 

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


        if (nextConfig == null)
        {
            //满了
            Btn_Strong.gameObject.SetActive(false);
            Btn_Active.gameObject.SetActive(false);
            LockLevel.gameObject.SetActive(false);
            LockMemo.gameObject.SetActive(false);
        }
        else
        {
            SoulRingConfig ringConfig = SoulRingConfigCategory.Instance.Get(sid);

            LockLevel.text = ringConfig.LevelMemo;
            LockMemo.text = ringConfig.AurasMemo;

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
            LockLevel.gameObject.SetActive(true);
            LockMemo.gameObject.SetActive(true);
        }

        //Fee
        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_SoulRingShard);
        if (nextConfig != null)
        {
            string color = materialCount >= nextConfig.Fee ? "#FFFF00" : "#FF0000";

            Fee.gameObject.SetActive(true);
            Fee.text = string.Format("<color={0}>{1}</color>", color, "需要:" + nextConfig.Fee + " 魂环碎片");
        }

        SoulRingAttrConfig showConfig = currentConfig == null ? nextConfig : currentConfig;

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

                string attrName = StringHelper.FormatAttrValueName(showConfig.AttrIdList[i]);
                string attrBase = "";
                string attrAdd = "";

                long ab = 0;

                if (currentConfig != null)
                {
                    attrBase = StringHelper.FormatAttrValueText(currentConfig.AttrIdList[i], currentConfig.AttrValueList[i]);
                    ab = currentConfig.AttrValueList[i];
                }
                if (nextConfig != null)
                {
                    attrAdd = " + " + StringHelper.FormatAttrValueText(nextConfig.AttrIdList[i], nextConfig.AttrValueList[i] - ab);
                }
                attrItem.SetContent(attrName, attrBase, attrAdd);
            }
        }
    }

    public void OnShowSoulRingEvent(ShowSoulRingEvent e) {
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

        if (materialCount < nextConfig.Fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (currentLevel == 0)
        {
            user.SoulRingData[Sid] = new MagicData();
        }
        user.SoulRingData[Sid].Data = currentLevel + 1; 

        GameProcessor.Inst.EventCenter.Raise(new MaterialUseEvent()
        {
            MaterialId = ItemHelper.SpecialId_SoulRingShard,
            Quantity = nextConfig.Fee
        });

        GameProcessor.Inst.UpdateInfo();

        ShowSoulRing(this.Sid);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
