using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dataBase
{

    [Serializable]
    public partial class Question : IQuestion
    {
        private int answerScore;
        private int enumerator;

        public int AnswerScore
        {
            get
            {
                return answerScore;
            }

            set
            {
                answerScore = value;
            }
        }

        public int Enumerator
        {
            get
            {
                return enumerator;
            }

            set
            {
                enumerator = value;
            }
        }

        public override bool Equals(object other)
        {
            if (other is Question)
            {
                var q = other as Question;
                return this.QuestionID == q.QuestionID;
            }
            else
            {
                return false;
            }
        }

            public override int GetHashCode()
            {
            if (this.QuestionID != null)
                return this.QuestionID.GetHashCode();
            else
                return this.QuestionText.GetHashCode();
             }
        
    }
}
