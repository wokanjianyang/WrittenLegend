using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_SoulRing : MonoBehaviour
{
    public Text Txt_Attr_Rise;
    public Text Txt_Name;
    public Text Txt_Level;
    public Text Txt_Attr_Current;

    public int ConfigId { get; set; }
    private bool can = false;

    [Title("插槽")]
    [LabelText("类型")]
    public SoulRingType Type;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

