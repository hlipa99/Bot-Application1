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

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class EducationControllerTests
    {
        EducationController ec;
        Mock<IUser> mockUser = new Mock<IUser>();
        Mock<IStudySession> mockStudySession = new Mock<IStudySession>();
        Mock<IQuestion> mockQuestion1 = new Mock<IQuestion>();
        Mock<IQuestion> mockQuestion2 = new Mock<IQuestion>();
        Mock<IQuestion> mockQuestion3 = new Mock<IQuestion>();
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
            ec = new EducationController(mockUser.Object, mockStudySession.Object);

            mockDB.Setup(x => x.getAllCategory()).Returns(new string[] { "לאומיות" });
            mockDB.Setup(x => x.getQuestion("לאומיות")).Returns(new IQuestion[] { mockQuestion1.Object });
            mockDB.Setup(x => x.getBotPhrase(It.IsAny<Pkey>(), new string[] { }, new string[] { })).Returns((Pkey key, string[] a, string[] b) => new string[] { Enum.GetName(typeof(Pkey), key) });

            ec.Db = mockDB.Object;
        }

       

        [TestMethod()]
        public void getQuestionTest()
        {
            //good
            Assert.AreEqual(ec.getQuestion("לאומיות", null, mockStudySession.Object), mockQuestion1.Object);

            //bad
            Assert.AreEqual(ec.getQuestion("dsdsds", null, mockStudySession.Object), null);
          
            //ugly
            try
            {
                var qs = new HashSet<IQuestion>();
                qs.Add(mockQuestion1.Object);
                mockStudySession.Setup(x => x.QuestionAsked).Returns(qs);
                ec.getQuestion("לאומיות", null, mockStudySession.Object);
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
            Mock<IQuestion> mockQuestion4 = new Mock<IQuestion>();
            mockQuestion4.SetupProperty(x => x.AnswerScore);
            //good
            Assert.AreEqual(ec.checkAnswer(mockQuestion4.Object, "תשובה טובה מאד מאד, אפילו מצויינת").AnswerScore, 100);

            //bad
            Assert.AreEqual(ec.checkAnswer(mockQuestion4.Object, "תשובה חלקית אבל בסדר").AnswerScore, 56);

            //ugly
            Assert.AreEqual(ec.checkAnswer(mockQuestion4.Object, "sdsdfsdfs").AnswerScore, 0);

        }

    private string EnumVal(Pkey key)
    {
        return Enum.GetName(typeof(Pkey), key);
    }
}
}