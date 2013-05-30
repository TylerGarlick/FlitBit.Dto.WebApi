using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FlitBit.Core;
using FlitBit.Core.Factory;

namespace FlitBit.Dto.WebApi.ModelBinding
{
    public class DtoModelBinderProvider : ModelBinderProvider
    {
        readonly IFactory _currentFactory;

        public DtoModelBinderProvider()
        {
            _currentFactory = FactoryProvider.Factory;
        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return _currentFactory.CanConstruct(modelType) ?
                _currentFactory.CreateInstance<DtoModelBinder>() :
                default(IModelBinder);
        }
    }
}