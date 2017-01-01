using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using NLPtest;

namespace Bot_Application1.IDialog
{

    [Serializable]
    public abstract class AbsDialog : IDialog<object>
    {
        public abstract Task StartAsync(IDialogContext context);
        internal User user;
       

        internal async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            foreach (var m in newMessage)
            {
                await context.PostAsync(m);
            }
        }

    }




}