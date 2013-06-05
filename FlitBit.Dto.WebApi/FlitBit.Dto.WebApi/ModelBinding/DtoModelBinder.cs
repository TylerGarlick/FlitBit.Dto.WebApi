using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using FlitBit.Core;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using FlitBit.Represent.Json;
using RedRocket;

namespace FlitBit.Dto.WebApi.ModelBinding
{
    public class DtoModelBinder : IModelBinder
    {
        readonly IFactory _currentFactory;
        static readonly MethodInfo TransformToModelMethod = typeof(DtoModelBinder).MatchGenericMethod("TransformToModel", BindingFlags.Instance | BindingFlags.NonPublic, 1, typeof(object), typeof(string));

        public DtoModelBinder()
        {
            _currentFactory = FactoryProvider.Factory;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.IsAbstract || bindingContext.ModelType.IsInterface)
            {
                var json = actionContext.Request.Content.ReadAsStringAsync().Result;
                var transformToModelMethod = TransformToModelMethod.MakeGenericMethod(bindingContext.ModelType);
                var model = transformToModelMethod.Invoke(this, new object[] { json });

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

        T TransformToModel<T>(string json)
        {
            if (_currentFactory.CanConstruct<T>())
            {
                var rep = _currentFactory.CreateInstance<IJsonRepresentation<T>>();
                return rep.RestoreItem(json);
            }
            return default(T);
        }
    }
}
