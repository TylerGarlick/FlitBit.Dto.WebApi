using System;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlitBit.Dto.WebApi.ModelBinding
{
    public class DtoJsonNetFormatter : MediaTypeFormatter
    {
        readonly JsonSerializerSettings _serializerSettings;
        public DtoJsonNetFormatter()
        {
            _serializerSettings = new JsonSerializerSettings();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Clear();
            SupportedEncodings.Add(new UTF8Encoding(false, true));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            var serializer = JsonSerializer.Create(_serializerSettings);

            return Task.Factory.StartNew(() =>
                                             {

                                                 using (var streamReader = new StreamReader(readStream, SupportedEncodings.FirstOrDefault()))
                                                 using (var jsonTextReader = new JsonTextReader(streamReader))
                                                     return serializer.Deserialize(jsonTextReader);
                                             });



        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var serializer = JsonSerializer.Create(_serializerSettings);

            return Task.Factory.StartNew(() =>
                                             {
                                                 using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(writeStream, SupportedEncodings.FirstOrDefault())) { CloseOutput = false })
                                                 {
                                                     serializer.Serialize(jsonTextWriter, value);
                                                     jsonTextWriter.Flush();
                                                 }
                                             });
        }
    }
}