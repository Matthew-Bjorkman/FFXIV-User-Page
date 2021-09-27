using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UserPage.Models;

namespace UserPage.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IOptions<ApplicationConfiguration> _config;

        private const string _baseUrl = "https://discord.com/api/oauth2/authorize?client_id={0}&redirect_uri={1}&response_type=code&scope=identify";

        public LoginController(ILogger<LoginController> logger, IOptions<ApplicationConfiguration> config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Send()
        {
            string clientId = _config.Value.DiscordClientId;
            string redirectUrl = WebUtility.UrlEncode(Url.Action("Authorize", "Login", null, HttpContext.Request.Scheme));

            string discordOAuth2Url = string.Format(_baseUrl, clientId, redirectUrl);

            return Redirect(discordOAuth2Url);
        }

        public IActionResult Authorize()
        {
            string clientId = _config.Value.DiscordClientId;
            string clientSecret = _config.Value.DiscordClientSecret;
            string code = Request.Query["code"];
            string redirectUrl = WebUtility.UrlEncode(Url.Action("Return"));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/oauth2/token");
            webRequest.Method = "POST";
            string parameters = $"client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code&code={code}&redirect_uri={redirectUrl}";
            byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();
  
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            WebResponse response = webRequest.GetResponse();
            postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            string responseFromServer = reader.ReadToEnd();
  
            string tokenInfo = responseFromServer.Split(',')[0].Split(':')[1];
            string access_token = tokenInfo.Trim().Substring(1, tokenInfo.Length - 3);

            HttpWebRequest webRequest1 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/users/@me");
            webRequest1.Method = "Get";
            webRequest1.ContentLength = 0;
            webRequest1.Headers.Add("Authorization", "Bearer " + access_token);
            webRequest1.ContentType = "application/x-www-form-urlencoded";
  
            string apiResponse1 = "";
            using (HttpWebResponse response1 = webRequest1.GetResponse() as HttpWebResponse)
            {
                StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                apiResponse1 = reader1.ReadToEnd();
            }

            dynamic item = JsonConvert.DeserializeObject(apiResponse1);

            Console.Write(item);

            string username = item.username;
            int discriminator = item.discriminator;
            bool mfa_enabled = item.mfa_enabled;
            decimal id = item.id;
            string avatar = item.avatar;

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
