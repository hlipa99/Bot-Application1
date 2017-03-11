using Model.dataBase;
using System;
using System.Collections.Generic;

namespace Model.Models
{
    public class AnswerFeedback
    {
        public List<Ientity> missingEntitis = new List<Ientity>();
        public List<string> missingAnswers = new List<string>();

        public int score;

        public void merge(AnswerFeedback f)
        {
            if (f != null)
            {
                score = (score + f.score) / 2;
                missingEntitis.AddRange(f.missingEntitis);
                missingAnswers.AddRange(f.missingAnswers);
            }
        }
    }
}