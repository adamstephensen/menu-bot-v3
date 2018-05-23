using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Connector;
using System;
using Microsoft.Bot.Builder.Dialogs;
using MenuBot.BotAssets.Dialogs;
using System.Linq;
using MenuBot.BotAssets.Extensions;

namespace MenuBot
{
    public static class run
    {
        [FunctionName("messages")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            // Initialize the azure bot
            using (BotService.Initialize())
            {
                log.Info($"Webhook was triggered! - messages");

                string jsonContent = await req.Content.ReadAsStringAsync();
                var activity = JsonConvert.DeserializeObject<Activity>(jsonContent);

                if (activity != null)
                {
                    // one of these will have an interface and process it
                    switch (activity.GetActivityType())
                    {
                        case ActivityTypes.Message:
                            //here is where we will navigate to root dialogue
                            await Conversation.SendAsync(activity, () => new RootDialog());
                            //var client = new ConnectorClient(new Uri(activity.ServiceUrl));
                            //var triggerReply = activity.CreateReply();

                            //triggerReply.Text = $"Hey you said '{activity.Text}'.";
                            //await client.Conversations.ReplyToActivityAsync(triggerReply);

                            break;

                        case ActivityTypes.ConversationUpdate:

                            if (activity.MembersAdded.Any(o => o.Id == activity.Recipient.Id))
                            {
                                var reply = activity.CreateReply("Welcome to our Bot!");
                                reply.AddHeroCard("Where do you want to go?", MenuHelpers.getMenuOptions("Home"));

                                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }

                            break;
                        default:
                            log.Error($"Unknown activity type ignored: {activity.GetActivityType()}");
                            break;
                    }
                }
                return req.CreateResponse(HttpStatusCode.Accepted);
            }

        }
    }

}
