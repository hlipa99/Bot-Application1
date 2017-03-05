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
            await userYesNo(context,null);
        }

        private async Task userYesNo(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Wait(userYesNoRes);
        }

        private async Task userYesNoRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await outDatedMessage(context, userYesNo, result)) return;


            var res = await result;
            if (res.Text == "כן")
            {
                context.Done(true);
            }else
            {
                context.Done(false);
            }
  
        }

       
    }
}