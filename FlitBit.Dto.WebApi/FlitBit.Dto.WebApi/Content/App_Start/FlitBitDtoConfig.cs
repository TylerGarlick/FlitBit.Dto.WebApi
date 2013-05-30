using System.Web.Http;
using System.Web.Http.ModelBinding;
using FlitBit.Dto.WebApi.App_Start;
using FlitBit.Dto.WebApi.MediaFormatters;
using FlitBit.Dto.WebApi.ModelBinding;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(FlitBitDtoConfig), "PreStart")]
namespace FlitBit.Dto.WebApi.App_Start
{
    public static class FlitBitDtoConfig
    {
        public static void PreStart()
        {
            GlobalConfiguration.Configuration.Formatters[0] = new DtoMediaTypeFormatter();
            GlobalConfiguration.Configuration.Services.Insert(typeof(ModelBinderProvider), 0, new DtoModelBinderProvider());
        }
    }
}
