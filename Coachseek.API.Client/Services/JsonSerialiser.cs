using Newtonsoft.Json;

namespace Coachseek.API.Client.Services
{
    public static class JsonSerialiser
    {
        public static string Serialise(object toBeSerialised)
        {
            return JsonConvert.SerializeObject(toBeSerialised, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static T Deserialise<T>(string toBeDeserialised)
        {
            return JsonConvert.DeserializeObject<T>(toBeDeserialised);
        }
    }
}
