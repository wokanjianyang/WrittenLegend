using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Dialog
{
    public class Dialog_Msg : MonoBehaviour
    {
        [Title("提示")]
        [LabelText("提示")]
        public Transform tran_Msg;
    
        [LabelText("提示内容")]
        public TextMeshProUGUI tmp_Msg_Content;
    }
}
