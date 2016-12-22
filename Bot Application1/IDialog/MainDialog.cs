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
using NLPtest;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MainDialog : IDialog<object>
    {

        User user;

        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(this.MessageReceivedAsync);

        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage = new string[20];



            var message = await result;
            String userId = message.From.Id;



            context.UserData.TryGetValue<User>("user", out user);

            //bool user = DataBaseControler.isUserExist(userId);

            if (user != null)
            {

                context.Wait(this.MessageReceivedAsync);
                await Greeting(context, result);
            }
            else
            {
                await context.Forward<object, IMessageActivity>(new NewUserDialog(),
                MainMenu,
                message,
                System.Threading.CancellationToken.None);

            }

            // message = context.MakeMessage();
            //ManagerCard MC = new ManagerCard();
            //var attachment = MC.GetMainMenuChoice("", "", "", "", buttonMessage);
            //message.Attachments.Add(attachment);
            //await context.PostAsync(message);
            //context.Wait(this.MessageReceivedAsync);


            //var message = await result;
            //await context.PostAsync("You said: " + message.Text);
            //context.Wait(MessageReceivedAsync);

        }

        private async Task Greeting(IDialogContext context, IAwaitable<object> result)
        {
            await writeMessageToUser(context, BotControler.greetings(user));
            await writeMessageToUser(context, BotControler.howAreYou(user));
            context.Wait(HowAreYouRes);
        }


        private async Task HowAreYouRes(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var text = await result;


            var feel = (BotControler.getGeneralFeeling(text.Text));
            if (feel == "good")
            {
                await writeMessageToUser(context, BotControler.veryGood(user));
            }
            else if (feel == "bad")
            {
                await writeMessageToUser(context, BotControler.SoSorry(user));
            }
            else
            {
                await writeMessageToUser(context, BotControler.OK(user));
            }


            await writeMessageToUser(context, BotControler.LetsStart());

            await MainMenu(context, result);
        }




        private async Task MainMenu(IDialogContext context, IAwaitable<object> result)
        {
            await writeMessageToUser(context, new string[] { "תפריט: בחר מה תרצה לעשות" });
            await writeMessageToUser(context, new string[] { "ללמוד" });
            context.Wait(MainMenuChooseOption);
        }

        private async Task MainMenuChooseOption(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            await writeMessageToUser(context, BotControler.letsLearn());
            await context.Forward<object, IMessageActivity>(new StartLerningDialog(),
               MainMenu,
               message,
               System.Threading.CancellationToken.None);


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

        private static async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            foreach (var m in newMessage)
            {
                await context.PostAsync(m);
            }
        }

    }
}