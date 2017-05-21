using Model.dataBase;
using System;
using System.Collections.Generic;

namespace Model.Models
{
    public class AnswerFeedback
    {
        public List<IentityBase> missingEntitis = new List<IentityBase>();
        public List<IentityBase> foundEntitis = new List<IentityBase>();
        public List<AnswerFeedback> answersFeedbacks = new List<AnswerFeedback>();
        //public List<string> foundAnswers = new List<string>();

        public int score;
        public string answer;
        private int need;

        public int Need { get => need; set => need = value; }

        public void addFeedback(AnswerFeedback f)
        {
            if (f != null)
            {
                Double d = (score* (answersFeedbacks.Count) + f.score) / (answersFeedbacks.Count + 1);
                score = (int)Math.Ceiling(d);
                answersFeedbacks.Add(f);
                missingEntitis.AddRange(f.missingEntitis);
                foundEntitis.AddRange(f.foundEntitis);
            }
        }
    }
}