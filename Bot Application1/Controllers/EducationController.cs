
using Bot_Application1.Exceptions;
using Model;
using Model.dataBase;
using Model.Models;
using NLP.Controllers;
using NLP.Models;
using NLP.NLP;
using NLP.view;
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
        NLPControler nlp = new NLPControler();
        QuestionsAnswersControllers qac;

        public virtual DataBaseController Db
        {
            get
            {
                return DataBaseController.getInstance();
            }
            set
            {
                DataBaseController.setStubInstance (value);
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
             Qac = new QuestionsAnswersControllers();

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
                var question = res[r.Next(res.Count)];
                question = ajustGender(question);
                return question;
            }
            else
            {
                throw new CategoryOutOfQuestionException();
            }
        }

        private IQuestion ajustGender(IQuestion question)
        {
            if (user.UserGender == "femenine")
            {
                question.QuestionText = ajustGender(question.QuestionText);
                foreach(var s in question.SubQuestion)
                {
                    s.questionText = ajustGender(question.QuestionText);
                }
                return question;
            }
            else
            {
                return question;
            }
        }

        private string ajustGender(string questionText)
        {
           questionText = questionText.Replace("הצג","הציגי");
           questionText = questionText.Replace("הסבר", "הסבירי");
           questionText = questionText.Replace("ציין", "צייני");
           questionText = questionText.Replace("לפניך", "לפנייך");
           questionText = questionText.Replace("כתוב", "כתבי");
           questionText = questionText.Replace("עיין", "עייני");
           questionText = questionText.Replace("הסתכל", "הסתכלי");
           return questionText;
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
            if (studySession.CurrentQuestion != null) studySession.QuestionAsked.Add(studySession.CurrentQuestion);

                studySession.CurrentQuestion = getQuestion();
                studySession.CurrentQuestion.Enumerator = 0;
        }

        internal void getNextSubQuestion()
        {
            studySession.CurrentQuestion.Enumerator++;
            studySession.CurrentSubQuestion = getSubQuestion(studySession.CurrentQuestion.Enumerator);
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
                case UserIntent.DefaultFallbackIntent:
                case UserIntent.unknown:
                case UserIntent.historyAnswer:
                    return createFeedBack(checkAnswer(text));

                case UserIntent.dontKnow:
                    return ConversationController.mergeText(ConversationController.getPhrase(Pkey.neverMind), ConversationController.mergeText(ConversationController.getPhrase(Pkey.MyAnswerToQuestion), studySession.CurrentSubQuestion.answerText));

                case UserIntent.stopSession:
                    throw new StopSessionException();

                case UserIntent.sessionBreak:
                    throw new sessionBreakException();

                default:
                    return createFeedBack(checkAnswer(text));

            }
           return null;
        }

        private string[] createFeedBack(AnswerFeedback answerFeedback)
        {
            string[] verbalFeedback = null;
            //check sub question
            studySession.CurrentSubQuestion.AnswerScore = answerFeedback.score;
            if (answerFeedback.score >= 60)
            {
                verbalFeedback =  ConversationController.getPhrase(Pkey.goodAnswer);
            }
            else if (answerFeedback.score >= 35)
            {
                verbalFeedback =  ConversationController.getPhrase(Pkey.partialAnswer);
            }
            else if (answerFeedback.answer.Split(' ').Length > 2)
            {
                verbalFeedback = ConversationController.getPhrase(Pkey.wrongAnswer);
            }else
            {
                verbalFeedback =  ConversationController.getPhrase(Pkey.notAnAnswer);
            }

            answerFeedback.missingEntitis.RemoveAll(x => x.entityType == "conceptWord");

            if (answerFeedback.missingAnswers.Count > 0)
            {
                verbalFeedback = ConversationController.mergeText(verbalFeedback, ConversationController.getPhrase(Pkey.missingAnswrPart));
                verbalFeedback = ConversationController.mergeText(verbalFeedback, answerFeedback.missingAnswers[0]);
                answerFeedback.missingAnswers.RemoveAt(0);
                foreach (var a in answerFeedback.missingAnswers)
                {
                    verbalFeedback = ConversationController.mergeText(verbalFeedback, ConversationController.mergeText(ConversationController.getPhrase(Pkey.and) , a));
                }
            }
            else if (answerFeedback.score < 75)
            {
                
                verbalFeedback = ConversationController.mergeText(verbalFeedback, ConversationController.getPhrase(Pkey.MyAnswerToQuestion));
                verbalFeedback = ConversationController.mergeText(verbalFeedback, studySession.CurrentSubQuestion.answerText);
            }

            return verbalFeedback;
        }
    }




}
