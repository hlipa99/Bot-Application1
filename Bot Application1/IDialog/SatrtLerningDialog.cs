using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using NLPtest.Models;
using Model.dataBase;
using Bot_Application1.Controllers;
using Model;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class StartLerningDialog : AbsDialog
    {



        public override async Task StartAsync(IDialogContext context)
        {
            context.UserData.TryGetValue<Users>("user", out user);
            context.UserData.TryGetValue<StudySession>("studySession", out studySession);

            if (user == null)
            {
                throw new unknownUserException();
            }

            if(studySession == null)
            {
                studySession = new StudySession();
            }


   
            await writeMessageToUser(context, conv().getPhrase(Pkey.letsLearn));
            await writeMessageToUser(context, conv().getPhrase(Pkey.chooseStudyUnits));
            var message = context.MakeMessage();

            foreach (var m in edc().getStudyCategory())
            {
                var hc = new HeroCard(title: m, images: getImage(m), buttons:
                    new CardAction[] { new CardAction(type: "imBack", value: m, title: m )});
                message.Attachments.Add(hc.ToAttachment());
                message.AttachmentLayout = "carousel";

            }



            context.UserData.RemoveValue("studySession");
            studySession = new StudySession();

            await context.PostAsync(message);
            updateRequestTime();
            context.Wait(StartLearning);
        }

        private CardImage[] getImage(string m)
        {
            var cardImg = new CardImage(url: edc().getRamdomImg(m));
            return new CardImage[] { cardImg };
        }


        public async virtual Task StartLearning(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (context.Activity.Timestamp <= request)
            {
                context.Wait(StartLearning);
                return;
            }

            var message = await result;
           if (edc().getStudyCategory().Contains(message.Text))
            {
                studySession.Category = message.Text;

                await writeMessageToUser(context, conv().getPhrase(Pkey.areUReaddyToLearn));
                await writeMessageToUser(context, conv().getPhrase(Pkey.firstQuestion));
                await askQuestion(context);
            }else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.NotAnOption, textVar: message.Text));
                await StartAsync(context);
            }
        }

        public async Task askQuestion(IDialogContext context)
        {


            var question = edc().getQuestion(studySession.Category, studySession.SubCategory, studySession);

            await writeMessageToUser(context, new string[] { question.QuestionText });
            studySession.CurrentQuestion = question;
            studySession.QuestionAsked.Add(studySession.CurrentQuestion);

            updateRequestTime();
            context.Wait(answerQuestion);
        }


        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (context.Activity.Timestamp <= request)
            {
                context.Wait(StartLearning);
                return;
            }

            var message = await result;
            //      context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (conv().isStopSession(message.Text))
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.stopLearningSession));
                context.Done("");
            }

            var question = studySession.CurrentQuestion;

            question = edc().checkAnswer(question, message.Text);
            if(question.AnswerScore > 85)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.goodAnswer));

            }
            else if(question.AnswerScore > 30)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.partialAnswer));
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.notAnAnswer));
            }
            await writeMessageToUser(context, conv().getPhrase(Pkey.MyAnswerToQuestion));

         
            await writeMessageToUser(context, new string[] { question.AnswerText });
            await writeMessageToUser(context, conv().getPhrase(Pkey.giveYourFeedback));

            updateRequestTime();
            context.Wait(giveFeedback);

        }


        public async Task giveFeedback(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (context.Activity.Timestamp <= request)
            {
                context.Wait(StartLearning);
                return;
            }

            var message = await result;
            int number;
            if ((number = conv().getNum(message.Text)) >= 0)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.GeneralAck,textVar:(number +"")));
                studySession.CurrentQuestion.AnswerScore = number;

                if (studySession.QuestionAsked.Count == studySession.SessionLength)
                {

                    await writeMessageToUser(context, conv().endOfSession());
                    await writeMessageToUser(context, conv().getPhrase(Pkey.endOfSession));

                    //TODO: save user sussion to DB
                    updateRequestTime();
                    context.Wait(EndOfLearningSession);
                }
                else
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextQuestion));
                    await writeMessageToUser(context, conv().getPhrase(Pkey.beforAskQuestion));
                    await askQuestion(context);
                }
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.notNumber));
                updateRequestTime();
                context.Wait(giveFeedback);
            }
        }


        public async Task EndOfLearningSession(IDialogContext context, IAwaitable<object> result)
        {
            if (context.Activity.Timestamp <= request)
            {
                context.Wait(EndOfLearningSession);
                return;
            }
            context.Done("learningSession");
        }

        private EducationController edc()
        {
            return new EducationController(user,studySession);
        }

    }
}