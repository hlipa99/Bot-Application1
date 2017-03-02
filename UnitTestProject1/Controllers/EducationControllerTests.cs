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

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class EducationControllerTests
    {
        EducationController ec;
        ConversationController cc;
        Mock<IUser> mockUser = new Mock<IUser>();
        Mock<IStudySession> mockStudySession = new Mock<IStudySession>();
        Mock<IQuestion> mockQuestion1 = new Mock<IQuestion>();
        Mock<IQuestion> mockQuestion2 = new Mock<IQuestion>();
        Mock<IQuestion> mockQuestion3 = new Mock<IQuestion>();
        Mock<ISubQuestion> mockSubQuestion = new Mock<ISubQuestion>();
        Mock<DataBaseController> mockDB = new Mock<DataBaseController>();

        [TestInitialize()]
        public void EducationControllerTest()
        {
            mockUser.Setup(x => x.UserName).Returns("יוחאי");
            mockUser.Setup(x => x.UserClass).Returns("יא");
            mockUser.Setup(x => x.UserGender).Returns("musculine");
            mockQuestion1.Setup(x => x.AnswerScore).Returns(100);
            mockQuestion1.Setup(x => x.Category).Returns("לאומיות");
            mockQuestion2.Setup(x => x.AnswerScore).Returns(100);
            mockQuestion3.Setup(x => x.AnswerScore).Returns(100);
            mockStudySession.Setup(x => x.Category).Returns("לאומיות");
            mockStudySession.Setup(x => x.SessionLength).Returns(3);
            mockStudySession.Setup(x => x.QuestionAsked).Returns(new HashSet<IQuestion>());
            mockStudySession.Setup(x => x.CurrentSubQuestion).Returns(mockSubQuestion.Object);
            cc = new ConversationController(mockUser.Object, mockStudySession.Object);
            ec = new EducationController(mockUser.Object, mockStudySession.Object,cc);
            mockDB.Setup(x => x.getAllCategory()).Returns(new string[] { "לאומיות" });
            mockDB.Setup(x => x.getQuestion("לאומיות")).Returns(new IQuestion[] { mockQuestion1.Object });
            mockDB.Setup(x => x.getBotPhrase(It.IsAny<Pkey>(), new string[] { }, new string[] { })).Returns((Pkey key, string[] a, string[] b) => new string[] { Enum.GetName(typeof(Pkey), key) });

            ec.Db = mockDB.Object;
        }

       

        [TestMethod()]
        public void getQuestionTest()
        {
            //good
            mockStudySession.Object.Category = "לאומיות";
            ec = new EducationController(mockUser.Object, mockStudySession.Object,cc);
            Assert.AreEqual(ec.getQuestion(), mockQuestion1.Object);

            //bad
            mockStudySession.Object.Category = "dsdsds";
            ec = new EducationController(mockUser.Object, mockStudySession.Object,cc);
            Assert.AreEqual(ec.getQuestion(), null);
          
            //ugly
            try
            {
                mockStudySession.Object.Category = "לאומיות";
                ec = new EducationController(mockUser.Object, mockStudySession.Object,cc);
                var qs = new HashSet<IQuestion>();
                qs.Add(mockQuestion1.Object);
                mockStudySession.Setup(x => x.QuestionAsked).Returns(qs);
                ec.getQuestion();
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
            mockQuestion4.SetupProperty(x => x.AnswerScore);
            mockSubQuestion.Setup(x => x.answerText).Returns("הגנה אצל יהודים ערבים");

            //good
            Assert.AreEqual(ec.checkAnswer( "הגנה אצל יהודים ערבים").score, 100);

            //bad
            Assert.AreEqual(ec.checkAnswer("הגנה יהודים ").score, 50);

            //ugly
            Assert.AreEqual(ec.checkAnswer("למה התרנגול חצה את הכביש?").score, 0);

        }

    private string EnumVal(Pkey key)
    {
        return Enum.GetName(typeof(Pkey), key);
    }
}
}