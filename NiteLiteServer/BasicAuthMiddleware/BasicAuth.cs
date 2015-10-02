using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using BCrypt.Net;
using NiteLiteServer.Model;
using STSdb4.Database;

namespace NiteLiteServer.BasicAuthMiddleware
{
    //Taken from
    //https://lbadri.wordpress.com/2013/07/13/basic-authentication-with-asp-net-web-api-using-owin-middleware/
    public class AuthenticationMiddleware : OwinMiddleware
    {
        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }


        public AuthenticationMiddleware(OwinMiddleware next) :
            base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                var response = context.Response;
                var request = context.Request;

                response.OnSendingHeaders(state =>
                {
                    var resp = (OwinResponse)state;

                    if (resp.StatusCode == 401)
                    {
                        resp.Headers.Add("WWW-Authenticate", new[] { "Basic realm=\"NiteLite Server Authentication\"" });
                        resp.StatusCode = 403;
                        resp.ReasonPhrase = "Forbidden";
                    }
                }, response);

                var header = request.Headers["Authorization"];

                if (!String.IsNullOrWhiteSpace(header))
                {
                    var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

                    if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                    {
                        var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));

                        var parts = parameter.Split(':');

                        var username = parts[0];
                        var password = parts[1];

                        if (ValidateUser(username, password))
                        {
                            var claims = new[]
                            {
                            new Claim(ClaimTypes.Name, username)
                        };
                            var identity = new ClaimsIdentity(claims, "Basic");
                            request.User = new ClaimsPrincipal(identity);
                        }
                    }
                }
                
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {

               Debug.WriteLine(ex.Message);
            }
        }


        private bool ValidateUser(string username, string password)
        {
            try
            {
                string userDb = default(string);

              

                if (System.Environment.OSVersion.ToString().Contains("Unix"))
                {

                    userDb = $@"{AssemblyDirectory}/{"Data"}/{"users.stsdb4"}";
                }
                else
                {
                    userDb = $@"{AssemblyDirectory}\{"Data"}\{"users.stsdb4"}"; ;
                }

                using (IStorageEngine engine = STSdb.FromFile(userDb))
                {
                    var userTable = engine.OpenXTable<string, User>("users");

                    var md5UserName = CreateMD5(username);

                    var user = userTable[md5UserName];

                    if (user != null)
                    {
                        return BCrypt.Net.BCrypt.Verify(password, user.Password);
                    }

                    return false;

                }
               
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);

                return false;
            }

       
        }

        //Microsoft Sample. Works.
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}