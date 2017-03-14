using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using NLP.Models;
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
            getUser(context);

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
              
                await context.Forward<Boolean,string[]>(new YesNoQuestionDialog(), shouldWeContinue, question, new System.Threading.CancellationToken());
                return;
            }

            await chooseSubject(context);
        }

        private async Task chooseSubject(IDialogContext context)
        {

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
        var hc = new HeroCard(title: m, images: getImage(m), tap: action, buttons:
            new CardAction[] { action });
        message.Attachments.Add(hc.ToAttachment());
                    message.AttachmentLayout = "carousel";

                }
            }else{
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
               await chooseSubject(context);
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

            await StartLearning(context, stringToMessageActivity(context,await result as string));
        }



        public async virtual Task StartLearning(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            await checkOutdatedMessage(context, StartLearning, result);

            string message = null;
            var res = await result;
            //if (res is string)
            //{
            //    message = res as string;
            //}
            //else
            //{
                var r =  res as IMessageActivity;
                message = r.Text;
       //     }

           
           if (edc().getStudyCategory().Contains(message))
            {
                StudySession.Category = message;
                setStudySession(context);

                try
                {
                    edc().getQuestion();
                    setStudySession(context);
                }
                catch(CategoryOutOfQuestionException ex)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.SubjectNotAvialable));
                    await StartAsync(context);
                    return;
                }
              
      
                await writeMessageToUser(context, conv().getPhrase(Pkey.areUReaddyToLearn));
                await writeMessageToUser(context, conv().getPhrase(Pkey.firstQuestion));



                await intreduceQuestion(context);
            }else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption, textVar: message));
                await chooseSubject(context);
            }
        }

        private async Task intreduceQuestion(IDialogContext context)
        {
            getStudySession(context);
            edc().getNextQuestion();
            setStudySession(context);

            if (StudySession.CurrentQuestion != null)
            {

                try
                {
                    context.Call(new QuestionDialog(), questionSummery);
                }
               catch (StopSessionException ex)
                {
                    await EndOfLearningSession(context);
                }
            }else
            {
                await EndOfLearningSession(context);
            }
        }

        private async Task questionSummery(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //var res = await result;
            await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextQuestion));
            await writeMessageToUser(context, conv().getPhrase(Pkey.beforAskQuestion));
            await intreduceQuestion(context);
        }



        public async Task EndOfLearningSession(IDialogContext context)
        {
            await writeMessageToUser(context, conv().endOfSession());
            await writeMessageToUser(context, conv().getPhrase(Pkey.endOfSession));

            //TODO: save user sussion to DB
            context.Done("learningSession");
        }


    }
}