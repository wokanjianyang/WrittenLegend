using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Item_Skill : MonoBehaviour
    {
        [Title("����")]
        [LabelText("����")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("�ȼ�")]
        public TextMeshProUGUI tmp_Level;

        [LabelText("��ȴ")]
        public TextMeshProUGUI tmp_CD;

        [LabelText("����")]
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
