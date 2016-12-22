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
            bool user = DataBaseControler.isUserExist(userId);
            String userName = DataBaseControler.getUserName(userId);
       //     message = context.MakeMessage();


                Dictionary<String, String> data = new Dictionary<string, string>();
                await context.PostAsync("הי "+ userName + "שמח שחזרת");
                // await context.PostAsync("בבקשה תבחר תחום לימוד כדי שנוכל להתחיל");

            context.UserData.TryGetValue<User>("user", out user);

            //bool user = DataBaseControler.isUserExist(userId);

                //  var attachment = ManagerCard.GetCardAction();
                //     message.Attachments.Add(attachment);
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

                //   await context.PostAsync(message);

            // message = context.MakeMessage();
            //ManagerCard MC = new ManagerCard();
            //var attachment = MC.GetMainMenuChoice("", "", "", "", buttonMessage);
            //message.Attachments.Add(attachment);
            //await context.PostAsync(message);
            //context.Wait(this.MessageReceivedAsync);
                //   context.Wait(this.SubCategory);

               // await Conversation.SendAsync((Activity)context, () => new SatrtLerningDialog());
                 context.Call(new SatrtLerningDialog(),aaa);
               //context.Forward(new SatrtLerningDialog(), aaa,message, CancellationToken.None);
                // context.Forward<object, IMessageActivity >(new SatrtLerningDialog(), aaa,message,System.Threading.CancellationToken.None);

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


        public async Task aaa(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("סוף");
        }

        public async Task SubCategory(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var selectedMainHS = await result;
            if(selectedMainHS.Text == "hsA")
            {

            }

            await context.PostAsync("יפה מאוד!");
            await context.PostAsync("בחר עכשיו תת נושא");

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