using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Fashion_Special : MonoBehaviour
{
    public Image Icon;
    public Text Txt_Name;

    public Transform Tf_Attr;
    private List<StrenthAttrItem> AttrList;

    public Text Txt_Fee;
    public Text Txt_Attr_Active;

    public Button Btn_Active;
    public Button Btn_Up;

    public FashionSpecialConfig Config { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnActive);
        Btn_Up.onClick.AddListener(OnUp);

        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(FashionSpecialConfig config)
    {
        this.Config = config;

        Txt_Name.text = this.Config.Name;
        Icon.sprite = PrefabHelper.Instance().GetFashion(Config.Id);

        this.Show();
    }

    public void Show()
    {
        if (this.Config == null)
        {
            return;
        }

        for (int i = 0; i < AttrList.Count; i++)
        {
            AttrList[i].SetContent(this.Config.AttrIdList[i], this.Config.AttrValueList[i], 0);
        }

        Txt_Attr_Active.text = "出战属性：" + StringHelper.FormatAttrText(this.Config.UpAttrId, this.Config.UpAttrValue);

        User user = GameProcessor.Inst.User;

        long fashionLevel = user.GetFashionSpecialLevel(this.Config.Id);

        if (fashionLevel > 0)
        {
            Btn_Active.gameObject.SetActive(false);

            if (this.Config.Id == user.FashionUpId)
            {
                Btn_Up.gameObject.SetActive(false);
            }
            else
            {
                Btn_Up.gameObject.SetActive(true);
            }
            Txt_Fee.gameObject.SetActive(false);
        }
        else
        {
            Btn_Active.gameObject.SetActive(true);
            Btn_Up.gameObject.SetActive(false);
            Txt_Fee.gameObject.SetActive(true);

            long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Fashion);

            int fee = this.Config.Fee;
            string color = materialCount >= fee ? "#00FF00" : "#FF0000";

            Txt_Fee.text = string.Format("<color={0}>时装精华:{1}/{2}</color>", color, materialCount, fee);
        }
    }

    private void OnActive()
    {
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;
        long fashionLevel = user.GetFashionSpecialLevel(Config.Id);

        if (fashionLevel > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经激活了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Fashion);

        int fee = Config.Fee;

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            this.Btn_Active.gameObject.SetActive(true);
            return;
        }

        user.SaveFashionSpecialLevel(Config.Id);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Fashion,
            Quantity = fee
        });

        GameProcessor.Inst.EventCenter.Raise(new FashionUIFreshEvent() { });

        user.EventCenter.Raise(new UserAttrChangeEvent());

    }

    private void OnUp()
    {
        Debug.Log("OnUp count:");

        this.Btn_Up.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;
        long fashionLevel = user.GetFashionSpecialLevel(Config.Id);

        if (fashionLevel <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "还没有激活", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.FashionUpId = Config.Id;

        this.Btn_Active.gameObject.SetActive(false);
        this.Btn_Up.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new FashionUIFreshEvent() { });

        user.EventCenter.Raise(new UserAttrChangeEvent());
    }
}

