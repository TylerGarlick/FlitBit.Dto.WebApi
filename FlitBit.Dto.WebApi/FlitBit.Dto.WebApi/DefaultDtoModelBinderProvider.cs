using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FlitBit.Core;
using FlitBit.Core.Factory;

namespace FlitBit.Dto.WebApi
{
    public class DefaultDtoModelBinderProvider : ModelBinderProvider
    {
        readonly IFactory _factory;
        public DefaultDtoModelBinderProvider()
        {
            _factory = FactoryProvider.Factory;
        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return (modelType.IsInterface || modelType.IsAbstract) && _factory.CanConstruct(modelType) ?
                _factory.CreateInstance<DefaultDtoModelBinder>() :
                default(IModelBinder);
        }
    }
}