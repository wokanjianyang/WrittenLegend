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

        [LabelText("红装属性")]
        public Transform tran_RedAttribute;

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
            tran_RedAttribute.gameObject.SetActive(false);

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

            Equip equip = this.boxItem.Item as Equip;

            string name = equip.Name + "(" + ConfigHelper.LayerChinaList[equip.Layer] + "阶)";
            this.tmp_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, name);

            string color = "green";

            User user = GameProcessor.Inst.User;





            long basePercent = 0;
            long qualityPercent = 0;

            long refineLevel = user.GetRefineLevel(equipPositioin);
            if (refineLevel > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel);
                basePercent = refineConfig.GetBaseAttrPercent(refineLevel);
                qualityPercent = refineConfig.GetQualityAttrPercent(refineLevel);
            }

            IDictionary<int, long> BaseAttrList = equip.GetBaseAttrList();

            if (BaseAttrList != null && BaseAttrList.Count > 0)
            {
                tran_BaseAttribute.gameObject.SetActive(true);
                tran_BaseAttribute.Find("Title").GetComponent<Text>().text = "[基础属性]";
                tran_BaseAttribute.Find("NeedLevel").GetComponent<Text>().text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);

                var btList = BaseAttrList.ToList();

                for (int index = 0; index < 6; index++)
                {
                    var child = tran_BaseAttribute.Find(string.Format("Attribute_{0}", index));

                    if (index < btList.Count())
                    {
                        child.GetComponent<Text>().text = FormatAttrText(btList[index].Key, btList[index].Value, basePercent);
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
                        int attrId = AttrEntryList[index].Key;
                        long attrBaseValue = AttrEntryList[index].Value;
                        long attrHoneVal = equip.GetHoneValue(attrId);
                        long attrRiseValue = (attrBaseValue + attrHoneVal) * qualityPercent / 100;

                        child.GetComponent<Text>().text = FormatEquipAttrText(attrId, attrBaseValue, attrHoneVal, attrRiseValue);
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
                List<int> runeIdList = new List<int>();
                if (equip.RuneConfigId > 0)
                {
                    runeIdList.Add(equip.RuneConfigId);
                }

                ShowRune(runeIdList);
            }

            if (equip.SkillSuitConfig != null)
            {
                int suitCount = user.GetSuitCount(equip.SkillSuitConfig.Id);

                List<int> suitIdList = new List<int>();
                suitIdList.Add(equip.SkillSuitConfig.Id);

                List<int> suitCountList = new List<int>();
                suitCountList.Add(suitCount);

                this.ShowSuit(suitIdList, suitCountList, user.SuitMax);
            }


            if (equip.Part <= 10)
            {
                EquipSuit equipSuit = user.GetEquipSuit(equip.EquipConfig);

                if (equipSuit.Config != null)
                {
                    tran_GroupAttribute.gameObject.SetActive(true);

                    int groupCount = 0;
                    int nameIndex = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        var nameChild = tran_GroupAttribute.Find(string.Format("Name_{0}", nameIndex++));

                        if (i >= equipSuit.ItemList.Count)
                        {
                            nameChild.gameObject.SetActive(false);
                        }
                        else
                        {
                            EquipSuitItem eg = equipSuit.ItemList[i];

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

                if (equip.GetQuality() >= 6)
                {
                    tran_RedAttribute.gameObject.SetActive(true);

                    EquipRedSuit red = user.GetEquipRedConfig(equip.EquipConfig.Role);

                    this.ShowRed(red);

                    if (equip.Layer > 1)
                    {
                        this.btn_Restore.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
                    }
                    //else
                    //{
                    //    this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
                    //}
                }
            }


            this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);
            this.btn_UnEquip.gameObject.SetActive(this.boxItem.BoxId == -1);
            this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
            //this.btn_Restore.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
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

        private void ShowRed(EquipRedSuit redSuit)
        {
            Item_Equip_Red[] reds = tran_RedAttribute.GetComponentsInChildren<Item_Equip_Red>(true);

            for (int i = 0; i < reds.Length; i++)
            {
                reds[i].gameObject.SetActive(true);
                reds[i].SetContent(redSuit.List[i]);
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

        private string FormatAttrText(int attr, long val, long percent)
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

        private string FormatEquipAttrText(int attrId, long baseValue, long riseValue, long percentValue)
        {
            string unit = "";

            List<int> percents = ConfigHelper.PercentAttrIdList.ToList().ToList(); ;

            if (percents.Contains(attrId))
            {
                unit = "%";
            }

            string text = baseValue + "";
            if (riseValue > 0)
            {
                text = "(" + baseValue + "+" + riseValue + ")";
            }

            if (percentValue > 0)
            {
                text += "+" + percentValue;
            }


            text = text + unit + PlayerHelper.PlayerAttributeMap[((AttributeEnum)attrId).ToString()];

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
