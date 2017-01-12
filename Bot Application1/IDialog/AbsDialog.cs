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

namespace Bot_Application1.IDialog
{

    [Serializable]
    public abstract class AbsDialog : IDialog<object>
    {
        public abstract Task StartAsync(IDialogContext context);
        internal Users user;
        internal StudySession studySession;

        internal async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            foreach (var m in newMessage)
            {
                if (m.Contains('|'))
                {
                     await writeMessageToUser(context,m.Split('|'));
                }
                else
                {
                    if (m.Length > 220)
                    {
                        var idx = m.IndexOf(' ', 200);
                        if (idx > 0)
                        {
                            var str1 = m.Substring(0, idx);
                            var str2 = m.Substring(idx);
                            await writeMessageToUser(context, new string[] { str1, str2 });
                        }else
                        {

                        }
                    }else
                    {
                        //facebook cuts messages from 300 chars
                        if (newMessage.Count() > 1)
                        {
                            typingTime(context);
                            Thread.Sleep(m.Length * 3);
                        }
                        await context.PostAsync(m);
                    }
                }
         


            }
        }

        private void typingTime(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Type = ActivityTypes.Typing;
        }


        public ConversationController conv()
        {
            return new ConversationController(user,studySession);
        }

    }




}