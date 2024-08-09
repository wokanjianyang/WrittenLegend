using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Rune_Suit : MonoBehaviour
    {
        public Text Txt_Rune;
        public Text Txt_Suit;


        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void Start()
        {
        }


        public void SetItem(int runeId, int suitId)
        {
            SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.Get(runeId);
            Txt_Rune.text = runeConfig.Name;

            SkillSuitConfig suitConfig = SkillSuitConfigCategory.Instance.Get(suitId);
            Txt_Suit.text = suitConfig.Name;
        }



    }
}
