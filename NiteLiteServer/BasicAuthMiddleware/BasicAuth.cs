using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace NiteLiteServer.BasicAuthMiddleware
{
    //Taken from
    //https://lbadri.wordpress.com/2013/07/13/basic-authentication-with-asp-net-web-api-using-owin-middleware/
    public class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) :
            base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var response = context.Response;
            var request = context.Request;

            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse) state;

                if (resp.StatusCode == 401)
                {
                    resp.Headers.Add("WWW - Authenticate", new[] {"Basic"});
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


        private bool ValidateUser(string username, string password)
        {
            // do actual password validation here
            return username != password;
        }
    }
}