using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Com_Skill : MonoBehaviour
    {
        [Title("����")]
        [LabelText("����")]
        public Transform tran_Skill;

        [LabelText("����")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("����")]
        public Button btn_Skill;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Skill.onClick.AddListener(this.OnClick_RemoveSkill);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnClick_RemoveSkill()
        {
        }
    }
}
