using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Wing : MonoBehaviour, IBattleLife
{
    public Text Fee;

    public Button Btn_Full;
    public Button Btn_Active;
    public Button Btn_Strong;

    public StrenthAttrItem Atrr1;
    public StrenthAttrItem Atrr2;
    public StrenthAttrItem Atrr3;
    public StrenthAttrItem Atrr4;
    public StrenthAttrItem Atrr5;

    List<StrenthAttrItem> AttrList = new List<StrenthAttrItem>();

    private int Sid = 0;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);
        Btn_Strong.onClick.AddListener(OnStrong);

        AttrList.Add(Atrr1);
        AttrList.Add(Atrr2);
        AttrList.Add(Atrr3);
        AttrList.Add(Atrr4);
        AttrList.Add(Atrr5);

        Show();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowSoulRingEvent>(this.OnShowSoulRingEvent);
    }

    private void Show()
    {

    }

    public void OnShowSoulRingEvent(ShowSoulRingEvent e)
    {
        this.gameObject.SetActive(true);
    }


    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.WingData.Data;

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Wing_Stone);

        WingConfig currentConfig = WingConfigCategory.Instance.GetByLevel(currentLevel);
        WingConfig nextConfig = WingConfigCategory.Instance.GetByLevel(currentLevel + 1);

        if (materialCount < nextConfig.Fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.WingData.Data = currentLevel + 1;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Wing_Stone,
            Quantity = nextConfig.Fee
        });

        GameProcessor.Inst.UpdateInfo();

        Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
