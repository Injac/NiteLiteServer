using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

            using (var cl = new HttpClient())
            {
                switch ((AvailableCommands) Enum.Parse(typeof (AvailableCommands), command))
                {
                    case AvailableCommands.ActivateAlarm:
                        //Send serial message and start an
                        //Alarm timer
                        break;
                    case AvailableCommands.AlarmOff:
                        //Send serial message and stop an
                        //Alarm timer
                        break;
                    case AvailableCommands.CallDaddy:
                        //Call node.js and send a SMS
                        break;
                    case AvailableCommands.CallMummy:
                        //Call node.js and sens a SMS
                        break;
                    case AvailableCommands.CallEmergency:
                        //Call node.js and send a SMS
                        break;
                }
            }

            //A test value returned
            return await Task.Run<bool>(() => true);
        }
    }
}
