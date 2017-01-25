
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

        private HashSet<IQuestion> questionAsked;
        private IQuestion currentQuestion;
        private int sessionLength = 3;
        private ISubQuestion currentSubQuestion;

        public string SubCategory { get; set; }
        public string Category { get; set; }

        public StudySession(){
                Category = "";
                SessionLength = 0;
                QuestionAsked = new HashSet<IQuestion>();
                currentQuestion = null;
                sessionLength = 3;
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

        public HashSet<IQuestion> QuestionAsked
        {
            get
            {
                return questionAsked;
            }

            set
            {
                questionAsked = value;
            }
        }

        public ISubQuestion CurrentSubQuestion
        {
            get
           {
                return currentSubQuestion;
            }
            set
            {
                currentSubQuestion = value;
            }

        }
       
    }
}