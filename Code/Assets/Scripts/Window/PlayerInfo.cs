using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerInfo : MonoBehaviour
    {
        [LabelText("HP")]
        public Text HP;
        [LabelText("PhyAtt")]
        public Text PhyAtt;
        [LabelText("SpiritAtt")]
        public Text SpiritAtt;
        [LabelText("MagicAtt")]
        public Text MagicAtt;
        [LabelText("Lucky")]
        public Text Lucky;
        [LabelText("Def")]
        public Text Def;
        [LabelText("LevelExp")]
        public Text LevelExp;
        [LabelText("DamageIncrea")]
        public Text DamageIncrea;
        [LabelText("DamageResist")]
        public Text DamageResist;
        [LabelText("CritRate")]
        public Text CritRate;
        [LabelText("CritRateResist")]
        public Text CritRateResist;
        [LabelText("CritDamage")]
        public Text CritDamage;

    

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
       
        }

        public void UpdateAttrInfo(User user) {
            HP.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.HP).ToString();
            PhyAtt.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt).ToString();
            SpiritAtt.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt).ToString();
            MagicAtt.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.MagicAtt).ToString();
            Lucky.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky).ToString();
            Def.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.Def).ToString();
            LevelExp.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.Exp).ToString();
            DamageIncrea.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea).ToString();
            DamageResist.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist).ToString();
            CritRate.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate).ToString();
            CritRateResist.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist).ToString();
            CritDamage.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage).ToString();
        }
        
    }
}
