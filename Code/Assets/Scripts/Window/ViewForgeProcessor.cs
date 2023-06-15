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

    private int SelectPosition = 1;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
        Btn_Strengthen_Batch.onClick.AddListener(OnClick_Strengthen_Batch);

        //var equipList = tran_EquiList.GetComponentsInChildren<SlotBox>();

        var attrList = tran_AttrList.GetComponentsInChildren<StrenthAttrItem>();
        foreach (var attrTxt in attrList)
        {
            attrTxt.gameObject.SetActive(false);
        }

        Txt_Fee.text = "0";

        GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);
    }

    private void OnEquipStrengthSelectEvent(EquipStrengthSelectEvent e)
    {
        this.SelectPosition = e.Position;
        ShowInfo();
    }

    private void ShowInfo()
    {
        User user = GameProcessor.Inst.User;
        int strengthLevel = 0;
        user.EquipStrength.TryGetValue(SelectPosition, out strengthLevel);

        EquipStrengthConfig config = null;
        if (strengthLevel > 0)
        {
            config = EquipStrengthConfigCategory.Instance.GetAll().Where(m => m.Value.Level == strengthLevel && m.Value.Position == SelectPosition).First().Value;
        }

        EquipStrengthConfig nextConfig = null;
        if (strengthLevel < PlayerHelper.Max_Level)
        {
            nextConfig = EquipStrengthConfigCategory.Instance.GetAll().Where(m => m.Value.Level == strengthLevel + 1 && m.Value.Position == SelectPosition).First().Value;
        }

        Txt_Fee.text = nextConfig.Fee + "";


    }

    private void OnClick_Strengthen()
    {
        Debug.Log("qianghua");
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
    }
}
