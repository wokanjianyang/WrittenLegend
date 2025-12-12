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
    public class Dialog_Detail_Pet : MonoBehaviour, IBattleLife
    {
        [LabelText("容器")]
        public RectTransform rect_Content;

        //[Title("背景图片")]
        //public Image img_Background;
        //public Sprite[] list_BackgroundImgs;

        [LabelText("名称")]
        public Text TxtName;
        public Text TxtLevel;
        public Text TxtLayer;

        [LabelText("资质")]
        public Transform tran_BaseAttribute;

        [LabelText("属性")]
        public Transform tran_FinalAttribute;

        [LabelText("技能")]
        public Transform tran_SkillAttribute;
        public Text TxtSkillName;
        public Text TxtSkillDes;

        [Title("导航")]
        public Button btn_Equip;
        public Button btn_UnEquip;
        public Button btn_Recovery;
        public Button btn_Restore;

        public Button btn_Lock;
        public Button btn_Unlock;

        public Button Btn_Close;

        private BoxItem boxItem;

        private RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            //this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);


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

            GameProcessor.Inst.EventCenter.AddListener<ShowPetDetailEvent>(this.OnShowEvent);
            this.rectTransform = this.transform.GetComponent<RectTransform>();
        }

        private void OnShowEvent(ShowPetDetailEvent e)
        {
            this.gameObject.SetActive(true);

            tran_BaseAttribute.gameObject.SetActive(false);
            tran_FinalAttribute.gameObject.SetActive(false);
            tran_SkillAttribute.gameObject.SetActive(false);

            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Restore.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);

            this.boxItem = e.boxItem;

            string titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);

            Pet pet = this.boxItem.Item as Pet;

            this.TxtName.text = string.Format("<color=#{0}>{1}</color>", titleColor, pet.Name);
            this.TxtLevel.text = pet.PetLevel.Data + "";
            this.TxtLayer.text = pet.PetLayer.Data + "";

            List<KeyValuePair<int, long>> flairs = pet.GetTotalFlairs().ToList();

            if (flairs != null && flairs.Count > 0)
            {
                tran_BaseAttribute.gameObject.SetActive(true);
                Transform gridBase = tran_BaseAttribute.Find("Grid_Base");

                for (int index = 0; index < 10; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", index));

                    if (index < flairs.Count())
                    {
                        child.GetComponent<Text>().text = StringHelper.FormatAttrValueName(flairs[index].Key) + "：" + flairs[index].Value;
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                int fc = pet.GetDevourCount();
                for (int index = 0; index < 2; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", 10 + index));
                    if (index < pet.DevourFlairs.Count() && fc <= index)
                    {
                        child.GetComponent<Text>().text = StringHelper.FormatAttrValueName(pet.DevourFlairs[index].Key) + "：" + pet.DevourFlairs[index].Value.Data;
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }


            List<KeyValuePair<int, double>> attrList = pet.GetBaseAttr().ToList();

            if (attrList.Count > 0)
            {
                tran_FinalAttribute.gameObject.SetActive(true);
                Transform gridBase = tran_FinalAttribute.Find("Grid_Base");

                for (int index = 0; index < 10; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", index));

                    if (index < attrList.Count())
                    {
                        child.GetComponent<Text>().text = StringHelper.FormatAttrText(attrList[index].Key, attrList[index].Value);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (pet.Role > 0)
            {
                tran_SkillAttribute.gameObject.SetActive(true);

                TxtSkillName.text = ConfigHelper.RoleName[pet.Role - 1] + "所有技能：";
                TxtSkillDes.text = "系数增幅" + pet.GetSkillPercent() + "%";
            }

            this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);

            if (!this.boxItem.Item.IsLock)
            {
                if (pet.PetLevel.Data > 1)
                {
                    this.btn_Restore.gameObject.SetActive(this.boxItem.BoxId != -1);
                }
                else
                {
                    this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1);
                }
            }

            if (this.boxItem.Item.IsLock)
            {
                this.btn_Unlock.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Lock.gameObject.SetActive(true);
            }
        }

        private void OnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new PetBattleUpEvent()
            {
                BoxItem = this.boxItem,
            });
        }

        private void OnClick_Restore()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能重生", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重生消耗5000兆金币，其他材料全部返回。是否确认？", true,
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

            if (this.boxItem.Item.GetQuality() >= 5)
            {
                GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认回收？", true,
                () =>
                {
                    this.gameObject.SetActive(false);

                    GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
                    {
                        BoxItem = this.boxItem,
                    });
                }, () =>
                {

                });
            }
            else
            {
                this.gameObject.SetActive(false);

                GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
                {
                    BoxItem = this.boxItem,
                });
            }

        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        public void OnClick_Lock()
        {
            this.btn_Lock.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = true
            });

            this.btn_Unlock.gameObject.SetActive(true);
        }

        private void OnClick_Unlock()
        {
            this.btn_Unlock.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = false
            });

            this.btn_Lock.gameObject.SetActive(true);
        }
    }
}
