using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace FlitBit.Dto.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class HttpBodyAttribute : ModelBinderAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            parameter.BindWithModelBinding(new DefaultDtoModelBinder());
            return base.GetBinding(parameter);
        }
    }
}
