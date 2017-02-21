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

        string[] options;

        private async Task MainMenu(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                getUser(context);
                if (User != null)
                {
                    options = conv().MainMenuOptions();
                    await createMenuOptions(context, conv().getPhrase(Pkey.MainMenuText)[0], options, MainMenuResualt);
                    
                    //var menu = new MenuOptionDialog<string>(
                  //      conv().MainMenuOptions(),
                  //      conv().getPhrase(Pkey.MainMenuText)[0],
                  //      conv().getPhrase(Pkey.wrongOption)[0],
                  //      3, new IDialog<object>[] {
                  //  new StartLerningDialog(),
                  //  new NotImplamentedDialog(),},
                  //       new ResumeAfter<object>[] {
                  //     EndOfLearningSession,EndOfLearningSession}
                  //       );

                  //  context.Call(menu, UnknownException);

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

        private async Task MainMenuResualt(IDialogContext context, IAwaitable<object> result)
        {
            var text = await result;
            var option = "";

            if (text is string)
            {
                option = (string)text;
            }
            else
            {
                option = ((IMessageActivity)text).Text;
            }



            if (options.Contains(option))
            {
                var optionIdx = options.ToList().IndexOf(option);
                switch (optionIdx)
                {
                    case 0:  //start learning
                        context.Call(new LerningDialog(),EndOfLearningSession);
                        break;
                    case 1:  //not implamented
                        context.Call(new NotImplamentedDialog(), returnToMainMenu);
                        break;
                    default:
                        break;
                        await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption));
                        await MainMenu(context,result);
                }
            }else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption));
                await MainMenu(context, result);
            }

        }

        private async Task returnToMainMenu(IDialogContext context, IAwaitable<object> result)
        {
       //     await writeMessageToUser(context, conv().getPhrase(Pkey.returnToMainMenu));
            await MainMenu(context, result);
        }

        private async Task UnknownException(IDialogContext context, IAwaitable<object> result)
        {
            await writeMessageToUser(context, new string[] { "אוקיי זה מביך " + "\U0001F633", "קרתה לי תקלה בשרת ואני לא יודע מה לעשות", "אני אתחיל עכשיו מהתחלה ונעמיד פנים שלא קרה כלום, " + "\U0001F648", "טוב" + "?" });
            await StartAsync(context);
        }



        private async Task EndOfLearningSession(IDialogContext context, IAwaitable<object> result)
        {
            await EndSession(context,result);
        }

        private async Task EndSession(IDialogContext context, IAwaitable<object> result)
        {
         //   context.Wait(MainMenu);
            context.Call(new FarewellDialog(), MainMenu);
        }

        private async Task NotAvialable(IDialogContext context, IAwaitable<object> result)
        {

            context.Call(new NotImplamentedDialog(), EndOfLearningSession);
        }


    }
}