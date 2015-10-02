using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NiteLiteServer.Commands;
using NLite.API.IoT.Commanding.Model;
using JsonConfig;


// ReSharper disable once CheckNamespace
namespace NiteLiteServer.Controllers
{
    [Authorize]
    [RoutePrefix("commands")]
    public class CommandController:ApiController
    {
        private readonly dynamic _config;

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public CommandController()
        {

            try
            {

                if (System.Environment.OSVersion.ToString().Contains("Unix"))
                {

                    _config = Config.ApplyFromDirectory(AssemblyDirectory + @"/commandconfig", null, false);
                }
                else
                {
                    _config = Config.ApplyFromDirectory(AssemblyDirectory + @"\commandconfig", null, false);
                }

               
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error in constructor");
            }
            
        }

        [Route("testauth")]
        [HttpGet]
        public async Task<string> TestAuth()
        {
           
            return await Task.Run<string>(() => "Authorized");
        }

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
                        await SendSMS(_config.PhoneNumberDaddy, "Daddy, please call me back. Thank you. Ellie.");
                        break;
                    case AvailableCommands.CallMummy:
                        //Call node.js and sens a SMS
                        await SendSMS(_config.PhoneNumberMummy, "Mummy, please call me back. Thank you. Ellie.");
                        break;
                    case AvailableCommands.CallEmergency:
                        //Call node.js and send a SMS
                        //Not used currently. Hardware to trigger calls needed
                        break;
                    case AvailableCommands.DimmClock:
                        await DimUndimmClock("dimm");
                        break;
                    case AvailableCommands.UndimmClock:
                        await DimUndimmClock("undim");
                        break;
                    case AvailableCommands.PlaySong1:
                        await SongCommands("playsong", "song1.mp3");
                        break;
                    case AvailableCommands.PlaySong2:
                        await SongCommands("playsong", "song2.mp3");
                        break;
                    case AvailableCommands.PlaySong3:
                        await SongCommands("playsong", "song3.mp3");
                        break;
                    case AvailableCommands.PlaySong4:
                        await SongCommands("playsong", "song4.mp3");
                        break;
                    case AvailableCommands.PauseSong:
                        await SongCommands("pausesong");
                        break;
                    case AvailableCommands.ResumeSong:
                        await SongCommands("resumesong");
                        break;
                    case AvailableCommands.StopSong:
                        await SongCommands("stopsong");
                        break;


                }
            }

            //A test value returned
            return await Task.Run<bool>(() => true);
        }

        private async Task<bool> SendSMS(string to, string text)
        {
            try
            {

                var callUrl = string.Format("{0}/{1}", _config.CommandingServerIp, "sendsms");

                HttpClient cl = new HttpClient();

                var data = new SMSData() { to = to, text = text };

                var response = await cl.PostAsync(callUrl,
                           new StringContent(JsonConvert.SerializeObject(data),
                               Encoding.UTF8, "application/json"));

                return CheckHttpStatusOk(response);
            }
            catch (HttpRequestException ex)
            {

                return false;
            }

        }

        private static bool CheckHttpStatusOk(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> DimUndimmClock(string command)
        {
            try
            {
                var callUrl = string.Format("{0}/{1}", _config.CommandingServerIp, command);

                HttpClient cl = new HttpClient();

                var data = new SerialData() { command = command };

                var response = await cl.PostAsync(callUrl,
                           new StringContent(JsonConvert.SerializeObject(data),
                               Encoding.UTF8, "application/json"));

                return CheckHttpStatusOk(response);
            }
            catch (HttpRequestException ex)
            {

                return false;
            }
        }

        private async Task<bool> SongCommands(string command, string songname="")
        {

            try
            {
                var callUrl = string.Format("{0}/{1}", _config.CommandingServerIp, command);

                if (songname != "")
                {
                    HttpClient cl = new HttpClient();

                    var data = new SongData() { songname = songname };



                    var response = await cl.PostAsync(callUrl,
                               new StringContent(JsonConvert.SerializeObject(data),
                                   Encoding.UTF8, "application/json"));

                    return CheckHttpStatusOk(response);
                }
                else
                {
                    HttpClient cl = new HttpClient();
                    var response = await cl.GetAsync(callUrl);

                    return CheckHttpStatusOk(response);
                }
            }
            catch (HttpRequestException ex)
            {

                return false;
            }
        }

    }
}
