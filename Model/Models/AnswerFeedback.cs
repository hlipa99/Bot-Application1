using Model.dataBase;
using System;
using System.Collections.Generic;

namespace Model.Models
{
    public class AnswerFeedback
    {
        public List<IentityBase> missingEntitis = new List<IentityBase>();
        public List<IentityBase> foundEntitis = new List<IentityBase>();
        public List<string> missingAnswers = new List<string>();
        public List<string> foundAnswers = new List<string>();

        public int score;
        public string answer;


        public void merge(AnswerFeedback f)
        {
            if (f != null)
            {
                Double d = (score + f.score) / 2.0;
                score = (int)Math.Ceiling(d);
                missingEntitis.AddRange(f.missingEntitis);
                missingAnswers.AddRange(f.missingAnswers);
            }
        }
    }
}