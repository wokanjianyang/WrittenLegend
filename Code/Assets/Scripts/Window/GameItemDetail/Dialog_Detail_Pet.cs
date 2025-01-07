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

        [Title("背景图片")]
        public Image img_Background;
        public Sprite[] list_BackgroundImgs;

        [LabelText("名称")]
        public Text tmp_Title;

        [LabelText("天赋")]
        public Transform tran_BaseAttribute;

        [LabelText("属性")]
        public Transform tran_HideAttribute;

        [LabelText("技能")]
        public Transform tran_SkillAttribute;

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
