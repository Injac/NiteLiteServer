using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using NiteLiteServer.Options;
using NLog;

namespace NiteLiteServer
{
    class Program
    {
        private static IDisposable _currentApp;
        static Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
          

            string adddress = default(string);
            int port = default(int);

            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // consume Options instance properties

                adddress = options.IpAddressOrHostname;
                port = options.Port;
                  
                Console.WriteLine("Please wait. Starting the server using address (or ip) {0} and port:{1}",adddress,port);

                var baseAddress = $"http://{adddress}:{port}";

               

                using (_currentApp = WebApp.Start<Startup>(url: baseAddress))
                {
                   
                    Console.WriteLine("NiteLite Server Started!");
                    log.Warn("Server started");
                    Thread.Sleep(Timeout.Infinite);
                }


            }
           
            
        }

    }
}
