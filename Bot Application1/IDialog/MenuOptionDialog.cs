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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MenuOptionDialog : AbsDialog<string>
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
            UserContext.dialog = "MenuOptionDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {

            await checkOutdatedMessage(context, null);
        }

        private async Task checkOutdatedMessage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = context.MakeMessage();
            message.AddHeroCard(prompt, options);
            await context.PostAsync(message);
            context.Wait(optionsRes);
        }

        protected async Task optionsRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await checkOutdatedMessage<object, IMessageActivity>(context, checkOutdatedMessage, result)) return;

            //    var message = await result;

            var message = await result;

            var coise = conv().FindMatchFromOptions(options,message.Text);
            if(coise != null)
            {
                context.Done(coise);
            }else
            {
                await writeMessageToUser(context, new string[] { retry });
                await StartAsync(context);
            }


        }

    }

}