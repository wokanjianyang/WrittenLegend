using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Attr_Exclusive_Up : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Value;

        private int RuneId = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetContent(int runeId, int count)
        {
            this.RuneId = runeId;

            if (runeId > 0)
            {
                SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);

                string name = config.Name;
                if (count > 0)
                {
                    name += "(+" + count + ")";
                }
                this.Txt_Name.text = name;
            }
            else
            {
                this.Txt_Name.text = "Î´¼¤»î";
            }
        }

        public void SetUp(int runeId)
        {
            if (this.RuneId != runeId)
            {
                this.RuneId = runeId;
                SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);
                this.Txt_Name.text = config.Name;
            }

            this.Txt_Value.text = "+1";
        }

        public void Clear()
        {
            this.RuneId = 0;
            this.Txt_Name.text = "";
            this.Txt_Value.text = "";
        }
    }
}
