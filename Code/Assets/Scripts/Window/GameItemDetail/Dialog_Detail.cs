using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;
using Newtonsoft.Json;

namespace Game
{
    public class Dialog_Detail : MonoBehaviour, IBattleLife
    {
        public Text Txt_Title;

        public Text Txt_NeedLevel;
        public Text Txt_Memo;

        public Button Btn_Recovery;
        public Button Btn_Recovery_All;
        public Button Btn_Lose;
        public Button Btn_Learn;

        public Button Btn_Use;
        public Button Btn_Use_Batch;
        public Button Btn_UseAll;

        public Button Btn_Egg;

        public Button Btn_Close;

        private BoxItem boxItem;
        private ComBoxType BoxType;

        public Transform tf_Count;
        public InputField if_Count;
        public Button Btn_Confirm;
        public Button Btn_Cancle;

        private int Type = 0;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.Btn_Recovery_All.onClick.AddListener(this.OnRecoveryAll);
            this.Btn_Lose.onClick.AddListener(this.OnLose);
            this.Btn_Use.onClick.AddListener(this.OnUse);
            this.Btn_Use_Batch.onClick.AddListener(this.OnUseBatch);
            this.Btn_UseAll.onClick.AddListener(this.OnUseAll);
            this.Btn_Confirm.onClick.AddListener(this.OnConfirm);
            this.Btn_Cancle.onClick.AddListener(this.OnCancle);
            this.Btn_Egg.onClick.AddListener(this.OnEgg);

            this.Btn_Learn.onClick.AddListener(this.OnLearnSkill);
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

            GameProcessor.Inst.EventCenter.AddListener<ShowDetailEvent>(this.OnShow);
        }

        private void OnShow(ShowDetailEvent e)
        {
            this.gameObject.SetActive(true);

            User user = GameProcessor.Inst.User;

            this.Btn_Recovery.gameObject.SetActive(false);
            this.Btn_Recovery_All.gameObject.SetActive(false);
            this.Btn_Lose.gameObject.SetActive(false);
            this.Btn_Learn.gameObject.SetActive(false);

            this.Btn_Use.gameObject.SetActive(false);
            this.Btn_UseAll.gameObject.SetActive(false);
            this.Btn_Use_Batch.gameObject.SetActive(false);

            this.Btn_Egg.gameObject.SetActive(false);

            this.tf_Count.gameObject.SetActive(false);
            this.if_Count.text = "";


            this.boxItem = e.boxItem;
            this.BoxType = e.Type;

            var titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);
            this.Txt_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, this.boxItem.Item.Name);

            long number = this.boxItem.MagicNubmer.Data;

            string color = "green";

            if (user.Cycle.Data < this.boxItem.Item.Level)
            {
                color = "red";
            }

            if (this.boxItem.Item.ItemConfig != null)
            {
                Txt_Memo.text = this.boxItem.Item.ItemConfig.Des;
            }
            Txt_NeedLevel.text = string.Format("<color={0}>需要轮回{1}转</color>", color, this.boxItem.Item.Level);



            switch ((ItemType)this.boxItem.Item.Type)
            {
                case ItemType.SkillBox://技能书
                    {
                        var skillBox = this.boxItem.Item as SkillBook;

                        var isLearn = user.SkillList.Find(b => b.SkillId == this.boxItem.Item.ConfigId) == null;

                        this.Btn_Learn.gameObject.SetActive(isLearn);
                        this.Btn_Use.gameObject.SetActive(!isLearn);
                        this.Btn_Use_Batch.gameObject.SetActive(!isLearn);
                        this.Btn_UseAll.gameObject.SetActive(!isLearn);
                    }
                    break;
                case ItemType.GiftPack:
                    {
                        GiftPack giftPack = this.boxItem.Item as GiftPack;
                        Txt_Memo.text = giftPack.Des;
                        this.Btn_Use.gameObject.SetActive(true);

                        GiftPackConfig giftPackConfig = GiftPackConfigCategory.Instance.Get(giftPack.ConfigId);
                        if (giftPackConfig.OpenType == 1)
                        {
                            this.Btn_UseAll.gameObject.SetActive(true);
                        }
                        else
                        {
                            this.Btn_UseAll.gameObject.SetActive(false);
                        }
                        //
                    }
                    break;
                case ItemType.ExpPack:
                case ItemType.GoldPack:
                case ItemType.Ticket:
                case ItemType.Material_Usable:
                    {
                        this.Btn_Use.gameObject.SetActive(true);
                        if (number > 1)
                        {
                            this.Btn_Use_Batch.gameObject.SetActive(true);
                            this.Btn_UseAll.gameObject.SetActive(true);
                        }
                    }
                    break;
                case ItemType.Pet:
                    {
                        if (!AppHelper.PetEgging)
                        {
                            this.Btn_Egg.gameObject.SetActive(true);
                        }
                    }
                    break;
                case ItemType.Material:
                    {
                        this.Btn_Lose.gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Card:
                    {
                        //this.Btn_Recovery.gameObject.SetActive(true);
                        //this.Btn_Lose.gameObject.SetActive(false);
                    }
                    break;
                default:
                    {

                    }
                    break;
            }

            if (this.boxItem.Item.ItemConfig != null)
            {
                if (this.boxItem.Item.ItemConfig.RecoveryItemId > 0)
                {
                    this.Btn_Recovery.gameObject.SetActive(true);
                    this.Btn_Recovery_All.gameObject.SetActive(true);
                    this.Btn_Lose.gameObject.SetActive(false);
                }
            }

            if (this.BoxType != ComBoxType.Bag || user.Cycle.Data < this.boxItem.Item.Level) //不可操作
            {
                this.Btn_Recovery.gameObject.SetActive(false);
                this.Btn_Recovery_All.gameObject.SetActive(false);
                this.Btn_Lose.gameObject.SetActive(false);
                this.Btn_Use.gameObject.SetActive(false);
                this.Btn_UseAll.gameObject.SetActive(false);
                this.Btn_Learn.gameObject.SetActive(false);
                this.Btn_Use_Batch.gameObject.SetActive(false);
            }
        }

        private void OnRecovery()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能回收", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.tf_Count.gameObject.SetActive(true);
            this.Type = 1;

            long count = this.boxItem.MagicNubmer.Data;
            if_Count.placeholder.GetComponent<Text>().text = "最大输入" + count;
        }

        private void OnRecoveryAll()
        {
            this.gameObject.SetActive(false);
            GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
            {
                Quantity = -1,
                BoxItem = this.boxItem,
            });
        }

        private void OnLose()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能丢弃", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (this.Btn_Recovery.IsActive())
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "能回收的不能丢弃", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.SetActive(false);

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("不会有任何收益,是否确认丢弃道具？", true, () =>
            {
                GameProcessor.Inst.EventCenter.Raise(new LoseEvent()
                {
                    BoxItem = this.boxItem,
                });
            }, null);
        }

        private void OnUse()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            {
                Quantity = 1,
                BoxItem = this.boxItem
            });
        }

        private void OnEgg()
        {
            //AppHelper.PetEgging = true;
            //Txt_Memo.text = "请稍等几秒，孵化中...";
            //this.Btn_Egg.gameObject.SetActive(false);

            //int configId = this.boxItem.Item.ConfigId;
            //int count = GameProcessor.Inst.User.GetPetCount(configId);

            //int role = this.boxItem.Item.ItemConfig.UseParam;
            //List<KeyValuePair<int, int>> flairs = PetConfigCategory.Instance.BuildPetAttr(configId, role);

            //GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            //{
            //    Quantity = 1,
            //    BoxItem = this.boxItem,
            //    Flairs = flairs,
            //    Role = role
            //});

            //this.gameObject.SetActive(false);
            //AppHelper.PetEgging = false;

            //NetworkHelper.GetPet(configId, count,
            //            (WebResultWrapper result) =>
            //            {
            //                if (result.Code == StatusMessage.OK && result.Data.Count > 0)
            //                {
            //                    this.Txt_Memo.text = "孵化成功，请查收包裹";

            //                    GameProcessor.Inst.User.SetPetCount(configId);
            //                    AppHelper.PetEgging = false;

            //                    this.gameObject.SetActive(false);

            //                    List<KeyValuePair<int, int>> flairs = new List<KeyValuePair<int, int>>();

            //                    foreach (var kv in result.Data)
            //                    {
            //                        int attrId = int.Parse(kv.Key);
            //                        int attrValue = int.Parse(kv.Value);

            //                        flairs.Add(new KeyValuePair<int, int>(attrId, attrValue));
            //                    }

            //                    GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            //                    {
            //                        Quantity = -1,
            //                        BoxItem = this.boxItem,
            //                        Flairs = flairs
            //                    });

            //                }
            //                else
            //                {
            //                    this.Txt_Memo.text = "孵化失败，请重试.";
            //                }
            //            },
            //            () =>
            //            {
            //                this.Txt_Memo.text = "孵化失败，请重试";
            //            }
            //            );
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

        private void OnUseBatch()
        {
            this.tf_Count.gameObject.SetActive(true);
            this.Type = 2;

            long count = this.boxItem.MagicNubmer.Data;
            if_Count.placeholder.GetComponent<Text>().text = "最大输入" + count;
        }

        private void OnConfirm()
        {
            this.tf_Count.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            int.TryParse(if_Count.text, out int quantity);

            long count = this.boxItem.MagicNubmer.Data;
            if (quantity > count)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "数量超出了最大值", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (quantity > 0)
            {
                if (this.Type == 1)
                {
                    GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
                    {
                        Quantity = quantity,
                        BoxItem = this.boxItem,
                    });
                }
                else if (Type == 2)
                {

                    GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
                    {
                        Quantity = quantity,
                        BoxItem = this.boxItem
                    });
                }
            }
        }

        private void OnCancle()
        {
            this.tf_Count.gameObject.SetActive(false);
        }



        private void OnLearnSkill()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new SkillBookLearnEvent()
            {
                BoxItem = this.boxItem,
            });
        }
        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
