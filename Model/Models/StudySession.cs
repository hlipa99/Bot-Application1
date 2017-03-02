
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

        private HashSet<Question> questionAsked = new HashSet<Question>();
        private Question currentQuestion;
        private int sessionLength = 3;
        private SubQuestion currentSubQuestion;

        public string SubCategory { get; set; }
        public string Category { get; set;}

        public StudySession(){
                Category = "";
                SessionLength = 0;
                QuestionAsked = new HashSet<Question>();
                currentQuestion = null;
                sessionLength = 3;
            }


        public int SessionLength{get;set;}

        public HashSet<IQuestion> IQuestionAsked
        {
            get
            {
                HashSet<IQuestion> ret = new HashSet<IQuestion>();
                foreach (var q in questionAsked)
                {
                    ret.Add(q);
                }
                return ret;
            }

            set
            {

                questionAsked = new HashSet<Question>();
                foreach (var q in value)
                {
                    questionAsked.Add((Question)q);
                }
            }
        }

        public HashSet<Question> QuestionAsked{ get;set;}
        public Question CurrentQuestion { get; set; }

        public IQuestion ICurrentQuestion
        {
            get
            {
                return CurrentQuestion;
            }

            set
            {
                CurrentQuestion = (Question) value;
            }
        }

        public ISubQuestion ICurrentSubQuestion
        {
            get
            {
                return CurrentSubQuestion;
            }

            set
            {
                CurrentSubQuestion = (SubQuestion) value;
            }
        }

        public SubQuestion CurrentSubQuestion
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