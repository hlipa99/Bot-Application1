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
    public class MainDialog : AbsDialog
    {



        public override async Task StartAsync(IDialogContext context)
        {

            context.Wait(this.MessageReceivedAsync);

        }


        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage = new string[20];



            var message = await result;
            String userId = message.From.Id;
            String userName = "";//DataBaseControler.getUserName(userId);
            //     message = context.MakeMessage();
            User user1 = new User();

            context.UserData.SetValue<User>("user", user1);
            context.UserData.TryGetValue<User>("user", out user);

            if (user != null)
            {

                await Greeting(context, result);
            }
            else
            {
                context.Call(new NewUserDialog(), MainMenu);
            }
        }

        private async Task Greeting(IDialogContext context, IAwaitable<object> result)
        {
              ConversationController conv = new ConversationController();
             await writeMessageToUser(context, conv.greetings(user));
            await writeMessageToUser(context, conv.howAreYou(user));
            context.Wait(HowAreYouRes);
        }


        private async Task HowAreYouRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            ConversationController conv = new ConversationController();
            var text = await result;
            await writeMessageToUser(context, conv.OK(user));
            await MainMenu(context, result);
        }


        private async Task MainMenu(IDialogContext context, IAwaitable<object> result)
        {
            ConversationController conv = new ConversationController();
            //  var message = await result;
            context.UserData.TryGetValue<User>("user", out user);
            if (user != null)
            {
                var menu = new MenuOptionDialog<string>(
                    conv.MainMenuOptions(),
                    conv.MainMenuText(user),
                    conv.wrongOption()[0],
                    3,new IDialog<object>[] {
                    new StartLerningDialog(),
                    new NotImplamentedDialog(),
                    new NotImplamentedDialog(),
                    new NotImplamentedDialog()},
                    MainMenu
                     );

                context.Call(menu, MainMenu);

            }
            else
            {
                context.Call(new NewUserDialog(), MainMenu);
            }

            
            //await context.Forward<object,MenuObject>(new MenuOptionDialog(), MainMenuChooseOption,
            //    new MenuObject(), CancellationToken.None);
            //await OptionsMenu(context,result, BotControler.MainMenuText(user), BotControler.MainMenuOptions());
            //context.Wait(MainMenuChooseOption);

        }

        private async Task MainMenuChooseOption(IDialogContext context, IAwaitable<object> result)
        {
            ConversationController conv = new ConversationController();
            var message = await result;
            var text = result as IMessageActivity;
            var choosen = conv.MainMenuText(user);
            switch (conv.MainMenuOptions().ToList().IndexOf(text.Text))
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



            //await writeMessageToUser(context, BotControler.letsLearn());
            //await context.Forward<object, IMessageActivity>(new StartLerningDialog(),
            //   MainMenu,
            //   message,
            //   System.Threading.CancellationToken.None);
        }



        private async Task OptionsMenu(IDialogContext context, IAwaitable<object> result,string headline,string[] options)
        {

            //  var message = await result;
            var message = context.MakeMessage();
            var attachment = ManagerCard.GetCardAction(headline, options);
            message.Attachments.Add(attachment);
            await context.PostAsync(message);
            context.ConversationData.SetValue<string[]>("optionsMenu", options);
            context.Wait(OptionsMenuChoose);
        }

        private async Task OptionsMenuChoose(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var text = message.Text;
            string[] options;
            context.ConversationData.TryGetValue<string[]>("optionsMenu", out options);
            if(options != null)
            {
               
                int num = -1;
                int.TryParse(text, out num);
                if(num >= 0 && num < options.Length)
                {
                    context.Done<string>(options[num]);
                }
             
            }
            context.Done<string>(text);
        }

   

        //public async Task userExist(IDialogContext context, IAwaitable<String> result)
        //{
        //    String[] buttonMessage = new string[20];

        //    //var message = await result;
        //    //await context.PostAsync("ברוך הבא חיים טןב לראותך שוב!");
        //    //context.Wait(this.MessageReceivedAsync);



        //    //var message = context.MakeMessage();
        //    //ManagerCard mc = new ManagerCard();
        //    //var attachment = mc.GetMainMenuChoice("", "", "", "", buttonMessage);
        //    //message.Attachments.Add(attachment);
        //    //await context.PostAsync(message);
        //    //context.Wait(this.MessageReceivedAsync);

        //}


    }
}