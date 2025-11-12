using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Shengxiao_Panel : MonoBehaviour
    {
        public Button Btn_Close;

        public List<SlotBox> ItemList = new List<SlotBox>();

        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            ItemList = this.GetComponentsInChildren<SlotBox>().ToList();
        }

        void Start()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");

            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].Init(prefab);
            }

            this.Show();
        }

        private void Show()
        {
            //Debug.Log("exclusive show");
            List<ShengxiaoConfig> configs = ShengxiaoConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < configs.Count; i++)
            {
                ShengxiaoConfig config = configs[i];

                //先重置初始状态
                SlotBox slot = ItemList[i];
                slot.UnEquip();
                slot.SetPart(config.Part, config.Name);

                //装载已装备的装备
                IDictionary<int, Shengxiao> currentPanel = user.ShengxiaoList;
                int part = config.Part;

                //Debug.Log("part:" + part);

                if (currentPanel.ContainsKey(part))
                {
                    CreateEquipPanelItem(slot, config, currentPanel[part]);
                }
            }
        }

        public void Wear(Shengxiao exclusive)
        {
            int part = exclusive.ShengxiaoConfig.Part;

            SlotBox slot = ItemList.Where(m => m.Part == part).FirstOrDefault();
            if (slot != null)
            {
                CreateEquipPanelItem(slot, exclusive.ShengxiaoConfig, exclusive);
            }
        }


        private void CreateEquipPanelItem(SlotBox slot, ShengxiaoConfig config, Item equip)
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

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
