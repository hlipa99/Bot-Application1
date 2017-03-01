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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class SideDialog : AbsDialog
    {
        public override string getDialogContext()
        {
            return "SideDialog";
        }

        public override async Task StartAsync(IDialogContext context)
        {
            context.Done("");
        }


    }
}