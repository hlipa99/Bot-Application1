using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_Application1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Bot_Application1.Exceptions;
using System.Threading.Tasks;
using Moq;
using Model.Models;
using NLPtest.Models;
using Model;
using Model.dataBase;
using UnitTestProject1;

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class EducationControllerTests : MockObjectTestBase
    {

        EducationController eduCtrl;
        [TestInitialize()]
        public void EducationControllerTest()
        {
            initializeMocksObject();
            eduCtrl = new EducationController(mockUserMus.Object, mockStudySession.Object, mockConvCtrl.Object);
        }

       

        [TestMethod()]
        public void getQuestionTest()
        {
            //good
            mockStudySession.Object.Category = "לאומיות";
            Assert.AreEqual(eduCtrl.getQuestion(), mockQuestion1.Object);

            //bad
            mockStudySession.Setup(x => x.Category).Returns("נושא שלא קיים");
            Assert.AreEqual(eduCtrl.getQuestion(), null);
          
            //ugly
            try
            {
                mockStudySession.Setup(x => x.Category).Returns("לאומיות");
                mockStudySession.Setup(x => x.QuestionAsked).Returns(mockDB.Object.getQuestion("לאומיות"));
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
            Mock<ISubQuestion> mockQuestion4 = new Mock<ISubQuestion>();
            eduCtrl.Nlp = mockNLPCtrl.Object;
            mockQuestion4.SetupProperty(x => x.AnswerScore);
            mockSubQqestion1.Setup(x => x.answerText).Returns("תשובה טובה כוללת את כל הדברים");

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