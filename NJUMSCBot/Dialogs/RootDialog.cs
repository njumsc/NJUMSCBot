using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;
using System.Linq;
using NJUMSCBot.Models;
using System.Threading;
using Newtonsoft.Json;
using static NJUMSCBot.Data.Data;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Net.Http;

namespace NJUMSCBot.Dialogs
{

    [Serializable]
    [LuisModel("204f9894-2f57-4c7d-889f-31f2df44f0f3", "ccad6263e2cd434bab371a2562823097")]
    public class RootDialog : LuisDialog<object>
    {
        public static HttpClient client = new HttpClient();
        public static string qnamakerURL = @"https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/e1e55b5c-d3c3-47a6-8b5a-cb7fc5452123/generateAnswer";
        static RootDialog()
        {
            client.BaseAddress = new Uri(qnamakerURL);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0b70551957a548d7b0c52c84270b305d");
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string text = result.Query;
            WebClient w = new WebClient();



            var s = await client.PostAsJsonAsync(qnamakerURL, new { question = text, top = 1 });
            var answer = JsonConvert.DeserializeObject<QnAResponse>(await s.Content.ReadAsStringAsync());
            await Reply(context, answer.Answers.First().Answer);

            string message = Constants.UnknownIntent;
            await Reply(context, message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("打招呼")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await SendGreeting(context);
            context.Wait(MessageReceived);
        }

        [LuisIntent("操作帮助")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await Reply(context, Constants.Help);
            context.Wait(MessageReceived);

        }

        [LuisIntent("询问信息")]
        public async Task Query(IDialogContext context, LuisResult result)
        {
            bool replied = false;
            EntityRecommendation entity;
            if (result.TryFindEntity("名字::部门", out entity))
            {
                replied = true;
                string department = entity.Entity;
                await Reply(context, DepartmentInfo.Items.FirstOrDefault(x => x.Name == department).ToString() ?? DepartmentInfo.NotExist);
            }
            if (result.TryFindEntity("名字::比赛", out entity))
            {
                replied = true;
                string competition = entity.Entity;
                await Reply(context, CompetitionInfo.Items.FirstOrDefault(x => x.Name == competition).Description ?? CompetitionInfo.NotExist);
            }
            if (result.TryFindEntity("名字::活动", out entity))
            {
                replied = true;
                string activity = entity.Entity;
                await Reply(context, ActivityInfo.Items.FirstOrDefault(x => x.Name == activity).Description ?? ActivityInfo.NotExist);
            }
            if (result.TryFindEntity("名字::福利", out entity))
            {
                replied = true;
                string benefit = entity.Entity;
                await Reply(context, BenefitInfo.Items.FirstOrDefault(x => x.Name == benefit).Description ?? BenefitInfo.NotExist);
            }
            if (result.TryFindEntity("名字::俱乐部", out entity))
            {
                replied = true;
                await Reply(context, ClubIntro.ToString());
            }

            if (!replied)
            {
                await Reply(context, Constants.UnknownIntent);
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("询问部门")]
        public async Task QueryDepartment(IDialogContext context, LuisResult result)
        {
            context.Call(new InfoDialog<Department>(DepartmentInfo), QueryDialogResumeAfter);

        }

        public async Task QueryDialogResumeAfter(IDialogContext context, IAwaitable<object> argument)
        {
            await SendGreeting(context);
            context.Wait(MessageReceived);
        }

        [LuisIntent("询问比赛")]
        public async Task QueryCompetitions(IDialogContext context, LuisResult result)
        {
            context.Call(new InfoDialog<Item>(CompetitionInfo), QueryDialogResumeAfter);
        }

        [LuisIntent("询问活动")]
        public async Task QueryActivities(IDialogContext context, LuisResult result)
        {
            context.Call(new InfoDialog<Item>(ActivityInfo), QueryDialogResumeAfter);
        }

        [LuisIntent("询问福利")]
        public async Task QueryBenefits(IDialogContext context, LuisResult result)
        {
            context.Call(new InfoDialog<Item>(BenefitInfo), QueryDialogResumeAfter);
        }

        [LuisIntent("询问加入俱乐部")]
        public async Task QueryJoining(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();

            var qqgroupImage = new Attachment()
            {
                ContentType = "image/jpg",
                ContentUrl = ClubIntro.GroupImage
            };

            reply.Attachments = new List<Attachment>
            {
                qqgroupImage
            };
            reply.Text = Constants.Joining;

            await Reply(context, reply);
            context.Wait(MessageReceived);
        }


        public async Task SendGreeting(IDialogContext context)
        {
            await Reply(context, Constants.Welcome);
        }

        public async Task Reply(IDialogContext context, string text)
        {
            IMessageActivity message = context.MakeMessage();
            var actions = Constants.Operations.Select(x => new CardAction() { Title = x.Key, Value = x.Value }).ToList();
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = actions
            };
            message.Text = text;
            await context.PostAsync(message);
        }

        public async Task Reply(IDialogContext context, IMessageActivity reply)
        {
            var actions = Constants.Operations.Select(x => new CardAction() { Title = x.Key, Value = x.Value }).ToList();
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = actions
            };
            await context.PostAsync(reply);
        }

    }
}