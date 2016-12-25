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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class NotImplamentedDialog : IDialog<object>
    {

        User user;

        public async Task StartAsync(IDialogContext context)
        {

            context.UserData.TryGetValue<User>("user", out user);


            await context.PostAsync(BotControler.NotImplamented(user)[0]);
            context.Done("");

        }

     
        private static async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            foreach (var m in newMessage)
            {
                await context.PostAsync(m);
            }
        }

    }
}