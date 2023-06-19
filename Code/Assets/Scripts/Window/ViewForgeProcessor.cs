using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class ViewForgeProcessor : AViewPage
{
    public Transform tran_EquiList;
    public Transform tran_AttrList;

    public Text Txt_Fee;

    public Button Btn_Strengthen;
    public Button Btn_Strengthen_Batch;

    private StrenthAttrItem[] AttrList;
    private StrenthBox[] EquiList;

    private int SelectPosition = 1;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
        Btn_Strengthen_Batch.onClick.AddListener(OnClick_Strengthen_Batch);

        //var equipList = tran_EquiList.GetComponentsInChildren<SlotBox>();

        AttrList = tran_AttrList.GetComponentsInChildren<StrenthAttrItem>();
        foreach (var attrTxt in AttrList)
        {
            attrTxt.gameObject.SetActive(false);
        }

        EquiList = tran_EquiList.GetComponentsInChildren<StrenthBox>();

        Txt_Fee.text = "0";
    }

    private void OnEquipStrengthSelectEvent(EquipStrengthSelectEvent e)
    {
        Debug.Log("SelectPosition:" + e.Position);

        if (e.Position == SelectPosition) {
            return;
        }

        //把之前的设为普通状态
        StrenthBox oldBox = EquiList.Where(m => ((int)m.SlotType) == SelectPosition).First();
        oldBox.SeSelect(false);

        this.SelectPosition = e.Position;
        ShowInfo();
    }

    private void ShowInfo()
    {
        User user = GameProcessor.Inst.User;
        int strengthLevel = 0;
        user.EquipStrength.TryGetValue(SelectPosition, out strengthLevel);

        EquipStrengthConfig currentConfig = EquipStrengthConfigCategory.Instance.GetByPositioinAndLevel(SelectPosition, strengthLevel);

        EquipStrengthConfig nextConfig = EquipStrengthConfigCategory.Instance.GetByPositioinAndLevel(SelectPosition, strengthLevel + 1);

        if (nextConfig != null)
        {
            string color = user.Gold >= nextConfig.Fee ? "#FFFF00" : "#FF0000";

            Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, nextConfig.Fee);
        }

        EquipStrengthConfig showConfig = currentConfig == null ? nextConfig : currentConfig;

        for (int i = 0; i < AttrList.Length; i++)
        {
            if (i < showConfig.AttrList.Length)
            {
                int attrId = showConfig.AttrList[i];
                long currentAttrValue = currentConfig == null ? 0 : currentConfig.AttrValueList[i];
                long nextAttrValue = nextConfig == null ? 0 : nextConfig.AttrValueList[i];

                string attrName = PlayerHelper.PlayerAttributeMap[((AttributeEnum)attrId).ToString()];
                string attrCurrent = currentAttrValue == 0 ? "" : currentAttrValue + "";
                string attrAdd = "+" + (nextAttrValue - currentAttrValue) + "";

                AttrList[i].SetContent(attrName, attrCurrent, attrAdd);
                AttrList[i].gameObject.SetActive(true);
            }
            else
            {
                AttrList[i].gameObject.SetActive(false);
            }
        }


        foreach (var box in EquiList)
        {
            int position = ((int)box.SlotType);
            int level = 0;
            user.EquipStrength.TryGetValue(position, out level);

            box.SetLevel(level);
        }
    }

    private void OnClick_Strengthen()
    {
        User user = GameProcessor.Inst.User;
        int strengthLevel = 0;
        user.EquipStrength.TryGetValue(SelectPosition, out strengthLevel);

        Debug.Log("tran_EquiList:"+ tran_EquiList.position.x + ","+ tran_EquiList.position.y);
        Debug.Log("tran_AttrList:" + tran_AttrList.position.x + "," + tran_AttrList.position.y);
        Debug.Log("Txt_Fee:" + Txt_Fee.transform.position.x + "," + Txt_Fee.transform.position.y);
        Debug.Log("Btn_Strengthen:" + Btn_Strengthen.transform.position.x + "," + Btn_Strengthen.transform.position.y);

        if (strengthLevel >= user.Level)
        {
            //
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "强化到满级了",Parent= tran_AttrList });
            return;
        }

        EquipStrengthConfig config = EquipStrengthConfigCategory.Instance.GetByPositioinAndLevel(SelectPosition, strengthLevel + 1);

        if (user.Gold < config.Fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", Parent = tran_AttrList });
            return;
        }

        user.EquipStrength[SelectPosition] = strengthLevel + 1;

        user.AddExpAndGold(0, -config.Fee);

        GameProcessor.Inst.UpdateInfo();

        ShowInfo();

        Debug.Log("Strengthen Success");
    }
    private void OnClick_Strengthen_Batch()
    {
        Debug.Log("piliangqianghua");
    }

    protected override bool CheckPageType(ViewPageType page)
    {
        return page == ViewPageType.View_Forge;
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();

        GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);

        ShowInfo();
    }
}
