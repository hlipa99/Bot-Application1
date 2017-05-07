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
                AnswerFeedback feedback = new AnswerFeedback();
                feedback.answer = answer;
                if (subquestion.answerText.Contains("|"))
                {

             

                        foreach (var ans in subquestion.answerText.Split('|'))
                        {
                            if (ans.Trim() != "")
                            {
                                var systemAnswerWords = Nlp.Analize(ans);
                                var f = matchAnswers(userAnswer, systemAnswerWords, ans);
                                feedback.addFeedback(f);
                            }
                        }

                   var feedbacks = feedback.answersFeedbacks;

                        if(feedbacks.Count == 0)
                        {
                            throw new Exception(subquestion.answerText);
                        }
                        var flags = subquestion.flags.Trim();
                   


                        if (flags == "needAll" || flags == "" || flags == null)
                        {
                          feedback.Need = feedback.answersFeedbacks.Count;
                          feedback.score = (int) feedback.answersFeedbacks.Average(x=>x.score); 
                        }
                        else
                        {
                            int numberInt;

                            var numberStr = flags.Replace("need", "");
                            int.TryParse(numberStr, out numberInt);
                            numberInt = numberInt == 0 ? feedbacks.Count : numberInt;
                            feedback.Need = numberInt;
                            feedbacks.Sort((x, y) => y.score - x.score);
                            feedbacks = feedbacks.GetRange(0, numberInt);
                            foreach (var f in feedbacks)
                            {
                            feedback.score = (int)feedback.answersFeedbacks.Average(x => x.score);
                            }
                            feedback.answersFeedbacks = feedbacks;
                        }

                    } else {
                    var systemAnswer = Nlp.Analize(subquestion.answerText);
                    var f = matchAnswers(userAnswer, systemAnswer);
                    f.answer = subquestion.answerText;
                    feedback.addFeedback(f);
                    feedback.Need = 1;
                }

               
                if (feedback == null) feedback = new AnswerFeedback();



                
                return feedback;
            }else
            {
                return new AnswerFeedback();
            }
        }


        public string[] createFeedBack(AnswerFeedback answerFeedback)
        {
            string[] verbalFeedback = new string[] { };
            //check sub question
          //  studySession.CurrentSubQuestion.AnswerScore = answerFeedback.score;

            var neededFeedback = answerFeedback.answersFeedbacks.OrderByDescending(x => x.score).Take(answerFeedback.Need);
            if (neededFeedback.Where(x => x.score >= 80).Count() >= answerFeedback.Need)
            {
                verbalFeedback = getPhrase(Pkey.goodAnswer);
            }
            else if (neededFeedback.Where(x => x.score <= 80 && x.score >= 20).Count() > 0)
            {
                var entities = neededFeedback.Where(x => x.score > 60).SelectMany(x => x.missingEntitis.Where(z => isSpecialEntity(z.entityType))).Distinct();
                if (neededFeedback.Where(x => x.score >= 60).Count() >= 0)
                {
                    if (entities.Count() > 0)
                    {
                        verbalFeedback = getPhrase(Pkey.goodPartialAnswer);
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.missingAnswerEntity));
                        var EntityIter = entities.Distinct().OrderBy(x => x.entityType).GetEnumerator();
                        EntityIter.MoveNext();
                        verbalFeedback = mergeText(verbalFeedback, EntityIter.Current.entityValue);

                        while (EntityIter.MoveNext())
                        {
                            verbalFeedback = mergeText(verbalFeedback, ", " + EntityIter.Current.entityValue);
                        }
                        verbalFeedback = mergeText(verbalFeedback, ".");
                    }
                    else
                    {
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.goodButNotAllAnswerParts));
                    }
                }
                else
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.partialAnswer));
                }

                var partial = neededFeedback.Where(x => x.score <= 60 && x.score >= 20);
                if (partial.Count() > 0)
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.MyAnswerToQuestion));
                    foreach (var f in partial)
                    {
                        verbalFeedback = mergeText(verbalFeedback, f.answer.Trim());
                    }
                    verbalFeedback = mergeText(verbalFeedback, ".");
                }

                var empty = neededFeedback.Where(x => x.score < 20);
                if (empty.Count() > 0)
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.shouldWrite));
                    foreach (var f in empty)
                    {
                        verbalFeedback = mergeText(verbalFeedback, f.answer.Trim());
                    }
                    verbalFeedback = mergeText(verbalFeedback, ".");
                }

            }

            else
            {
                if (answerFeedback.answer != null && answerFeedback.answer.Split(' ').Length > 2)
                {
                    verbalFeedback = getPhrase(Pkey.wrongAnswer);
                }
                else
                {
                    verbalFeedback = getPhrase(Pkey.notAnAnswer);
                }
                verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.MyAnswerToQuestion));


                if (answerFeedback.answersFeedbacks.Count > 1)
                {
                    var feedbackEnumerator = answerFeedback.answersFeedbacks.GetEnumerator();
                    feedbackEnumerator.MoveNext();
                    verbalFeedback = mergeText(verbalFeedback, feedbackEnumerator.Current.answer);
                    while (feedbackEnumerator.MoveNext())
                    {
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.and));
                        verbalFeedback = mergeText(verbalFeedback, feedbackEnumerator.Current.answer.Trim());
                    }
                    verbalFeedback = mergeText(verbalFeedback, ".");
                }

            }

            var optionalAnswers = answerFeedback.answersFeedbacks.Except(neededFeedback);
            if (optionalAnswers.Any())
            {
                verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.possibleAnswer));
                foreach (var f in optionalAnswers)
                {
                    verbalFeedback = mergeText(verbalFeedback, f.answer.Trim());
                }
                verbalFeedback = mergeText(verbalFeedback, ".");
            }

            return verbalFeedback;

        }

        private string[] getPhrase(Pkey goodAnswer)
        {
            return new string[] { "<p:" + Enum.GetName(typeof(Pkey), goodAnswer) + ">" };
        }

        internal string[] mergeText(string[] v1, string v2)
        {
            var space = v2.Length > 1 ? " " : "";
            return new string[] { mergeText(v1).Trim() + space + v2.Trim() };
        }

        internal string[] mergeText(string[] v1, string[] v2)
        {
            return new string[] { mergeText(v1) + " " +  mergeText(v2) };
        }

        private string mergeText(string[] phraseValue)
        {
            if(phraseValue.Count() > 0)
            {
                var res = phraseValue[0];
                var left = phraseValue.ToList();
                left.RemoveAt(0);
                foreach (var s in left)
                {
                    if (s.Length > 1)
                    {
                        res += " " + s;
                    }
                    else
                    {
                        res +=  s;

                    }
                }
                return res;
            }
            else
            {
                return "";
            }
           
        }
        private bool isSpecialEntity(string entityType)
        {
            return entityType == "personWord" ||
               entityType == "organizationWord" ||
                entityType == "locationWord" ||
                entityType == "conceptWord" ||
                 entityType == "eventWord";
        }


        private AnswerFeedback matchAnswers(List<WorldObject> userAnswer, List<WorldObject> systemAnswerWords, string ans)
        {
                AnswerFeedback f = matchAnswers(userAnswer, systemAnswerWords);
                f.answer = ans;
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
