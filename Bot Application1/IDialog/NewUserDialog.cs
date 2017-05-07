using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Bot_Application1.Cardatt_achment;

using NLP;
using Bot_Application1.YAndex;
using NLP.WorldObj;
using Model.dataBase;
using static Bot_Application1.Controllers.ConversationController;
using Model;
using Model.Models;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class NewUserDialog : AbsDialog<IMessageActivity>
    {
        public override UserContext getDialogContext()
        {
            base.getDialogContext();
            UserContext.dialog = "NewUserDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {


            getDialogsVars(context);

            User = new User();
            User.UserID = context.Activity.From.Id;
            User.UserAddress = context.Activity.Id;
            User.UserCreated = DateTime.UtcNow;
            User.UserOverallTime = new TimeSpan(0).ToString();
            User.UserTimesConnected = 0;
            User.UserLastSession = DateTime.UtcNow;

            setDialogsVars(context);

            context.Wait(NewUser);
        }



        public async virtual Task NewUser(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var message = await result;
            //if (conv().isEnglish(message.Text))
            //{
            //    await writeMessageToUser(context, conv().getPhrase(Pkey.replaceLanguge));
            //}

            var newMessage = conv().getPhrase(Pkey.selfIntroduction);
            await writeMessageToUser(context, newMessage);
            await NewUserGetName(context);
          
        }

        private async Task replaceLanguge(IDialogContext context, IAwaitable<bool> result)
        {

            var res = await result;
            //if (res)
            //{
            //    User.Language = "en";
            //}

            var newMessage = conv().getPhrase(Pkey.selfIntroduction);
            await writeMessageToUser(context, newMessage);

            await NewUserGetName(context);
        }

        public async virtual Task NewUserGetName(IDialogContext context)
        {
          
            if (User.UserName == "" || User.UserName == null)
            {
                if (context.Activity.ChannelId == "facebook" &&  context.Activity.ChannelId == "slack")
                {
                    var userFBname = context.Activity.From.Name;
                    var userTranslation = ControlerTranslate.Translate(userFBname);


                    if(userTranslation != "")
                    {
                        User.UserName = userTranslation.Split(' ')[0];
                        setDialogsVars(context);
                        await NewUserGetName(context);
                    }
                }
                else
                {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NewUserGetName));

                    updateRequestTime(context);
                    context.Wait(CheckName);
                    return;
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
            if(await checkOutdatedMessage<IMessageActivity>(context,CheckName,result)) return;

            var message = await result;
            var userText = await result;

            if ((User.UserName = conv().getName(userText.Text)) != null)
            {
                setDialogsVars(context);
                var newMessage = conv().getPhrase(Pkey.NewUserGreeting);



                await writeMessageToUser(context, newMessage);
                await NewUserGetGender(context);

            }
            else
            {
                var newMessage = conv().getPhrase(Pkey.MissingUserInfo, textVar: "שם");
                await writeMessageToUser(context, newMessage);
                updateRequestTime(context);
                context.Wait(CheckName);
            }
        }

        public async virtual Task NewUserGetGender(IDialogContext context)
        {
            //user Name
            
  
            //user Gender
            if (User.UserGender == "" || User.UserGender == null)
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
            if (context.Activity.Timestamp <= Request)
            {
                var userText2 = ((IMessageActivity)await result).Text;
                context.Wait(CheckGender);
                return;
            }
            string userText = null;
            var res = await result;
            if (res is string)
            {
                userText = (string)res;
            }
            else
            {
                userText = ((IMessageActivity)res).Text;
            }


            if ((User.UserGender = conv().getGenderValue(userText)) != null)
            {
                setDialogsVars(context);

                await writeMessageToUser(context, conv().getPhrase(Pkey.GenderAck, textVar: conv().getGenderName("single")));
        
             
                await NewUserGetClass(context);

            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.MissingUserInfo, textVar: "מין"));
                await NewUserGetGender(context);
            }
        }

        public async virtual Task NewUserGetClass(IDialogContext context)
        {

   
 

            if (User.UserClass == "" || User.UserClass == null)
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
            
            if (context.Activity.Timestamp <= Request)
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
            
            if ((User.UserClass = conv().getClass(userText)) != null)
            {
                setDialogsVars(context);

                await writeMessageToUser(context, conv().getPhrase(Pkey.GeneralAck,textVar:User.UserClass));


                await LetsStart(context);

            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.MissingUserInfo, textVar: "כיתה"));
                await NewUserGetClass(context);

            }

        }


        public async virtual Task LetsStart(IDialogContext context)
        {
            //user class


            await writeMessageToUser(context, conv().getPhrase(Pkey.LetsStart));
            setDialogsVars(context);
            context.Done("");
        }

    }
}