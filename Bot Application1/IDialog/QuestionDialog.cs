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
using Bot_Application1.YAndex;
using NLP.NLP;

namespace Bot_Application1.IDialog
{
    [Serializable]
    public class QuestionDialog : AbsDialog<string>
    {


        public override UserContext getDialogContext()
        {
            base.getDialogContext();
            UserContext.dialog = "QuestionDialog";
            return UserContext;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            getDialogsVars(context);
            if (StudySession.CurrentSubQuestion == null)
            {
                await intreduceQuestion(context);
            }
            else
            {
                await askSubQuestion(context, null);
            }
        }

        public async Task intreduceQuestion(IDialogContext context)
        {
            
            var question = StudySession.CurrentQuestion;


            if (question.Flags.Contains("sorcePic"))
            {
                var mediaKey = question.questionMedia;
                await postImageToUser(context, mediaKey);
            }
            if (question.SubQuestion.Count > 1)
                {
                    
                    await writeMessageToUser(context, new string[] { '"' + question.QuestionText + '"' });
                    await writeMessageToUser(context, conv().getPhrase(Pkey.takeQuestionApart));
                }
            setDialogsVars(context);
            await askNextSubQuestion(context, null);
       
        }

      

        public async Task askNextSubQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            getDialogsVars(context);
            //if (result != null) await result;
            edc().getNextSubQuestion();
            var question = StudySession.CurrentSubQuestion;
         
            if (question != null)
            {
                setDialogsVars(context);
                await askSubQuestion(context, null);
            }
            else
            {
                getDialogsVars(context);
                edc().updateUserScore();
                setDialogsVars(context);
                context.Done("");
            }
        }

        public async Task askSubQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            getDialogsVars(context);
            var question = StudySession.CurrentSubQuestion;
            if (question != null)
            {
                await writeMessageToUser(context, new string[] { Emoji.get("questionMark") + '"' + question.questionText.Trim() + '"' });

                updateRequestTime(context);
                setDialogsVars(context);
                context.Wait(answerQuestion);
                return;
            }
            else
            {
                await StartAsync(context);
            }
        }


        public async Task answerQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            try
            {
                if (await checkOutdatedMessage<IMessageActivity>(context, answerQuestion, result)) return;

          

        //    if(User.Language == "en") {
        //        message.Text = ControlerTranslate.TranslateToEng(message.Text);
        //    }

            //      context.UserData.TryGetValue<StudySession>("studySession", out studySession);
            var question = StudySession.CurrentSubQuestion;

          
                typingTime(context);
                

                //jokes,info,swear, etc.
                var response = conv().getUserConvResponse(message.Text,getDialogContext());
                if (response != null)
                {
                    await writeMessageToUser(context, response);
                    await writeMessageToUser(context, conv().getPhrase(Pkey.letsContinue));
                    setDialogsVars(context);
                    if (StudySession.SwearCounter > 2)
                    {
                        await writeMessageToUser(context, conv().getPhrase(Pkey.swearSuspention));
                        updateRequestTime();
                        context.Wait(swearSuspention);
                        return;
                    }
                    else
                    {
                        await askSubQuestion(context, null);
                    }
                    return;
                }
                else
                {
                    typingTime(context);
                    var replay = conv().createReplayToUser(message.Text, getDialogContext());
                    await writeMessageToUser(context, replay);
                    setDialogsVars(context);
                }
            }
            catch(UnrelatedSubjectException ex) {
      
                await context.Forward<IMessageActivity, IMessageActivity>(new SideDialog(), askSubQuestion, message, CancellationToken.None);
                return;
            }
            catch(menuException ex){

                var msg = conv().getPhrase(Pkey.areYouSureMenu);
                await context.Forward<bool, string[]>(new YesNoQuestionDialog(), stopSessionMenu, msg, CancellationToken.None);
                return;
            }
            catch (StopSessionException ex)
            {
                await writeMessageToUser(context, conv().endOfSession());
                var msg = conv().getPhrase(Pkey.areYouSure);
                await context.Forward<bool, string[]>(new YesNoQuestionDialog(), stopSession, msg, CancellationToken.None);
                return;
               
            }catch (sessionBreakException ex){
                throw new sessionBreakException();
                return;
            }
            catch (Exception ex)
            {
                await generalExceptionError(context, ex);
                await askNextSubQuestion(context, null);
                return;
            }


            //     await writeMessageToUser(context, conv().getPhrase(Pkey.MyAnswerToQuestion));


            //       await writeMessageToUser(context, new string[] { question.answerText.Trim() });



            if (StudySession.CurrentQuestion.Enumerator < StudySession.CurrentQuestion.SubQuestion.Count)
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.moveToNextSubQuestion));
                await askNextSubQuestion(context, null);
            }
            else
            {
                //await writeMessageToUser(context, conv().getPhrase(Pkey.giveYourFeedback));
                //updateRequestTime(context);
                //await giveFeedbackMessage(context);
                getDialogsVars(context); 
                edc().updateUserScore(); 
                context.Done("");
            }
        }

        private async Task swearSuspention(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (Request.AddMinutes(10) > DateTime.UtcNow)
            {
                var res = await result;
                if (conv().getUserIntente(res.Text, getDialogContext()) == UserIntent.sorry)
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.ok));
                    await writeMessageToUser(context, conv().getPhrase(Pkey.letsContinue));
                    StudySession.SwearCounter = 0;
                    setDialogsVars(context);
                    await askSubQuestion(context, null);
                }
                else
                {
                    await writeMessageToUser(context, conv().getPhrase(Pkey.duringSwearSuspention));
                    context.Wait(swearSuspention);
                    return;
                }
            }
            else
            {
                StudySession.SwearCounter = 0;
                setDialogsVars(context);
                await askSubQuestion(context, null);
            }
        }

        private async Task stopSession(IDialogContext context, IAwaitable<bool> result)
        {
            var sure = await result;
            if (sure)
            {
             //   await writeMessageToUser(context, conv().getPhrase(Pkey.goodbye));
                throw new StopSessionException();
                return;
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.letsContinue));
                await askSubQuestion(context,null);
            }

        }

        private async Task stopSessionMenu(IDialogContext context, IAwaitable<bool> result)
        {
            var sure = await result;
            if (sure)
            {
                //   await writeMessageToUser(context, conv().getPhrase(Pkey.goodbye));
                throw new menuException();
                return;
            }
            else
            {
                await writeMessageToUser(context, conv().getPhrase(Pkey.letsContinue));
                await askSubQuestion(context, null);
            }

        }




    }
}