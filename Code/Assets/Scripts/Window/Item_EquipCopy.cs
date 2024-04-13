using Game.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_EquipCopy : MonoBehaviour, IPointerClickHandler
    {
        [Title("插槽")]
        [LabelText("类型")]
        public CopyType Type;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Type == CopyType.装备副本)
            {
                GameProcessor.Inst.EventCenter.Raise(new BossInfoEvent());
            }
            else if (Type == CopyType.幻影挑战)
            {
                GameProcessor.Inst.EventCenter.Raise(new PhantomEvent());
            }
            else if (Type == CopyType.BossFamily)
            {
                GameProcessor.Inst.EventCenter.Raise(new OpenBossFamilyEvent());
            }
            else if (Type == CopyType.AnDian)
            {
                var vm = this.GetComponentInParent<ViewMore>();
                vm.StartAnDian();
            }
            else if (Type == CopyType.Defend)
            {
                GameProcessor.Inst.EventCenter.Raise(new OpenDefendEvent());
            }
            else if (Type == CopyType.HeorPhantom)
            {
                User user = GameProcessor.Inst.User;

                HeroPhatomRecord record = user.HeroPhatomData.GetCurrentRecord();

                if (record == null)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "今天挑战已经通关了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                var vm = this.GetComponentInParent<ViewMore>();
                vm.StartHeroPhantom();
            }
            else if (Type == CopyType.Infinite)
            {
                User user = GameProcessor.Inst.User;

                InfiniteRecord record = user.InfiniteData.GetCurrentRecord();

                if (record == null)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "今天挑战已经通关了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                var vm = this.GetComponentInParent<ViewMore>();
                vm.StartInfinite();
            }
            else if (Type == CopyType.Mine)
            {
                GameProcessor.Inst.EventCenter.Raise(new OpenMineEvent());
            }
        }
    }
}
