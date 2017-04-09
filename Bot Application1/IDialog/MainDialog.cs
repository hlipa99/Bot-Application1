using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Bot_Application1.Cardatt_achment;

using System.Threading;
using NLP;
using Model.dataBase;
using Bot_Application1.log;
using static Bot_Application1.Controllers.ConversationController;
using Model;
using Model.Models;
using Bot_Application1.Exceptions;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class MainDialog : AbsDialog<IMessageActivity>
    {

        public override UserContext getDialogContext(IDialogContext context)
        {
            base.getDialogContext(context);
            UserContext.dialog = "SideDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
          
            try
           {
                getDialogsVars(context);

                if (checkUser())
                {
        
                    context.Call(new GreetingDialog(), MainMenu);
                    
                }
                else
                {

                    context.Call(new NewUserDialog(), MainMenu);

                }
            }
            catch (Exception ex)
            {
                await writeMessageToUser(context, new string[] { "אוקיי זה מביך " + "\U0001F633", "קרתה לי תקלה בשרת ואני לא יודע מה לעשות", "אני אתחיל עכשיו מהתחלה ונעמיד פנים שלא קרה כלום, " + "\U0001F648", "טוב" + "?" });
                Logger.addErrorLog(getDialogContext(context).dialog, ex.Message + Environment.NewLine + ex.StackTrace + ex.InnerException);
                //    await writeMessageToUser(context, new string[] { ex.Data.ToString(), ex.InnerException.ToString(), ex.StackTrace.ToString(), ex.TargetSite.ToString(), ex.ToString() });
                //     Logger.log("MainDialog", "MainMenu", ex.ToString());
                await StartAsync(context);
            }
        }

        private bool checkUser()
        {
            if (User == null) return false;
            if (User.UserGender == null) return false;
            if (User.UserClass == null) return false;
            if (User.UserLastSession == null) User.UserLastSession = DateTime.UtcNow;
            return true;
        }

        string[] options;

        private async Task MainMenu(IDialogContext context, IAwaitable<object> result)
        {
           
                getDialogsVars(context);

            if (User != null){
 
                    options = conv().MainMenuOptions();
                    updateRequestTime(context);
                    await createMenuOptions(context, conv().getPhrase(Pkey.MainMenuText)[0], options, MainMenuResualt);
                    User.UserTimesConnected ++;
                    User.UserLastSession = DateTime.UtcNow;
                    setDialogsVars(context);
                    conv().saveUserToDb(User);
            }else{

                context.Call(new NewUserDialog(), MainMenu);
                }



        }

        private async Task MainMenuResualt(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await checkOutdatedMessage<IMessageActivity>(context, MainMenuResualt, result)) return;
            getDialogsVars(context);

            var text = await result;
            var option = "";
            try
            {

                if (text is string)
            {
          //      option = (string)text;
            }
            else
            {
                option = ((IMessageActivity)text).Text;
            }


            if (options.Contains(option))
            {
                var optionIdx = options.ToList().IndexOf(option);
                switch (optionIdx)
                {
                    case 0:  //start learning
                        context.Call<string>(new LerningDialog(), MainMenu);
                        break;
                    case 1:  //not implamented
                        context.Call(new NotImplamentedDialog(), returnToMainMenu);
                        break;
                    default:
                        break;
                        await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption, textVar: option));
                        await MainMenu(context,result);
                }
            }else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption,textVar: option));
                await MainMenu(context, result);
            }


            }catch(Exception ex)
            {

            }
        }



        private async Task returnToMainMenu(IDialogContext context, IAwaitable<object> result)
        {
       //     await writeMessageToUser(context, conv().getPhrase(Pkey.returnToMainMenu));
            await MainMenu(context, result);
        }


        private async Task EndSession(IDialogContext context, IAwaitable<string> result)
        {
            //   context.Wait(MainMenu);
            try
            {
                await result;
            }catch(Exception ex)
            {
                try
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.innerException));
                    Logger.addErrorLog(getDialogContext(context).dialog, ex.Message + Environment.NewLine + ex.StackTrace + ex.InnerException);
                    await MainMenu(context, null);
                    return;
                }
                catch
                {
                    await writeMessageToUser(context, new string[] { "אוקיי זה מביך " + "\U0001F633", "קרתה לי תקלה בשרת ואני לא יודע מה לעשות", "אני מציע שנחכה כמה דקות שהכל יסתדר, " + "\U0001F648", "טוב" + "?" });
                    await StartAsync(context);
                }
            }

         //   setDialogsVars(context);
            context.Call(new FarewellDialog(), MainMenu);
        }



    }
}