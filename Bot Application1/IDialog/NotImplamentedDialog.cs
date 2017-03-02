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
using Model;
using Model.Models;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class NotImplamentedDialog : AbsDialog<IMessageActivity>
    {

      
        public override async Task StartAsync(IDialogContext context)
        {
            if(User == null)
            {
                User = new User();
                User.UserName = "";
                User.UserGender = "masculine";
            }
         

            await context.PostAsync(conv().getPhrase(Pkey.NotImplamented)[0]);
            context.Done("");

        }

        public override UserContext getDialogContext()
        {
            UserContext.dialog = "NotImplamentedDialog";
            return UserContext;
        }



    }
}