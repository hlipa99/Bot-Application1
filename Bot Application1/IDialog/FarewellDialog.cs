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
using Model.dataBase;
using Bot_Application1.log;
using static Bot_Application1.Controllers.ConversationController;
using Model;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class FarewellDialog : AbsDialog<IMessageActivity>
    {
        public override UserContext getDialogContext()
        {
            base.getDialogContext();
            UserContext.dialog = "FarewellDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            getDialogsVars(context);
            await writeMessageToUser(context, conv().getPhrase(Pkey.goodbye));
            context.Wait(userGoodbye);
        }

        private async Task userGoodbye(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(waitForNextInteraction);
        }

        private async Task waitForNextInteraction(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(waitForNextInteraction2);
        }

        private async Task waitForNextInteraction2(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("");
        }



    }
}