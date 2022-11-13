using UnityEngine;

namespace Game
{
    public class Hero : APlayer
    {
        public override void Load()
        {
            base.Load();
            
            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            var box = GameObject.Instantiate(boxPrefab).transform;
            box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform);
            
            this.Camp = PlayerType.Hero;
        }
    }
    
}
