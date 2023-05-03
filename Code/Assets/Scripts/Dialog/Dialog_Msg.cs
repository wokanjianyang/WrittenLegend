using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dialog
{
    public class Dialog_Msg : MonoBehaviour
    {
        [Title("提示")]
        [LabelText("提示")]
        public Transform tran_Msg;
    
        [LabelText("提示内容")]
        public Text tmp_Msg_Content;
    }
}
