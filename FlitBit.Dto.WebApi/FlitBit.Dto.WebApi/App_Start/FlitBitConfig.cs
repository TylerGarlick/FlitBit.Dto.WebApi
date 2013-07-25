using System.Web.Http;
using System.Web.Http.Dispatcher;
using FlitBit.Dto.WebApi.App_Start;
using FlitBit.IoC.WebApi;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(FlitBit.Dto.WebApi.App_Start.FlitBitConfig), "PreStart")]
namespace FlitBit.Dto.WebApi.App_Start
{
    public static class FlitBitConfig
    {
        public static void PreStart()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new FlitBitHttpControllerActivator());
        }
    }
}
