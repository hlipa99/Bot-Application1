using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Bot_Application1.Cardatt_achment;
using Bot_Application1.dataBase;
using System.Threading;
using NLPtest;
using Model.dataBase;
using Bot_Application1.log;
using static Bot_Application1.Controllers.ConversationController;
using Model;
using Model.Models;
using Bot_Application1.Exceptions;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class GreetingDialog : AbsDialog<IMessageActivity>
    {


        public override UserContext getDialogContext()
        {
            UserContext.dialog = "GreetingDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.greetings));
            await HowAreYouQuestion(context,null);
        }


        private async Task HowAreYouQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.howAreYou));
            context.Wait(HowAreYouRes);
        }

        private async Task HowAreYouRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await outDatedMessage(context, returnToParent, result)) return;

            var text = await result;
            await writeMessageToUser(context, conv().getPhrase(Pkey.ok));
            context.Done("");
        }


        private async Task returnToParent(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done("");
        }


    }
}