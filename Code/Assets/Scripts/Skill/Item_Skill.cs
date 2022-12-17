using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Item_Skill : MonoBehaviour
    {
        [Title("技能")]
        [LabelText("名称")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("等级")]
        public TextMeshProUGUI tmp_Level;

        [LabelText("冷却")]
        public TextMeshProUGUI tmp_CD;

        [LabelText("描述")]
        public TextMeshProUGUI tmp_Des;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
