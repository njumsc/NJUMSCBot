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

namespace NJUMSCBot.Dialogs
{

    [Serializable]
    [LuisModel("204f9894-2f57-4c7d-889f-31f2df44f0f3", "ccad6263e2cd434bab371a2562823097")]
    public class RootDialog : LuisDialog<object>
    {

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
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
                await Reply(context, DepartmentInfo.Items.FirstOrDefault(x => x.Name == department).Description ?? DepartmentInfo.NotExist);
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
            if (result.TryFindEntity("名字::俱乐部",out entity))
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
            string[] splitedText = text.Split('\n');

            for(int i = 0; i < splitedText.Length - 1; i++)
            {
                await context.PostAsync(splitedText[i]);
            }


            IMessageActivity message = context.MakeMessage();
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                    new CardAction(){ Title = Constants.OperationsIndexes.ClubIntroduction, Value = Constants.Operations.ClubIntroduction},
                    new CardAction(){ Title = Constants.OperationsIndexes.Activities, Value =Constants.Operations.Activities},
                    new CardAction(){ Title = Constants.OperationsIndexes.Benefits, Value = Constants.Operations.Benefits},
                    new CardAction(){ Title = Constants.OperationsIndexes.Competitions, Value = Constants.Operations.Competitions},
                    new CardAction(){ Title = Constants.OperationsIndexes.Departments, Value = Constants.Operations.Departments},
                    new CardAction(){ Title = Constants.OperationsIndexes.Joining, Value = Constants.Operations.Joining},
                    new CardAction(){ Title = Constants.OperationsIndexes.Help, Value = Constants.Operations.Help},
                }
            };
            message.Text = splitedText.Last();
            await context.PostAsync(message);
        }

        public async Task Reply(IDialogContext context, IMessageActivity reply)
        {
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                    new CardAction(){ Title = Constants.OperationsIndexes.ClubIntroduction, Value = Constants.Operations.ClubIntroduction},
                    new CardAction(){ Title = Constants.OperationsIndexes.Activities, Value =Constants.Operations.Activities},
                    new CardAction(){ Title = Constants.OperationsIndexes.Benefits, Value = Constants.Operations.Benefits},
                    new CardAction(){ Title = Constants.OperationsIndexes.Competitions, Value = Constants.Operations.Competitions},
                    new CardAction(){ Title = Constants.OperationsIndexes.Departments, Value = Constants.Operations.Departments},
                    new CardAction(){ Title = Constants.OperationsIndexes.Joining, Value = Constants.Operations.Joining},
                    new CardAction(){ Title = Constants.OperationsIndexes.Help, Value = Constants.Operations.Help},
                }
            };
            await context.PostAsync(reply);
        }

    }
}