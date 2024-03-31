using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Suit : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Des;

        public void SetContent(int suitId, int count, int max)
        {
            SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(suitId);

            this.Txt_Name.text = config.Name + string.Format("({0}/{1})", count, max); ;
            this.Txt_Des.text = config.Des;
        }

        public void SetContent(int suitId)
        {
            SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(suitId);
            this.Txt_Name.text = config.Name;
            this.Txt_Des.text = config.Des;
        }
    }
}
