using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using NJUMSCBot.Configs;
using System.Collections.Generic;

namespace NJUMSCBot.Dialogs
{
    [Serializable]
    [LuisModel("204f9894-2f57-4c7d-889f-31f2df44f0f3", "ccad6263e2cd434bab371a2562823097")]
    public class RootDialog : LuisDialog<object>
    {
        public RootDialog()
        {
        }
        public RootDialog(ILuisService service) : base(service)
        {
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = StringConstants.INTENT_UNKNOWN;
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]

        [LuisIntent("打招呼")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            IMessageActivity message = context.MakeMessage();
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                new CardAction(){ Title = StringConstants.HELP, Value=StringConstants.HELP },
                new CardAction(){ Title = StringConstants.METAINFO, Value=StringConstants.METAINFO }
                }
            };
            message.Text = StringConstants.SELF_INTRODUCTION;
            await context.PostAsync(message);
            context.Wait(MessageReceived);

        }
    }
}