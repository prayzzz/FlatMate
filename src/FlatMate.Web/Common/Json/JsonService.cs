using FlatMate.Common.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlatMate.Web.Common.Json
{
    public interface IJsonService
    {
        JsonSerializerSettings ViewSerializerSettings { get; }
    }

    [Inject(DependencyLifetime.Singleton)]
    public class JsonService : IJsonService
    {
        public JsonService()
        {
            ViewSerializerSettings = new JsonSerializerSettings();
            ViewSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public JsonSerializerSettings ViewSerializerSettings { get; }
    }
}