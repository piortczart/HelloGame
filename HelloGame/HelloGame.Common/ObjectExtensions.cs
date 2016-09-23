using Newtonsoft.Json;

namespace HelloGame.Common
{
    public static class ObjectExtensions
    {
        public static string SerializeJson(this object thing)
        {
            return JsonConvert.SerializeObject(thing);
        }

        public static TType DeSerializeJson<TType>(this string serialized)
        {
            return JsonConvert.DeserializeObject<TType>(serialized);
        }
    }
}