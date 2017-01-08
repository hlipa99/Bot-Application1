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
using Bot_Application1.YAndex;
using NLPtest.WorldObj;
using Model.dataBase;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class NewUserDialog : AbsDialog
    {
     
        public override async Task StartAsync(IDialogContext context)
        {

            context.UserData.TryGetValue<Users >("user", out user);
            if (user == null)
            {
                user = new Users();
                context.UserData.SetValue<Users >("user", user);
            }
            context.Wait(this.NewUser);
        }



        public async virtual Task NewUser(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            var message = await result;

            var newMessage = conv.selfIntroduction();
            await writeMessageToUser(context, newMessage);
            await NewUserGetName(context, result);
            //user Name

        }

        public async virtual Task NewUserGetName(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
         



            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            context.UserData.TryGetValue<Users >("user", out user);
            if (user.UserName == "" || user.UserName == null)
            {
                if (context.Activity.ChannelId == "facebook")
                {
                    var userFBname = context.Activity.From.Name;
                    var userTranslation = ControlerTranslate.Translate(userFBname);
                    if(userTranslation != "")
                    {
                        user.UserName = userTranslation;
                        context.UserData.SetValue<Users >("user", user);
                        await NewUserGetName(context, result);
                    }
                }
                else
                {
                await writeMessageToUser(context, conv.NewUserGetName());
                context.Wait(CheckName);

                } 

            }
            else
            {
                await writeMessageToUser(context, conv.NewUserGreeting(user.UserName));
                await NewUserGetGender(context, result);
            }
        }

        public async virtual Task CheckName(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            var message = await result;
            var userText = await result;

      
            if ((user.UserName = NLPControler.getInstence().getName(userText.Text)) != null)
            {
                var newMessage = conv.NewUserGreeting(user.UserName);

                context.UserData.SetValue<Users >("user", user);

                 await writeMessageToUser(context, newMessage);
                 await NewUserGetGender(context, result);
  
            }
            else
            {
                var newMessage = conv.MissingUserInfo("name");
                await writeMessageToUser(context, newMessage);
                context.Wait(CheckName);
            }
        }


        public async virtual Task NewUserGetGender(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //user Name
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            context.UserData.TryGetValue<Users >("user", out user);
            //user Gender
            if (user.UserGender == "" || user.UserGender == null)
            {
                await writeMessageToUser(context, conv.NewUserGetGender());
                context.Wait(CheckGender);

            }else
            {
                await NewUserGetClass(context, result);
            }
        }

        public async virtual Task CheckGender(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            var userText = await result;

         
            if ((user.UserGender = conv.getGender(userText.Text)) != null)
            {
                context.UserData.SetValue<Users >("user", user);

                await writeMessageToUser(context, conv.GenderAck(user.UserGender));
        
             
                await NewUserGetClass(context, result);

            }
            else
            {
                var newMessage = conv.MissingUserInfo("gender");
                await writeMessageToUser(context, newMessage);
                context.Wait(CheckGender);
            }
        }

        public async virtual Task NewUserGetClass(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            ConversationController conv =  new ConversationController(user.UserName, user.UserGender);
            context.UserData.TryGetValue<Users >("user", out user);
 

            if (user.UserClass == "" || user.UserClass == null)
            {
                await writeMessageToUser(context, conv.NewUserGetClass());
                context.Wait(CheckClass);

            }else
            {
                await LetsStart(context, result);
            }

        }



        public async virtual Task CheckClass(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //user class
            var userText = await result;
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            if ((user.UserClass = conv.getClass(userText.Text)) != null)
            {
                context.UserData.SetValue<Users >("user", user);

                await writeMessageToUser(context, conv.GeneralAck(user.UserClass));


                await LetsStart(context, result);

            }
            else
            {

                await writeMessageToUser(context, conv.MissingUserInfo("class"));
                context.Wait(CheckClass);
            }

        }


        public async virtual Task LetsStart(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //user class
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            await writeMessageToUser(context, conv.LetsStart());
            context.Done("");
        }





        

    }
}