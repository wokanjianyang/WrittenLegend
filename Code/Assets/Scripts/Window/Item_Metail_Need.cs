using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Metail_Need : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Count;

        // Start is called before the first frame update
        void Awake()
        {

        }

        // Update is called once per frame
        void Start()
        {

        }

        public void SetContent(int metailId, int upCount)
        {
            ItemConfig config = ItemConfigCategory.Instance.Get(metailId);
            this.Txt_Name.text = config.Name;

            User user = GameProcessor.Inst.User;
            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == metailId).Select(m => m.MagicNubmer.Data).Sum();

            string color = stoneTotal >= upCount ? "#11FF11" : "#FF0000";
            this.Txt_Count.text = string.Format("<color={0}>{1}/{2}</color>", color, stoneTotal, upCount);
        }
    }
}
