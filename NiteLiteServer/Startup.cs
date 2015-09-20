using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using NiteLiteServer.AssemblyResolver;
using Owin;
using Pysco68.Owin.Logging.NLogAdapter;
using JsonConfig;

[assembly: OwinStartup(typeof(NiteLiteServer.Startup))]

namespace NiteLiteServer
{
    public class Startup
    {
       

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            if (Config.Global.EnableAttributeRouting)
            {
                config.MapHttpAttributeRoutes();
            }

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.Add(config.Formatters.JsonFormatter);

            config.Services.Replace(typeof(IAssembliesResolver), new ApiAssemblyResolver());

            if (Config.Global.EnableCors)
            {
                app.UseCors(CorsOptions.AllowAll);
            }

            if (Config.Global.EnableSignalR)
            {
                app.MapSignalR();
            }

            if (Config.Global.EnableNLog)
            {
                app.UseNLog();
            }

            app.UseWebApi(config);

            

        }

        
    }
}
