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

        public Transform Tf_Plan;
        public Transform Tf_Cycle;

        private List<Toggle> Toggle_Plan_List = new List<Toggle>();
        private List<Toggle> Toggle_Cycle_List = new List<Toggle>();

        public List<SlotBox> ItemList = new List<SlotBox>();

        //public List<Button> Btn_Plan_List = new List<Button>();
        private int CycleIndex = 0;

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

            User user = GameProcessor.Inst.User;
            bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

            if (user.MapId >= 1130 && !ac)
            {
                Toggle_Cycle_List[1].gameObject.SetActive(true);
            }
            else
            {
                Toggle_Cycle_List[1].gameObject.SetActive(false);
            }

            if (user.MapId >= 1169 && !ac)
            {
                Toggle_Cycle_List[2].gameObject.SetActive(true);
            }
            else
            {
                Toggle_Cycle_List[2].gameObject.SetActive(false);
            }
        }

        public void OnBattleStart()
        {
            Toggle_Plan_List = Tf_Plan.GetComponentsInChildren<Toggle>().ToList();
            Toggle_Cycle_List = Tf_Cycle.GetComponentsInChildren<Toggle>().ToList();
            ItemList = this.GetComponentsInChildren<SlotBox>().ToList();

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

            for (int i = 0; i < Toggle_Cycle_List.Count; i++)
            {
                int index = i;
                Toggle_Cycle_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangeCycle(index);
                    }
                });
            }

            this.Show();
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
            User user = GameProcessor.Inst.User;
            user.ExclusiveIndex = i;

            this.Show();
            //GameProcessor.Inst.EventCenter.Raise(new ChangeExclusiveEvent() { Index = i });

            GameProcessor.Inst.User.EventCenter.Raise(new SkillChangePlanEvent());
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void ChangeCycle(int i)
        {
            this.CycleIndex = i;
            this.Show();
        }

        private void Show()
        {
            //Debug.Log("exclusive show");
            List<ExclusiveConfig> configs = ExclusiveConfigCategory.Instance.GetByCycle(CycleIndex + 1);

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < configs.Count; i++)
            {
                ExclusiveConfig config = configs[i];

                //先重置初始状态
                SlotBox slot = ItemList[i];
                slot.UnEquip();
                slot.SetPart(config.Part, config.Name);

                //装载已装备的装备
                IDictionary<int, ExclusiveItem> currentPanel = user.ExclusivePanelList[user.ExclusiveIndex];
                int part = config.Part;

                //Debug.Log("part:" + part);

                if (currentPanel.ContainsKey(part))
                {
                    CreateEquipPanelItem(slot, config, currentPanel[part]);
                }
            }
        }

        private void CreateEquipPanelItem(SlotBox slot, ExclusiveConfig config, Item equip)
        {
            if (slot.GetEquip() != null) //防止叠加，无限刷道具
            {
                return;
            }

            //生成格子
            BoxItem boxItem = new BoxItem();
            boxItem.Item = equip;
            boxItem.MagicNubmer.Data = 1;
            boxItem.BoxId = -1;

            Com_Box comItem = PrefabHelper.Instance().CreateComBox(boxItem);
            comItem.transform.SetParent(slot.transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);
            comItem.SetEquipPosition(config.Part);

            //穿戴
            slot.Equip(comItem);
        }

        public void Wear(ExclusiveItem exclusive)
        {
            int part = exclusive.ExclusiveConfig.Part;

            SlotBox slot = ItemList.Where(m => m.Part == part).FirstOrDefault();
            if (slot != null)
            {
                CreateEquipPanelItem(slot, exclusive.ExclusiveConfig, exclusive);
            }
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
