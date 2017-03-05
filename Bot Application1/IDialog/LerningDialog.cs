using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using NLPtest.Models;
using Model.dataBase;
using Bot_Application1.Controllers;
using Model;
using Model.Models;
using Bot_Application1.Exceptions;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class LerningDialog : AbsDialog<IMessageActivity>
    {
        public override UserContext getDialogContext()
        {
            UserContext.dialog = "LerningDialog";
            return UserContext;
        }


        public override async Task StartAsync(IDialogContext context)
        {
            User thisUser = User as User;
            UserContext = new UserContext("LerningDialog");
            context.UserData.TryGetValue<User>("user", out thisUser);
            User = thisUser;

            if (User == null)
            {
                throw new unknownUserException();
            }

            if(StudySession == null)
            {
                StudySession = new StudySession();
                setStudySession(context);
            }
            else
            {
                var question = conv().getPhrase(Pkey.shouldWeContinue);
                await writeMessageToUser(context, question);
                await context.Forward<Boolean,string[]>(new YesNoQuestionDialog(), shouldWeContinue, question, new System.Threading.CancellationToken());
                return;
            }

            StudySession.CurrentQuestion = null;
            await writeMessageToUser(context, conv().getPhrase(Pkey.letsLearn));

            IMessageActivity message;
            //choose study subject menu gallery
            if (context.Activity.ChannelId != "telegram")
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.chooseStudyUnits));
                message = context.MakeMessage();
                foreach (var m in edc().getStudyCategory())
                {
                    var action = new CardAction(type: "imBack", value: m, title: m);
                    var hc = new HeroCard(title: m, images: getImage(m),tap: action, buttons:
                        new CardAction[] { action });
                    message.Attachments.Add(hc.ToAttachment());
                    message.AttachmentLayout = "carousel";

                }
            }else
            {
                await createMenuOptions(context, conv().getPhrase(Pkey.chooseStudyUnits)[0], edc().getStudyCategory(), StartLearning);
                return;
            }


            context.UserData.RemoveValue("studySession");
            StudySession = new StudySession();
            setStudySession(context);
            await context.PostAsync(message);
            updateRequestTime(context);
            context.Wait(StartLearning);
        }

        private async Task shouldWeContinue(IDialogContext context, IAwaitable<Boolean> result)
        {
            var cont = await result;
            if (cont)
            {
                context.Wait(StartLearning);
            }else
            {
                setStudySession(context);
                context.Done("");
            }
        }

        private CardImage[] getImage(string m)
        {
            MediaController mc = new MediaController();
            var key = edc().getRamdomImg(m);
            var urlAdd = mc.getFileUrl(key);
            var cardImg = new CardImage(url: urlAdd);
            return new CardImage[] { cardImg };
        }


        public async virtual Task StartLearning(IDialogContext context, IAwaitable<object> result)
        {
            if (context.Activity.Timestamp <= Request)
            {
                context.Wait(StartLearning);
                return;
            }

            if(StudySession.Category != "")
            {
                return;
            }

            string message = null;
            var res = await result;
            if (res is string)
            {
                message = res as string;
            }
            else
            {
                var r =  res as IMessageActivity;
                message = r.Text;
            }

           
           if (edc().getStudyCategory().Contains(message))
            {
                StudySession.Category = message;
                setStudySession(context);

                try
                {
                    edc().getNextQuestion();
                }catch(CategoryOutOfQuestionException ex)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.SubjectNotAvialable));
                    await StartAsync(context);
                    return;
                }
              
                var question = StudySession.CurrentQuestion;
                if(question.QuestionText == null)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.SubjectNotAvialable));
                    await StartAsync(context);
                }

                await writeMessageToUser(context, conv().getPhrase(Pkey.areUReaddyToLearn));
                await writeMessageToUser(context, conv().getPhrase(Pkey.firstQuestion));



                await intreduceQuestion(context);
            }else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption, textVar: message));
                await StartAsync(context);
            }
        }

        private async Task intreduceQuestion(IDialogContext context)
        {
            getStudySession(context);
            edc().getNextQuestion();
            setStudySession(context);

            if (StudySession.CurrentQuestion != null)
            {
             
                await writeMessageToUser(context, conv().getPhrase(Pkey.beforAskQuestion));
                try
                {
                    context.Call(new QuestionDialog(), questionSummery);
                }
               catch (StopSessionException ex)
                {

                }
            }else
            {
                context.Wait(EndOfLearningSession);
            }
        }

        private async Task questionSummery(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextQuestion));
            await intreduceQuestion(context);
        }



        public async Task EndOfLearningSession(IDialogContext context, IAwaitable<object> result)
        {
            await writeMessageToUser(context, conv().endOfSession());
            await writeMessageToUser(context, conv().getPhrase(Pkey.endOfSession));

            //TODO: save user sussion to DB
            updateRequestTime(context);
            if (context.Activity.Timestamp <= Request)
            {
                context.Wait(EndOfLearningSession);
                return;
            }
            context.Done("learningSession");
        }


    }
}