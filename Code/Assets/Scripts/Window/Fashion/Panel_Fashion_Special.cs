using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Panel_Fashion_Special : MonoBehaviour
    {
        public ScrollRect sr_Panel;

        private List<Item_Fashion_Special> Items = new List<Item_Fashion_Special>();

        private GameObject ItemPrefab;

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Fashion/Item_Fashion_Special");

            GameProcessor.Inst.EventCenter.AddListener<FashionUIFreshEvent>(this.UIFresh);

            Init();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);

            this.Fresh();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;

            List<FashionSpecialConfig> configs = FashionSpecialConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            foreach (FashionSpecialConfig config in configs)
            {
                var item = GameObject.Instantiate(ItemPrefab);
                item.transform.SetParent(this.sr_Panel.content);
                item.transform.localScale = Vector3.one;

                var com = item.GetComponentInChildren<Item_Fashion_Special>();
                com.SetItem(config);

                Items.Add(com);
            }
        }

        private void UIFresh(FashionUIFreshEvent e)
        {
            this.Fresh();
        }

        private void Fresh() {
            foreach (var sp in Items)
            {
                sp.Show();
            }
        }
    }
}
