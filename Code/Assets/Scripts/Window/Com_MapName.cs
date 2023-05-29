using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Com_MapName : MonoBehaviour
    {
        private Button btn_MapName;

        private ViewBattleProcessor.MapNameData Data;
        
        // Start is called before the first frame update
        void Start()
        {
            this.btn_MapName.onClick.AddListener(this.OnClick_MapName);
        }

        public void SetData(ViewBattleProcessor.MapNameData data)
        {
            this.Data = data;
        }

        private void OnClick_MapName()
        {
            Log.Debug(Data.Name);
        }
    }
}