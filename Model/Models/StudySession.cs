
using Bot_Application1.dataBase;
using Model.dataBase;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLPtest.Models
{
    [Serializable]
    public class StudySession : IStudySession
    {

        private HashSet<IQuestion> questionAsked = new HashSet<IQuestion>();
        private IQuestion currentQuestion = null;
        private int sessionLength = 3;

        public string SubCategory { get; set; }
        public string Category { get; set; }

        public HashSet<IQuestion> QuestionAsked
        {
            get
            {
                return QuestionAsked;
            }

            set
            {
                QuestionAsked = value;
            }
        }

      

        public IQuestion CurrentQuestion
        {
            get
            {
                return currentQuestion;
            }

            set
            {
                currentQuestion = value;
            }
        }

        public int SessionLength
        {
            get
            {
                return sessionLength;
            }

            set
            {
                sessionLength = value;
            }
        }
    }
}