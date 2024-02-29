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
    public class Dialog_Detail : MonoBehaviour, IBattleLife
    {
        public Text Txt_Title;

        public Text Txt_NeedLevel;
        public Text Txt_Memo;

        public Button Btn_Recovery;
        public Button Btn_Use;
        public Button Btn_UseAll;

        public Button Btn_Learn;
        public Button Btn_Close;

        private BoxItem boxItem;
        private ComBoxType BoxType;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.Btn_Use.onClick.AddListener(this.OnUse);
            this.Btn_UseAll.onClick.AddListener(this.OnUseAll);

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

            this.Btn_Recovery.gameObject.SetActive(false);
            this.Btn_Use.gameObject.SetActive(false);
            this.Btn_UseAll.gameObject.SetActive(false);
            this.Btn_Learn.gameObject.SetActive(false);

            this.boxItem = e.boxItem;
            this.BoxType = e.Type;

            var titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);
            this.Txt_Title.text = string.Format("<color=#{0}>{1}</color>", titleColor, this.boxItem.Item.Name);

            string color = "green";
            Txt_Memo.text = this.boxItem.Item.ItemConfig.Des;
            Txt_NeedLevel.text = string.Format("<color={0}>需要等级{1}</color>", color, this.boxItem.Item.Level);

            User user = GameProcessor.Inst.User;

            switch ((ItemType)this.boxItem.Item.Type)
            {
                case ItemType.SkillBox://技能书
                    {
                        var skillBox = this.boxItem.Item as SkillBook;

                        var isLearn = user.SkillList.Find(b => b.SkillId == this.boxItem.Item.ConfigId) == null;

                        this.Btn_Learn.gameObject.SetActive(isLearn);
                        this.Btn_Use.gameObject.SetActive(!isLearn);
                        this.Btn_UseAll.gameObject.SetActive(!isLearn);
                    }
                    break;
                case ItemType.GiftPack:
                    {
                        GiftPack giftPack = this.boxItem.Item as GiftPack;
                        this.Btn_Use.gameObject.SetActive(true);
                    }
                    break;
                case ItemType.ExpPack:
                case ItemType.GoldPack:
                case ItemType.Ticket:
                    {
                        this.Btn_Use.gameObject.SetActive(true);
                        this.Btn_UseAll.gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Material:
                    {

                    }
                    break;
                case ItemType.Card:
                    {
                        Btn_Recovery.gameObject.SetActive(true);
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
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

        private void OnUse()
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
