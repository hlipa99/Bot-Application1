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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MainDialog : IDialog<object>
    {
        private IEnumerable<string> options = new List<string> { "s", "2", "3", "4" };

        public async Task StartAsync(IDialogContext context)
        {
            
            context.Wait(this.MessageReceivedAsync);
           
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage= new string[20];

            try
            {

            
             var message = await result;
            String userId = message.From.Id;
            bool user = DataBaseControler.isUserExist(userId);

            if(user)
            {
                userExist(context,result);
            }
            else if(!user)
            {
                newUser(context,result);
            }
            }
            catch(Exception e)
            {
                 var message = await result;
                 await context.PostAsync("Eror in Server: Class: Main Dialog : " + e);
                 context.Wait(MessageReceivedAsync);
            }
            //var message = context.MakeMessage();
            //ManagerCard MC = new ManagerCard();
            //var attachment = MC.GetMainMenuChoice("","","","",buttonMessage);
            //message.Attachments.Add(attachment);
            //await context.PostAsync(message);
            //context.Wait(this.MessageReceivedAsync);


            // var message = await result;
            // await context.PostAsync("You said: " + message.Text);
            // context.Wait(MessageReceivedAsync);

        }


        public async virtual Task newUser(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage = new string[20];

            // var message = await result;
            // await context.PostAsync("You said: " + message.Text);
            // context.Wait(MessageReceivedAsync);

            

            //var message = context.MakeMessage();
            //ManagerCard MC = new ManagerCard();
            //var attachment = MC.GetMainMenuChoice("","","","",buttonMessage);
            //message.Attachments.Add(attachment);
            //await context.PostAsync(message);
            //context.Wait(this.MessageReceivedAsync);

        }


        public async virtual Task userExist(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage = new string[20];

            // var message = await result;
            // await context.PostAsync("You said: " + message.Text);
            // context.Wait(MessageReceivedAsync);

            

            //var message = context.MakeMessage();
            //ManagerCard MC = new ManagerCard();
            //var attachment = MC.GetMainMenuChoice("","","","",buttonMessage);
            //message.Attachments.Add(attachment);
            //await context.PostAsync(message);
            //context.Wait(this.MessageReceivedAsync);

        }

        //public async Task DisplaySelectedCard(IDialogContext context, IAwaitable<string> result)
        //{
        //    var selectedCard = await result;

        //    var message = context.MakeMessage();

        //    var attachment = GetHeroCard();
        //    message.Attachments.Add(attachment);

        //    await context.PostAsync(message);

        //    context.Wait(this.MessageReceivedAsync);
        //}



        //private static Attachment GetHeroCard()
        //{
        //    var heroCard = new HeroCard
        //    {
        //        Title = "הבוט שלנו",
        //        Subtitle = "Your bots — wherever your users are talking",
        //        Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
        //        Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
        //        Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Get Started", value: "start"),
        //        new CardAction(ActionTypes.PostBack, "Yes We can", value: "yes")}
        //    };

        //    return heroCard.ToAttachment();
        //}

    }
}