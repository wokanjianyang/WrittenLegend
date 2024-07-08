using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Rune_Level : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Count;
        public Text Txt_Max;

        public void SetContent(int runeId, int count)
        {
            SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);

            this.Txt_Name.text = config.Name;
            this.Txt_Count.text = "+" + count + "级";
            this.Txt_Max.text = string.Format("最大{0}", config.Max);
        }
    }
}
