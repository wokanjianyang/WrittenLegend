using Newtonsoft.Json;

namespace Game
{
    public abstract class Object
    {
        public override string ToString()
        {
            return JsonConvert.ToString(this);
        }
    }
}