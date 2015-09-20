using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NiteLiteServer.Commands;

namespace NiteLiteServer.Controllers
{
    [RoutePrefix("commands")]
    public class CommandController:ApiController
    {
        [Route("execute")]
        [HttpPost]
        public async Task<bool> AcceptDeviceCommand([FromBody] string command)
        {

            if (string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if(!EnumTools.ContainsName(typeof(AvailableCommands),command))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //A test value returned
            return await Task.Run<bool>(() => true);
        }
    }
}
