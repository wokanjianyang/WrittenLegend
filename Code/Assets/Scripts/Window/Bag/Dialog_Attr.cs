using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Attr : MonoBehaviour, IBattleLife
{
    public Button btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowDialogUserAttrEvent>(this.Show);
    }

    private void Show(ShowDialogUserAttrEvent e)
    {
        this.gameObject.SetActive(true);

        Item_Attr[] items = this.GetComponentsInChildren<Item_Attr>();

        User user = GameProcessor.Inst.User;

        AttributeEnum[] list = new AttributeEnum[] {
            AttributeEnum.AttIncrea, AttributeEnum.HpIncrea, AttributeEnum.DefIncrea,
            AttributeEnum.PhyAttIncrea, AttributeEnum.MagicAttIncrea, AttributeEnum.SpiritAttIncrea,
            //AttributeEnum.PanelHp, AttributeEnum.PanelAtt, AttributeEnum.PanelDef,
            //AttributeEnum.PanelPhyAtt, AttributeEnum.PanelMagicAtt, AttributeEnum.PanelSpiritAtt,
            AttributeEnum.MulAttr, AttributeEnum.MulHp, AttributeEnum.MulDef,
            AttributeEnum.MulAttrPhy, AttributeEnum.MulAttrMagic, AttributeEnum.MulAttrSpirit,
            AttributeEnum.MoveSpeed, AttributeEnum.DefIgnore, AttributeEnum.Miss,
            AttributeEnum.AurasDamageResist,   AttributeEnum.AurasDamageResist,AttributeEnum.AurasAttrIncrea,
        };

        for (int i = 0; i < items.Length; i++)
        {
            Item_Attr item = items[i];
            if (i < list.Length)
            {
                item.gameObject.SetActive(true);

                AttributeEnum attrId = list[i];
                item.SetContent((int)attrId, user.AttributeBonus.GetBaseAttr(attrId));
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
