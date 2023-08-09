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
        private bool can = false;

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
            if (can)
            {
                var vm = this.GetComponentInParent<ViewMore>();
                vm.SelectPhantomMap(ConfigId);
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经通关了", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        public void SetContent(PhantomConfig config, int level)
        {
            this.ConfigId = config.Id;

            PhantomAttrConfig currentConfig = PhantomConfigCategory.Instance.GetAttrConfig(config.Id, level - 1);

            this.Txt_Name.text = config.Name;
            this.Txt_Level.text = $"({level}转)";

            if (currentConfig != null)
            {
                this.Txt_Attr_Current.text = StringHelper.FormatPhantomText(currentConfig.RewardId, currentConfig.RewardBase);
                this.Txt_Attr_Rise.text = StringHelper.FormatPhantomText(currentConfig.RewardId, currentConfig.RewardIncrea);
            }

            PhantomAttrConfig nextConfig = PhantomConfigCategory.Instance.GetAttrConfig(config.Id, level);
            if (nextConfig != null)
            {
                can = true;

                if (level == 1)
                {
                    this.Txt_Attr_Current.text = StringHelper.FormatPhantomText(nextConfig.RewardId, 0);
                    this.Txt_Attr_Rise.text = StringHelper.FormatPhantomText(nextConfig.RewardId, nextConfig.RewardBase);
                }
            }
        }
    }
}
