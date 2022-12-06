using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewBattleProcessor : AViewPage
    {
        [Title("ս����Ϣ")]
        [LabelText("������Ʒ")]
        public ScrollRect sr_BattleMsg;

        private bool isViewMapShowing = false;

        private GameObject msgPrefab;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }


        protected override bool CheckPageType(ViewPageType page)
        {
            var ret = page == ViewPageType.View_Battle;
            if(ret)
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

            this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item-DropMsg");

            GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        }

        private void OnBattleMsgEvent(BattleMsgEvent e)
        {
            var msg = GameObject.Instantiate(this.msgPrefab);
            msg.transform.SetParent(this.sr_BattleMsg.content);
            MonsterBase config = MonsterBaseCategory.Instance.Get(e.MonsterId);
            string drops = ",����";
            foreach(var drop in e.Drops)
            {
                drops += $"<color=#D800FF>[{drop.Name}]";
            }
            msg.GetComponent<TextMeshProUGUI>().text = $"<color=#D800FF>[{config.Name}]<color=white>����,��������:{e.Exp},�������:{e.Gold}{drops}";
        }
    }
}
