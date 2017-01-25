using Bot_Application1.dataBase;
using Bot_Application1.Exceptions;
using Model.dataBase;
using Model.Models;
using NLPtest.Models;
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

        public EducationController(IUser user, IStudySession studySession)
        {
            this.user = user;
            this.studySession = studySession;
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

        public ISubQuestion checkAnswer(ISubQuestion question, string text)
        {
            if (text.Split(' ').Length > 5 && !text.Contains("לא יודע"))
            {
                question.AnswerScore = 100;
            }
            else if (text.Split(' ').Length > 3 && !text.Contains("לא יודע"))
            {
                question.AnswerScore = 56;
            }
            else
            {
                question.AnswerScore = 0;
            }
            return question;
        }


        internal void getNextQuestion()
        {
            if (studySession.CurrentQuestion == null)
            {
                studySession.CurrentQuestion = getQuestion();
                studySession.CurrentQuestion.Enumerator = 0;
            }


            studySession.CurrentSubQuestion = getSubQuestion(studySession.CurrentQuestion.Enumerator);
            studySession.CurrentQuestion.Enumerator++;
            if (studySession.CurrentSubQuestion == null)
            {
                studySession.QuestionAsked.Add(studySession.CurrentQuestion);
                studySession.CurrentQuestion = null;
                getNextQuestion();
            }


        }
    

        private ISubQuestion getSubQuestion(int enumerator)
        {
           var qEnumerator =  studySession.CurrentQuestion.SubQuestion.GetEnumerator();
           for(int i = 0; i <= enumerator; i++)
            {
                qEnumerator.MoveNext();
            }
            return qEnumerator.Current;
        }
    }
 }
