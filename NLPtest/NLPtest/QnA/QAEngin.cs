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
                AnswerFeedback feedback = null ;

                if (subquestion.answerText.Contains("|"))
                {

             
                    var feedbacks = new List<AnswerFeedback>();

                        foreach (var ans in subquestion.answerText.Split('|'))
                        {
                            if (ans.Trim() != "")
                            {
                                var systemAnswerWords = Nlp.Analize(ans);
                                var f = matchAnswers(userAnswer, systemAnswerWords, ans);
                                feedbacks.Add(f);
                            }
                        }

                        if(feedbacks.Count == 0)
                        {
                            throw new Exception(subquestion.answerText);
                        }
                        var flags = subquestion.flags.Trim();
                        if (flags == "needAll" || flags == "" || flags == null)
                        {
                            foreach (var f in feedbacks)
                            {
                                f.merge(feedback);
                                feedback = f;
                            }
                        }
                        else
                        {
                            int numberInt;

                            var numberStr = flags.Replace("need", "");
                            int.TryParse(numberStr, out numberInt);
                            numberInt = numberInt == 0 ? feedbacks.Count : numberInt;

                            feedbacks.Sort((x, y) => y.score - x.score);
                            feedbacks = feedbacks.GetRange(0, numberInt);
                            foreach (var f in feedbacks)
                            {
                                f.merge(feedback);
                                feedback = f;
                            }
                        }

                    } else {
                    var systemAnswer = Nlp.Analize(subquestion.answerText);
                    feedback = matchAnswers(userAnswer, systemAnswer);
                    if(feedback.score < 75)
                    {
                        feedback.missingAnswers.Add(subquestion.answerText);
                    }
                    else
                    {
                        feedback.foundAnswers.Add(subquestion.answerText);
                    }
                }

                Logger.addAnswerOutput(subquestion.answerText, answer, feedback);
                if (feedback == null) feedback = new AnswerFeedback();

                feedback.answer = answer;
                
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
                }else
                {
                    f.foundAnswers.Add(ans);
                }
                return f;
            
        }

        public AnswerFeedback matchAnswers(List<WorldObject> userAnswer, List<WorldObject> systemAnswer)
        {
         

            if (userAnswer != null && systemAnswer != null)
            {
                var userEntities = userAnswer.Distinct();
                var systemEntities = systemAnswer.Distinct();
                var systemEntitisFiltered = systemAnswer.Where(o => o is EntityObject);
                if (systemEntitisFiltered.Count() > 1)
                {
                    systemEntities = systemEntitisFiltered;
                }
                var feedback = new AnswerFeedback();
              
                if (systemAnswer.Count() != 0)
                {
                    foreach (var se in systemEntities)
                    {
                        var found = false;


                        foreach (var ue in userEntities)
                        {
                            if (se.GetType() == ue.GetType())
                            {

                                var a = se.Word.Equals(ue.Word);
                                var b = se.Negat == ue.Negat;


                                if (se.Word.Equals(ue.Word) && se.Negat == ue.Negat)
                                {
                                    Double d = 100.0 / systemEntities.Count();

                                    feedback.score += (int)Math.Ceiling(d);

                                    found = true;
                                    break;
                                }
                           
                            }
                        }

                        if (!found)
                        {
                            feedback.missingEntitis.Add(se.Entity);
                        }else
                        {
                            feedback.foundEntitis.Add(se.Entity);
                        }

                    }

                }
                else
                {
                    feedback.score = 100;

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
