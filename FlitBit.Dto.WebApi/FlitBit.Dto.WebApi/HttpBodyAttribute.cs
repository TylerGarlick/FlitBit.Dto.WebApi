using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using FlitBit.Dto.WebApi.ModelBinding;

namespace FlitBit.Dto.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class HttpBodyAttribute : ModelBinderAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            parameter.BindWithModelBinding(new DtoModelBinder());
            return base.GetBinding(parameter);
        }
    }
}
