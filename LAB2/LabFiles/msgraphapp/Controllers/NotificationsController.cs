using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using msgraphapp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Threading;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
namespace msgraphapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly MyConfig config;
        public NotificationsController(MyConfig config)
        {
            this.config = config;
        }
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var graphServiceClient = GetGraphClient();
            var sub = new Microsoft.Graph.Subscription();
            sub.ChangeType = "updated";
            sub.NotificationUrl = config.Ngrok + "/api/notifications";
            sub.Resource = "/users";
            sub.ExpirationDateTime = DateTime.UtcNow.AddMinutes(5);
            sub.ClientState = "SecretClientState";
            var newSubscription = await graphServiceClient
                .Subscriptions
                .Request()
                .AddAsync(sub);
            return $"Subscribed. Id: {newSubscription.Id}, Expiration: {newSubscription.ExpirationDateTime}";
        }
        public async Task<ActionResult<string>> Post([FromQuery]string validationToken = null)
        {
            // handle validation
            if(!string.IsNullOrEmpty(validationToken))
            {
                Console.WriteLine($"Received Token: '{validationToken}'");
                return Ok(validationToken);
            }
            // handle notifications
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                string content = await reader.ReadToEndAsync();
                Console.WriteLine(content);
                var notifications = JsonConvert.DeserializeObject<Notifications>(content);
                foreach(var notification in notifications.Items)
                {
                    Console.WriteLine($"Received notification: '{notification.Resource}', {notification.ResourceData?.Id}");
                }
            }
            return Ok();
        }
        private GraphServiceClient GetGraphClient()
        {
            var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
                    // get an access token for Graph
                    var accessToken = GetAccessToken().Result;
                    requestMessage
                            .Headers
                            .Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    return Task.FromResult(0);
            }));
            return graphClient;
        }
        private async Task<string> GetAccessToken()
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(config.AppId)
                .WithClientSecret(config.AppSecret)
                .WithAuthority($"https://login.microsoftonline.com/{config.TenantId}")
                .WithRedirectUri("https://daemon")
                .Build();
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}