using Game.Data;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
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
            else if (Type == CopyType.HeorPhantom) //
            {
                User user = GameProcessor.Inst.User;

                HeroPhatomRecord record = user.HeroPhatomData.GetCurrentRecord();

                if (record == null)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "今天挑战已经通关了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                if (user.Cycle.Data >= 1)
                {  //如果是1转之后，自动完成

                    user.HeroPhatomData.Complete();

                    List<Item> items = DropLimitHelper.Build((int)DropLimitType.HeroPhatom, 0, 1, 1, 9999999, 1); ;

                    foreach (Item item in items)
                    {
                        item.Count *= 10; //十层
                    }

                    user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

                    //显示掉落列表
                    GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Gold = 0, Exp = 0, Items = items });

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
            else if (Type == CopyType.Legacy)
            {
                GameProcessor.Inst.EventCenter.Raise(new OpenLegacyEvent());
            }
            else if (Type == CopyType.Pill)
            {
                GameProcessor.Inst.EventCenter.Raise(new OpenPillEvent());
            }
            else if (Type == CopyType.Babel)
            {
                GameProcessor.Inst.EventCenter.Raise(new OpenBabelEvent());
            }
        }
    }
}
