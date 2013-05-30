using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using FlitBit.Core;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using FlitBit.Represent.Json;

namespace FlitBit.Dto.WebApi.MediaFormatters
{
    public class DtoMediaTypeFormatter : MediaTypeFormatter
    {
        readonly IFactory _currentFactory;
        static readonly MethodInfo TransformToModelMethod = typeof(DtoMediaTypeFormatter).MatchGenericMethod("TransformToModel", BindingFlags.Instance | BindingFlags.NonPublic, 1, typeof(object), typeof(string));
        public DtoMediaTypeFormatter()
        {
            _currentFactory = FactoryProvider.Factory;
        }

        public override bool CanReadType(Type type)
        {
            return _currentFactory.CanConstruct(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _currentFactory.CanConstruct(type);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var streamReader = new StreamReader(readStream))
                {
                    var json = streamReader.ReadToEnd();
                    var transformToModelMethod = TransformToModelMethod.MakeGenericMethod(type);
                    return transformToModelMethod.Invoke(this, new object[] { json });
                }
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var streamWriter = new StreamWriter(writeStream))
                    streamWriter.Write(value.ToJson());
            });
        }

        T TransformToModel<T>(string json)
        {
            return _currentFactory.CreateInstance<IJsonRepresentation<T>>().RestoreItem(json);
        }
    }
}
