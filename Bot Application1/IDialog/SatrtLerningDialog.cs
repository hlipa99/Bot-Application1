using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Bot_Application1.Cardatt_achment;
using Microsoft.Bot.Connector;
using NLPtest;
using NLPtest.WorldObj;
using Bot_Application1.dataBase;
using Catharsis.Commons;
using NLPtest.Models;
using Model.dataBase;
using Bot_Application1.Controllers;
using static Bot_Application1.Controllers.ConversationController.Pkey;
using static Bot_Application1.Controllers.ConversationController;

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
            

            //var menu = new PromptDialog.PromptChoice<string>(
            //ec.getStudyCategory(),
            //conv().chooseStudyUnits(),
            //conv().wrongOption()[0],
            //3);
            await writeMessageToUser(context, conv().getPhrase(Pkey.chooseStudyUnits));
            var message = context.MakeMessage();
            //     List<HeroCard> hcList = new List<HeroCard>();
            foreach (var m in edc().getStudyCategory())
            {
                var hc = new HeroCard(title: m, images: getImage(m), buttons: new CardAction[] { new CardAction(type: "imBack", value: m, title: conv().getPhrase(Pkey.letsLearn)[0]) });

                //  hcList.Add(hc);
                message.Attachments.Add(hc.ToAttachment());
                message.AttachmentLayout = "carousel";

            }



            context.UserData.RemoveValue("studySession");
            studySession = new StudySession();
            //    context.UserData.SetValue<StudySession>("studySession",new StudySession());
            //    context.Call(menu, StartLearning);
            await context.PostAsync(message);
            context.Wait(StartLearning);
        }

        private CardImage[] getImage(string m)
        {
  

            var cardImg = new CardImage(url: edc().getRamdomImg(m));
            return new CardImage[] { cardImg };
        }

        //    public async virtual Task chooseCategory(IDialogContext context, IAwaitable<string> result)
        //    {
        //        var message = await result;
        ////        context.UserData.TryGetValue<Users>("user", out user);
        //        
        //  //      context.UserData.TryGetValue<StudySession>("studySession",out studySession);
        //        studySession.Category = message;

        // //       context.UserData.SetValue<StudySession>("studySession", new StudySession());

        //        var menu = new PromptDialog.PromptChoice<string>(
        //             ec.getStudyCategory(message),
        //             conv().chooseStudyUnits(),
        //             conv().wrongOption()[0],
        //             3);

        //        context.Call(menu, StartLearning);
        //    }



        public async virtual Task StartLearning(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            
            var message = await result;
            //        context.UserData.TryGetValue<Users>("user", out user);
            //         context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            studySession.Category = message.Text;
            //         context.UserData.SetValue<StudySession>("studySession", studySession);
            await writeMessageToUser(context, conv().getPhrase(Pkey.areUReaddyToLearn));
            await writeMessageToUser(context, conv().getPhrase(Pkey.firstQuestion));
            await askQuestion(context);
        }

        public async Task askQuestion(IDialogContext context)
        {
            //  var message = await result;
            //      context.UserData.TryGetValue<Users>("user", out user);
            
            //         context.UserData.TryGetValue<StudySession>("studySession", out studySession);

            //         context.UserData.TryGetValue<StudySession>("studySession", out studySession);
   

            var question = edc().getQuestion(studySession.Category, studySession.SubCategory, studySession);

            await writeMessageToUser(context, new string[] { question.QuestionText });
            studySession.currentQuestion = question;
            studySession.QuestionAsked.Add(studySession.currentQuestion);
            //    context.UserData.SetValue<StudySession>("studySession", studySession);
            context.Wait(answerQuestion);
        }


        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            
  
            var message = await result;
            //      context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (conv().isStopSession(message.Text))
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.stopLearningSession));
                context.Done("");
            }

            var question = studySession.currentQuestion;

            question = edc().checkAnswer(question, message.Text);
            if(question.answerScore > 85)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.goodAnswer));

            }
            else if(question.answerScore > 30)
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

            context.Wait(giveFeedback);

        }


        public async Task giveFeedback(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            
            var message = await result;
            int number;
            if ((number = conv().getNum(message.Text)) >= 0)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.GeneralAck,textVar:(number +"")));
                studySession.currentQuestion.answerScore = number;

                if (studySession.questionAsked.Count == studySession.sessionLength)
                {

                    await writeMessageToUser(context, conv().getPhrase(Pkey.endOfSession));
                    await writeMessageToUser(context, conv().endOfSession());

                    //TODO: save user sussion to DB

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
                context.Wait(giveFeedback);
            }
        }


        public async Task EndOfLearningSession(IDialogContext context, IAwaitable<object> result)
        {
            await writeMessageToUser(context, conv().getPhrase(Pkey.goodbye));
            context.Reset();
            context.Call(new MainDialog(), EndOfLearningSession);

        }

        private EducationController edc()
        {
            return new EducationController(user,studySession);
        }

    }
}