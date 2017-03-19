using Model;
using Model.Models;
using NLP.Controllers;
using NLP.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.QnA
{
    public class QAEngin
    {
        NLPControler nlp = new NLPControler();

        public NLPControler Nlp
        {
            get
            {
                return nlp;
            }

            set
            {
                nlp = value;
            }
        }

        public AnswerFeedback matchAnswers(ISubQuestion subquestion, string answer)
        {
            if (subquestion != null && answer != null)
            {
                var userAnswer = Nlp.Analize(answer, subquestion.questionText);
                AnswerFeedback feedback = new AnswerFeedback() ;
                if (subquestion.answerText.Contains("|"))
                {

                    AnswerFeedback f = new AnswerFeedback(); //helpers
                    AnswerFeedback f1 = new AnswerFeedback();
                    AnswerFeedback f2 = new AnswerFeedback();

                    foreach (var ans in subquestion.answerText.Split('|'))
                    {
                        var systemAnswerWords = Nlp.Analize(ans);
                        if (ans.Trim() != "" && subquestion.flags != null)
                        {
                            switch (subquestion.flags.Trim())
                            {
                                default:
                                case ("needAll"):

                                    f = matchAnswers(userAnswer, systemAnswerWords, ans);
                                    f.merge(feedback);
                                    feedback = f;
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
                                //default:
                                //    throw new DBDataException();
                                //    break;
                            }

                        }
                    }

                }
                else
                {
                    var systemAnswer = Nlp.Analize(subquestion.answerText);
                    feedback = matchAnswers(userAnswer, systemAnswer);
                }

                Logger.addAnswerOutput(subquestion.answerText, answer, feedback);

                return feedback;
            }else
            {
                return new AnswerFeedback();
            }
        }


        private AnswerFeedback matchAnswers(List<WorldObject> userAnswer, List<WorldObject> systemAnswerWords, string ans)
        {
           
                AnswerFeedback f = matchAnswers(userAnswer, systemAnswerWords);
                if (f.missingEntitis.Count == systemAnswerWords.Count)
                {
                    f.missingAnswers.Add(ans);
                }
                return f;
            
        }

        public AnswerFeedback matchAnswers(List<WorldObject> userAnswer, List<WorldObject> systemAnswer)
        {
            if (userAnswer != null && systemAnswer != null)
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
                                    feedback.score += (100 / systemEntitis.Count())*2;
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
            else if (userAnswer == null && systemAnswer != null)
            {
                var fe = new AnswerFeedback();
                fe.score = 0;
                return fe;
            }else
            {
                var fe = new AnswerFeedback();
                fe.score = 100;
                return fe;
            }
        }
    }
}
