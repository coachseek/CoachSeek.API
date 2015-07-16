using Newtonsoft.Json;

namespace Coachseek.API.Client.Services
{
    public static class JsonSerialiser
    {
        public static string Serialise(object toBeSerialised)
        {
            return JsonConvert.SerializeObject(toBeSerialised, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
