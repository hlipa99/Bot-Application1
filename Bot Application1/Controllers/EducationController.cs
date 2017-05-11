
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
using System.Linq;

namespace Bot_Application1.Controllers
{

    [Serializable]
    public class EducationController
    {
        private IUser user;
        private IStudySession studySession;
        ConversationController conversationController;
        NLPControler nlp = new NLPControler();
        QuestionsAnswersControllers qac ;
        DataBaseController db;
        public virtual DataBaseController Db
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
            this.db = new DataBaseController();
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
                throw new CategoryOutOfQuestionException();
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
            if (user.UserGender == "feminine")
            {
                question.QuestionText = ajustGender(question.QuestionText);
                foreach(var s in question.SubQuestion)
                {
                    s.questionText = ajustGender(s.questionText);
                }
                return question;
            }
            else
            {
                return question;
            }
        }


        public async void updateUserScore()
        {
            var score = new userScore();
            var question = studySession.CurrentQuestion;

            score.category = question.Category;
            score.subCategory = question.SubCategory;
            
            score.userID = user.UserID;
            score.dateTime = DateTime.UtcNow;
            score.QuestionID = studySession.CurrentQuestion.QuestionID.Trim();
            foreach (var sq in studySession.CurrentQuestion.SubQuestion)
            {
                score.SubquestionID = sq.subQuestionID.Trim();
                score.Score = sq.AnswerScore;
                await Db.addUserScore(score);
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
            questionText = questionText.Replace("הגדר", "הגדירי");
            questionText = questionText.Replace("קרא", "קראי");
            return questionText;
        }

        public AnswerFeedback checkAnswer(string text)
        {

            ISubQuestion question = studySession.CurrentSubQuestion;

            var answerFeedback = Qac.matchAnswers(question, text);
            try
            {
                Logger.addAnswerOutput(question.answerText, text, answerFeedback,user.UserID,question.questionID + "." + question.subQuestionID);
            }
            catch (Exception ex) { }

            return answerFeedback;
        }

        public void getNextQuestion()
        {
            if (studySession.CurrentQuestion != null)
            {
                studySession.QuestionAsked.Add(studySession.CurrentQuestion);
            }

                studySession.CurrentQuestion = getQuestion();
                studySession.CurrentQuestion.Enumerator = 0;
                studySession.CurrentSubQuestion = null;
        }

        public void getNextSubQuestion()
        {
            if(studySession.CurrentQuestion == null)
            {
                getNextQuestion();
            }
            else
            {
                studySession.CurrentQuestion.Enumerator++;

            }
            if (studySession.CurrentQuestion != null)
            {
                studySession.CurrentSubQuestion = getSubQuestion(studySession.CurrentQuestion.Enumerator);
                studySession.CurrentSubQuestion.questionText = studySession.CurrentSubQuestion.questionText;
            }
            else 
            {
                getNextSubQuestion();
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

        public string[] createReplayToUser(string text, UserIntent answerIntent)
        {
            switch (answerIntent)
            {                         
                case UserIntent.dontKnow:
                    var feedback = ConversationController.getPhrase(Pkey.neverMind);
                    feedback = ConversationController.mergeText(feedback,ConversationController.getPhrase(Pkey.MyAnswerToQuestion));
                    feedback = answerArrayToString(new List<string>(studySession.CurrentSubQuestion.answerText.Split('|')),feedback);
                    return feedback;
                case UserIntent.menu:
                    throw new menuException();
                case UserIntent.stopSession:
                    throw new StopSessionException();
                case UserIntent.sessionBreak:
                    throw new sessionBreakException();
                case UserIntent.bot_questions:
                case UserIntent.DefaultFallbackIntent:
                case UserIntent.unknown:
                case UserIntent.historyAnswer:
                default:
                    return formatFeedbackPhrases(qac.createFeedBack(checkAnswer(text)));
                    
            }

           return null;
        }

        private string[] formatFeedbackPhrases(string[] val)
        {
            var res = new List<string>();
            foreach(var s in val)
            {
                res.Add(conversationController.formatPhrases(s));
            }
            return res.ToArray();
        }

        private string[] answerArrayToString(List<string> answers, string[] verbalFeedback)
        {

            if (answers.Count > 1) {
                var last = answers[answers.Count - 1];
                answers.RemoveAt(answers.Count - 1);
                foreach (var a in answers)
                {
                    verbalFeedback = ConversationController.mergeText(verbalFeedback, a.Trim() + ",");
                }

                verbalFeedback = ConversationController.mergeText(verbalFeedback, ConversationController.mergeText(ConversationController.getPhrase(Pkey.and), last));
            }
            else if (answers.Count == 1)
            {
                verbalFeedback = ConversationController.mergeText(verbalFeedback, answers[0]);
            }

            return verbalFeedback;
        }
    }




}
