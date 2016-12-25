using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Models
{
    [Serializable]
    public class StudySession
    {
        string studyUnit = "";
        string studySubject = "";
        HashSet<string> questionAsked = new HashSet<string>();
        internal string currentQuestion = "";

        public string StudyUnit
        {
            get
            {
                return studyUnit;
            }

            set
            {
                studyUnit = value;
            }
        }

        public string StudySubject
        {
            get
            {
                return studySubject;
            }

            set
            {
                studySubject = value;
            }
        }

        public HashSet<string> QuestionAsked
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
    }
}