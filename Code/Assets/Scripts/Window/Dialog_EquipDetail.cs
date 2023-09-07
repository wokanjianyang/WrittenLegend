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

        [LabelText("随机属性")]
        public Transform tran_RandomAttribute;

        [LabelText("品质属性")]
        public Transform tran_QualityAttribute;

        [LabelText("技能属性")]
        public Transform tran_SkillAttribute;

        [LabelText("词条套装")]
        public Transform tran_SuitAttribute;

        [LabelText("套装属性")]
        public Transform tran_GroupAttribute;

        [LabelText("普通道具属性")]
        public Transform tran_NormalAttribute;

        [Title("导航")]
        [LabelText("穿戴")]
        public Button btn_Equip;

        [LabelText("卸下")]
        public Button btn_UnEquip;

        [LabelText("学习")]
        public Button btn_Learn;

        [LabelText("使用")]
        public Button btn_Upgrade;

        [LabelText("全部使用")]
        public Button btn_UseAll;

        [LabelText("回收")]
        public Button btn_Recovery;

        [LabelText("锻造")]
        public Button btn_Forging;
        
        [LabelText("锁定装备")]
        public Button btn_Lock;
                
        [LabelText("解除锁定装备")]
        public Button btn_Unlock;
        
        private BoxItem boxItem;
        private int equipPositioin;

        private RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);
            this.btn_Learn.onClick.AddListener(this.OnLearnSkill);

            this.btn_Upgrade.onClick.AddListener(this.OnUpgradeSkill);
            this.btn_UseAll.onClick.AddListener(this.OnUseAll);

            this.btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.btn_Forging.onClick.AddListener(this.OnForging);
            
            this.btn_Lock.onClick.AddListener(this.OnClick_Lock);
            this.btn_Unlock.onClick.AddListener(this.OnClick_Unlock);
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            this.gameObject.SetActive(false); 

            GameProcessor.Inst.EventCenter.AddListener<ShowEquipDetailEvent>(this.OnShowEquipDetailEvent);
            this.rectTransform = this.transform.GetComponent<RectTransform>();
        }

        private void OnShowEquipDetailEvent(ShowEquipDetailEvent e)
        {
            this.gameObject.SetActive(true);
            tran_BaseAttribute.gameObject.SetActive(false);
            tran_NormalAttribute.gameObject.SetActive(false);
            tran_RandomAttribute.gameObject.SetActive(false);
            tran_QualityAttribute.gameObject.SetActive(false);
            tran_SkillAttribute.gameObject.SetActive(false);
            tran_SuitAttribute.gameObject.SetActive(false);
            tran_GroupAttribute.gameObject.SetActive(false);
            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Learn.gameObject.SetActive(false);
            this.btn_Upgrade.gameObject.SetActive(false);
            this.btn_UseAll.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Forging.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);

            // this.transform.position = this.GetBetterPosition(e.Position);
            // this.img_Background.sprite = this.list_BackgroundImgs[this.item.GetQuality() - 1];
            this.boxItem = e.boxItem;
            this.equipPositioin = e.EquipPosition;

            var titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);
            this.tmp_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, this.boxItem.Item.Name);

            string color = "green";

            User user = GameProcessor.Inst.User;
            switch ((ItemType)this.boxItem.Item.Type)
            {
                case ItemType.Equip://装备
                    {
                        Equip equip = this.boxItem.Item as Equip;

                        int basePercent = 0;
                        int qualityPercent = 0;
                        if (user.MagicEquipRefine.TryGetValue(equipPositioin, out MagicData refineData))
                        {
                            EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(refineData.Data);
                            if (refineConfig != null)
                            {
                                basePercent = refineConfig.BaseAttrPercent;
                                qualityPercent = refineConfig.QualityAttrPercent;
                            }
                        }

                        if (equip.BaseAttrList != null && equip.BaseAttrList.Count > 0)
                        {
                            tran_BaseAttribute.gameObject.SetActive(true);
                            tran_BaseAttribute.Find("Title").GetComponent<Text>().text = "[基础属性]";
                            tran_BaseAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);

                            var BaseAttrList = equip.BaseAttrList.ToList();

                            for (int index = 0; index < 6; index++)
                            {
                                var child = tran_BaseAttribute.Find(string.Format("Attribute_{0}", index));

                                if (index < BaseAttrList.Count)
                                {
                                    child.GetComponent<Text>().text = FormatAttrText(BaseAttrList[index].Key, BaseAttrList[index].Value, basePercent);
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

                            var AttrEntryList = equip.AttrEntryList.ToList();

                            for (int index = 0; index < 6; index++)
                            {
                                var child = tran_RandomAttribute.Find(string.Format("Attribute_{0}", index));

                                if (index < AttrEntryList.Count)
                                {
                                    child.GetComponent<Text>().text = FormatAttrText(AttrEntryList[index].Key, AttrEntryList[index].Value, qualityPercent);
                                    child.gameObject.SetActive(true);
                                }
                                else
                                {
                                    child.gameObject.SetActive(false);
                                }
                            }
                        }

                        if (equip.QualityAttrList != null && equip.QualityAttrList.Count > 0)
                        {
                            tran_QualityAttribute.gameObject.SetActive(true);
                            tran_QualityAttribute.Find("Title").GetComponent<Text>().text = "[品质属性]";

                            var QualityAttrList = equip.QualityAttrList.ToList();

                            for (int index = 0; index < 4; index++)
                            {
                                var child = tran_QualityAttribute.Find(string.Format("Attribute_{0}", index));

                                if (index < QualityAttrList.Count)
                                {
                                    child.GetComponent<Text>().text = FormatAttrText(QualityAttrList[index].Key, QualityAttrList[index].Value, qualityPercent);
                                    child.gameObject.SetActive(true);
                                }
                                else
                                {
                                    child.gameObject.SetActive(false);
                                }
                            }
                        }

                        if (equip.SkillRuneConfig != null)
                        {
                            int index = 0;
                            tran_SkillAttribute.gameObject.SetActive(true);
                            tran_SkillAttribute.Find("Title").GetComponent<Text>().text = equip.SkillRuneConfig.Name;

                            var child = tran_SkillAttribute.Find(string.Format("Attribute_{0}", index));
                            child.GetComponent<Text>().text = string.Format(" {0}", equip.SkillRuneConfig.Des);
                            child.gameObject.SetActive(true);

                            index = 1;
                            child = tran_SkillAttribute.Find(string.Format("Attribute_{0}", index));
                            child.GetComponent<Text>().text = string.Format(" 最大生效数量{0}", equip.SkillRuneConfig.Max);
                            child.gameObject.SetActive(true);
                        }

                        if (equip.SkillSuitConfig != null)
                        {
                            int suitCount = user.GetSuitCount(equip.SkillSuitConfig.Id);

                            int index = 0; 
                            tran_SuitAttribute.gameObject.SetActive(true);
                            tran_SuitAttribute.Find("Title").GetComponent<Text>().text = equip.SkillSuitConfig.Name + string.Format("({0}/{1})", suitCount, user.SuitMax);

                            var child = tran_SuitAttribute.Find(string.Format("Attribute_{0}", index));
                            child.GetComponent<Text>().text = string.Format(" {0}", equip.SkillSuitConfig.Des);
                            child.gameObject.SetActive(true);
                        }

                        if (equip.Part <= 10)
                        {
                            tran_GroupAttribute.gameObject.SetActive(true);

                            EquipSuit equipSuit = user.GetEquipSuit(equip.EquipConfig);

                            int groupCount = 0;
                            int nameIndex = 0;
                            for (int i = 0; i < 2; i++)
                            {
                                EquipSuitItem eg = equipSuit.ItemList[i];

                                var nameChild = tran_GroupAttribute.Find(string.Format("Name_{0}", nameIndex++));
                                string groupColor = QualityConfigHelper.GetEquipGroupColor(eg.Active);
                                if (eg.Active)
                                {

                                    nameChild.GetComponent<Text>().text = string.Format("<color=#{0}>{1}</color>", groupColor, eg.Name);
                                    groupCount++;
                                }
                                else
                                {
                                    nameChild.GetComponent<Text>().text = string.Format("<color=#{0}>{1}</color>", groupColor, eg.Name);
                                }

                                nameChild.gameObject.SetActive(true);
                            }

                            tran_GroupAttribute.Find("Title").GetComponent<Text>().text = string.Format("[套装属性] ({0}/2)", groupCount);

                            //
                            EquipGroupConfig config = equipSuit.Config;

                            for (int index = 0; index < 3; index++)
                            {
                                var attrChild = tran_GroupAttribute.Find(string.Format("Attribute_{0}", index));

                                if (index < config.AttrIdList.Length)
                                {
                                    string groupColor = QualityConfigHelper.GetEquipGroupColor(groupCount >= 2);

                                    string attrText = FormatAttrText(config.AttrIdList[index], config.AttrValueList[index], 0);
                                    attrChild.GetComponent<Text>().text = string.Format("<color=#{0}>{1}</color>", groupColor, attrText);

                                    attrChild.gameObject.SetActive(true);
                                }
                                else
                                {
                                    attrChild.gameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            tran_GroupAttribute.gameObject.SetActive(false);
                        }

                        this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);
                        this.btn_UnEquip.gameObject.SetActive(this.boxItem.BoxId == -1);

                        if (equip.Part <= 10 && !this.boxItem.Item.IsLock)
                        {
                            this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1);
                        }
                        else
                        {
                            this.btn_Recovery.gameObject.SetActive(false);
                        }

                        this.btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
                        this.btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);
                    }
                    break;
                case ItemType.SkillBox://技能书
                    {
                        var skillBox = this.boxItem.Item as SkillBook;
                        tran_NormalAttribute.gameObject.SetActive(true);
                        tran_NormalAttribute.Find("Title").GetComponent<Text>().text = skillBox.ItemConfig.Des;
                        tran_NormalAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);
                        var isLearn = user.SkillList.Find(b => b.SkillId == this.boxItem.Item.ConfigId) == null;
                        
                        this.btn_Learn.gameObject.SetActive(isLearn);
                        this.btn_Upgrade.gameObject.SetActive(!isLearn);
                        this.btn_UseAll.gameObject.SetActive(!isLearn);
                        //this.btn_Learn.interactable = this.item.Level <= UserData.Load().Level;
                    }
                    break;
                case ItemType.GiftPack:
                case ItemType.ExpPack:
                case ItemType.GoldPack:
                case ItemType.Ticket:
                    {
                        //var giftPack = this.item as GiftPack;

                        tran_NormalAttribute.gameObject.SetActive(true);
                        tran_NormalAttribute.Find("Title").GetComponent<Text>().text = this.boxItem.Item.ItemConfig.Des;
                        //tran_NormalAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.item.Level);

                        this.btn_Learn.gameObject.SetActive(false);

                        this.btn_Upgrade.gameObject.SetActive(true);
                        this.btn_UseAll.gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Material:
                    {
                        tran_NormalAttribute.gameObject.SetActive(true);
                        tran_NormalAttribute.Find("Title").GetComponent<Text>().text = this.boxItem.Item.ItemConfig.Des;
                        tran_NormalAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);
                    }
                    break;
                default:
                    Log.Debug("未知的类型");
                    break;
            }


            // var size = this.rect_Content.sizeDelta;
            // size.x = 804;
            // this.rectTransform.sizeDelta = size;
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

        private void OnLearnSkill()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new SkillBookLearnEvent()
            {
                BoxItem = this.boxItem,
            });
        }

        private void OnRecovery()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
            {
                BoxItem = this.boxItem,
            });
        }

        private void OnForging()
        {
            this.gameObject.SetActive(false);
            
            GameProcessor.Inst.EventCenter.Raise(new ForgingEvent()
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
        private void OnUpgradeSkill()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            {
                Quantity = 1,
                BoxItem = this.boxItem
            });
        }
        private void OnUseAll()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            {
                Quantity = -1,
                BoxItem = this.boxItem
            });
        }
    }
}
