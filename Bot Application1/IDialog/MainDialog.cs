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

            if(user)
            {

                      
                      ManagerCard MC = new ManagerCard();
                      var attachment = MC.GetMainMenuChoice("","","","",buttonMessage);
                      message.Attachments.Add(attachment);
                      await context.PostAsync(message);
                      context.Wait(this.MessageReceivedAsync);


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


        public async  Task newUser(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage = new string[20];


             var message = await result;
             await context.PostAsync("You said: " + message.Text);
             context.Wait(MessageReceivedAsync);

            

            //var message = context.MakeMessage();
            //ManagerCard MC = new ManagerCard();
            //var attachment = MC.GetMainMenuChoice("","","","",buttonMessage);
            //message.Attachments.Add(attachment);
            //await context.PostAsync(message);
            //context.Wait(this.MessageReceivedAsync);

        }


        public async Task userExist(IDialogContext context, IAwaitable<String> result)
        {
            String[] buttonMessage = new string[20];

            //var message = await result;
            //await context.PostAsync("ברוך הבא חיים טןב לראותך שוב!");
            //context.Wait(this.MessageReceivedAsync);



            var message = context.MakeMessage();
            ManagerCard mc = new ManagerCard();
            var attachment = mc.GetMainMenuChoice("", "", "", "", buttonMessage);
            message.Attachments.Add(attachment);
            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);

        }



    }
}