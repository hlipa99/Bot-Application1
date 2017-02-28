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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class LerningDialog : AbsDialog
    {
        public override string getDialogContext()
        {
            return "LerningDialog";
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
            }

            StudySession.CurrentQuestion = null;

            await writeMessageToUser(context, conv().getPhrase(Pkey.letsLearn));
          



            IMessageActivity message;
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

            await context.PostAsync(message);
            updateRequestTime(context);
            context.Wait(StartLearning);
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


        public async Task intreduceQuestion(IDialogContext context)
        {


            var question = StudySession.CurrentQuestion;
         
            await writeMessageToUser(context, new string[] { '"'+question.QuestionText + '"' });

            if (question.SubQuestion.Count > 1)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.takeQuestionApart));
            }
            await askQuestion(context);
        }


        public async Task askQuestion(IDialogContext context)
        {


            var question = StudySession.CurrentSubQuestion;
            await writeMessageToUser(context, new string[] { question.questionText.Trim() });

            updateRequestTime(context);
            context.Wait(answerQuestion);
        }



        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (context.Activity.Timestamp <= Request)
            {
                context.Wait(StartLearning);
                return;
            }

            var message = await result;
            //      context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (conv().isStopSession(message.Text))
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.stopLearningSession));
                await EndOfLearningSession(context,result);
            }

            var question = StudySession.CurrentSubQuestion;


            var feedback = conv().createReplayToUser(message.Text, UserContext);

            //    edc().checkAnswer(message.Text);

            await writeMessageToUser(context, feedback);


            await writeMessageToUser(context, conv().getPhrase(Pkey.MyAnswerToQuestion));

         
            await writeMessageToUser(context, new string[] { question.answerText.Trim()});


            if(StudySession.CurrentQuestion.Enumerator == StudySession.CurrentQuestion.SubQuestion.Count)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.giveYourFeedback));
                updateRequestTime(context);
                context.Wait(giveFeedback);
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextSubQuestion));
                edc().getNextQuestion();
                await askQuestion(context);
            }
        }


        public async Task questionEnd(IDialogContext context)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.giveYourFeedback));
            updateRequestTime(context);
            context.Wait(giveFeedback);
        }



        public async Task giveFeedback(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (context.Activity.Timestamp <= Request)
            {
                context.Wait(StartLearning);
                return;
            }

            var message = await result;

            if (conv().isStopSession(message.Text))
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.stopLearningSession));
                await EndOfLearningSession(context, result);
            }


            int number;
            if ((number = conv().getNum(message.Text)) >= 0)
            {
               if(number < 45)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.neverMind));
                }
                else if(number < 75)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.GeneralAck, textVar: (number + "")));
                }
                else
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.veryGood));
                }
              
                StudySession.CurrentQuestion.AnswerScore = number;
                setStudySession(context);

                await questionSummery(context);
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.notNumber));
                updateRequestTime(context);
                context.Wait(giveFeedback);
            }
        }


        public async Task questionSummery(IDialogContext context)
        {
            if (StudySession.QuestionAsked.Count == StudySession.SessionLength)
            {

                await writeMessageToUser(context, conv().endOfSession());
                await writeMessageToUser(context, conv().getPhrase(Pkey.endOfSession));

                //TODO: save user sussion to DB
                updateRequestTime(context);
                context.Wait(EndOfLearningSession);
            }
            else
            {
                edc().getNextQuestion();
                await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextQuestion));
                await writeMessageToUser(context, conv().getPhrase(Pkey.beforAskQuestion));
                await intreduceQuestion(context);
            }
        }


        public async Task EndOfLearningSession(IDialogContext context, IAwaitable<object> result)
        {
            if (context.Activity.Timestamp <= Request)
            {
                context.Wait(EndOfLearningSession);
                return;
            }
            context.Done("learningSession");
        }

        private EducationController edc()
        {
            return new EducationController(User,StudySession,null);
        }

    }
}