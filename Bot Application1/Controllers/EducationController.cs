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
        private IUser user;
        private IStudySession studySession;
        ConversationController conversationController;
        QuestionsAnswersControllers qac = new QuestionsAnswersControllers();
        NLPControler nlp = new NLPControler();

        public virtual DataBaseController Db
        {
            get
            {
                return DataBaseController.getInstance();
            }

        }

        public ConversationController ConversationController
        {
            get
            {
                return conversationController;
            }

            set
            {
                conversationController = value;
            }
        }

        public QuestionsAnswersControllers Qac
        {
            get
            {
                return qac;
            }

            set
            {
                qac = value;
            }
        }

        public NLPControler Nlp
        {
            get
            {
                return nlp;
            }

            set
            {
                nlp = value;
            }
        }

        public EducationController(IUser user, IStudySession studySession, ConversationController cc)
        {
            this.user = user;
            this.studySession = studySession;
            this.ConversationController = cc;
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

            res.RemoveAll(x => studySession.QuestionAsked.Contains(x));
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

            ISubQuestion question = studySession.CurrentSubQuestion;

            //var systemAnswerText = studySession.CurrentSubQuestion.answerText;
            //var systemAnswer = nlp.Analize(text, question.questionText);
            var answerFeedback = Qac.matchAnswers(question, text);


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

            if (studySession.CurrentQuestion == null)
            {
                studySession.CurrentQuestion = getQuestion();
                studySession.CurrentQuestion.Enumerator = 0;
            }


            studySession.CurrentQuestion.Enumerator++;
            studySession.CurrentSubQuestion = getSubQuestion(studySession.CurrentQuestion.Enumerator);


            if (studySession.CurrentSubQuestion == null)
            {
                studySession.QuestionAsked.Add(studySession.CurrentQuestion);
                studySession.CurrentQuestion = null;
                getNextQuestion();

            }
        }





        private ISubQuestion getSubQuestion(int enumerator)
        {
            var qEnumerator = studySession.CurrentQuestion.SubQuestion.GetEnumerator();
            foreach (var sq in studySession.CurrentQuestion.SubQuestion)
            {
                if (int.Parse(sq.subQuestionID.Trim()) == enumerator) return sq;
            }

            return null;
        }

        internal string[] createReplayToUser(string text, UserIntent answerIntent)
        {
            switch (answerIntent)
            {
                case UserIntent.unknown:
                case UserIntent.answer:
                    return createFeedBack(checkAnswer(text));

                case UserIntent.dontKnow:
                    return ConversationController.getPhrase(Pkey.neverMind);

                case UserIntent.question:
                    return ConversationController.getPhrase(Pkey.unknownQuestion);

                case UserIntent.stopSession:
                    throw new StopSessionException();

                default:
                    return createFeedBack(checkAnswer(text));

            }
           return null;
        }

        private string[] createFeedBack(AnswerFeedback answerFeedback)
        {
            //check sub question
            if (answerFeedback.score >= 85)
            {
                return  ConversationController.getPhrase(Pkey.goodAnswer);
            }
            else if (answerFeedback.score >= 30)
            {
                return ConversationController.getPhrase(Pkey.partialAnswer);
            }
            else
            {
                return ConversationController.getPhrase(Pkey.notAnAnswer);
            }
        }
    }




}
