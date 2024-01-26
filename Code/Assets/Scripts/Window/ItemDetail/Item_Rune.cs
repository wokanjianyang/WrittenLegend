using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Rune : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Des;
        public Text Txt_Count;

        public void SetContent(int runeId)
        {
            SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);

            this.Txt_Name.text = config.Name;
            this.Txt_Des.text = config.Des;
            this.Txt_Count.text = string.Format("最大{0}", config.Max);
        }
    }
}
