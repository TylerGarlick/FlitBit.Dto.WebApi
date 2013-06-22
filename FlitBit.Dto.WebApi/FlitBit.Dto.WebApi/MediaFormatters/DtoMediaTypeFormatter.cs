﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FlitBit.Core;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using FlitBit.Represent.Json;
using Newtonsoft.Json;

namespace FlitBit.Dto.WebApi.MediaFormatters
{
    public class DtoMediaTypeFormatter : MediaTypeFormatter
    {
        readonly JsonSerializerSettings _serializerSettings;
        readonly IFactory _currentFactory;
        static readonly MethodInfo TransformToModelMethod = typeof(DtoMediaTypeFormatter).MatchGenericMethod("TransformToModel", BindingFlags.Instance | BindingFlags.NonPublic, 1, typeof(object), typeof(string));
        public DtoMediaTypeFormatter()
        {

            _currentFactory = FactoryProvider.Factory;
            _serializerSettings = new JsonSerializerSettings();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(() =>
            {
                var serializer = JsonSerializer.Create(_serializerSettings);
                using (var streamReader = new StreamReader(readStream, new UTF8Encoding(false)))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                    return serializer.Deserialize(jsonTextReader);
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
            var rep = _currentFactory.CreateInstance<IJsonRepresentation<T>>();
            return rep.RestoreItem(json);
        }
    }
}
