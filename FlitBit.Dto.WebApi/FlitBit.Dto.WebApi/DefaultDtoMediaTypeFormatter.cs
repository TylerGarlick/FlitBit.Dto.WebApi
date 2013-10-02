using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using FlitBit.IoC;
using Newtonsoft.Json;

namespace FlitBit.Dto.WebApi
{
    
    public class DefaultDtoMediaTypeFormatter : JsonMediaTypeFormatter
    {
        readonly IContainer _container;
        public DefaultDtoMediaTypeFormatter()
        {
            _container = Container.Root;

            SerializerSettings.ContractResolver = DefaultJsonSerializerSettings.Current.ContractResolver;
            SerializerSettings.MissingMemberHandling = DefaultJsonSerializerSettings.Current.MissingMemberHandling;
            SerializerSettings.ReferenceLoopHandling = DefaultJsonSerializerSettings.Current.ReferenceLoopHandling;
            SerializerSettings.Converters.Insert(0, DefaultJsonSerializerSettings.Current.Converters.First());
        }

        public override bool CanReadType(Type type)
        {
            return _container.CanConstruct(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _container.CanConstruct(type);
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var instance = await Task.Run(() =>
            {
                var serializer = JsonSerializer.Create(SerializerSettings);
                using (var streamReader = new StreamReader(readStream, SupportedEncodings.FirstOrDefault() ?? new UTF8Encoding()))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                    return serializer.Deserialize(jsonTextReader);
            });

            return instance;
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            await Task.Run(() =>
            {
                using (var streamWriter = new StreamWriter(writeStream))
                    streamWriter.Write(JsonConvert.SerializeObject(value, Formatting.Indented, DefaultJsonSerializerSettings.Current));
            });
        }
    }
}
