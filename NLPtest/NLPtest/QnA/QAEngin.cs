
using Model.Models;
using NLPtest.Controllers;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.QnA
{
    public class QAEngin
    {
        NLPControler nlp = NLPControler.getInstence();

        public AnswerFeedback matchAnswers(ISubQuestion subquestion, string answer)
        {
            var userAnswer = nlp.Analize(answer, subquestion.questionText);
            var systemAnswer = nlp.Analize(subquestion.answerText);
            return matchAnswers(userAnswer, systemAnswer);
        }


        public AnswerFeedback matchAnswers(List<WorldObject> userAnswer, List<WorldObject> systemAnswer)
        {
            var feedback = new AnswerFeedback();
            var systemEntitis = systemAnswer.Where(x => x is NounObject);
            var userEntitis = userAnswer.Where(x => x is NounObject);

            if (systemEntitis.Count() != 0)
            {
                foreach (var se in systemEntitis)
                {
                    var found = false;
                    foreach (var ue in userEntitis)
                    {

                        if (se.GetType() == ue.GetType())
                        {
                            if (se.Word == ue.Word && se.Negat == ue.Negat)
                            {
                                feedback.score += 100 / systemEntitis.Count();
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        feedback.missingEntitis.Add(se.Word);
                    }

                }
            }
            else
            {

            }

            return feedback;
        }
    }
}
