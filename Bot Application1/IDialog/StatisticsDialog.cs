using Bot_Application1.Models;
using Microsoft.Bot.Builder.Dialogs;
using Model;
using System.Threading.Tasks;

namespace Bot_Application1.IDialog
{
    internal class StatisticsDialog : AbsDialog<string>
    {
        public override UserContext getDialogContext()
        {
            UserContext.dialog = "LerningDialog";
            return UserContext;
        }


        public override async Task StartAsync(IDialogContext context)
        {
            getDialogsVars(context);
            await writeMessageToUser(context, edc().getUserStatistics());
            context.Done("");
        }

    }
}