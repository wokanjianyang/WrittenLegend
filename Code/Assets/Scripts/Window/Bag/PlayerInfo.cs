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
        [LabelText("SecondExp")]
        public Text SecondExp;

        [LabelText("GoldIncrea")]
        public Text GoldIncrea;
        [LabelText("ExpIncrea")]
        public Text ExpIncrea;
        [LabelText("BurstIncrea")]
        public Text BurstIncrea;
        [LabelText("QualityIncrea")]
        public Text QualityIncrea;

        [LabelText("SecondGold")]
        public Text SecondGold;
        [LabelText("Speed")]
        public Text Speed;
        [LabelText("RestoerHpPercent")]
        public Text RestoerHpPercent;
        [LabelText("CritDamageResist")]
        public Text CritDamageResist;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateAttrInfo(User user)
        {
            double hp = user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            HP.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
            PhyAtt.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt));
            SpiritAtt.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt));
            MagicAtt.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt));
            double def = user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Def);
            Def.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Def));

            Lucky.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky).ToString();
            LevelExp.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.Exp).ToString();
            DamageIncrea.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea).ToString() + "%";
            DamageResist.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist).ToString() + "%";
            CritRate.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate).ToString() + "%";
            CritRateResist.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist).ToString() + "%";
            CritDamage.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage).ToString() + "%";


            GoldIncrea.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea).ToString() + "%";
            ExpIncrea.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea).ToString() + "%";
            BurstIncrea.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea).ToString() + "%";
            QualityIncrea.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea).ToString() + "%";

            SecondExp.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp));
            SecondGold.text = StringHelper.FormatNumber(user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold));

            RestoerHpPercent.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHpPercent).ToString() + "%";
            Speed.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.Speed).ToString() + "%";
            CritDamageResist.text = user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist).ToString() + "%";
        }

    }
}
