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
                var questionAnlize = new List<WorldObject>();
                if (subquestion.questionText != null)
                {
                    questionAnlize = Nlp.Analize(subquestion.questionText); //TODO : fix double computing
                }
                var userAnswer = Nlp.Analize(answer, subquestion.questionText);
                 AnswerFeedback feedback = new AnswerFeedback();
                 feedback.answer = answer;
                 if (subquestion.answerText.Contains("|"))
                 {
 
              
 
                         foreach (var ans in subquestion.answerText.Split('|'))
                         {
                             if (ans.Trim() != "")
                             {
                                 var systemAnswerWords = Nlp.Analize(ans).Except(questionAnlize);
                                 var f = matchAnswers(userAnswer, systemAnswerWords.ToList(), ans);
                                 feedback.addFeedback(f);
                             }
                         }
 
                    var feedbacks = feedback.answersFeedbacks;
 
                         if(feedbacks.Count == 0)
                         {
                             throw new Exception(subquestion.answerText);
                         }

                         var flags = subquestion.flags;
                    
 
 
                         if (flags == null || flags.Trim() == "" || flags.Trim() == "needAll")
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
                             feedback.score = (int)feedbacks.Average(x => x.score);
                             }
                             //feedback.answersFeedbacks = feedbacks;
                         }
 
                     } else {
                     var systemAnswer = Nlp.Analize(subquestion.answerText).Except(questionAnlize);
                     var f = matchAnswers(userAnswer, systemAnswer.ToList());
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
