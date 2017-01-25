using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dataBase
{

    [Serializable]
    public partial class SubQuestion : ISubQuestion
    {
        private int answerScore;

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

        public override bool Equals(object other)
        {
            if (other is SubQuestion)
            {
                var q = other as SubQuestion;
                return this.questionID == q.questionID && this.subQuestionID == q.subQuestionID;
            }
            else
            {
                return false;
            }
        }

            public override int GetHashCode()
            {
                   return this.questionID.GetHashCode() + this.subQuestionID.GetHashCode();
             }
        
    }
}
