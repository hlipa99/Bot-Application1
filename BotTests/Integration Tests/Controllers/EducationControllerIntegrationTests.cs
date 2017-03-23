using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_Application1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Bot_Application1.Exceptions;
using System.Threading.Tasks;
using Moq;
using Model.Models;
using NLP.Models;
using Model;
using Model.dataBase;
using UnitTestProject1;

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class EducationControllerIntegrationTests :  ObjectTestBase
    {

        EducationController eduCtrl;
        [TestInitialize()]
        public void EducationControllerTest()
        {
            initializeObject();
            eduCtrl = new EducationController( UserMus ,  SStudySession ,  ConvCtrl );
        }

       

        [TestMethod()]
        public void getQuestionIntegrationTest()
        {
            //good
            var counter = 0;
            foreach (var c in DB.getAllCategory())
            {
                counter = 0;
                SStudySession.Category = c;
                    IQuestion q;
                    while(true)
                    {
                    
                    try
                    {
                        eduCtrl.getNextQuestion();
                    }
                    catch (Exception ex)
                    {
                        //ugly
                        Assert.AreEqual(typeof(CategoryOutOfQuestionException), ex.GetType());
                        break;
                    }

                    Assert.IsTrue(counter++ < DB.getQuestion(c).Count());

                    q = SStudySession.CurrentQuestion;
                        Assert.AreEqual(q.Category, c);

                        //bad
                        Assert.IsFalse(SStudySession.QuestionAsked.Contains(q), null);


                        foreach(var sq in q.SubQuestion)
                        {
                        eduCtrl.getNextSubQuestion();
                        Assert.AreEqual(SStudySession.CurrentSubQuestion, sq);
                          

                        }

                    }

           

            }
        }

        [TestMethod()]
        public void checkAnswerIntegrationTest()
        {
           
         
            //good
            foreach(var c in DB.getAllCategory())
            {
                foreach (var q in DB.getQuestion(c))
                {
                    foreach (var sq in q.SubQuestion)
                    {
                        SStudySession.CurrentSubQuestion = sq;
                        eduCtrl = new EducationController(UserMus, SStudySession, ConvCtrl);
                        var feedback = eduCtrl.checkAnswer(sq.answerText);
                        if(feedback.score < 95)
                        {
                             feedback = eduCtrl.checkAnswer(sq.answerText);
                            //debug
                   //         feedback = eduCtrl.checkAnswer(sq.answerText);

                       //     feedback = eduCtrl.checkAnswer(sq.answerText);
                        }
                        Assert.IsTrue(feedback.score >= 95);
                    }
                }
            }

            //bad
            Assert.AreEqual(eduCtrl.checkAnswer("תשובה לא נכונה ולא קשורה בשיט ").score, 0);

            //ugly
            Assert.AreEqual(eduCtrl.checkAnswer("").score, 0);
            Assert.AreEqual(eduCtrl.checkAnswer(null).score, 0);

        }

    private string EnumVal(Pkey key)
    {
        return Enum.GetName(typeof(Pkey), key);
    }
}
}