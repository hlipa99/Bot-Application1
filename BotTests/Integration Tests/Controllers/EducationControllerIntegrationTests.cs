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
             SStudySession .Category = "לאומיות";
            Assert.AreEqual(eduCtrl.getQuestion(),  Question1 );

            //bad
             Assert.AreEqual(eduCtrl.getQuestion(), null);
          
            //ugly
            try
            {
                 eduCtrl.getQuestion();
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.AreEqual(typeof(CategoryOutOfQuestionException), ex.GetType());
            }
    
        }

        [TestMethod()]
        public void checkAnswerIntegrationTest()
        {
           
         
            //good
            foreach(var c in DataBaseController.getInstance().getAllCategory())
            {
                foreach (var q in DataBaseController.getInstance().getQuestion(c))
                {
                    foreach (var sq in q.SubQuestion)
                    {
                        SStudySession.CurrentSubQuestion = sq;
                        eduCtrl = new EducationController(UserMus, SStudySession, ConvCtrl);
                        var feedback = eduCtrl.checkAnswer(sq.answerText);
                        if(feedback.score < 99)
                        {
                             feedback = eduCtrl.checkAnswer(sq.answerText);
                            //debug
                        }
                        Assert.IsTrue(feedback.score >= 99);
                    }
                }
            }

            //doog
            foreach (var c in DataBaseController.getInstance().getAllCategory())
            {
                foreach (var q in DataBaseController.getInstance().getQuestion(c))
                {
                    foreach (var sq in q.SubQuestion)
                    {
                        SStudySession.CurrentSubQuestion = sq;
                        eduCtrl = new EducationController(UserMus, SStudySession, ConvCtrl);
                        Assert.AreEqual(eduCtrl.checkAnswer(sq.answerText.Substring(sq.answerText.Length/2)).score, 100);
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