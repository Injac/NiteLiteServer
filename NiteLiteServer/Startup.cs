using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using NiteLiteServer.AssemblyResolver;
using Owin;
using Pysco68.Owin.Logging.NLogAdapter;
using JsonConfig;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;

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

            if (Config.Global.EnableStaticFiles)
            {

                var root = $"{AppDomain.CurrentDomain.BaseDirectory}/{"www"}";

                var fileServerOptions = new FileServerOptions()
                {
                    EnableDefaultFiles = true,
                    EnableDirectoryBrowsing = false,
                    RequestPath = new PathString("/www"),
                    FileSystem = new PhysicalFileSystem(root)
                };

                app.UseFileServer(fileServerOptions);


            }

            app.UseWebApi(config);

            

        }

        
    }
}
