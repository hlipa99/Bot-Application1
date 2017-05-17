using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Model.dataBase;
using Microsoft.Bot.Connector;
using System.Threading;
using NLP;
using Bot_Application1.Controllers;
using NLP.Models;
using Newtonsoft.Json.Linq;
using Model;
using Model.Models;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Bot_Application1.IDialog;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{

    [Serializable]
    public abstract class AbsDialog<T> : IDialog<T>
    {

        private IUser user;
        private StudySession studySession;
        private DateTime request = DateTime.UtcNow;
        private UserContext userContext = new UserContext("abs");
        //IDialogContext context;

        internal void updateRequestTime()
        {
            request = DateTime.UtcNow;
        }

        internal void updateRequestTime(IDialogContext context)
        {
            request = DateTime.UtcNow;
           // request = context.Activity.Timestamp.Value;
        }




        public void setDialogsVars(IDialogContext context)
        {
            try
            {

                if (user == null)
                {
                    user = new User();
                }
                User thisUser = user as User;
                context.UserData.SetValue<User>("user", thisUser);

                if (studySession == null) studySession = new StudySession();
                context.UserData.SetValue<StudySession>("studySession", studySession);
            }
            catch (Exception ex)
            {
              
            }

        }

        public void getDialogsVars(IDialogContext context)
        {
            try
            {
                context.UserData.TryGetValue<StudySession>("studySession", out studySession);


                User thisUser = User as User;
                context.UserData.TryGetValue<User>("user", out thisUser);
                user = thisUser;
            }catch(Exception ex)
            {
                setDialogsVars(context);
            }
        }

        public EducationController edc()
        {
            return new EducationController(User, StudySession, conv());
        }

        public async Task generalExceptionError(IDialogContext context, Exception error)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.innerException));
            Logger.addErrorLog("PostUnhandledExceptionToUser", error.Message + error.Data + error.Source + error.TargetSite + Environment.NewLine + error.StackTrace + error.InnerException);
            return;
        }




        internal DateTime Request
        {
            get
            {
                return request;
            }
        }

        public IUser User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public StudySession StudySession
        {
            get
            {
                return studySession;
            }

            set
            {
                studySession = value;
            }
        }

        public UserContext UserContext
        {
            get
            {
                return userContext;
            }

            set
            {
                userContext = value;
            }
        }



        internal async Task writeMessageToUser(IDialogContext context, IMessageActivity newMessage)
        {
            if(newMessage.Text != null && newMessage.Text != "")
            {
                await writeMessageToUser(context, newMessage.Text.Split('|'));
            }
            else
            {
                throw new NotImplementedException();
            }

        }




            internal async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            var typingReplay = context.MakeMessage();
            typingReplay.Type = ActivityTypes.Typing;
            var msgList = newMessage.ToList();
            msgList.ForEach(x => x.Trim());
            foreach (var m in msgList)
            {
                var m2 = m.Trim();
                if (m2.Contains('|'))
                {


                    await context.PostAsync(typingReplay);
                    //| is a sign for new line
                  //  await writeMessageToUser(context,m.Split('|').Where(x=> x.Trim().Length > 0 ).Select(x=> "\U00002705" + x).ToArray());
                    await writeMessageToUser(context, m2.Split('|'));
                }
                else
                {
                    if (m2.Length > 400)
                    {
                        await context.PostAsync(typingReplay);

                        var idx = m2.IndexOf(' ', 385);
                        if (idx > 0)
                        {
                            var str1 = m2.Substring(0, idx);
                            var str2 = m2.Substring(idx);
                            await writeMessageToUser(context, new string[] { str1, str2 });
                        } else
                        {

                        }
                    } else
                    {
                        //facebook cuts messages from 300 chars


                        //send message
                        if (m2 != null && m2 != "")
                        {
                            typingTime(context);
                            Thread.Sleep(m2.Length * 30); //writing time
                            await context.PostAsync(m2);
                        }
                    }
                }

            }
        }


        public async Task postImageToUser(IDialogContext context, string mediaKey)
        {

            var message = context.MakeMessage();

            conv().createImgMessage(message, mediaKey);
            await context.PostAsync(message);
        }


        public virtual async Task createMenuOptions(IDialogContext context, string title, string[] options, ResumeAfter<IMessageActivity> resume)
        {
            var menu = new MenuOptionDialog(
            options,
           title,
           conv().getPhrase(Pkey.wrongOption)[0]);
           context.Call(menu, resume);
        }


        internal void typingTime(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Type = ActivityTypes.Typing;
            context.PostAsync(message);
        }


        public ConversationController conv()
        {

            return new ConversationController(User, StudySession);
        }

        public void updateUserSession(IDialogContext context)
        {
            getDialogsVars(context);
            var userDB = (User)user;
            try
            {
                //support for privies version users
                if (user.LastSeen == null) user.LastSeen = new DateTime?();
                if (!user.LastSeen.HasValue) user.LastSeen = DateTime.UtcNow;
                if (user.UserOverallTime == null) user.UserOverallTime = new TimeSpan().ToString();


                if (user.LastSeen.Value.AddMinutes(30) > DateTime.UtcNow)
                {
                    user.UserOverallTime = TimeSpan.Parse(user.UserOverallTime).Add(DateTime.UtcNow.Subtract(user.LastSeen.Value)).ToString();
                    user.LastSeen = DateTime.UtcNow;
                }
                else
                {
                    user.UserTimesConnected++;
                    user.UserLastSession = DateTime.UtcNow;
                    user.LastSeen = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
               user.LastSeen = DateTime.UtcNow;
               user.UserOverallTime = new TimeSpan().ToString();
            }
            setDialogsVars(context);
            new DataBaseController().addUpdateUser(userDB);
        }

        public async Task<bool> checkOutdatedMessage<R>(IDialogContext context,ResumeAfter<R> resume, IAwaitable<IMessageActivity> message)
        {
            updateUserSession(context);



         

            var mes = await message;
            if (mes.Text.Length == 0) return false;
            if (mes.Timestamp <= Request)
            {
             //   mes.Summary = getDialogContext();
                context.Wait(resume);
                //await context.Forward<IMessageActivity, IMessageActivity>(new SideDialog(), resume, mes, CancellationToken.None);
                return true;
            }else if(context.Activity.Timestamp >= Request.AddHours(1))
            {
                
                await writeMessageToUser(context, conv().getPhrase(Pkey.whereDidYouGone));
                context.ConversationData.SetValue<ResumeAfter<R>>("resume", resume);
             //   await context.Forward<bool,string[]>(new YesNoQuestionDialog(), continuFromLastPlace, conv().getPhrase(Pkey.youWantToContinue),new CancellationToken());

            }

            return false;
        }


        internal async Task continuFromLastPlace(IDialogContext context, IAwaitable<bool> resualt)
        {
            var boolres = await resualt;
            if (boolres)
            {
                ResumeAfter<object> resume = null;
                 context.ConversationData.SetValue<ResumeAfter<object>>("resume",resume);
                if (resume != null)
                {
                    context.Wait(resume);
                    return;
                }
                else
                {
                    context.Done("");
                }
            }
            else
            {
                context.Done("");
            }


        }
        public virtual UserContext getDialogContext()
        {
            if (UserContext == null) UserContext = new UserContext("absDialog");
            return UserContext;
        }

        public virtual Task StartAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }
        public async Task waitForUserInputToContinu(IDialogContext context, ResumeAfter<IMessageActivity> resume)
        {
            context.Wait(resume);
        }


        internal  AwaitableFromItem<IMessageActivity> stringToMessageActivity(IDialogContext context,string message)
        {
            var messageActivity = context.MakeMessage();
            messageActivity.Text = message;
            var awaitble = new AwaitableFromItem<IMessageActivity>(messageActivity);
            return awaitble;
        }
    }




}