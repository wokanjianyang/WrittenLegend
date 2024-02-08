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
    public class Card_Exclusive : MonoBehaviour, IBattleLife
    {
        [LabelText("名称")]
        public Text tmp_Title;

        [LabelText("基础属性")]
        public Transform tran_BaseAttribute;

        [LabelText("技能属性")]
        public Transform tran_SkillAttribute;

        [LabelText("词条套装")]
        public Transform tran_SuitAttribute;

        [LabelText("套装属性")]
        public Transform tran_GroupAttribute;

        [LabelText("追击属性")]
        public Transform tran_DoubleHitAttribute;

        [Title("导航")]
        [LabelText("穿戴")]
        public Button btn_Equip;

        [LabelText("卸下")]
        public Button btn_UnEquip;

        [LabelText("回收")]
        public Button btn_Recovery;

        [LabelText("锁定装备")]
        public Button btn_Lock;

        [LabelText("解除锁定装备")]
        public Button btn_Unlock;

        public Button btn_Select;
        public Button btn_Deselect;

        public Button Btn_Close;

        private BoxItem boxItem;
        private int equipPositioin;
        private ComBoxType BoxType;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);
            this.btn_Recovery.onClick.AddListener(this.OnRecovery);

            this.btn_Lock.onClick.AddListener(this.OnClick_Lock);
            this.btn_Unlock.onClick.AddListener(this.OnClick_Unlock);

            this.btn_Select.onClick.AddListener(this.OnClick_Select);
            this.btn_Deselect.onClick.AddListener(this.OnClick_Deselect);

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

            GameProcessor.Inst.EventCenter.AddListener<ShowExclusiveCardEvent>(this.OnShow);
        }

        private void OnShow(ShowExclusiveCardEvent e)
        {
            this.gameObject.SetActive(true);
            tran_BaseAttribute.gameObject.SetActive(false);
            tran_SkillAttribute.gameObject.SetActive(false);
            tran_SuitAttribute.gameObject.SetActive(false);
            tran_GroupAttribute.gameObject.SetActive(false);
            tran_DoubleHitAttribute.gameObject.SetActive(false);

            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);
            this.btn_Select.gameObject.SetActive(false);
            this.btn_Deselect.gameObject.SetActive(false);

            // this.transform.position = this.GetBetterPosition(e.Position);
            // this.img_Background.sprite = this.list_BackgroundImgs[this.item.GetQuality() - 1];
            this.boxItem = e.boxItem;
            this.equipPositioin = e.EquipPosition;
            this.BoxType = e.Type;

            var titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);
            this.tmp_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, this.boxItem.Item.Name);

            string color = "green";

            User user = GameProcessor.Inst.User;


            ExclusiveItem exclusive = this.boxItem.Item as ExclusiveItem;
            int exclusiveLevel = exclusive.GetLevel();
            if (exclusive.BaseAttrList != null && exclusive.BaseAttrList.Count > 0)
            {
                tran_BaseAttribute.gameObject.SetActive(true);
                tran_BaseAttribute.Find("Title").GetComponent<Text>().text = "[基础属性]";
                tran_BaseAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);

                var BaseAttrList = exclusive.BaseAttrList.ToList();

                for (int index = 0; index < 6; index++)
                {
                    var child = tran_BaseAttribute.Find(string.Format("Attribute_{0}", index));

                    if (index < BaseAttrList.Count)
                    {
                        child.GetComponent<Text>().text = StringHelper.FormatAttrText(BaseAttrList[index].Key, BaseAttrList[index].Value * exclusiveLevel);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            if (exclusive.SkillRuneConfig != null)
            {
                List<int> runeIdList = new List<int>();
                if (exclusive.RuneConfigId > 0)
                {
                    runeIdList.Add(exclusive.RuneConfigId);
                }
                if (exclusive.GetLevel() > 1)
                {
                    runeIdList.AddRange(exclusive.RuneConfigIdList);
                }
                ShowRune(runeIdList);
            }
            if (exclusive.SkillSuitConfig != null)
            {
                List<int> suitIdList = new List<int>();
                if (exclusive.RuneConfigId > 0)
                {
                    suitIdList.Add(exclusive.SuitConfigId);
                }
                if (exclusive.GetLevel() > 1)
                {
                    suitIdList.AddRange(exclusive.SuitConfigIdList);
                }

                List<int> suitCountList = new List<int>();
                foreach (int suitId in suitIdList)
                {
                    int suitCount = user.GetSuitCount(exclusive.SkillSuitConfig.Id);
                    suitCountList.Add(suitCount);
                }

                ShowSuit(suitIdList, suitCountList, user.SuitMax);
            }
            if (exclusive.DoubleHitConfig != null)
            {
                tran_DoubleHitAttribute.gameObject.SetActive(true);
                var child = tran_DoubleHitAttribute.Find(string.Format("Attribute_{0}", 0));
                child.GetComponent<Text>().text = string.Format(" {0}", exclusive.DoubleHitConfig.Des);
                child.gameObject.SetActive(true);
            }
            if (exclusive.ExclusiveConfig.Type > 0)
            {
                tran_GroupAttribute.gameObject.SetActive(true);

                ExclusiveSuit exclusiveSuit = user.GetExclusiveSuit(exclusive.ExclusiveConfig);

                tran_GroupAttribute.Find("Title").GetComponent<Text>().text = string.Format("[套装属性] ({0}/6)   增加一个上阵技能栏", exclusiveSuit.ActiveCount);

                for (int index = 0; index < 3; index++)
                {
                    var nameChild = tran_GroupAttribute.Find(string.Format("Name_{0}", index));
                    nameChild.gameObject.SetActive(true);

                    ExclusiveSuitItem suitItem1 = exclusiveSuit.ItemList[index * 2];
                    string groupColor = QualityConfigHelper.GetEquipGroupColor(suitItem1.Active);
                    nameChild.GetComponent<Text>().text = string.Format("<color=#{0}>{1}</color>", groupColor, suitItem1.Name);

                    var attrChild = tran_GroupAttribute.Find(string.Format("Attribute_{0}", index));
                    attrChild.gameObject.SetActive(true);
                    ExclusiveSuitItem suitItem2 = exclusiveSuit.ItemList[index * 2 + 1];
                    groupColor = QualityConfigHelper.GetEquipGroupColor(suitItem2.Active);
                    attrChild.GetComponent<Text>().text = string.Format("<color=#{0}>{1}</color>", groupColor, suitItem2.Name);
                }
            }

            this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
            this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);
            this.btn_UnEquip.gameObject.SetActive(this.boxItem.BoxId == -1);
            this.btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
            this.btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);

            if (equipPositioin < -1 || this.BoxType != ComBoxType.Bag) //不可操作
            {
                this.btn_Equip.gameObject.SetActive(false);
                this.btn_UnEquip.gameObject.SetActive(false);
                this.btn_Recovery.gameObject.SetActive(false);
                this.btn_Lock.gameObject.SetActive(false);
                this.btn_Unlock.gameObject.SetActive(false);
            }

            if (this.BoxType == ComBoxType.Exclusive_Devour)
            {
                if (this.equipPositioin <= 0)
                {
                    this.btn_Select.gameObject.SetActive(true);
                }
                else
                {
                    this.btn_Deselect.gameObject.SetActive(true);
                }
            }
        }

        private void ShowRune(List<int> runeIdList)
        {
            Item_Rune[] runes = tran_SkillAttribute.GetComponentsInChildren<Item_Rune>(true);

            for (int i = 0; i < runes.Length; i++)
            {
                if (i < runeIdList.Count)
                {
                    runes[i].gameObject.SetActive(true);
                    runes[i].SetContent(runeIdList[i]);
                }
                else
                {
                    runes[i].gameObject.SetActive(false);
                }
            }
            tran_SkillAttribute.gameObject.SetActive(true);
        }

        private void ShowSuit(List<int> suitIdList, List<int> countList, int max)
        {
            Item_Suit[] suits = tran_SuitAttribute.GetComponentsInChildren<Item_Suit>(true);

            for (int i = 0; i < suits.Length; i++)
            {
                if (i < suitIdList.Count)
                {
                    suits[i].gameObject.SetActive(true);
                    suits[i].SetContent(suitIdList[i], countList[i], max);
                }
                else
                {
                    suits[i].gameObject.SetActive(false);
                }
            }
            tran_SuitAttribute.gameObject.SetActive(true);
        }

        private string FormatAttrText(int attr, long val, int percent)
        {
            string unit = "";

            List<int> percents = ConfigHelper.PercentAttrIdList.ToList().ToList(); ;

            if (percents.Contains(attr))
            {
                unit = "%";
            }

            string refineText = "";
            long refineAttr = val * percent / 100;
            if (refineAttr > 0)
            {
                refineText = "+" + StringHelper.FormatNumber(refineAttr);
            }

            string text = StringHelper.FormatNumber(val) + refineText + unit + PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()];

            return text;
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

        private void OnClick_Select()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new ComBoxSelectEvent()
            {
                Type = this.BoxType,
                BoxItem = this.boxItem
            });
        }

        private void OnClick_Deselect()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new ComBoxDeselectEvent()
            {
                Type = this.BoxType,
                BoxItem = this.boxItem,
                Position = this.equipPositioin
            });
        }
    }
}
