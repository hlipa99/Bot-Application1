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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MainDialog: IDialog<object>
    {
        private const string Continue = "להמשיך מהמקום האחרון שבו למדנו (1  ";
        private const string New = "לבחור חומר לימוד אחר";
        private const string Test = "לבדוק כמה התקדמתי עד היום";
        private const string Different = "בא לי משהוא אחר";

        private IEnumerable<string> options = new List<string> { Continue, New, Test, Different,"1","2","3","4"};

        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(this.MessageReceivedAsync);

        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage= new string[20];
            

            
             var message = await result;
            String userId = message.From.Id;
            bool user = DataBaseControler.isUserExist(userId);
            String userName = DataBaseControler.getUserName(userId);
       //     message = context.MakeMessage();

            if (user)
            {

                Dictionary<String, String> data = new Dictionary<string, string>();
                await context.PostAsync("הי "+ userName + "שמח שחזרת");
                // await context.PostAsync("בבקשה תבחר תחום לימוד כדי שנוכל להתחיל");

                // var attachment = ManagerCard.getFacebookButtons();
                //var attachment = ManagerCard.GetMainMenuChoice("כותרת","כותרת משנה","הודעה","url",data);


                //  var attachment = ManagerCard.GetCardAction();
                //     message.Attachments.Add(attachment);


                //   await context.PostAsync(message);

                //   context.Wait(this.SubCategory);

               // await Conversation.SendAsync((Activity)context, () => new SatrtLerningDialog());
                 context.Call(new SatrtLerningDialog(),aaa);
               //context.Forward(new SatrtLerningDialog(), aaa,message, CancellationToken.None);
                // context.Forward<object, IMessageActivity >(new SatrtLerningDialog(), aaa,message,System.Threading.CancellationToken.None);


            }
            else if(!user)
            {
                
            }

            //      var message = context.MakeMessage();
            //      ManagerCard MC = new ManagerCard();
            //      var attachment = MC.GetMainMenuChoice("","","","",buttonMessage);
            //      message.Attachments.Add(attachment);
            //      await context.PostAsync(message);
            //      context.Wait(this.MessageReceivedAsync);


            // var message = await result;
            // await context.PostAsync("You said: " + message.Text);
            // context.Wait(MessageReceivedAsync);

        }

        public async Task aaa(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("סוף");
           // context.Done();
        }

        public async Task SubCategory(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if(message.Text == "hsA")
            {
                await context.PostAsync("יפה מאוד!");
                await context.PostAsync("בחר עכשיו תת נושא");

                message = context.MakeMessage();
                var attachment = ManagerCard.GetCardSubCategoryAction();
                message.Attachments.Add(attachment);
                await context.PostAsync(message);
                context.Wait(this.SubCategory);
            }

            
        }



        private static Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "BotFramework Hero Card",
                Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.botframework.com/en-us/") }
            };

            return heroCard.ToAttachment();
        }


       











    }
}