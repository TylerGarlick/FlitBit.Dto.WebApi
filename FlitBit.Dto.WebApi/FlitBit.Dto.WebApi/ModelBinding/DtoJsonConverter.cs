using System;
using System.Reflection;
using FlitBit.Core;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlitBit.Dto.WebApi.ModelBinding
{
    public class DtoJsonConverter : JsonConverter
    {
        static readonly MethodInfo CreateNewMethod = typeof(DtoJsonConverter).MatchGenericMethod("CreateNew", BindingFlags.Instance | BindingFlags.NonPublic, 1, typeof(object), new Type[] { });
        static readonly MethodInfo CanConstructMethod = typeof(DtoJsonConverter).MatchGenericMethod("CanConstruct", BindingFlags.Instance | BindingFlags.NonPublic, 1, typeof(bool), new Type[] { });

        readonly IFactory _factory;

        /// <summary>
        /// Creates a DTO Converter
        /// </summary>
        public DtoJsonConverter()
        {
            _factory = FactoryProvider.Factory;
        }

        /// <summary>
        /// We don't need to serialize, therefore, false
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Creates a new object via the Factory.  Loads the instance via the serializer.
        /// </summary>
        /// <param name="reader">The JsonReader</param>
        /// <param name="objectType">The Type of DTO</param>
        /// <param name="existingValue">The DTO</param>
        /// <param name="serializer">JsonSerializer</param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JsonReader objectReader = null;
            var create = CreateNewMethod.MakeGenericMethod(objectType);
            var instance = create.Invoke(this, new object[] { });

            if (reader.TokenType == JsonToken.StartObject)
            {
                var jObject = JObject.Load(reader);
                objectReader = jObject.CreateReader();
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                var jArray = JArray.Load(reader);
                objectReader = jArray.CreateReader();
            }

            serializer.Populate(objectReader, instance);
            return instance;
        }
        /// <summary>
        /// Determines if we can convert the type.  
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsAbstract || objectType.IsInterface)
            {
                var canContruct = CanConstructMethod.MakeGenericMethod(objectType);
                return (bool)canContruct.Invoke(this, new object[] { });
            }
            return false;
        }

        bool CanConstruct<TNew>()
        {
            return _factory.CanConstruct<TNew>();
        }

        TNew CreateNew<TNew>()
        {
            return _factory.CanConstruct<TNew>() ?
                       _factory.CreateInstance<TNew>() :
                       default(TNew);
        }

    }
}