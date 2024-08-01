using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pill : MonoBehaviour
{
    public Text Txt_Fee;
    public Text Txt_Level;
    public Text Txt_Layer;

    public Transform Tf_Attr;
    public Transform Tf_Item;

    public Button Btn_Close;
    public Button Btn_Active;

    private Item_Pill[] ItemList;
    private StrenthAttrItem[] AtrrList;

    public int Order => (int)ComponentOrder.Dialog;

    private string[] PillNameList = new string[]{"��̫��","������","������","��̫��","������","��̫��","��̫��","������","������","������","������","������"
        ,"����","����","����","����","������","������","��ά��","��ά��"};

    void Awake()
    {
        ItemList = Tf_Item.GetComponentsInChildren<Item_Pill>();
        AtrrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>();

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);
    }

    // Start is called before the first frame update
    void Start()
    {
        Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.PillData.Data;
        long nextLevel = currentLevel + 1;
        //Debug.Log("currentLevel show:" + currentLevel);

        long PillLayer = (nextLevel / 100) % 20;
        long PillLevel = (nextLevel) / (100 * 20);
        long PillPoint = (currentLevel % 100) / 10 + 1;

        this.Txt_Layer.text = PillNameList[PillLayer];
        this.Txt_Level.text = ConfigHelper.LayerChinaList[PillLevel] + "��" + PillPoint + "��";

        PillConfig config = PillConfigCategory.Instance.GetByLevel(nextLevel);

        //Fee
        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Pill);

        long fee = config.FeeRise * (PillLevel + 1);

        string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

        Txt_Fee.gameObject.SetActive(true);
        Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, "��Ҫ:" + fee + " ���嵤");


        for (int i = 0; i < AtrrList.Length; i++)
        {
            StrenthAttrItem attrItem = AtrrList[i];

            attrItem.gameObject.SetActive(true);
            long attrBase = 0;
            attrItem.SetContent(14, attrBase, 99);

        }
    }

    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.WingData.Data;
        long nextLevel = currentLevel + 1;

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Pill);

        PillConfig config = PillConfigCategory.Instance.GetByLevel(nextLevel);
        long fee = config.FeeRise * 1;

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "û���㹻�Ĳ���", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.WingData.Data = nextLevel;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Wing_Stone,
            Quantity = fee
        });

        Show();

        GameProcessor.Inst.UpdateInfo();

        //Debug.Log("OnStrong :" + user.WingData.Data);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
