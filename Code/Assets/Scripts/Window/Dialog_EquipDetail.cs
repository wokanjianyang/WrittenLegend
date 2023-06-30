﻿using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public class Dialog_EquipDetail : MonoBehaviour, IBattleLife
    {
        [LabelText("容器")]
        public RectTransform rect_Content;

        [Title("道具数据")]
        [LabelText("背景")]
        public Image img_Background;

        [LabelText("背景图片")]
        public Sprite[] list_BackgroundImgs;

        [LabelText("名称")]
        public Text tmp_Title;

        [LabelText("基础属性")]
        public Transform tran_BaseAttribute;

        [LabelText("隐藏属性")]
        public Transform tran_HideAttribute;

        [LabelText("品质属性")]
        public Transform tran_QualityAttribute;

        [LabelText("技能属性")]
        public Transform tran_SkillAttribute;

        [LabelText("套装属性")]
        public Transform tran_SuitAttribute;

        [LabelText("普通道具属性")]
        public Transform tran_NormalAttribute;

        [Title("导航")]
        [LabelText("穿戴")]
        public Button btn_Equip;

        [LabelText("卸下")]
        public Button btn_UnEquip;

        [LabelText("学习")]
        public Button btn_Learn;

        [LabelText("升级")]
        public Button btn_Upgrade;

        [LabelText("回收")]
        public Button btn_Recovery;

        private Item item;
        private int boxId;
        private int equipPositioin;

        private RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);
            this.btn_Learn.onClick.AddListener(this.OnLearnSkill);
            this.btn_Upgrade.onClick.AddListener(this.OnUpgradeSkill);
            this.btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.gameObject.SetActive(false); 
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowEquipDetailEvent>(this.OnShowEquipDetailEvent);
            this.rectTransform = this.transform.GetComponent<RectTransform>();
        }

        private void OnShowEquipDetailEvent(ShowEquipDetailEvent e)
        {
            this.gameObject.SetActive(true);
            tran_BaseAttribute.gameObject.SetActive(false);
            tran_NormalAttribute.gameObject.SetActive(false);
            tran_QualityAttribute.gameObject.SetActive(false);
            tran_SkillAttribute.gameObject.SetActive(false);
            tran_SuitAttribute.gameObject.SetActive(false);
            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Learn.gameObject.SetActive(false);
            this.btn_Upgrade.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);

            this.transform.position = this.GetBetterPosition(e.Position);
            this.tmp_Title.text = e.Item.Name;
            this.item = e.Item;
            this.boxId = e.BoxId;
            this.equipPositioin = e.EquipPosition;

            var titleColor = QualityConfigHelper.GetColor(this.item.GetQuality());
            this.img_Background.sprite = this.list_BackgroundImgs[this.item.GetQuality() - 1];
            tmp_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, this.item.Name);

            string color = "green";
            bool levelLess = this.item.Level > GameProcessor.Inst.User.Level;
            if (levelLess)
            {
                color = "red";
            }

            User user = GameProcessor.Inst.User;
            switch ((ItemType)this.item.Type)
            {
                case ItemType.Equip://装备
                    {
                        Equip equip = this.item as Equip;

                        if (equip.BaseAttrList != null)
                        {
                            int index = 0;
                            tran_BaseAttribute.gameObject.SetActive(true);
                            tran_BaseAttribute.Find("Title").GetComponent<Text>().text = "[基础属性]";
                            tran_BaseAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.item.Level);

                            foreach (var a in equip.BaseAttrList)
                            {
                                var child = tran_BaseAttribute.Find(string.Format("Attribute_{0}", index));
                                child.GetComponent<Text>().text =  FormatAttrText(a.Key, a.Value);
                                child.gameObject.SetActive(true);
                                index++;
                            }
                        }

                        if (equip.AttrEntryList != null && equip.AttrEntryList.Count > 0)
                        {
                            int index = 0;
                            tran_QualityAttribute.gameObject.SetActive(true);
                            tran_QualityAttribute.Find("Title").GetComponent<Text>().text = "[品质属性]";
                            foreach (var a in equip.AttrEntryList) //TODO 有bug，如果之前看的有4条属性，这次看的只有3条，那么最后一条显示的是上次的属性
                            {
                                var child = tran_QualityAttribute.Find(string.Format("Attribute_{0}", index));
                                child.GetComponent<Text>().text = FormatAttrText(a.Key, a.Value);
                                child.gameObject.SetActive(true);
                                index++;
                            }
                        }

                        if (equip.SkillRuneConfig != null) {
                            int index = 0;
                            tran_SkillAttribute.gameObject.SetActive(true);
                            tran_SkillAttribute.Find("Title").GetComponent<Text>().text = equip.SkillRuneConfig.Name;

                            var child = tran_SkillAttribute.Find(string.Format("Attribute_{0}", index));
                            child.GetComponent<Text>().text = string.Format(" {0}", equip.SkillRuneConfig.Des);
                            child.gameObject.SetActive(true);
                        }

                        if (equip.SkillSuitConfig != null)
                        {
                            int suitCount = user.GetSuitCount(equip.SkillSuitConfig.Id);

                            int index = 0;
                            tran_SuitAttribute.gameObject.SetActive(true);
                            tran_SuitAttribute.Find("Title").GetComponent<Text>().text = equip.SkillSuitConfig.Name + string.Format("({0}/{1})", suitCount, SkillSuitHelper.SuitMax);

                            var child = tran_SuitAttribute.Find(string.Format("Attribute_{0}", index));
                            child.GetComponent<Text>().text = string.Format(" {0}", equip.SkillSuitConfig.Des);
                            child.gameObject.SetActive(true);
                        }

                        this.btn_Equip.gameObject.SetActive(this.boxId != -1 && !levelLess);
                        this.btn_UnEquip.gameObject.SetActive(this.boxId == -1);
                        this.btn_Recovery.gameObject.SetActive(this.boxId != -1);
                    }
                    break;
                case ItemType.SkillBox://技能书
                    {
                        var skillBox = this.item as SkillBook;
                        tran_NormalAttribute.gameObject.SetActive(true);
                        tran_NormalAttribute.Find("Title").GetComponent<Text>().text = skillBox.ItemConfig.Des;
                        tran_NormalAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.item.Level);
                        var isLearn = user.SkillList.Find(b => b.SkillId == this.item.ConfigId) == null;
                        this.btn_Learn.gameObject.SetActive(isLearn && !levelLess);
                        if (isLearn) {
                            var text = btn_Learn.GetComponentInChildren<Text>();
                            text.text = "学习";
                        }
                        this.btn_Upgrade.gameObject.SetActive(!isLearn);

                        //this.btn_Learn.interactable = this.item.Level <= UserData.Load().Level;
                    }
                    break;
                case ItemType.GiftPack: 
                    {
                        var giftPack = this.item as GiftPack;

                        tran_NormalAttribute.gameObject.SetActive(true);
                        tran_NormalAttribute.Find("Title").GetComponent<Text>().text = giftPack.Config.Des;
                        tran_NormalAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.item.Level);

                        this.btn_Learn.gameObject.SetActive(true && !levelLess);
                        var text = btn_Learn.GetComponentInChildren<Text>();
                        text.text = "使用";

                        this.btn_Upgrade.gameObject.SetActive(false);
                    }
                    break;
                case ItemType.Material: {
                        tran_NormalAttribute.gameObject.SetActive(true);
                        tran_NormalAttribute.Find("Title").GetComponent<Text>().text = item.ItemConfig.Des;
                        tran_NormalAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.item.Level);
                    }
                    break;
                default:
                    Log.Debug("未知的类型");
                    break;
            }


            var size = this.rect_Content.sizeDelta;
            size.x = 289;
            this.rectTransform.sizeDelta = size;
        }

        private string FormatAttrText(int attr, long val)
        {
            string unit = "点";

            List<int> percents = (new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 41, 43, 45 }).ToList(); ;

            if (percents.Contains(attr))
            {
                unit = "%";
            }

            string text = val + unit + PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()];

            return text;
        }

        private void OnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                Item = this.item,
                BoxId = this.boxId
            });
        }

        private void OnUnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                IsWear = false,
                Item = this.item,
                BoxId = this.boxId,
                Position = this.equipPositioin,
            });
        }

        private void OnLearnSkill()
        {
            this.gameObject.SetActive(false);

            if ((ItemType)this.item.Type == ItemType.SkillBox)
            {
                GameProcessor.Inst.EventCenter.Raise(new SkillBookEvent()
                {
                    IsLearn = true,
                    Item = this.item,
                    BoxId = this.boxId
                });
            }
            else if ((ItemType)this.item.Type == ItemType.GiftPack)
            {
                GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
                {
                    Item = this.item,
                    BoxId = this.boxId
                });
            }
        }

        private void OnUpgradeSkill()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new SkillBookEvent()
            {
                IsLearn = false,
                Item = this.item,
                BoxId = this.boxId
            }); ;
        }

        private void OnRecovery()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
            {
                Item = this.item,
                BoxId = this.boxId
            }); ;
        }

        private Vector3 GetBetterPosition(Vector3 position)
        {

            var maxRight = position.x + this.rectTransform.sizeDelta.x;
            var maxDown = position.y - this.rectTransform.sizeDelta.y;
            Vector3 offset = Vector3.zero;
            if (maxDown < 0)
            {
                offset.y = 0 - maxDown;
            }
            var maxWidth = 960 - 63;
            if (maxRight > maxWidth)
            {
                offset.x = maxWidth - maxRight;
            }

            Vector3 betterPosition = position + offset;


            return betterPosition;
        }
    }
}
