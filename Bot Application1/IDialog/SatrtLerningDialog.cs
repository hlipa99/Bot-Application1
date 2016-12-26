using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Bot_Application1.Cardatt_achment;
using Microsoft.Bot.Connector;
using NLPtest;
using Bot_Application1.Models;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class StartLerningDialog: IDialog<object>
    {
        StudySession studySession;
        User user;
        EducationController eduC = new EducationController();
        public async Task StartAsync(IDialogContext context)
        {
            context.UserData.TryGetValue<User>("user", out user);
            if(user == null)
            {
                throw new unknownUserException();
            }


                    var menu = new PromptDialog.PromptChoice<string>(
                    eduC.getStudyUnits(),
                    BotControler.chooseStudyUnits(user),
                    BotControler.wrongOption()[0],
                    3);
            context.UserData.SetValue<StudySession>("studySession",new StudySession());
            context.Call(menu, chooseCategory);


        }

        public async virtual Task chooseCategory(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;

            context.UserData.TryGetValue<StudySession>("studySession",out studySession);
            studySession.StudyUnit = message;

            context.UserData.SetValue<StudySession>("studySession", new StudySession());

            var menu = new PromptDialog.PromptChoice<string>(
                 eduC.getStudyCategory(message),
                 BotControler.chooseStudyUnits(user),
                 BotControler.wrongOption()[0],
                 3);

            context.Call(menu, StartLearning);
        }



        public async virtual Task StartLearning(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            
            context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            studySession.StudySubject = message;
            context.UserData.SetValue<StudySession>("studySession", studySession);
            await writeMessageToUser(context, BotControler.areUReaddyToLearn(user, message));
            context.Wait(askQuestion);
        }




        public async Task askQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (BotControler.isStopSession(message.Text))
            {
                await writeMessageToUser(context, BotControler.stopLearningSession(user));
                return;
            }

            context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            await writeMessageToUser(context, BotControler.beforAskQuestion(user,studySession.QuestionAsked.Count + 1));

            //var question = EducationController.getQuestion(studySession.StudySubject);

            await writeMessageToUser(context, new string[] { " שאלה כלשהי בנושא " + studySession.StudySubject });
            studySession.currentQuestion = " שאלה כלשהי בנושא " + studySession.StudySubject;
            studySession.QuestionAsked.Add(studySession.currentQuestion + studySession.QuestionAsked.Count);
            context.UserData.SetValue<StudySession>("studySession", studySession);
            context.Wait(answerQuestion);
        }


        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            if (BotControler.isStopSession(message.Text))
            {
                await writeMessageToUser(context, BotControler.stopLearningSession(user));
                return;
            }

            await writeMessageToUser(context, BotControler.MyAnswerToQuestion());
            //var question = EducationController.getQuestion(studySession.StudySubject);
            await writeMessageToUser(context, new string[] { "תשובה לשאלה:" + studySession.currentQuestion });

            context.Wait(askQuestion);
        }




        private static async Task writeMessageToUser(IDialogContext context, string[] newMessage)
        {
            foreach (var m in newMessage)
            {
                await context.PostAsync(m);
            }
        }


    }



}