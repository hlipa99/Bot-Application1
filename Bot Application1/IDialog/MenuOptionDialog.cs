using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Bot_Application1.Cardatt_achment;

using System.Threading;
using NLP;
using Bot_Application1.Controllers;
using NLP.Models;
using Model.Models;
using Bot_Application1.Models;
using Model;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Builder.ConnectorEx;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MenuOptionDialog : AbsDialog<IMessageActivity>
    {
        private string[] options;
        private string prompt;
        private string retry;

        public MenuOptionDialog(string[] options, string prompt, string retry)
        {
            this.options = options;
            this.prompt = prompt;
            this.retry = retry;
        }

        public override UserContext getDialogContext()
        {
            base.getDialogContext();
            UserContext.dialog = "MenuOptionDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await choosePlatform(context, null);
            
        }


        public async virtual Task choosePlatform(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            getDialogsVars(context);
            if (context.Activity.ChannelId == "facebook")
            {
                await createQuickReplay(context);
            }
            else
            {
                await displayHeroCardWithOptions(context);
            }


        }
        public async virtual Task createQuickReplay(IDialogContext context)
        {

            //     await writeMessageToUser(context, new string[] { title });

            var reply = context.MakeMessage();
            var channelData = new JObject();
            var quickReplies = new JArray();
            var qrList = new List<FacebookQuickReply>();
            foreach (var s in options)
            {
                var r = new FacebookQuickReply("text", s, s);
                qrList.Add(r);
            }
            var message = new FacebookMessage(prompt, qrList);
            reply.ChannelData = message;
            await context.PostAsync(reply);

            updateRequestTime(context);
            context.Wait(optionsRes);
        }


        private async Task displayHeroCardWithOptions(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.AddHeroCard(prompt, options);
            await context.PostAsync(message);
            context.Wait(optionsRes);
        }

        protected async Task optionsRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await checkOutdatedMessage<IMessageActivity>(context, choosePlatform, result)) return;
            getDialogsVars(context);
            //    var message = await result;

            var message = await result;
            var choise = conv().FindMatchFromOptions(options, message.Text);
            if (choise != null)
            {
                message.Text = choise;
                context.Done(message);
            }
            else
            {
                var response = conv().getUserConvResponse(message.Text, getDialogContext());
                if (response != null)
                {
                    await writeMessageToUser(context, response);
                    await writeMessageToUser(context, conv().getPhrase(Pkey.letsContinue));
                    await StartAsync(context);
                    return;
                }
                else
                {
                    await writeMessageToUser(context, new string[] { retry });
                    await StartAsync(context);
                    return;
                }
   
            }


        }

    }

}