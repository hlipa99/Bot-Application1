using Bot_Application1.dataBase;
using Bot_Application1.Exceptions;
using Model;
using Model.dataBase;
using Model.Models;
using NLPtest.Controllers;
using NLPtest.Models;
using NLPtest.NLP;
using NLPtest.view;
using System;
using System.Collections.Generic;

namespace Bot_Application1.Controllers
{

    [Serializable]
    public class EducationController
    {
        DataBaseController db = new DataBaseController();
        private IUser user;
        private IStudySession studySession;
        ConversationController convCtrl;
        public DataBaseController Db
        {
            get
            {
                return db;
            }

            set
            {
                db = value;
            }
        }

        public EducationController(IUser user, IStudySession studySession, ConversationController cc)
        {
            this.user = user;
            this.studySession = studySession;
            this.convCtrl = cc;
        }

        public string[] getStudyCategory()
        {
            var res = Db.getAllCategory();
            return res;
        }

        public IEnumerable<string> getStudySubCategory(string category)
        {
            var res = Db.getAllSubCategory(category);
            return res;
        }

        public string getRamdomImg(string mediaKey)
        {
            var media = Db.getMedia(mediaKey, "img", "");
            var r = new Random();

            if (media.Length > 0)
            {
                return media[r.Next(media.Length - 1)];

            } else
            {
                return "https://img.clipartfest.com/d82385630de0b6201f6a6bd5d2367726_clipart-question-mark-clip-art-clipart-question-mark-3d_494-743.jpeg";
            }
        }

        public IQuestion getQuestion()
        {
            List<IQuestion> res = new List<IQuestion>();
            IQuestion[] questions;


            if (studySession.SubCategory == null)
            {
                questions = Db.getQuestion(studySession.Category);
            }
            else
            {
                questions = Db.getQuestion(studySession.Category, studySession.SubCategory);
            }
            if (questions.Length > 0)
            {
                res.AddRange(questions);
            }
            else
            {
                return null;
            }

            res.RemoveAll(x => studySession.IQuestionAsked.Contains(x));
            var r = new Random();
            if (res.Count > 0)
            {
                return res[r.Next(res.Count)];
            }
            else
            {
                throw new CategoryOutOfQuestionException();
            }
        }

        public AnswerFeedback checkAnswer(string text)
        {
            QuestionsAnswersControllers qac = new QuestionsAnswersControllers();
            ISubQuestion question = studySession.ICurrentSubQuestion;
            var nlp = NLPControler.getInstence();
            //var systemAnswerText = studySession.CurrentSubQuestion.answerText;
            //var systemAnswer = nlp.Analize(text, question.questionText);
            var answerFeedback = qac.matchAnswers(question, text);


            //var answerFeedback = new AnswerFeedback();
            //var anslength = text.Split(' ').Length;
            //    if (anslength > 7)
            //    {
            //    answerFeedback.score =  100;
            //    }else if(anslength > 4)
            //    {
            //    answerFeedback.score = 60;
            //}else if (anslength >= 2)
            //{
            //    answerFeedback.score = 30;
            //}else
            //{
            //    answerFeedback.score = 0;
            //}
            
         
            return answerFeedback;
        }

        internal void getNextQuestion()
        {

            if (studySession.ICurrentQuestion == null)
            {
                studySession.ICurrentQuestion = getQuestion();
                studySession.ICurrentQuestion.Enumerator = 0;
            }


            studySession.ICurrentQuestion.Enumerator++;
            studySession.ICurrentSubQuestion = getSubQuestion(studySession.ICurrentQuestion.Enumerator);


            if (studySession.ICurrentSubQuestion == null)
            {
                studySession.IQuestionAsked.Add(studySession.ICurrentQuestion);
                studySession.ICurrentQuestion = null;
                getNextQuestion();

            }
        }





        private ISubQuestion getSubQuestion(int enumerator)
        {
            var qEnumerator = studySession.ICurrentQuestion.SubQuestion.GetEnumerator();
            foreach (var sq in studySession.ICurrentQuestion.SubQuestion)
            {
                if (int.Parse(sq.subQuestionID.Trim()) == enumerator) return sq;
            }

            return null;
        }

        internal string[] createReplayToUser(string text, UserIntent answerIntent)
        {
            switch (answerIntent)
            {

                case UserIntent.dontKnow:
                    var message = convCtrl.merge(convCtrl.getPhrase(Pkey.neverMind),convCtrl.getPhrase(Pkey.MyAnswerToQuestion));
                    message = convCtrl.merge(message, studySession.ICurrentSubQuestion.answerText);
                    return message;
                case UserIntent.question:
                    throw new UnrelatedSubjectException();

                case UserIntent.stopSession:
                    throw new StopSessionException();
                case UserIntent.unknown:
                case UserIntent.answer:
                default:
                    var feedback = checkAnswer(text);
                    studySession.ICurrentQuestion.AnswerScore = feedback.score;
                    return createVerbalFeedBack(feedback);

            }
            return null;
        }

        private string[] createVerbalFeedBack(AnswerFeedback answerFeedback)
        {
            List<string> verbalAssasment = new List<string>();
            //check sub question
            if (answerFeedback.score >= 85)
            {
                verbalAssasment.AddRange(convCtrl.getPhrase(Pkey.goodAnswer));
            }
            else if (answerFeedback.score >= 30)
            {
                verbalAssasment.AddRange(convCtrl.getPhrase(Pkey.partialAnswer));
            }
            else
            {
                verbalAssasment.AddRange(convCtrl.getPhrase(Pkey.notAnAnswer));
            }

            if(answerFeedback.missingAnswers.Count > 0)
            {
                verbalAssasment.AddRange(convCtrl.getPhrase(Pkey.missingAnswrPart));
                verbalAssasment.Add('"' + answerFeedback.missingAnswers[0] + '"');
                answerFeedback.missingAnswers.RemoveAt(0);
                foreach (var a in answerFeedback.missingAnswers)
                { 
                
                    verbalAssasment.Add(convCtrl.getPhrase(Pkey.and)[0] + " \"" + a + "\"");
                }
            }

            return verbalAssasment.ToArray();

        }
    }




}
