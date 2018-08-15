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
    public class LuisDialog : LuisDialog<object>
    {
        public LuisDialog() : base()
        {

        }

        public LuisDialog(ILuisService service) : base(service)
        {
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.Forward(new QnADialog(), ResumeAfterQna, context.Activity.AsMessageActivity(), CancellationToken.None);
        }

        public async Task ResumeAfterQna(IDialogContext context, IAwaitable<bool> result)
        {
            if (!await result)
            {
                await ReplyAsync(context, Constants.UnknownIntent);
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("打招呼")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await ReplyAsync(context, Constants.Welcome);
            context.Wait(MessageReceived);
        }

        [LuisIntent("操作帮助")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await ReplyAsync(context, Constants.Help);
            context.Wait(MessageReceived);

        }

        [LuisIntent("询问信息")]
        public async Task Query(IDialogContext context, LuisResult result)
        {
            bool replied = false;
            if (result.TryFindEntity("名字::部门", out EntityRecommendation entity))
            {
                replied = true;
                string department = entity.Entity;
                await ReplyAsync(context, DepartmentInfo.Items.FirstOrDefault(x => x.Names.Contains(department)).ToString() ?? DepartmentInfo.NotExist);
            }
            if (result.TryFindEntity("名字::比赛", out entity))
            {
                replied = true;
                string competition = entity.Entity;
                await ReplyAsync(context, CompetitionInfo.Items.FirstOrDefault(x => x.Names.Contains(competition)).Description ?? CompetitionInfo.NotExist);
            }
            if (result.TryFindEntity("名字::活动", out entity))
            {
                replied = true;
                string activity = entity.Entity;
                await ReplyAsync(context, ActivityInfo.Items.FirstOrDefault(x => x.Names.Contains(activity)).Description ?? ActivityInfo.NotExist);
            }
            if (result.TryFindEntity("名字::福利", out entity))
            {
                replied = true;
                string benefit = entity.Entity;
                await ReplyAsync(context, BenefitInfo.Items.FirstOrDefault(x => x.Names.Contains(benefit)).Description ?? BenefitInfo.NotExist);
            }
            if (result.TryFindEntity("名字::俱乐部", out entity))
            {
                replied = true;
                await ReplyAsync(context, ClubIntro.ToString());
            }

            if (!replied)
            {
                await None(context, result);
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
            await ReplyAsync(context, Constants.Welcome);
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

            await ReplyAsync(context, reply);
            context.Wait(MessageReceived);
        }

        public async static Task ReplyAsync(IDialogContext context, string text)
        {
            IMessageActivity message = context.MakeMessage();
            message.Text = text;
            await ReplyAsync(context, message);
        }

        public async static Task ReplyAsync(IDialogContext context, IMessageActivity reply)
        {
            var actions = Constants.Operations.Select(
                x => new CardAction() {
                    Title = x.Key,
                    Value = x.Value,
                    Type = ActionTypes.ImBack
                })
                .ToList();
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = actions
            };
            await context.PostAsync(reply);
        }

    }
}