using Model.Models;
using NLP.NLP;
using NLP.QnA;
using NLPtest.QnA;
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
        MessageComposer messageComposer = new MessageComposer();
        StatisticAgent statisticAgent = new StatisticAgent();
        public AnswerFeedback matchAnswers(ISubQuestion subquestion, string answer)
        {
            return qna.matchAnswers(subquestion, answer);
        }

        public string[] createFeedBack(AnswerFeedback answerFeedback)
        {
            return messageComposer.createFeedBack(answerFeedback);

        }

        public string[] getUserStatistics(string userID)
        {
            var userStat = statisticAgent.createUserLerningStatistics(userID);
            return messageComposer.CreateUserStatMessage(userStat);
        }
    }
}
