using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.UI.WebControls;


using System.Threading;
using NLP;
using Model.dataBase;
using Bot_Application1.log;
using static Bot_Application1.Controllers.ConversationController;
using Model;
using Bot_Application1.Models;
using NLP.NLP;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class YesNoQuestionDialog : AbsDialog<Boolean>
    {
        public override UserContext getDialogContext()
        {
            base.getDialogContext();
            UserContext.dialog = "YesNoQuestionDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            getDialogsVars(context);
            
            context.Wait<string[]>(userYesNo);
        }

        private async Task userYesNo(IDialogContext context, IAwaitable<string[]> result)
        {
            var question = await result;
           // await writeMessageToUser(context, question);
            await createMenuOptions(context, question[0], conv().getPhrase(Pkey.yesNoOptions), userYesNoRes);
        
            // context.Wait(userYesNoRes);
        }

        private async Task userYesNoRes(IDialogContext context, IAwaitable<object> result)
        {
    
              var res = await result;

            var text = res as string;
            if( text == null)
            {
                text = (res as IMessageActivity).Text;
            }

            UserIntent intent;
            if (text != null)
            {
                intent = conv().getUserIntente(text, getDialogContext());

            }
            else //facebook like
            {
                intent = NLP.NLP.UserIntent.yes;
            }


            if (intent == NLP.NLP.UserIntent.yes)
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