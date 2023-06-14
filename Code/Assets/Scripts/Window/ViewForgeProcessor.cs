using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class ViewForgeProcessor : AViewPage
{

    public Text Txt_Fee;

    public Button Btn_Strengthen;
    public Button Btn_Strengthen_Batch;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
        Btn_Strengthen_Batch.onClick.AddListener(OnClick_Strengthen_Batch);

        Txt_Fee.text = "100";
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
