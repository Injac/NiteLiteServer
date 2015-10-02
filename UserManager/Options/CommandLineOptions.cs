using System.Diagnostics;
using System.Text;
using CommandLine;

namespace UserManager.Options
{
    public class CommandLineOptions
    {
        [Option('u', "user", Required = true, HelpText = "The username to manage")]
        public string UserName { get; set; }

        [Option('p', "password", Required = false, HelpText = "The password for the user")]
        public string Password { get; set; }

        [Option('a', "action", Required = true, HelpText = "The action to perform: adduser, deleteuser, changepassword")]
        public string Action { get; set; }

        [Option('c', "passwordconfirm", Required = false, HelpText = "Password confirmation")]
        public string PasswordConfirmation { get; set; }

        [Option('n', "newpassword", Required = false, HelpText = "New password")]
        public string NewPassword { get; set; }

        [HelpOption]
        public string GetUsage()
        {

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            
            var usage = new StringBuilder();
            
            usage.AppendLine("Version " + version + " - written by Ilija Injac aka @awsomedevsigner\n");

            usage.AppendLine("Usage is (on Windows): UserManager.exe <options>");

            usage.AppendLine("Usage is (on Linxu): mono UserManager.exe <options>");
            usage.Append("\n");
            usage.AppendLine("Options:");
            usage.AppendLine("\t-u or username \t\t The username to add");
            usage.AppendLine("\t-n or newpassword (used when action is changeuser)");
            usage.AppendLine("\t-c or passwordconfirm (used when action is changeuser)");
            usage.AppendLine("\t-p or password \t\t The password for the user. No restrictions here. At least 8 characters are good, combined with special characters.");
            usage.AppendLine("\t-a or action \t\t The action to perform: adduser,deleteuser,changepassword.");
            usage.AppendLine("\n\n----SAMPLES----\n");
            usage.AppendLine("---ADDING A NEW USER---");
            usage.AppendLine("UserManager.exe -u john -a adduser -p test -c test\n");
            usage.AppendLine("---CHANGING THE PASSWORD FOR AN EXISTING USER---");
            usage.AppendLine("UserManager.exe -u john -a changepassword -p test  -n test2 -c test2\n");
            usage.AppendLine("---DELETING AN EXISTING USER---");
            usage.AppendLine("UserManager.exe -u john -a deleteuser");



            usage.AppendLine();

            return usage.ToString();
        }
    }
}