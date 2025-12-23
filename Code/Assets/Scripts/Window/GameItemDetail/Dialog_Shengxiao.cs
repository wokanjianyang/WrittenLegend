using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Dialog_Shengxiao : MonoBehaviour, IBattleLife
    {
        [LabelText("名称")]
        public Text tmp_Title;

        [LabelText("基础属性")]
        public Transform tran_BaseAttribute;

        [LabelText("随机属性")]
        public Transform tran_RandomAttribute;

        [LabelText("套装属性")]
        public Transform tran_GroupAttribute;

        [Title("导航")]

        public Button btn_Equip;
        public Button btn_UnEquip;

        public Button btn_Recovery;
        public Button btn_Restore;

        public Button btn_Lock;
        public Button btn_Unlock;

        public Button Btn_Close;

        private BoxItem boxItem;
        private int equipPositioin;
        private ComBoxType BoxType;

        private RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);


            this.btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.btn_Restore.onClick.AddListener(this.OnClick_Restore);

            this.btn_Lock.onClick.AddListener(this.OnClick_Lock);
            this.btn_Unlock.onClick.AddListener(this.OnClick_Unlock);

            this.Btn_Close.onClick.AddListener(this.OnClick_Close);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.AddListener<ShowShengxiaoDetailEvent>(this.OnShowEvent);
            this.rectTransform = this.transform.GetComponent<RectTransform>();
        }

        private void OnShowEvent(ShowShengxiaoDetailEvent e)
        {
            this.gameObject.SetActive(true);
            tran_BaseAttribute.gameObject.SetActive(false);
            tran_RandomAttribute.gameObject.SetActive(false);
            tran_GroupAttribute.gameObject.SetActive(false);

            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Restore.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);

            // this.transform.position = this.GetBetterPosition(e.Position);
            // this.img_Background.sprite = this.list_BackgroundImgs[this.item.GetQuality() - 1];
            this.boxItem = e.boxItem;
            this.equipPositioin = e.EquipPosition;
            this.BoxType = e.Type;

            var titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);

            Shengxiao equip = this.boxItem.Item as Shengxiao;

            ShengxiaoConfig config = equip.ShengxiaoConfig;

            string name = equip.Name;

            if (equip.LevelData.Data > 0)
            {
                name += "(" + equip.LevelData.Data + "级)";
            }

            this.tmp_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, name);

            string color = "green";

            User user = GameProcessor.Inst.User;

            IDictionary<int, long> BaseAttrList = equip.GetBaseAttrList();

            if (BaseAttrList != null && BaseAttrList.Count > 0)
            {
                tran_BaseAttribute.gameObject.SetActive(true);
                tran_BaseAttribute.Find("Title").GetComponent<Text>().text = "[基础属性]";
                tran_BaseAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);

                Transform gridBase = tran_BaseAttribute.Find("Grid_Base");

                List<KeyValuePair<int, long>> btList = BaseAttrList.ToList();

                for (int index = 0; index < 8; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", index));

                    if (index < btList.Count())
                    {
                        child.GetComponent<Text>().text = FormatAttrText(btList[index].Key, btList[index].Value, 0);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (equip.AttrEntryList != null && equip.AttrEntryList.Count > 0)
            {
                tran_RandomAttribute.gameObject.SetActive(true);
                tran_RandomAttribute.Find("Title").GetComponent<Text>().text = "[随机属性]";
                Transform gridRandom = tran_RandomAttribute.Find("Grid_Random");

                var AttrEntryList = equip.AttrEntryList.ToList();

                for (int index = 0; index < 4; index++)
                {
                    var child = gridRandom.Find(string.Format("Attribute_{0}", index));

                    if (index < AttrEntryList.Count)
                    {
                        int attrId = AttrEntryList[index].Key;
                        long attrBaseValue = AttrEntryList[index].Value;

                        child.GetComponent<Text>().text = FormatAttrText(attrId, attrBaseValue, config.LayerValueList[index] * equip.LayerData.Data);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }


            ShengxiaoGroup group = user.GetShengxiaoGroup();

            this.ShowGroup(group);

            if (user.Cycle.Data >= 10)
            {
                this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);
                this.btn_UnEquip.gameObject.SetActive(this.boxItem.BoxId == -1);
            }

            if (equip.LevelData.Data > 1 || equip.LayerData.Data > 1)
            {
                this.btn_Restore.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
                this.btn_Recovery.gameObject.SetActive(false);
            }
            else
            {
                this.btn_Restore.gameObject.SetActive(false);
                this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
            }

            this.btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
            this.btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);


            if (equipPositioin < -1 || this.BoxType != ComBoxType.Bag) //不可操作
            {
                this.btn_Equip.gameObject.SetActive(false);
                this.btn_UnEquip.gameObject.SetActive(false);
                this.btn_Recovery.gameObject.SetActive(false);
                this.btn_Restore.gameObject.SetActive(false);
                this.btn_Lock.gameObject.SetActive(false);
                this.btn_Unlock.gameObject.SetActive(false);
            }
        }

        private void ShowGroup(ShengxiaoGroup group)
        {
            tran_GroupAttribute.gameObject.SetActive(true);

            Text redTitle = tran_GroupAttribute.Find("Title").GetComponent<Text>();

            Item_Equip_Red[] reds = tran_GroupAttribute.GetComponentsInChildren<Item_Equip_Red>(true);

            for (int i = 0; i < reds.Length; i++)
            {
                if (i < group.List.Count)
                {
                    reds[i].gameObject.SetActive(true);
                    reds[i].SetShengxiaoGroup(group.List[i]);
                }
                else
                {
                    reds[i].gameObject.SetActive(false);
                }
            }
        }

        private string FormatAttrText(int attr, double val, double rise)
        {
            string unit = "";

            List<int> percents = ConfigHelper.BaseAttrIdList.ToList().ToList(); ;

            if (!percents.Contains(attr))
            {
                unit = "%";
            }

            if (rise > 0)
            {
                return PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()] + " + " + StringHelper.FormatNumber(val) + "+" + rise + unit;
            }
            else
            {
                return PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()] + " + " + StringHelper.FormatNumber(val) + unit;
            }
        }



        private void OnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                IsWear = true,
                BoxItem = this.boxItem,
            });
        }

        private void OnUnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                IsWear = false,
                BoxItem = this.boxItem,
                Part = this.equipPositioin,
            });
        }

        private void OnClick_Restore()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能重生", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重生消耗5000兆金币，其他材料全额返回。是否确认？", true,
                () =>
                {
                    this.gameObject.SetActive(false);
                    GameProcessor.Inst.EventCenter.Raise(new RestoreEvent()
                    {
                        BoxItem = this.boxItem,
                    });
                }, () =>
                {

                });
        }

        private void OnRecovery()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能回收", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
            {
                BoxItem = this.boxItem,
            });
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        public void OnClick_Lock()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = true
            });
        }

        private void OnClick_Unlock()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = false
            });
        }
    }
}
