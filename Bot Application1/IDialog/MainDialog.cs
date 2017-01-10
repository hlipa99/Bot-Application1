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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MainDialog : AbsDialog
    {



        public override async Task StartAsync(IDialogContext context)
        {

            context.UserData.TryGetValue<Users >("user", out user);

            if (user != null)
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
            context.UserData.TryGetValue<Users>("user", out user);
            
            await writeMessageToUser(context, conv().greetings());
            await writeMessageToUser(context, conv().howAreYou());
            context.Wait(HowAreYouRes);
        }


        private async Task HowAreYouRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.UserData.TryGetValue<Users>("user", out user);
            
            var text = await result;
            await writeMessageToUser(context, conv().OK());
            await MainMenu(context, result);
        }


        private async Task MainMenu(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                context.UserData.TryGetValue<Users>("user", out user);
                
                //  var message = await result;
                context.UserData.TryGetValue<Users>("user", out user);
                if (user != null)
                {
                    var menu = new MenuOptionDialog<string>(
                        conv().MainMenuOptions(),
                        conv().MainMenuText(),
                        conv().wrongOption()[0],
                        3, new IDialog<object>[] {
                    new StartLerningDialog(),
                    new NotImplamentedDialog(),},
                         new ResumeAfter <object>[] {
                       EndSession,MainMenu}
                         );

                    context.Call(menu, MainMenu);

                }
                else
                {
                    context.Call(new NewUserDialog(), MainMenu);
                }
            }catch(Exception ex)
            {
                await writeMessageToUser(context, new string[] { "אוקיי זה מביך " + "\U0001F633", "קרתה לי תקלה בשרת ואני לא יודע מה לעשות", "אני אתחיל עכשיו מהתחלה ונעמיד פנים שלא קרה כלום, " + "\U0001F648", "טוב"+ "?" });
                Logger.log("MainDialog","MainMenu",ex.ToString());
                await StartAsync(context);
            }

            
            //await context.Forward<object,MenuObject>(new MenuOptionDialog(), MainMenuChooseOption,
            //    new MenuObject(), CancellationToken.None);
            //await OptionsMenu(context,result, BotControler.MainMenuText(), BotControler.MainMenuOptions());
            //context.Wait(MainMenuChooseOption);

        }


        private async Task EndSession(IDialogContext context, IAwaitable<object> result)
        {

            context.UserData.TryGetValue<Users>("user", out user);
            
            await writeMessageToUser(context, conv().goodbye());
            context.Done("");

            //await context.Forward<object,MenuObject>(new MenuOptionDialog(), MainMenuChooseOption,
            //    new MenuObject(), CancellationToken.None);
            //await OptionsMenu(context,result, BotControler.MainMenuText(), BotControler.MainMenuOptions());
            //context.Wait(MainMenuChooseOption);

        }

        private async Task MainMenuChooseOption(IDialogContext context, IAwaitable<object> result)
        {
            context.UserData.TryGetValue<Users>("user", out user);
            
            var message = await result;
            var text = result as IMessageActivity;
            var choosen = conv().MainMenuText();
            switch (conv().MainMenuOptions().ToList().IndexOf(text.Text))
            {
                case 1:
                    //StartAsync learning session
                    context.Call(new StartLerningDialog(), MainMenu);
                    break;
                case 2:
                    //Edit User Data
                    context.Call(new NotImplamentedDialog(), MainMenu);
                    break;
                default:
                    context.Call(new NotImplamentedDialog(), MainMenu);
                    break;
            }


    }
}