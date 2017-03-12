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
    public class YesNoQuestionDialog : AbsDialog<Boolean>
    {
        public override UserContext getDialogContext()
        {
            UserContext.dialog = "YesNoQuestionDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            getUser(context);
            getStudySession(context);
            context.Wait<string[]>(userYesNo);
        }

        private async Task userYesNo(IDialogContext context, IAwaitable<string[]> result)
        {
            var question = await result;
            await writeMessageToUser(context, question);
            context.Wait(userYesNoRes);
        }

        private async Task userYesNoRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
          
            var res = await result;
            var intent = conv().getUserIntente(res.Text, getDialogContext());

            if(intent == NLP.NLP.UserIntent.yes)
            {
                context.Done(true);
            }
            else if(intent == NLP.NLP.UserIntent.no)
            {
                context.Done(false);
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.didntUnderstand));
                context.Wait(userYesNoRes);
            }

  
        }

       
    }
}