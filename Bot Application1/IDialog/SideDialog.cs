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
    public class SideDialog : AbsDialog<IMessageActivity>
    {
        public override UserContext getDialogContext()
        {
            UserContext.dialog = "SideDialog";
            return UserContext;
        }
    


        public override async Task StartAsync(IDialogContext context)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.unknownQuestion));
            context.Done("");
        }


    }
}