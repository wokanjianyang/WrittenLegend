using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Achievement : MonoBehaviour, IBattleLife
    {
        [Title("成就面板")]
        public ScrollRect sr_Panel;

        public Button Btn_Full;

        public Toggle toggle_Hide;

        private List<Item_Achivement> activeItems = new List<Item_Achivement>();

        private List<Item_Achivement> currentItems = new List<Item_Achivement>();

        private GameObject ItemPrefab;

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Full.onClick.AddListener(OnClick_Close);

            toggle_Hide.onValueChanged.AddListener((isOn) =>
            {
                this.ShowItem(isOn);
            });
        }

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowAchievementEvent>(this.OnShowAchievement);
            GameProcessor.Inst.EventCenter.AddListener<ActiveAchievementEvent>(OnActiveAchievement);

            ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Achievement");

            Init();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;

            List<AchievementConfig> configs = AchievementConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            List<AchievementConfig> activeConfigs = configs.Where(m => user.AchievementData.ContainsKey(m.Id)).ToList();

            //已激活的
            foreach (AchievementConfig config in activeConfigs)
            {
                var item = GameObject.Instantiate(ItemPrefab);
                item.transform.SetParent(this.sr_Panel.content);
                item.transform.localScale = Vector3.one;

                var com = item.GetComponentInChildren<Item_Achivement>();
                com.SetItem(config, 0, true);
                com.gameObject.SetActive(false);
                activeItems.Add(com);
            }

            var groups = configs.Where(m => !user.AchievementData.ContainsKey(m.Id)).GroupBy(m => m.Type);
            //未激活的
            foreach (var group in groups)
            {
                int currentId = group.Min(m => m.Id);

                AchievementConfig currentConfig = configs.Where(m => m.Id == currentId).FirstOrDefault();

                long progress = user.GetAchievementProgeress((AchievementSourceType)group.Key);

                var item = GameObject.Instantiate(ItemPrefab);
                item.transform.SetParent(this.sr_Panel.content);
                item.transform.localScale = Vector3.one;

                var com = item.GetComponentInChildren<Item_Achivement>();
                com.SetItem(currentConfig, progress, false);
                currentItems.Add(com);
            }


        }

        public void OnShowAchievement(ShowAchievementEvent e)
        {
            this.gameObject.SetActive(true);
            this.Refresh();
        }

        private void Refresh()
        {
            User user = GameProcessor.Inst.User;
            foreach (var com in currentItems)
            {
                AchievementConfig config = com.Config;

                long progress = user.GetAchievementProgeress((AchievementSourceType)config.Type);

                com.SetItem(config, progress, false);
            }
        }


        private void OnActiveAchievement(ActiveAchievementEvent e)
        {
            User user = GameProcessor.Inst.User;

            user.AchievementData.Add(e.Id, 1);
            user.EventCenter.Raise(new UserAttrChangeEvent());

            //
            var com = currentItems.Where(m => m.Config.Id == e.Id).FirstOrDefault();
            if (com != null)
            {
                AchievementConfig oldConfig = com.Config;
                long progress = user.GetAchievementProgeress((AchievementSourceType)oldConfig.Type);
                com.SetItem(com.Config, progress, true);
                currentItems.Remove(com);
                activeItems.Add(com);

                AchievementConfig newConfig = AchievementConfigCategory.Instance.GetNext(oldConfig);
                if (newConfig != null)
                {
                    var item = GameObject.Instantiate(ItemPrefab);
                    item.transform.SetParent(this.sr_Panel.content);
                    item.transform.localScale = Vector3.one;

                    var newCom = item.GetComponentInChildren<Item_Achivement>();
                    newCom.SetItem(newConfig, progress, false);
                    currentItems.Add(newCom);
                }
            }
        }

        private void ShowItem(bool hide)
        {
            for (int i = 0; i < activeItems.Count; i++)
            {
                activeItems[i].gameObject.SetActive(!hide);
            }
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
