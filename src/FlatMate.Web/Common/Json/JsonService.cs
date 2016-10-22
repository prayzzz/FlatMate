using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlatMate.Web.Common.Json
{
    public class JsonService : IJsonService
    {
        public JsonService()
        {
            ViewSerializerSettings = new JsonSerializerSettings();
            ViewSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public JsonSerializerSettings ViewSerializerSettings { get; }
    }

    public interface IJsonService
    {
        JsonSerializerSettings ViewSerializerSettings { get; }
    }
}