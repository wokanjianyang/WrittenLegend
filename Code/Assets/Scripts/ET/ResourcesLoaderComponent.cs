using System.Collections.Generic;

namespace ET
{
    [FriendClass(typeof(ResourcesLoaderComponent))]
    public static class ResourcesLoaderComponentSystem
    {
        [ObjectSystem]
            public class ResourcesLoaderComponentDestroySystem: DestroySystem<ResourcesLoaderComponent>
            {
                public override void Destroy(ResourcesLoaderComponent self)
                {

                }
            }
    }
    
    [ComponentOf(typeof(Scene))]
    public class ResourcesLoaderComponent: Entity, IAwake, IDestroy
    {
        public HashSet<string> LoadedResource = new HashSet<string>();
    }
}