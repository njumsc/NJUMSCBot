using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using NJUMSCBot.Models;
using static NJUMSCBot.Data.Data;
using static NJUMSCBot.Dialogs.LuisDialog;
using System.Linq;
using System.Threading;

namespace NJUMSCBot.Dialogs
{
    [Serializable]
    public class QnADialog : IDialog<bool>
    {

        public static HttpClient client = new HttpClient();
        public static string qnamakerURL = @"https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/e1e55b5c-d3c3-47a6-8b5a-cb7fc5452123/generateAnswer";
        static QnADialog()
        {
            client.BaseAddress = new Uri(qnamakerURL);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0b70551957a548d7b0c52c84270b305d");
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            string text = activity.Text;

            var s = await client.PostAsJsonAsync(qnamakerURL, new { question = text, top = 1 });
            var answerResponse = JsonConvert.DeserializeObject<QnAResponse>(await s.Content.ReadAsStringAsync());

            var answersOrdered = answerResponse.Answers.OrderByDescending(x => x.Score);

            if (answersOrdered.First().Score == 0)
            {
                context.Done(false);
            }
            else
            {
                await ReplyAsync(context, answersOrdered.First().Answer);
                context.Done(true);
            }
        }
    }
}