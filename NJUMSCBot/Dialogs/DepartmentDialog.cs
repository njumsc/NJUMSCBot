using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using NJUMSCBot.Models;
using NJUMSCBot.Data;

namespace NJUMSCBot.Dialogs
{
    [Serializable]
    public class DepartmentDialog : IDialog<object>
    {
        Department[] departments;


        public DepartmentDialog(Department[] departments)
        {
            this.departments = departments;
        }


        public async Task Reply(IDialogContext context, string text)
        {
            var reply = context.MakeMessage();
            List<CardAction> actions = new List<CardAction>();

            actions.AddRange(departments.Select(x => new CardAction()
            {
                Value = x.Name,
                Title = x.Name
            }));

            actions.Add(new CardAction()
            {
                Value = "全部",
                Title = "全部"
            });

            actions.Add(new CardAction()
            {
                Value = "返回",
                Title = "返回"
            });

            reply.SuggestedActions = new SuggestedActions() { Actions = actions };
            reply.Text = text;
            await context.PostAsync(reply);
        }

        public async Task StartAsync(IDialogContext context)
        {

            await Reply(context, StringConstants.DepartmentIntroduction);
           
            context.Wait(AfterEnterDepartmentName);
        }


        public async Task AfterEnterDepartmentName(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (await argument).Text;

            if (message == "全部")
            {
                foreach (Department d in departments)
                {
                    await Reply(context, d.Description);
                }
                context.Wait(AfterEnterDepartmentName);
                return;
            }

            else if (message == "返回")
            {
                context.Done("");
                return;
            }

            bool output = false;
            foreach (Department d in departments)
            {
                if (message.Contains(d.Name))
                {
                    output = true;
                    await Reply(context, d.Description);
                }
            }
            if (!output)
            {
                await Reply(context, StringConstants.DepartmentNotExist);
            }
            context.Wait(AfterEnterDepartmentName);
        }
    }
}