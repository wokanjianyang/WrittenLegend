using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Game
{
    public class RefreshAttr : MonoBehaviour
    {
        public List<Text> AttrList;
        public Item_Rune ItemRune;
        public Item_Suit ItemSuit;

        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void Start()
        {

        }

        public void Clear()
        {

        }

        public void Show(List<KeyValuePair<int, long>> attrEntryList, int runeId, int suitId)
        {
            for (int i = 0; i < AttrList.Count; i++)
            {
                Text text = AttrList[i];

                if (i < attrEntryList.Count)
                {
                    text.gameObject.SetActive(true);
                    text.text = StringHelper.FormatAttrText(attrEntryList[i].Key, attrEntryList[i].Value);
                }
                else
                {
                    text.gameObject.SetActive(false);
                }
            }

            ItemRune.SetContent(runeId);
            ItemSuit.SetContent(suitId);
        }
    }
}
