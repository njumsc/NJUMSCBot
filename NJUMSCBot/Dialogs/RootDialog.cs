using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;
using System.Linq;
using NJUMSCBot.Models;

namespace NJUMSCBot.Dialogs
{
    public class StringConstants
    {
        public const string IntentUnknown = "Intent unknown";
        public const string Help = "help";
        public const string Metainfo = "metainfo";
        public const string SelfIntroduction = "self introduction";
    }


    [Serializable]
    [LuisModel("204f9894-2f57-4c7d-889f-31f2df44f0f3", "ccad6263e2cd434bab371a2562823097")]
    public class RootDialog : LuisDialog<object>
    {

        public Department[] departments = Item.Read<Department, Department[]>();


        public RootDialog()
        {
        }
        public RootDialog(ILuisService service) : base(service)
        {
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = StringConstants.IntentUnknown;
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("打招呼")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            IMessageActivity message = context.MakeMessage();
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                new CardAction(){ Title = StringConstants.Help, Value=StringConstants.Help },
                new CardAction(){ Title = StringConstants.Metainfo, Value=StringConstants.Metainfo }
                }
            };
            message.Text = StringConstants.SelfIntroduction;
            await context.PostAsync(message);
            context.Wait(MessageReceived);

        }

        [LuisIntent("操作帮助")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            context.Wait(MessageReceived);

        }

        [LuisIntent("询问信息")]
        public async Task Query(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entity;
            if (result.TryFindEntity("名字::部门", out entity))
            {
                string department = entity.Entity;
                await context.PostAsync(departments.First(x => x.Name == department).Description);
            }
            else if (result.TryFindEntity("名字", out entity))
            {
                string output = await FindInfo(entity.Entity);
                await context.PostAsync(output);
            }
            else
            {
                await context.PostAsync($"抱歉我不清楚 {entity} 是什么……");
            }
            context.Wait(MessageReceived);
        }

        public async Task<string> FindInfo(string entity)
        {
            await Task.Delay(1000);
            return "123";
        }

        [LuisIntent("询问部门")]
        public async Task QueryDepartment(IDialogContext context, LuisResult result)
        {
            if (result.TryFindEntity("名字", out EntityRecommendation entity))
            {
                string output = await FindInfo(entity.Entity);
                await context.PostAsync(output);
            }
            else
            {
                await context.PostAsync($"抱歉我不清楚 {entity} 是什么……");
            }
            context.Wait(MessageReceived);
        }

    }
}