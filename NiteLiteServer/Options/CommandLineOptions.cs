using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace NiteLiteServer.Options
{
    public class CommandLineOptions
    {
        [Option('h', "host", Required = true, HelpText = "IP Address or hostname")]
        public string IpAddressOrHostname { get; set; }

        [Option('p', "port", Required = true, HelpText = "The port to listen to")]
        public int Port { get; set; }


        [HelpOption]
        public string GetUsage()
        {

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            
            var usage = new StringBuilder();
            usage.Append(@"  _   _ _ _       _     _ _       
 | \ | (_) |_ ___| |   (_) |_ ___ 
 |  \| | | __/ _ \ |   | | __/ _ \
 | |\  | | ||  __/ |___| | ||  __/
 |_| \_|_|\__\___|_____|_|\__\___|
                                  "+"\n");
            usage.Append("==================================================================================\n");
            usage.Append(
                @"         \ \      / /__| |__      / \  |  _ \_ _| / ___|  ___ _ ____   _____ _ __    
          \ \ /\ / / _ \ '_ \    / _ \ | |_) | |  \___ \ / _ \ '__\ \ / / _ \ '__|   
           \ V  V /  __/ |_) |  / ___ \|  __/| |   ___) |  __/ |   \ V /  __/ |      
            \_/\_/ \___|_.__/  /_/   \_\_|  |___| |____/ \___|_|    \_/ \___|_|      
           __              ____                 _                            ____  _ 
          / _| ___  _ __  |  _ \ __ _ ___ _ __ | |__   ___ _ __ _ __ _   _  |  _ \(_)
         | |_ / _ \| '__| | |_) / _` / __| '_ \| '_ \ / _ \ '__| '__| | | | | |_) | |
         |  _| (_) | |    |  _ < (_| \__ \ |_) | |_) |  __/ |  | |  | |_| | |  __/| |
         |_|  \___/|_|    |_| \_\__,_|___/ .__/|_.__/ \___|_|  |_|   \__, | |_|   |_|
           ___     __  __                |_|                         |___/           
          ( _ )   |  \/  | ___  _ __   ___                                           
          / _ \/\ | |\/| |/ _ \| '_ \ / _ \                                          
         | (_>  < | |  | | (_) | | | | (_) |                                         
          \___/\/ |_|  |_|\___/|_| |_|\___/              "
                );


            usage.Append("\n====================================================================================\n");

            usage.AppendLine("Version " + version + " - written by Ilija Injac aka @awsomedevsigner\n");

            usage.AppendLine("Usage is (on Windows): NiteLiteServer.exe <options>");

            usage.AppendLine("Usage is (on Linxu): mono NiteLiteServer.exe <options>");
            usage.Append("\n");
            usage.AppendLine("Options:");
            usage.AppendLine("\t-p or port \t\t The port the NiteLite (the server) will listen to. (required)");
            usage.AppendLine("\t-h or host \t\t The hostname or ip address to bind the server to. (required)");


            usage.AppendLine();

            return usage.ToString();
        }
    }
}