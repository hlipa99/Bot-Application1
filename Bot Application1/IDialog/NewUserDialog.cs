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
using static Bot_Application1.Controllers.ConversationController;
using Model;

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
           
            await NewUser(context);
        }



        public async virtual Task NewUser(IDialogContext context)
        {
            var newMessage = conv().getPhrase(Pkey.selfIntroduction);
            await writeMessageToUser(context, newMessage);
            await NewUserGetName(context);
          
        }

        public async virtual Task NewUserGetName(IDialogContext context)
        {
            context.UserData.TryGetValue<Users >("user", out user);
            if (user.UserName == "" || user.UserName == null)
            {
                if (context.Activity.ChannelId == "facebook" || context.Activity.ChannelId == "slack")
                {
                    var userFBname = context.Activity.From.Name;
                    var userTranslation = ControlerTranslate.Translate(userFBname);


                    if(userTranslation != "")
                    {
                        user.UserName = userTranslation.Split(' ')[0];
                        context.UserData.SetValue<Users >("user", user);
                        await NewUserGetName(context);
                    }
                }
                else
                {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NewUserGetName));
                    request = DateTime.UtcNow;
                    context.Wait(CheckName);

                } 
                   
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NewUserGreeting));
                await NewUserGetGender(context);
            }
        }

        public async virtual Task CheckName(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if(context.Activity.Timestamp <= request)
            {
                context.Wait(CheckName);
                return;
            }


            var message = await result;
            var userText = await result;

        
            if ((user.UserName = conv().getName(userText.Text)) != null)
            {
                var newMessage = conv().getPhrase(Pkey.NewUserGreeting);

                context.UserData.SetValue<Users >("user", user);

                 await writeMessageToUser(context, newMessage);
                 await NewUserGetGender(context);
  
            }
            else
            {
                var newMessage = conv().getPhrase(Pkey.MissingUserInfo,textVar:"שם");
                await writeMessageToUser(context, newMessage);
                request = DateTime.UtcNow;
                context.Wait(CheckName);
            }
        }


        public async virtual Task NewUserGetGender(IDialogContext context)
        {
            //user Name
            
           context.UserData.TryGetValue<Users >("user", out user);
            //user Gender
            if (user.UserGender == "" || user.UserGender == null)
            {
             

                //var menu = new PromptDialog.PromptChoice<string>(
                // conv().getGenderOptions(),
                //conv().getPhrase(Pkey.NewUserGetGender)[0],
                //conv().getPhrase(Pkey.wrongOption)[0],
                //3);

                //context.Call(menu, CheckGender);

                await createMenuOptions(context,conv().getPhrase(Pkey.NewUserGetGender)[0], conv().getGenderOptions(), CheckGender);
                

            }
            else
            {
                await NewUserGetClass(context);
            }
        }

        public async virtual Task CheckGender(IDialogContext context, IAwaitable<object> result)
        {
            if (context.Activity.Timestamp <= request)
            {
                context.Wait(CheckGender);
                return;
            }
            string userText = null;
            var res = await result;
            if (res is string)
            {
                userText = res as string;
            }
            else
            {
                userText = ((IMessageActivity)res).Text;
            }


            if ((user.UserGender = conv().getGenderValue(userText)) != null)
            {
                context.UserData.SetValue<Users >("user", user);

                await writeMessageToUser(context, conv().getPhrase(Pkey.GenderAck, textVar: conv().getGenderName("single")));
        
             
                await NewUserGetClass(context);

            }
            else
            {
                var newMessage = conv().getPhrase(Pkey.MissingUserInfo,textVar:"מין");
                await writeMessageToUser(context, newMessage);
                request = DateTime.UtcNow;
                context.Wait(CheckGender);
            }
        }

        public async virtual Task NewUserGetClass(IDialogContext context)
        {

   
 

            if (user.UserClass == "" || user.UserClass == null)
            {
                await createMenuOptions(context, conv().getPhrase(Pkey.NewUserGetClass)[0], conv().getClassOptions(), CheckClass);

            }
            else
            {
                await LetsStart(context);
            }

        }



        public async virtual Task CheckClass(IDialogContext context, IAwaitable<object> result)
        {

            if (context.Activity.Timestamp <= request)
            {
                context.Wait(CheckClass);
                return;
            }

            //user class
            string userText = null;
            var res = await result;
            if (res is string) {
                userText = res as string;
            }else
            {
                userText = ((IMessageActivity)res).Text;
            }
            
            if ((user.UserClass = conv().getClass(userText)) != null)
            {
                context.UserData.SetValue<Users >("user", user);

                await writeMessageToUser(context, conv().getPhrase(Pkey.GeneralAck,textVar:user.UserClass));


                await LetsStart(context);

            }
            else
            {
                var newMessage = conv().getPhrase(Pkey.MissingUserInfo, textVar: "כיתה");
                await writeMessageToUser(context, newMessage);
                request = DateTime.UtcNow;
                context.Wait(CheckClass);

            }

        }


        public async virtual Task LetsStart(IDialogContext context)
        {
            //user class
            
            await writeMessageToUser(context, conv().getPhrase(Pkey.LetsStart));

            context.Done("");
        }

    }
}