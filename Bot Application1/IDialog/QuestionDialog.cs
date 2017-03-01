using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class QuestionDialog : AbsDialog
    {


        public override string getDialogContext()
        {
            return "NotImplamentedDialog";
        }

        public override async Task StartAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }
    }
}