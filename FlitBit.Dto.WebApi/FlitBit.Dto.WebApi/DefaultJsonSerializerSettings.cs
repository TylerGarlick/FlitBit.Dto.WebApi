using Newtonsoft.Json;

namespace FlitBit.Dto.WebApi
{
    public static class DefaultJsonSerializerSettings
    {
        static DefaultJsonSerializerSettings()
        {
            Current = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultCamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new[] { new DefaultJsonConverter() }

            };
        }
        public static JsonSerializerSettings Current { get; set; }
    }
}