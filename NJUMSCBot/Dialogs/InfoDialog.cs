using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NJUMSCBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static NJUMSCBot.Data.Data;

namespace NJUMSCBot.Dialogs
{
    [Serializable]
    public class InfoDialog<T> : IDialog<object> where T:Item
    {
        Info<T> info;
        T[] items;

        public InfoDialog(Info<T> info )
        {
            this.info = info;
            this.items = info.Items;
        }

        public async Task Reply(IDialogContext context, string text)
        {
            var reply = context.MakeMessage();
            List<CardAction> actions = new List<CardAction>();

            actions.AddRange(items.Select(x => new CardAction()
            {
                Value = x.Name,
                Title = x.Name
            }));

            actions.Add(new CardAction()
            {
                Value = "全部",
                Title = "全部"
            });


            if (info.Previous != null)
            {
                actions.Add(new CardAction()
                {
                    Value = "以往的",
                    Title = "以往的活动"
                });
            }

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

            await Reply(context, info.Introduction);

            context.Wait(AfterEnterFurtherInformation);
        }


        public async Task AfterEnterFurtherInformation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (await argument).Text;

            if (message == "全部")
            {
                foreach (T d in items)
                {
                    await Reply(context, d.Description);
                }
                context.Wait(AfterEnterFurtherInformation);
                return;
            }

            else if (message == "返回")
            {
                context.Done("");
                return;
            }

            else if (message.Contains("以往的"))
            {
                if (info.Previous == null)
                {
                    await Reply(context, "抱歉在这个主题下我们没有以往的活动。");
                }
                else
                {
                    string content = string.Join("\n\n", info.Previous.Select(x => $"{x.Key}: [点击查看微信推送]({x.Value}"));
                    await Reply(context, content);
                }
                context.Wait(AfterEnterFurtherInformation);
                return;

            }

            

            bool output = false;
            foreach (T d in items)
            {
                if (message.Contains(d.Name))
                {
                    output = true;
                    await Reply(context, d.Description);
                    if (d.ReadMoreUrl != null)
                    {
                        await Reply(context, $"点[这里]({d.ReadMoreUrl})知道更多有关{d.Name}的信息！！");
                    }
                }
            }
            if (!output)
            {
                await Reply(context, info.NotExist);
            }
            context.Wait(AfterEnterFurtherInformation);
        }
    }
}