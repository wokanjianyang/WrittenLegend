using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_SoulBone : MonoBehaviour
{
    public Text txt_Fee;
    public Button Btn_Active;

    public Transform Tf_Ring;
    public Transform Tf_Attr;

    private List<Toggle> RingList;
    private List<StrenthAttrItem> AttrList;

    private int Sid = 0;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Active.onClick.AddListener(OnStrong);

        RingList = Tf_Ring.GetComponentsInChildren<Toggle>().ToList();
        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();

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

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        for (int i = 0; i < RingList.Count; i++)
        {
            int sid = i + 1;

            if (user.SoulBoneData.TryGetValue(sid, out MagicData data)) //active
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

        //��ʼδѡ��,���ؾ�����Ϣ
        for (int i = 0; i < AttrList.Count; i++)
        {
            AttrList[i].gameObject.SetActive(false);
        }

        Btn_Active.gameObject.SetActive(false);

        Text[] txtList = ring.GetComponentsInChildren<Text>();

        for (int i = 0; i < txtList.Length; i++)
        {
            if (txtList[i].name == "lb_Name")
            {
                SoulBoneConfig config = SoulBoneConfigCategory.Instance.Get(sid);
                txtList[i].text = config.Name.Insert(2, "\n");
            }
            else
            {
                txtList[i].text = level + "";
            }
        }
    }

    private void ShowSoulRing(int sid)
    {
        this.Sid = sid;

        User user = GameProcessor.Inst.User;

        long currentLevel = 0;

        if (user.SoulBoneData.TryGetValue(sid, out MagicData data))
        {
            currentLevel = data.Data;
        }
        InitRing(sid, currentLevel);

        SoulBoneConfig config = SoulBoneConfigCategory.Instance.Get(sid);

        long RingLevel = user.SoulRingData[sid].Data;

        long materialCount = user.GetMaterialCount(config.ItemId);
        long fee = 1;
        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(config.ItemId);

        if (currentLevel >= RingLevel)
        {
            txt_Fee.text = "������";
        }
        else
        {
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";
            txt_Fee.text = string.Format("<color={0}>{1}</color>", color, itemConfig.Name + ":" + materialCount + "/ " + fee);
        }

        if (currentLevel >= RingLevel || materialCount < fee)
        {
            Btn_Active.gameObject.SetActive(false);
        }
        else
        {
            Btn_Active.gameObject.SetActive(true);
        }

        //Attr
        for (int i = 0; i < AttrList.Count; i++)
        {
            StrenthAttrItem attrItem = AttrList[i];

            int attrId = config.AttrIdList[i];
            double baseValue = config.AttrValueist[i];

            if (i >= config.AttrIdList.Length)
            {
                attrItem.gameObject.SetActive(false);
            }
            else
            {
                attrItem.gameObject.SetActive(true);

                attrItem.SetContent(attrId, baseValue * currentLevel, baseValue);
            }
        }
    }


    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;

        SoulBoneConfig config = SoulBoneConfigCategory.Instance.Get(this.Sid);
        long materialCount = user.GetMaterialCount(config.ItemId);

        long fee = 1;

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "û���㹻�Ĳ���", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_SoulRingShard,
            Quantity = fee
        });

        user.AddSoulBoneLevel(Sid);

        GameProcessor.Inst.UpdateInfo();

        ShowSoulRing(this.Sid);
    }
}
