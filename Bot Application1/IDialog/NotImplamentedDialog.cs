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
using NLPtest.WorldObj;
using Model.dataBase;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class NotImplamentedDialog : AbsDialog
    {

      
        public override async Task StartAsync(IDialogContext context)
        {
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            context.UserData.TryGetValue<Users>("user", out user);


            await context.PostAsync(conv.NotImplamented()[0]);
            context.Done("");

        }

     

    }
}