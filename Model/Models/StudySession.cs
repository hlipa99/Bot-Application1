

using Model;
using Model.dataBase;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
    [Serializable]
    public class StudySession : IStudySession
    {
    
        private List<IQuestion> questionAsked;

   
        private IQuestion currentQuestion;
        private int sessionLength = 3;
        private int swearCounter = 0;
   
        private ISubQuestion currentSubQuestion;

        public string SubCategory { get; set; }
        public string Category { get; set; }

        public StudySession(){
                QuestionAsked = new List<IQuestion>();
                sessionLength = 3;
                 startTime = DateTime.UtcNow;
            }




        [JsonConverter(typeof(ConcreteTypeConverter<Question>))]
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

        [JsonConverter(typeof(ConcreteListTypeConverter<IQuestion,Question>))]
        public List<IQuestion> QuestionAsked
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

        [JsonConverter(typeof(ConcreteTypeConverter<SubQuestion>))]
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

        public DateTime startTime { get ; set ; }
        public int SwearCounter { get => swearCounter; set => swearCounter = value; }
    }
}