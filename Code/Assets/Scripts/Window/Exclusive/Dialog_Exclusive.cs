using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Exclusive : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;

        public List<Toggle> Toggle_Plan_List = new List<Toggle>();
        //public List<Button> Btn_Plan_List = new List<Button>();

        public Toggle toggle;

        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);

            toggle.onValueChanged.AddListener((isOn) =>
            {
                GameProcessor.Inst.User.ExclusiveSetting = isOn;
            });
        }

        void Start()
        {
            toggle.isOn = GameProcessor.Inst.User.ExclusiveSetting;
        }

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowExclusiveEvent>(this.OnShowExclusive);

            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");

            SlotBox[] items = this.GetComponentsInChildren<SlotBox>();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Init(prefab);
            }

            this.InitPlanName();

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                int index = i;
                Toggle_Plan_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangePlan(index);
                    }
                });
            }
        }

        private void InitPlanName()
        {
            int ExclusiveIndex = GameProcessor.Inst.User.ExclusiveIndex;
            Toggle_Plan_List[ExclusiveIndex].isOn = true;

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                user.PlanNameList.TryGetValue(i, out string name);
                if (name != null)
                {
                    Text tt = Toggle_Plan_List[i].GetComponentInChildren<Text>();
                    tt.text = name;
                }
            }
        }

        private void ChangePlan(int i)
        {
            GameProcessor.Inst.EventCenter.Raise(new ChangeExclusiveEvent() { Index = i });
        }

        public void OnShowExclusive(ShowExclusiveEvent e)
        {
            this.gameObject.SetActive(true);

            this.InitPlanName();
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
