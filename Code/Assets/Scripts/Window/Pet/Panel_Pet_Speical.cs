using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pet_Speical : MonoBehaviour
{
    public Text txt_Fee;
    public Text Txt_Group;

    public Button Btn_Level_OK;
    public Button Btn_Active;
    public Button Btn_Layer_OK;

    public Transform Tf_List;
    public Transform Tf_Attr;

    private List<Item_Pet_Speical> ItemList;
    private List<StrenthAttrItem> AttrList;

    private int SelectId = 0;

    private int LayerRate = 30;
    private int PetExpRate = 3;

    private

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnActive);
        Btn_Level_OK.onClick.AddListener(OnLevelUp);
        Btn_Layer_OK.onClick.AddListener(OnLayerUp);

        ItemList = Tf_List.GetComponentsInChildren<Item_Pet_Speical>().ToList();

        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].AddListener(SelectItem);
        }

        this.ChangeItem(1);
    }

    public void Start()
    {

    }

    public void OnEnable()
    {
        this.Show();
    }

    public void ChangeItem(int id)
    {
        this.SelectId = id;

        this.Show();
    }

    public void Show()
    {
        this.Init();

        User user = GameProcessor.Inst.User;

        int groupLevel = user.GetPetSpeicalGroupLevel();

        string des = "(每阶增加宠物20级等级上限，第3阶增加一个宠物位置)"; // ，额外增加的宠物位不能上阵同种宠物

        long maxLevel = ConfigHelper.PetSpeicalMaxLayer;

        string levelDes = string.Format("神宠之力【{0}阶】-【最高" + maxLevel + "阶】：", groupLevel);

        des = levelDes + des; //groupConfig.Name + levelDes + des;

        this.Txt_Group.text = string.Format("{0}", des);
    }

    private void Init()
    {
        Btn_Active.gameObject.SetActive(false);

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetContent(i + 1);
        }

        if (this.SelectId == 0)
        {
            this.SelectId = 1;
        }

        SelectItem(this.SelectId);
    }

    private void SelectItem(int id)
    {
        Debug.Log("select item id:" + id);

        this.SelectId = id;
        this.Btn_Active.gameObject.SetActive(false);
        this.Btn_Layer_OK.gameObject.SetActive(false);
        this.Btn_Level_OK.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int layer = user.GetPetSpeicalLayer(id);
        int maxLayer = ConfigHelper.PetSpeicalMaxLayer;

        int currentLevel = user.GetPetSpeicalLevel(id);
        int maxLevel = layer * LayerRate;

        List<PetSpeicalAttrConfig> configs = PetSpeicalAttrConfigCategory.Instance.GetList(id, Math.Max(layer, 1));

        for (int i = 0; i < AttrList.Count; i++)
        {
            if (i >= configs.Count)
            {
                AttrList[i].gameObject.SetActive(false);
            }
            else
            {
                AttrList[i].gameObject.SetActive(true);

                PetSpeicalAttrConfig config = configs[i];

                double attrValue = currentLevel >= config.StartLayer ? config.AttrValue * currentLevel : 0;

                AttrList[i].SetContent(config.AttrId, attrValue, config.AttrValue);
            }
        }

        if (layer == 0)
        {  //显示激活按钮

            int count = user.Bags.Where(m => m.Item.Type == ItemType.Pet && m.Item.GetQuality() == 7 && !m.Item.IsLock && (m.Item as Pet).Role == SelectId && (m.Item as Pet).PetLayer.Data <= 1 && (m.Item as Pet).PetLevel.Data <= 1).Count();
            int fee = 10;

            string color = count >= fee ? "#FFFF00" : "#FF0000";
            txt_Fee.text = string.Format("激活消耗：金色" + ConfigHelper.PetName[id - 1] + " " + "<color={0}>{1}</color> (需要未绑定未培养的对应金色宠物)", color, count + "/ " + fee);

            if (count >= fee)
            {
                Btn_Active.gameObject.SetActive(true);
            }

        }
        else if (currentLevel < maxLevel)
        {
            //显示升级按钮
            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
            long levelFee = PetConfigCategory.Instance.GetPetFee(currentLevel) * PetExpRate;


            string color = stoneTotal >= levelFee ? "#FFFF00" : "#FF0000";
            txt_Fee.text = string.Format("<color={0}>{1}</color>", color, "宠物口粮" + ":" + stoneTotal + "/ " + levelFee);

            if (stoneTotal >= levelFee)
            {
                Btn_Level_OK.gameObject.SetActive(true);
            }
        }
        else if (currentLevel >= maxLevel && layer < maxLayer)
        {
            //显示进阶按钮
            int materilId = ItemHelper.Specail_Pet_Speical;
            long fee = PetConfigCategory.Instance.GetPetLayerFee(layer);

            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == materilId).Select(m => m.MagicNubmer.Data).Sum();
            if (stoneTotal >= fee)
            {
                Btn_Layer_OK.gameObject.SetActive(true);
            }

            string color = stoneTotal >= fee ? "#FFFF00" : "#FF0000";
            txt_Fee.text = string.Format("<color={0}>{1}</color>", color, "暗金魂心" + ":" + stoneTotal + "/ " + fee);
        }
        else
        {
            //满级，全隐藏
            txt_Fee.text = "已满阶";

        }

    }


    public void OnActive()
    {
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Pet && m.Item.GetQuality() == 7 && !m.Item.IsLock && (m.Item as Pet).Role == SelectId && (m.Item as Pet).PetLayer.Data <= 1 && (m.Item as Pet).PetLevel.Data <= 1).ToList();
        int fee = 10;

        if (list.Count < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的未绑定未培养金色宠物", ToastType = ToastTypeEnum.Failure });
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent()
            {
                BoxItem = list[i]
            });
        }

        user.AddPetSpeicalLayer(this.SelectId);
        user.AddPetSpeicalLevel(this.SelectId);

        user.EventCenter.Raise(new UserAttrChangeEvent());

        Show();
    }

    public void OnLevelUp()
    {
        User user = GameProcessor.Inst.User;

        int currentLevel = user.GetPetSpeicalLevel(this.SelectId);

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        long levelFee = PetConfigCategory.Instance.GetPetFee(currentLevel) * PetExpRate;

        if (stoneTotal <= levelFee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "口粮不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Pet_Exp,
            Quantity = levelFee
        });

        user.AddPetSpeicalLevel(this.SelectId);

        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Show();
    }

    public void OnLayerUp()
    {
        this.Btn_Layer_OK.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int current = user.GetPetSpeicalLayer(this.SelectId);

        long max = ConfigHelper.PetSpeicalMaxLayer;

        if (current >= max)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满阶", ToastType = ToastTypeEnum.Failure });
            return;
        }

        int materilId = ItemHelper.Specail_Pet_Speical;
        long fee = PetConfigCategory.Instance.GetPetLayerFee(current);

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == materilId).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = materilId,
            Quantity = fee
        });

        user.AddPetSpeicalLayer(this.SelectId);

        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Show();
    }
}

