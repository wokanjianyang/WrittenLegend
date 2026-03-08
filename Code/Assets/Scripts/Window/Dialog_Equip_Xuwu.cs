using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Equip_Xuwu : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;

        public List<Toggle> Toggle_Plan_List = new List<Toggle>();

        public Toggle toggle;

        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);

            toggle.onValueChanged.AddListener((isOn) =>
            {
                GameProcessor.Inst.User.EquipXuwuSetting = isOn;
            });
        }

        public void Init()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");

            SlotBox[] items = this.GetComponentsInChildren<SlotBox>();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Init(prefab);
            }

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

        public void OnBattleStart()
        {
            this.Init();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);

            toggle.isOn = GameProcessor.Inst.User.EquipXuwuSetting;
            this.InitPlanName();
        }

        private void InitPlanName()
        {
            User user = GameProcessor.Inst.User;

            int EquipXuwuIndex = user.EquipXuwuIndex;
            Toggle_Plan_List[EquipXuwuIndex].isOn = true;

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
            GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 6, Index = i });

            GameProcessor.Inst.User.EventCenter.Raise(new SkillChangePlanEvent());
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
