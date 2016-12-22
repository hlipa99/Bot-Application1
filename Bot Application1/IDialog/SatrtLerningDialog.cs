using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Bot_Application1.Cardatt_achment;
using Microsoft.Bot.Connector;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class StartLerningDialog: IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }




        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            String[] buttonMessage = new string[20];
             var message = await result;
            message = context.MakeMessage();
           Dictionary<String, String> data = new Dictionary<string, string>();
           await context.PostAsync("בבקשה תבחר תחום לימוד כדי שנוכל להתחיל");

                // var attachment = ManagerCard.getFacebookButtons();
                //var attachment = ManagerCard.GetMainMenuChoice("כותרת","כותרת משנה","הודעה","url",data);

            var attachment = ManagerCard.GetCardAction();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);
            context.Wait(this.SubCategory);
                         
                   

        }


        public async Task SubCategory(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var selectedMainHS = await result;
            
             if(selectedMainHS.Text == "hsA")
             {
                await context.PostAsync("יפה מאוד!");
                await context.PostAsync("בחר עכשיו תת נושא");
                var attachment = ManagerCard.GetCardSubCategoryAction();

                context.Wait(this.SubSubCategory);
            }

            
          
        }

        public async Task SubSubCategory(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("יפה מאוד!");
            await context.PostAsync("בוא נתחיל עם השאלות");
        }

    }


  
}