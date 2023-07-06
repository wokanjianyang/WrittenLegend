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

        private ViewBattleProcessor.MapNameData Data;


        // Start is called before the first frame update
        void Start()
        {
            if (btn_MapName != null)
            {
                this.btn_MapName.onClick.AddListener(this.OnClick_MapName);
            }
        }

        public void SetData(ViewBattleProcessor.MapNameData data)
        {
            this.Data = data;
        }

        private void OnClick_MapName()
        {
            MapConfig config = MapConfigCategory.Instance.Get(this.Data.Id);

            int level = GameProcessor.Inst.User.Level;

            //if (level < config.LevelRequired)
            //{
            //    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "µÈ¼¶²»×ã" });
            //    return;
            //}

            GameProcessor.Inst.EventCenter.Raise(new ChangeMapEvent() { MapId = Data.Id });
            Log.Debug(Data.Name);
        }
    }
}