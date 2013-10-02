using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using FlitBit.Core;
using FlitBit.Core.Factory;
using Newtonsoft.Json;
using RedRocket;

namespace FlitBit.Dto.WebApi
{
    public class DefaultDtoModelBinder : IModelBinder
    {
        protected IFactory Factory { get; private set; }

        public DefaultDtoModelBinder()
        {
            Factory = FactoryProvider.Factory;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (Factory.CanConstruct(bindingContext.ModelType))
            {
                var json = actionContext.Request.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject(json, bindingContext.ModelType, DefaultJsonSerializerSettings.Current);
                if (model != null)
                {
                    bindingContext.Model = model;

                    var errors = model.GetValidationErrors();
                    if (errors.Any())
                        foreach (var error in errors)
                            actionContext.ModelState.AddModelError(error.PropertyName, error.Message);

                    return true;
                }
            }

            return false;
        }
    }
}