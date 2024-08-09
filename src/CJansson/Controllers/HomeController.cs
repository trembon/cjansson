using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CJansson.ViewModels;
using CJansson.ViewModels.Home;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace CJansson.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Blog");
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ContactViewModel model = new();
            model.GoogleRecaptchaKey = configuration.GetValue("recaptcha:key", string.Empty);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient httpClient = new())
                    {
                        string googleResult = await httpClient.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={configuration.GetValue("recaptcha:secret", string.Empty)}&response={Request.Form["g-recaptcha-response"]}&remoteip={HttpContext.Connection.RemoteIpAddress}");
                        Dictionary<string, object> resultObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(googleResult);

                        if ((bool)resultObject["success"])
                        {
                            Slack.Webhooks.SlackMessage slackMessage = new()
                            {
                                Username = "cjansson.se",
                                Channel = model.Source == "FormSubmit" ? "blogg" : "blogg_spam",
                                Text = $"*{model.Name} ({model.Email})*{Environment.NewLine}{model.Text}",
                                IconEmoji = Slack.Webhooks.Emoji.Computer
                            };

                            Slack.Webhooks.SlackClient slackClient = new(configuration["SlackURL"]);
                            bool sendResult = await slackClient.PostAsync(slackMessage);

                            if (sendResult)
                                return RedirectToAction("ThankYou");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"ERROR: {ex.Message}");
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("home/contact/thankyou")]
        public IActionResult ThankYou()
        {
            return View();
        }

        public class SendMessageModel
        {
            public bool Result { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}
