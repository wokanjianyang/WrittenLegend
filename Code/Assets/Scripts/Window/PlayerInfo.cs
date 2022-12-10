using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerInfo : MonoBehaviour
    {
        [LabelText("HP")]
        public TextMeshProUGUI HP;
        [LabelText("PhyAtt")]
        public TextMeshProUGUI PhyAtt;
        [LabelText("SpiritAtt")]
        public TextMeshProUGUI SpiritAtt;
        [LabelText("MagicAtt")]
        public TextMeshProUGUI MagicAtt;
        [LabelText("Lucky")]
        public TextMeshProUGUI Lucky;
        [LabelText("Def")]
        public TextMeshProUGUI Def;
        [LabelText("LevelExp")]
        public TextMeshProUGUI LevelExp;
        [LabelText("DamageIncrea")]
        public TextMeshProUGUI DamageIncrea;
        [LabelText("DamageResist")]
        public TextMeshProUGUI DamageResist;
        [LabelText("CritRate")]
        public TextMeshProUGUI CritRate;
        [LabelText("CritRateResist")]
        public TextMeshProUGUI CritRateResist;
        [LabelText("CritDamage")]
        public TextMeshProUGUI CritDamage;

    

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
