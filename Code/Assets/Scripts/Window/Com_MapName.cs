using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Com_MapName : MonoBehaviour
    {
        public Button btn_MapName;
        public Image Icon;

        private ViewBattleProcessor.MapNameData Data { get;  set; }


        // Start is called before the first frame update
        void Start()
        {
            if (btn_MapName != null)
            {
                this.btn_MapName.onClick.AddListener(this.OnClick_MapName);
            }
            this.Icon.gameObject.SetActive(true);
        }

        public void SetData(ViewBattleProcessor.MapNameData data)
        {
            this.Data = data;
            
        }

        private void OnClick_MapName()
        {
            MapConfig config = MapConfigCategory.Instance.Get(this.Data.Id);

            GameProcessor.Inst.EventCenter.Raise(new ChangeMapEvent() { MapId = Data.Id });
            Log.Debug(Data.Name);
        }

        public void ShowIcon(int mapId)
        {
            this.Icon.transform.localScale = mapId == this.Data.Id ? Vector3.one : Vector3.zero;
        }
    }
}