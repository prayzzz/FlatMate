using Newtonsoft.Json.Serialization;

namespace FlatMate.Web.Common.Json
{
    public class FlatMateContractResolver : DefaultContractResolver
    {
        private static FlatMateContractResolver _instance;

        private FlatMateContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy();
        }

        public static FlatMateContractResolver Instance => _instance ?? (_instance = new FlatMateContractResolver());
    }
}