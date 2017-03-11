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
        public void getQuestionTest()
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
        public void checkAnswerTest()
        {
             ISubQuestion  Question4 = new  SubQuestion();
            eduCtrl.Nlp =  NLPCtrl ;
         
            //good
            Assert.AreEqual(eduCtrl.checkAnswer( "תשובה טובה כוללת את כל הדברים").score, 100);

            //doog
            Assert.AreEqual(eduCtrl.checkAnswer("תשובה טובה אבל לא מושלמת ").score, 50);

            //bad
            Assert.AreEqual(eduCtrl.checkAnswer("תשובה לא נכונה ולא קשורה בשיט ").score, 0);

            //ugly
            Assert.AreEqual(eduCtrl.checkAnswer("").score, 0);

        }

    private string EnumVal(Pkey key)
    {
        return Enum.GetName(typeof(Pkey), key);
    }
}
}