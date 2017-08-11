using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;
using System.Linq;
using NJUMSCBot.Models;
using NJUMSCBot.Data;
using System.Threading;

namespace NJUMSCBot.Dialogs
{

    [Serializable]
    [LuisModel("204f9894-2f57-4c7d-889f-31f2df44f0f3", "ccad6263e2cd434bab371a2562823097")]
    public class RootDialog : LuisDialog<object>
    {

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = StringConstants.UnknownIntent;
            await context.PostAsync(message);
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
            await context.PostAsync(StringConstants.Help);
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
                await context.PostAsync(Department.Departments.FirstOrDefault(x => x.Name == department).Description ?? StringConstants.DepartmentNotExist);
            }
            if (result.TryFindEntity("名字::比赛", out entity))
            {
                replied = true;
                string competition = entity.Entity;
                await context.PostAsync(Competition.Competitions.FirstOrDefault(x => x.Name == competition).Description ?? StringConstants.CompetitionNotExist);
            }
            if (result.TryFindEntity("名字::活动", out entity))
            {
                replied = true;
                string activity = entity.Entity;
                await context.PostAsync(ClubActivity.Activities.FirstOrDefault(x => x.Name == activity).Description ?? StringConstants.ActivityNotExist);
            }
            if (result.TryFindEntity("名字::福利", out entity))
            {
                replied = true;
                string benefit = entity.Entity;
                await context.PostAsync(Benefit.Benefits.FirstOrDefault(x => x.Name == benefit).Description ?? StringConstants.BenefitNotExist);
            }
            if (result.TryFindEntity("名字::俱乐部",out entity))
            {
                replied = true;
                await context.PostAsync(ClubIntroduction.Introduction);
            }

            if (!replied)
            {
                await context.PostAsync($"啊你想问什么……请重新输入一下问题，谢谢！");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("询问部门")]
        public async Task QueryDepartment(IDialogContext context, LuisResult result)
        {
            context.Call(new DepartmentDialog(), ResumeAfter);
            
        }

        public async Task ResumeAfter(IDialogContext context, IAwaitable<object> argument)
        {
            await SendGreeting(context);
            context.Wait(MessageReceived);
        }

        public async Task SendGreeting(IDialogContext context)
        {
            IMessageActivity message = context.MakeMessage();
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                new CardAction(){ Title = StringConstants.HelpPrompt, Value=StringConstants.HelpPrompt },
                new CardAction(){ Title = StringConstants.Metainfo, Value=StringConstants.Metainfo }
                }
            };
            message.Text = StringConstants.Welcome;
            await context.PostAsync(message);
        }

    }
}