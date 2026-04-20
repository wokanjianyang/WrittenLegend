using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Spirit_Convert : MonoBehaviour
{
    public Text Txt_Content;

    public Button Btn_Close;
    public Button Btn_Ok;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnOk);
    }

    private void OnEnable()
    {
    }

    public void Open()
    {
        this.gameObject.SetActive(true);

        this.Show();
    }

    private int[] Rates = { 1, 2, 3, 4, 6, 8, 10, 12, 15 };
    private int[] CRates = { 1, 2, 3 };

    public void Show()
    {
        User user = GameProcessor.Inst.User;

        long total = 0;

        foreach (var sp in user.SpiritRecord)
        {
            if (sp.Value.Level.Data >= 50)
            {
                SpiritConfig config = SpiritConfigCategory.Instance.Get(sp.Key);

                long mc = user.GetItemMeterialCount(config.ItemId);
                int rate = Rates[config.Quality - 1];

                total += mc;
            }
        }

        Txt_Content.text = string.Format("此次一共转化兽粮数量为：{0}", total);

        if (total > 0)
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }
    }



    public void OnOk()
    {
        Btn_Ok.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long total = 0;

        foreach (var sp in user.SpiritRecord)
        {
            if (sp.Value.Level.Data >= 50)
            {
                SpiritConfig config = SpiritConfigCategory.Instance.Get(sp.Key);

                long mc = user.GetItemMeterialCount(config.ItemId);

                if (mc > 0)
                {
                    user.UseItemMeterialCount(config.ItemId, mc);

                    int rate = Rates[config.Quality - 1];
                    int crate = CRates[config.Type - 1];
                    total += mc * rate * crate;
                }
            }
        }


        List<Item> items = new List<Item>();
        items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Steed_Exp, total));

        user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        this.Show();
    }



    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
