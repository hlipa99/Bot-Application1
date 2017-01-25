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
using Model.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MainDialog : AbsDialog
    {

        public override async Task StartAsync(IDialogContext context)
        {
            getUser(context);
            if (User != null)
            {
                await Greeting(context);
            }
            else
            {
                context.Call(new NewUserDialog(), MainMenu);
            }
        }

   

        private async Task Greeting(IDialogContext context)
        {
        

            await writeMessageToUser(context, conv().getPhrase(Pkey.greetings));
            await writeMessageToUser(context, conv().getPhrase(Pkey.howAreYou));
            context.Wait(HowAreYouRes);

        }


        private async Task HowAreYouRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
    

            var text = await result;
         //   await writeMessageToUser(context, conv().getPhrase(Pkey.ok));
            await MainMenu(context, result);

        }


        private async Task MainMenu(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                getUser(context);
                if (User != null)
                {
                    var menu = new MenuOptionDialog<string>(
                        conv().MainMenuOptions(),
                        conv().getPhrase(Pkey.MainMenuText)[0],
                        conv().getPhrase(Pkey.wrongOption)[0],
                        3, new IDialog<object>[] {
                    new StartLerningDialog(),
                    new NotImplamentedDialog(),},
                         new ResumeAfter<object>[] {
                       EndSession,EndSession,EndSession}
                         );

                    context.Call(menu, EndSession2);

                }
                else
                {
                    context.Call(new NewUserDialog(), MainMenu);
                }
            }
            catch (EndOfLearningSessionException ex)
            {
                context.Wait(EndOfLearningSession);
            }
            catch (Exception ex)
            {
                await writeMessageToUser(context, new string[] { "אוקיי זה מביך " + "\U0001F633", "קרתה לי תקלה בשרת ואני לא יודע מה לעשות", "אני אתחיל עכשיו מהתחלה ונעמיד פנים שלא קרה כלום, " + "\U0001F648", "טוב" + "?" });
                await writeMessageToUser(context,  new string[] { ex.Data.ToString(), ex.InnerException.ToString(), ex.StackTrace.ToString(), ex.TargetSite.ToString(), ex.ToString() });
          //     Logger.log("MainDialog", "MainMenu", ex.ToString());
               await StartAsync(context);
            }




        }

        private async Task EndOfLearningSession(IDialogContext context, IAwaitable<object> result)
        {
            await StartAsync(context);
        }

        private async Task EndSession(IDialogContext context, IAwaitable<object> result)
        {
         //   context.Wait(MainMenu);
            context.Call(new FarewellDialog(), MainMenu);
        }

        private async Task EndSession2(IDialogContext context, IAwaitable<object> result)
        {
            //   context.Wait(MainMenu);
            context.Call(new FarewellDialog(), MainMenu);
        }


    }
}