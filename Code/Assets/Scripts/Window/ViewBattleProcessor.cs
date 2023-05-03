using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewBattleProcessor : AViewPage
    {
        [Title("掉落")]
        [LabelText("掉落信息")]
        public ScrollRect sr_BattleMsg;

        private bool isViewMapShowing = false;

        private GameObject msgPrefab;

        protected override bool CheckPageType(ViewPageType page)
        {
            var ret = page == ViewPageType.View_Battle;
            if (ret)
            {
                GameProcessor.Inst.Resume();
            }
            else if (this.isViewMapShowing)
            {
                GameProcessor.Inst.Pause();
            }
            this.isViewMapShowing = ret;
            return ret;
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item_DropMsg");

            GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        }

        private void OnBattleMsgEvent(BattleMsgEvent e)
        {
            var msg = GameObject.Instantiate(this.msgPrefab);
            msg.transform.SetParent(this.sr_BattleMsg.content);
            msg.transform.localScale = Vector3.one;

            msg.GetComponent<Text>().text = e.Message;
            this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);

            //if (e.MsgType == MsgType.SecondExp)
            //{
            //    msg.GetComponent<TextMeshProUGUI>().text = $"�����ݵ㾭��{e.Exp}";
                
            //}
            //else if (e.BattleType == BattleType.Normal)
            //{
            //    MonsterBase config = MonsterBaseCategory.Instance.Get(e.MonsterId);
            //    string drops = "";
            //    if (e.Drops != null && e.Drops.Count > 0)
            //    {
            //        drops = ",����";
            //        foreach (var drop in e.Drops)
            //        {
            //            drops += $"<color=#{ItemHelper.GetColor(drop.GetQuality())}>[{drop.Name}]";
            //        }
            //    }
            //    msg.GetComponent<TextMeshProUGUI>().text = $"<color=#{ItemHelper.GetColor(4)}>[{config.Name}]<color=white>����,��������:{e.Exp},�������:{e.Gold}{drops}";
            //    msg.name = $"msg_{e.RoundNum}";

            //    this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
            //}
        }
    }
}
