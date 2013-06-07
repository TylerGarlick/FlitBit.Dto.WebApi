using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using FlitBit.Core;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using Newtonsoft.Json;
using RedRocket;

namespace FlitBit.Dto.WebApi.ModelBinding
{
    public class DtoModelBinder : IModelBinder
    {
        readonly IFactory _currentFactory;
        static readonly MethodInfo TransformToModelMethod = typeof(DtoModelBinder).MatchGenericMethod("TransformToModel", BindingFlags.Instance | BindingFlags.NonPublic, 1, typeof(object), typeof(string));

        readonly JsonSerializerSettings _settings;
        public DtoModelBinder()
        {
            _currentFactory = FactoryProvider.Factory;
            _settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new[] { new DtoJsonConverter() }
            };
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
            return _currentFactory.CanConstruct<T>() ?
                JsonConvert.DeserializeObject<T>(json, _settings) :
                default(T);
        }
    }
}
