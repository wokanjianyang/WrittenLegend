using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TaskNav : MonoBehaviour
    {
        [LabelText("新手指引")]
        public Button PlayerGuide;

        public Text text;

        public Text Txt_Time;

        private bool over;

        // Start is called before the first frame update
        void Start()
        {
            PlayerGuide.onClick.AddListener(Reward);
        }

        public void Init()
        {
            User user = GameProcessor.Inst.User;

            if (GameProcessor.Inst.isTimeError)
            {
                PlayerGuide.gameObject.SetActive(false);
                Txt_Time.gameObject.SetActive(true);
                //Txt_Time.text = "您的收益时间：" + TimeHelper.SecondsToDate(user.SecondExpTick).AddHours(8).ToString()+",时间正常且过了这个时间才有收益";
                Txt_Time.text = "您的时间不正确或者没有更新，请校检时间或者更新游戏";
                return;
            }

            if (GameProcessor.Inst.isCheckError)
            {
                PlayerGuide.gameObject.SetActive(false);
                Txt_Time.gameObject.SetActive(true);
                Txt_Time.text = "您修改了存档";
                return;
            }

            TaskConfig config = TaskConfigCategory.Instance.GetById(user.TaskId);

            if (config == null)
            {  //over
                this.gameObject.SetActive(false);
                return;
            }

            user.TaskLog.TryGetValue(user.TaskId, out bool isOver);

            Text btnText = PlayerGuide.GetComponentInChildren<Text>(true);

            btnText.text = $"<color=#{QualityConfigHelper.GetTaskColor(isOver)}>[{config.Memo}]</color>";

        }

        private void Reward()
        {
            User user = GameProcessor.Inst.User;

            user.TaskLog.TryGetValue(user.TaskId, out bool isOver);

            if (!isOver)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "任务没有完成" });
                return;
            }

            //build reward
            TaskConfig config = TaskConfigCategory.Instance.GetById(user.TaskId);
            user.AddExpAndGold(config.RewardExp, config.RewardGold);

            List<Item> items = new List<Item>();
            if (config.RewardIdList != null)
            {
                for (int i = 0; i < config.RewardIdList.Length; i++)
                {
                    int itemId = config.RewardIdList[i];
                    ItemType type = (ItemType)config.RewardTypeList[i];

                    Item item = ItemHelper.BuildItem(type, itemId, 1, config.QuanlityList[i]);
                    if (item != null)
                    {
                        items.Add(item);
                    }
                }
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildGiftPackMessage("完成任务奖励:", config.RewardExp, config.RewardGold, items)
            });

            user.TaskId = user.TaskId + 1;

            Init();
        }


    }
}
