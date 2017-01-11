using Bot_Application1.dataBase;
using Bot_Application1.Exceptions;
using Model.dataBase;
using NLPtest.Models;
using System;
using System.Collections.Generic;

namespace Bot_Application1.Controllers
{
 
    [Serializable]
    internal class EducationController
    {
         DataBaseController db = new DataBaseController();
        private Users user;
        private StudySession studySession;

        public EducationController(Users user, StudySession studySession)
        {
            this.user = user;
            this.studySession = studySession;
        }

        internal string[] getStudyCategory()
        {
            var res = db.getAllCategory();
            return res;
        }

        internal IEnumerable<string> getStudySubCategory(string category)
        {
            var res = db.getAllSubCategory(category);
            return res;
        }

        public string getRamdomImg(string mediaKey)
        {
            var media = db.getMedia(mediaKey, "img", "");
            var r = new Random();

            if(media.Length > 0)
            {
                return media[r.Next(media.Length - 1)];

            }else
            {
                return "https://img.clipartfest.com/d82385630de0b6201f6a6bd5d2367726_clipart-question-mark-clip-art-clipart-question-mark-3d_494-743.jpeg";
            }
        }

        internal Question getQuestion(string category, string subCategory, StudySession studySession)
        {
            List<Question> res = new List<Question>();
            if(subCategory == null)
            {
                res.AddRange(db.getQuestion(category));
            }
            else
            {
                res.AddRange(db.getQuestion(category, subCategory));
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

        internal Question checkAnswer(Question question, string text)
        {
            if(text.Split(' ').Length > 2)
            {
                question.answerScore = 100;
            }
            else
            {
                question.answerScore = 0;
            }
            return question;
        }
    }
}