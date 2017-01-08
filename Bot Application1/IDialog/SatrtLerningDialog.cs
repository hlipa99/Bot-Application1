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

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class StartLerningDialog: AbsDialog
    {
        StudySession studySession;
        EducationController ec = new EducationController();

        public override async Task StartAsync(IDialogContext context)
        {
            context.UserData.TryGetValue<Users>("user", out user);
            if(user == null)
            {
                throw new unknownUserException();
            }

            ConversationController conv = new ConversationController(user.UserName,user.UserGender);

                    var menu = new PromptDialog.PromptChoice<string>(
                    ec.getStudyCategory(),
                    conv.chooseStudyUnits(),
                    conv.wrongOption()[0],
                    3);
            context.UserData.RemoveValue("studySession");
            studySession = new StudySession();
       //    context.UserData.SetValue<StudySession>("studySession",new StudySession());
            context.Call(menu, StartLearning);


        }

    //    public async virtual Task chooseCategory(IDialogContext context, IAwaitable<string> result)
    //    {
    //        var message = await result;
    ////        context.UserData.TryGetValue<Users>("user", out user);
    //        ConversationController conv = new ConversationController(user.UserName, user.UserGender);
    //  //      context.UserData.TryGetValue<StudySession>("studySession",out studySession);
    //        studySession.Category = message;

    // //       context.UserData.SetValue<StudySession>("studySession", new StudySession());

    //        var menu = new PromptDialog.PromptChoice<string>(
    //             ec.getStudyCategory(message),
    //             conv.chooseStudyUnits(),
    //             conv.wrongOption()[0],
    //             3);

    //        context.Call(menu, StartLearning);
    //    }



        public async virtual Task StartLearning(IDialogContext context, IAwaitable<string> result)
        {
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            var message = await result;
    //        context.UserData.TryGetValue<Users>("user", out user);
   //         context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            studySession.Category = message;
   //         context.UserData.SetValue<StudySession>("studySession", studySession);
            await writeMessageToUser(context, conv.areUReaddyToLearn( message));
            context.Wait(askQuestion);
        }




        public async Task askQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
      //      context.UserData.TryGetValue<Users>("user", out user);
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
   //         context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (conv.isStopSession(message.Text))
            {
                await writeMessageToUser(context, conv.stopLearningSession());
                context.Done("");
            }

   //         context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            await writeMessageToUser(context, conv.beforAskQuestion(studySession));

            var question = ec.getQuestion(studySession.Category, studySession.SubCategory, studySession);

            await writeMessageToUser(context, new string[] { question.QuestionText});
            studySession.currentQuestion = question;
            studySession.QuestionAsked.Add(studySession.currentQuestion);
        //    context.UserData.SetValue<StudySession>("studySession", studySession);
            context.Wait(answerQuestion);
        }


        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            ConversationController conv = new ConversationController(user.UserName, user.UserGender);
            var message = await result;
      //      context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (conv.isStopSession(message.Text))
            {
                await writeMessageToUser(context, conv.stopLearningSession());
                context.Done("");
            }

            var question = studySession.currentQuestion;

            await writeMessageToUser(context, conv.MyAnswerToQuestion());
            await writeMessageToUser(context, new string[] { question.AnswerText });

            context.Wait(askQuestion);
        }






    }



}