using Model.Models;
using NLP.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.Controllers
{
    public class QuestionsAnswersControllers
    {
        QAEngin qna = new QAEngin();
        public AnswerFeedback matchAnswers(ISubQuestion subquestion, string answer)
        {
            return qna.matchAnswers(subquestion, answer);
        }
    }
}
