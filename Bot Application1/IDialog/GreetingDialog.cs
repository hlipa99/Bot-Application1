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
            getUser(context);
            if (User.UserLastSession == null || User.UserLastSession.Value.AddHours(1) > context.Activity.Timestamp)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.shortHello));
                context.Done("");
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.greetings));
                User.UserLastSession = context.Activity.Timestamp;
                setUser(context);
                context.Wait(HowAreYouQuestion);
            }
          
        }


        private async Task HowAreYouQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            //the user sent a message
            if (result.GetAwaiter().IsCompleted)
            {
                var text = await result;
                var replay = conv().createReplayToUser(text.Text, UserContext);
                await writeMessageToUser(context, replay);
            }

            await writeMessageToUser(context, conv().getPhrase(Pkey.howAreYou));
            context.Wait(HowAreYouRes);


        }

        private async Task HowAreYouRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
             if (await checkOutdatedMessage(context, returnToParent, result)) return;
  
            await writeMessageToUser(context, conv().getPhrase(Pkey.ok));
            context.Done("");
        }


        private async Task returnToParent(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done("");
        }


    }
}