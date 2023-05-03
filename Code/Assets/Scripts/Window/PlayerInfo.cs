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

        public void UpdateAttrInfo(Hero hero) {
            HP.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.HP).ToString();
            PhyAtt.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt).ToString();
            SpiritAtt.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt).ToString();
            MagicAtt.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.MagicAtt).ToString();
            Lucky.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky).ToString();
            Def.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.Def).ToString();
            LevelExp.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.Exp).ToString();
            DamageIncrea.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea).ToString();
            DamageResist.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist).ToString();
            CritRate.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate).ToString();
            CritRateResist.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist).ToString();
            CritDamage.text = hero.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage).ToString();
        }
        
    }
}
