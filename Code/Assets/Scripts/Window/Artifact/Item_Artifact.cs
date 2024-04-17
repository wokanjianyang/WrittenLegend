using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Artifact : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Des;

        public ArtifactConfig Config { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SetContent(ArtifactConfig config, long level)
        {
            this.Config = config;

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(6), config.Name);

            this.Txt_Level.text = level + "个";
            this.Txt_Des.text = config.Des;
        }
    }
}
