using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Model;
using Microsoft.Bot.Connector;
using Bot_Application1.Models;
using Bot_Application1.Controllers;
using Bot_Application1.Exceptions;
using System.Threading;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class QuestionDialog : AbsDialog<IMessageActivity>
    {


        public override UserContext getDialogContext()
        {
            UserContext.dialog = "QuestionDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await intreduceQuestion(context);
        }

        public async Task intreduceQuestion(IDialogContext context)
        {
            getStudySession(context);
            var question = StudySession.CurrentQuestion;

            await writeMessageToUser(context, new string[] { '"' + question.QuestionText + '"' });

            if (question.SubQuestion.Count > 1)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.takeQuestionApart));
            }
            await askQuestion(context,null);
        }



        public async Task askQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var question = StudySession.CurrentSubQuestion;
            await writeMessageToUser(context, new string[] { question.questionText.Trim() });

            updateRequestTime(context);
            context.Wait(answerQuestion);
        }



        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await outDatedMessage(context, askQuestion, result)) return;

            var message = await result;
            //      context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            var question = StudySession.CurrentSubQuestion;

            try
            {
                var replay = conv().createReplayToUser(message.Text, getDialogContext());
                await writeMessageToUser(context, replay);
            }
            catch(UnrelatedSubjectException ex) {
                await context.Forward<IMessageActivity, IMessageActivity>(new SideDialog(), askQuestion, message, CancellationToken.None);
                return;
            }



            //     await writeMessageToUser(context, conv().getPhrase(Pkey.MyAnswerToQuestion));


            //       await writeMessageToUser(context, new string[] { question.answerText.Trim() });






            if (StudySession.CurrentQuestion.Enumerator == StudySession.CurrentQuestion.SubQuestion.Count)
            {
                //await writeMessageToUser(context, conv().getPhrase(Pkey.giveYourFeedback));
                //updateRequestTime(context);
                //await giveFeedbackMessage(context);
                setStudySession(context);
                context.Done("");
            }
            else
            {
             
                await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextSubQuestion));
                edc().getNextQuestion();
                await intreduceQuestion(context);
            }
        }


        public async Task giveFeedbackMessage(IDialogContext context)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.giveYourFeedback));
            updateRequestTime(context);
            context.Wait(giveFeedback);
        }



        public async Task giveFeedback(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await outDatedMessage(context, askQuestion, result)) return;

            var message = await result;

            int number;
            if ((number = conv().getNum(message.Text)) >= 0)
            {
                if (number < 45)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.neverMind));
                }
                else if (number < 75)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.GeneralAck, textVar: (number + "")));
                }
                else
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.veryGood));
                }

                StudySession.CurrentQuestion.AnswerScore = number;
                setStudySession(context);

                context.Done("");
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.notNumber));
                updateRequestTime(context);
                context.Wait(giveFeedback);
            }
        }


    }
}