using Game.Data;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Defend : MonoBehaviour
    {
        //public Image Img_Active;
        public Text Txt_Name;

        public Button Btn_Start;
        public Text Txt_Start;
        public Text Txt_Over;

        private string[] names = new string[] { "��ͨ", "����", "ج��", "����" };

        private int Level = 0;
        private int Type = 0;

        // Update is called once per frame
        void Start()
        {
            Btn_Start.onClick.AddListener(() => { this.OnClick_Start(); });
        }

        private void OnEnable()
        {
            if (this.Level > 0)
            {
                this.Show();
            }
        }

        private void Show()
        {
            User user = GameProcessor.Inst.User;

            long p = user.MagicRecord[AchievementSourceType.Defend].Data - (this.Level - 1) * 100;

            //p = 0;
            if (p > 100)
            {
                Type = 3;
                Txt_Start.text = "ɨ��";
                Btn_Start.gameObject.SetActive(true);
            }
            else if (p >= 0)
            {
                Type = 2;
                Txt_Start.text = "��ս";
                Btn_Start.gameObject.SetActive(true);
            }
            else
            {
                Type = 1;
                Txt_Start.text = "��ս";
                Btn_Start.gameObject.SetActive(false);
            }


            DefendRecord record = user.DefendData.GetCurrentRecord(this.Level);
            if (record == null)
            {
                this.Txt_Over.gameObject.SetActive(true);
                Btn_Start.gameObject.SetActive(false);
            }
        }

        public void SetContent(int index)
        {
            Txt_Name.text = names[index];
            this.Level = index + 1;

            this.Show();
        }

        private void OnClick_Start()
        {
            AppHelper.DefendLevel = Level;

            User user = GameProcessor.Inst.User;
            DefendRecord record = user.DefendData.GetCurrentRecord(this.Level);

            if (record == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "û������ս����", ToastType = ToastTypeEnum.Failure });
                return;
            }

            record.Count.Data--;

            if (Type < 3)
            {
                this.GetComponentInParent<Dialog_Defend>().gameObject.SetActive(false);
                GameProcessor.Inst.EventCenter.Raise(new CloseViewMoreEvent());
                GameProcessor.Inst.EventCenter.Raise(new DefendStartEvent());
            }
            else
            {
                this.Btn_Start.gameObject.SetActive(false);
                this.Txt_Over.gameObject.SetActive(true);

                double exp = 0;
                double gold = 0;

                for (int i = 1; i <= 100; i++)
                {
                    DefendConfig rewardConfig = DefendConfigCategory.Instance.GetByLayerAndLevel(this.Level, i);

                    exp += rewardConfig.Exp;
                    gold += rewardConfig.Gold;
                }

                //���Ӿ���,���
                user.AddExpAndGold(exp, gold);

                List<int> dropIdList = user.DefendData.GetDropIdList(this.Level);

                List<Item> items = DropHelper.BuildDropItem(dropIdList);

                if (items.Count > 0)
                {
                    user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
                }

                user.DefendData.Complete();

                //��ʾ�����б�
                GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Gold = gold, Exp = exp, Items = items });
            }
        }
    }
}
