using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Bot_Application1.dataBase;
using Model.dataBase;
using Microsoft.Bot.Connector;
using System.Threading;
using NLPtest;
using Bot_Application1.Controllers;
using NLPtest.Models;
using Newtonsoft.Json.Linq;
using Model;
using Model.Models;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{

    [Serializable]
    public abstract class AbsDialog : IDialog<object>
    {

        private IUser user;
        private StudySession studySession;
        private DateTime request = DateTime.UtcNow;
        private UserContext userContext;
        //IDialogContext context;

        internal void updateRequestTime2()
        {
            request = DateTime.UtcNow;
        }

        internal void updateRequestTime(IDialogContext context)
        {
            request = context.Activity.Timestamp.Value;
        }


        public void getUser(IDialogContext context)
            {
                if (user == null) 
                {
                    User thisUser = User as User;
                    context.UserData.TryGetValue<User>("user", out thisUser);
                    user = thisUser;
                }
               
            }

        public void setUser(IDialogContext context)
        {
                User thisUser = user as User;
                context.UserData.SetValue<User>("user",thisUser);
            
        }

        public void getStudySession(IDialogContext context)
        {
            if (studySession == null)
            {
                context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            }
        }

        public void setStudySession(IDialogContext context)
        {
            context.UserData.SetValue<StudySession>("studySession", studySession);
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

        internal async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            var typingReplay = context.MakeMessage();
            typingReplay.Type = ActivityTypes.Typing;

            foreach (var m in newMessage)
            {
                if (m.Contains('|'))
                {
                    await context.PostAsync(typingReplay);
                    //| is a sign for new line
                  //  await writeMessageToUser(context,m.Split('|').Where(x=> x.Trim().Length > 0 ).Select(x=> "\U00002705" + x).ToArray());
                    await writeMessageToUser(context, m.Split('|'));
                }
                else
                {
                    if (m.Length > 220)
                    {
                        await context.PostAsync(typingReplay);
  
                        var idx = m.IndexOf(' ', 200);
                        if (idx > 0)
                        {
                            var str1 = m.Substring(0, idx);
                            var str2 = m.Substring(idx);
                            await writeMessageToUser(context, new string[] { str1, str2 });
                        } else
                        {

                        }
                    } else
                    {
                        //facebook cuts messages from 300 chars
                        if (newMessage.Length > 10)
                        {
                            typingTime(context);
                            Thread.Sleep(m.Length * 10);
                        }

                        //send message
                        if (m != null && m != "") await context.PostAsync(m);
                    }
                }
         


            }
        }


        public virtual async Task createMenuOptions(IDialogContext context, string title, string[] options, ResumeAfter<object> resume)
        {
            if(context.Activity.ChannelId == "facebook")
            {
                await createQuickReplay(context, title, options, resume);
            }
            else
            {
                await createRMenuOptions(context, title, options, resume);
            }
        }

        public async virtual Task createQuickReplay(IDialogContext context,string title, string[] options, ResumeAfter<IMessageActivity> resume)
        {

            //     await writeMessageToUser(context, new string[] { title });
           
            var reply = context.MakeMessage();
            var channelData = new JObject();
            var quickReplies = new JArray();

            
            var qrList = new List<FacebookQuickReply>();
            foreach (var s in options)
            {
                var r = new FacebookQuickReply("text", s, s);
                qrList.Add(r);
            }

          
            var message = new FacebookMessage(title, qrList);
            reply.ChannelData = message;



            await context.PostAsync(reply);

            updateRequestTime(context);
            context.Wait(resume);

        }

   
        public async virtual Task createRMenuOptions(IDialogContext context, string title, string[] options, ResumeAfter<object> resume)
        {

        //    await writeMessageToUser(context, new string[] { title });

         

            var menu = new PromptDialog.PromptChoice<string>(
              options,
             title,
             conv().getPhrase(Pkey.wrongOption)[0],
             3);

            context.Call(menu, resume);

        }

        private void typingTime(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Type = ActivityTypes.Typing;
            context.PostAsync(message);
        }


        public ConversationController conv()
        {
            return new ConversationController(User, StudySession);
        }

        public virtual Task StartAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }
    }




}