using System.Web.Http;
using System.Web.Http.Dispatcher;
using $rootnamespace$.App_Start;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(FlitBitConfig), "PreStart")]
namespace $rootnamespace$.App_Start
{
    public static class FlitBitConfig
    {
        public static void PreStart()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new IoCEnabledWebApiControllerFactory());
        }
    }
}
