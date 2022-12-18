using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Skill : MonoBehaviour, IPointerClickHandler
    {
        [Title("����")]
        [LabelText("����")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("�ȼ�")]
        public TextMeshProUGUI tmp_Level;

        [LabelText("��ȴ")]
        public TextMeshProUGUI tmp_CD;

        [LabelText("����")]
        public TextMeshProUGUI tmp_Des;

        public SkillBook Book { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetItem(SkillBook item)
        {
            this.Book = item;

            this.tmp_Name.text = item.Name;
            this.tmp_Level.text = string.Format("LV:{0}", item.Level);
            this.tmp_CD.text = string.Format("��ȴʱ��{0}�غ�", item.BookConfig.CD);
            this.tmp_Des.text = item.Config.Des;

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(item.Exp, item.GetUpExp());
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.Book == null) return;

            this.Book.SkillType = SkillBookType.Equip;
            GameProcessor.Inst.PlayerManager.hero.EventCenter.Raise(new HeroUpdateSkillEvent
            {
                Book = this.Book
            });
        }

    }
}
