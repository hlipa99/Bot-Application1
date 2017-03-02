
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
            AnswerFeedback feedback = new AnswerFeedback();

            if (subquestion.answerText.Contains("|")){

                AnswerFeedback f = null; //helpers
                AnswerFeedback f1 = null;
                AnswerFeedback f2 = null;

                foreach (var ans in subquestion.answerText.Split('|'))
                {
                    var systemAnswerWords = nlp.Analize(ans);
                    if (ans.Trim() != "")
                    {
                        switch (subquestion.flags.Trim())
                        {
                            case ("needAll"):

                                f = matchAnswers(userAnswer, systemAnswerWords, ans);
                                feedback.merge(f);
                                break;

                            case ("need1"):
                                f = matchAnswers(userAnswer, systemAnswerWords, ans);
                                if (f.score > feedback.score)
                                {
                                    feedback = f;
                                }

                                break;
                            case ("need2"):
                                f = matchAnswers(userAnswer, systemAnswerWords, ans);
                                if (f.score > f1.score || f.score > f2.score)
                                {
                                    if (f1.score > f2.score)
                                    {
                                        f2 = f;
                                    }
                                    else
                                    {
                                        f1 = f;
                                    }
                                }

                                f1.merge(f2);
                                feedback = f1;
                                break;
                            default:
                                throw new DBDataException();
                                break;
                        }

                    }
                }

            }else
            {
                var systemAnswer = nlp.Analize(subquestion.answerText);
                feedback = matchAnswers(userAnswer, systemAnswer);
            }
            return feedback;
        }


        private AnswerFeedback matchAnswers(List<WorldObject> userAnswer, List<WorldObject> systemAnswerWords, string ans)
        {
            AnswerFeedback f = matchAnswers(userAnswer, systemAnswerWords);
            if (f.missingEntitis.Where(x => x.entityType != "locationWord").Count() > 0)
            {
                f.missingAnswers.Add(ans);
            }
            return f;
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
                        feedback.missingEntitis.Add(se.Entity);
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
