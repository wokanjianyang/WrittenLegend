using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Phantom : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Attr_Rise;
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Attr_Current;

        public int ConfigId { get; set; }
        private PhantomAttrConfig Config;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            User user = GameProcessor.Inst.User;
            user.PhantomRecord.TryGetValue(ConfigId, out int phLevel);

            if (phLevel > this.Config.EndLevel)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经通关了", ToastType = ToastTypeEnum.Failure });
                return;
            }

            var vm = this.GetComponentInParent<ViewMore>();
            vm.SelectPhantomMap(ConfigId);
        }

        public void SetContent(PhantomConfig config, int level)
        {
            this.ConfigId = config.Id;
            PhantomAttrConfig currentConfig = PhantomConfigCategory.Instance.GetAttrConfig(config.Id, level - 1);

            PhantomAttrConfig nextConfig = PhantomConfigCategory.Instance.GetAttrConfig(config.Id, level);

            this.Config = nextConfig == null ? currentConfig : nextConfig;

            this.Txt_Name.text = config.Name;
            this.Txt_Level.text = $"({level}转)";

            if (level > 1)
            {
                this.Txt_Attr_Current.text = StringHelper.FormatAttrText(Config.RewardId, Config.GetRewardAttr(level - 1));
            }
            else
            {
                this.Txt_Attr_Current.text = "未激活";
            }

            this.Txt_Attr_Rise.text = StringHelper.FormatAttrText(Config.RewardId, Config.RewardRise, "+");
        }
    }
}
