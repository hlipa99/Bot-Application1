using Model.Models;
using NLPtest.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.Controllers
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
