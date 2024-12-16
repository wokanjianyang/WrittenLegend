using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Reform : MonoBehaviour
{
    public Transform Tran_Item_List;
    public Transform Tran_Attr_List;
    private ItemForge[] items;

    public Text Reform_Txt_Fee;
    public Text Reform_Txt_Fee1;
    public Button Btn_Reform;

    private int Refine_Position = 1;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<ItemForge>();
        Btn_Reform.onClick.AddListener(OnClick_Refine);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<EquipRefineSelectEvent>(this.OnEquipRefineSelectEvent);

        this.Init();
        this.ShowRefine();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;
            long level = user.GetReformLevel(position);

            items[i].Init(2, position, level, toggleGroup);
        }
    }

    private void ShowRefine()
    {
        User user = GameProcessor.Inst.User;

        long MaxLevel = user.GetReformLimit(Refine_Position);
        long currentLevel = user.GetReformLevel(Refine_Position);

        items[Refine_Position - 1].SetLevel(currentLevel);

        long nextLevel = currentLevel + 1;
        EquipReformFeeConfig feeConfig = EquipReformFeeConfigCategory.Instance.GetByLevel(nextLevel);

        if (feeConfig == null || nextLevel > MaxLevel)
        {
            Reform_Txt_Fee.text = "已满级";
            Btn_Reform.gameObject.SetActive(false);
        }
        else
        {
            long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_EquipRefineStone);

            double goldCount = feeConfig.GetFee(nextLevel) * ConfigHelper.RestoreGold * 2; //京单位

            //string color = materialCount >= nextConfig.GetFee(nextLevel) ? "#FFFF00" : "#FF0000";

            //Reform_Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, nextConfig.GetFee(nextLevel));
            //Btn_Reform.gameObject.SetActive(true);
        }
    }

    private void OnEquipRefineSelectEvent(EquipRefineSelectEvent e)
    {
        this.Refine_Position = e.Position;
        ShowRefine();
    }

    private void OnClick_Refine()
    {
        User user = GameProcessor.Inst.User;
        long currentLevel = user.GetRefineLevel(Refine_Position);

        long MaxLevel = user.GetRefineLimit();
        if (currentLevel >= MaxLevel)
        {
            //
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "改造等级满级了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long refineLevel = currentLevel + 1;
        EquipRefineConfig config = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel);

        var materialCount = user.GetMaterialCount(ItemHelper.SpecialId_EquipRefineStone);

        if (materialCount < config.GetFee(refineLevel))
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的精炼石", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.MagicEquipRefine[Refine_Position].Data++;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_EquipRefineStone,
            Quantity = config.GetFee(refineLevel)
        });

        GameProcessor.Inst.UpdateInfo();

        ShowRefine();

        GameProcessor.Inst.SaveData();
    }


}

