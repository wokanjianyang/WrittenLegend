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

        private int MetailId = 0;
        private int UpCount = 0;

        // Start is called before the first frame update
        void Awake()
        {

        }

        // Update is called once per frame
        void Start()
        {

        }

        public void Init(int metailId, int upCount)
        {
            this.MetailId = metailId;

            ItemConfig config = ItemConfigCategory.Instance.Get(metailId);

            this.Txt_Name.text = config.Name;
        }

        public void Refesh()
        {
            User user = GameProcessor.Inst.User;
            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == MetailId).Select(m => m.MagicNubmer.Data).Sum();

            string color = stoneTotal >= UpCount ? "#11FF11" : "#FF0000";
            this.Txt_Count.text = string.Format("<color={0}>{1}/{2}</color>", color, stoneTotal, UpCount);
        }
    }
}
