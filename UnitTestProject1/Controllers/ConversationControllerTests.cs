using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_Application1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Model.dataBase;
using NLPtest.Models;
using Model;
using Model.Models;

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class ConversationControllerTests
    {
        ConversationController cv;
        Mock<IUser> mockUser = new Mock<IUser>();
        Mock<IStudySession> mockStudySession = new Mock<IStudySession>();
        Mock<IQuestion> mockQuestion1 = new Mock<IQuestion>();
        Mock<IQuestion> mockQuestion2 = new Mock<IQuestion>();
        Mock<IQuestion> mockQuestion3 = new Mock<IQuestion>();
        Mock<DataBaseController> mockDB = new Mock<DataBaseController>();

        [TestInitialize()]
        public void ConversationControllerTest()
        {
            mockUser.Setup(x => x.UserName).Returns("יוחאי");
            mockUser.Setup(x => x.UserClass).Returns("יא");
            mockUser.Setup(x => x.UserGender).Returns("musculine");
            mockQuestion1.Setup(x => x.AnswerScore).Returns(100);
            mockQuestion2.Setup(x => x.AnswerScore).Returns(100);
            mockQuestion3.Setup(x => x.AnswerScore).Returns(100);
            mockStudySession.Setup(x => x.Category).Returns("לאומיות");
            mockStudySession.Setup(x => x.SessionLength).Returns(3);

            cv = new ConversationController(mockUser.Object,mockStudySession.Object);

            mockDB.Setup(x => x.getAllCategory()).Returns(new string[] { "לאומיות"});
            mockDB.Setup(x => x.getQuestion("לאומיות")).Returns(new IQuestion[] { mockQuestion1.Object });
            mockDB.Setup(x=> x.getBotPhrase(It.IsAny<Pkey>(), new string[] { }, new string[] { })).Returns((Pkey key,string [] a,string[] b) => new string[] { Enum.GetName(typeof(Pkey), key) });

            cv.Db = mockDB.Object;
        }

        [TestMethod()]
        public void FindMatchFromOptionsTest()
        {
            //good
           Assert.AreEqual(cv.FindMatchFromOptions("3 אופציה", new string[] { "1", "2", "3" }),"3");
            //bad
            Assert.AreEqual(cv.FindMatchFromOptions("dsds אופציה", new string[] { "1", "2", "3" }), null);
            //sad
            Assert.AreEqual(cv.FindMatchFromOptions("", new string[] { "1", "2", "3" }), null);
        }

        [TestMethod()]
        public void isStopSessionTest()
        {
            //good
            Assert.IsTrue(cv.isStopSession("מספיק"));
            //bad
            Assert.IsFalse(cv.isStopSession("תמשיך"));
            //sad
            Assert.IsFalse(cv.isStopSession(""));
        }



        [TestMethod()]
        public void endOfSessionTest()
        {
            var hs = new HashSet<IQuestion>();
            hs.Add(mockQuestion1.Object);
            hs.Add(mockQuestion2.Object);
            hs.Add(mockQuestion3.Object);

            mockStudySession.Setup(x => x.QuestionAsked).Returns(hs);
            //good
            Assert.AreEqual(cv.endOfSession()[0], EnumVal(Pkey.goodSessionEnd));


            mockQuestion1.Setup(x => x.AnswerScore).Returns(30);
            mockQuestion2.Setup(x => x.AnswerScore).Returns(30);
            mockQuestion3.Setup(x => x.AnswerScore).Returns(30);

            hs = new HashSet<IQuestion>();
            hs.Add(mockQuestion1.Object);
            hs.Add(mockQuestion2.Object);
            hs.Add(mockQuestion3.Object);
            mockStudySession.Setup(x => x.QuestionAsked).Returns(hs);

            Assert.AreEqual(cv.endOfSession()[0], EnumVal(Pkey.badSessionEnd));

            //bad
            mockStudySession.Setup(x => x.QuestionAsked).Returns(new HashSet<IQuestion>());
            Assert.AreEqual(cv.endOfSession()[0], EnumVal(Pkey.earlyDiparture));



            //sad
            mockQuestion1.Setup(x => x.AnswerScore).Returns(-99999);

            hs = new HashSet<IQuestion>();
            hs.Add(mockQuestion1.Object);
            hs.Add(mockQuestion2.Object);
            hs.Add(mockQuestion3.Object);
            mockStudySession.Setup(x => x.QuestionAsked).Returns(hs);

            Assert.AreEqual(cv.endOfSession()[0], EnumVal(Pkey.badSessionEnd));
        }

        private string EnumVal(Pkey key)
        {
            return Enum.GetName(typeof(Pkey), key);
        }

        [TestMethod()]
        public void getNumTest()
        {
            //good
            Assert.AreEqual(cv.getNum("50"), 50);
          //  Assert.AreEqual(cv.getNum("מאה"), 100);


            //bad
            Assert.AreEqual(cv.getNum("גדגדגדגד"), -1);

            //sad
            Assert.AreEqual(cv.getNum(int.MinValue.ToString()), -1);
        }

        [TestMethod()]
        public void getNameTest()
        {
            //good
          //  Assert.AreEqual(cv.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(cv.getName("יוחאי"), "יוחאי");


            //bad
            Assert.AreEqual(cv.getName("אין לי שם"), null);

            //sad
            Assert.AreEqual(cv.getName("asassasaas"), null);
        }

        [TestMethod()]
        public void getGenderValueTest()
        {
           
            //good
            //  Assert.AreEqual(cv.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(cv.getGenderValue("בן"), "masculine");
            Assert.AreEqual(cv.getGenderValue("בת"), "feminine");
            //Assert.AreEqual(cv.getGenderValue("בחור"), "masculine");
            //Assert.AreEqual(cv.getGenderValue("בחורה"), "feminine");
            //bad

            Assert.AreEqual(cv.getGenderValue("אני א-מיני"), null);

            //sad
            Assert.AreEqual(cv.getGenderValue("asassasaas"), null);
        }

        [TestMethod()]
        public void getClassTest()
        {
            //good
            //  Assert.AreEqual(cv.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(cv.getClass("יא"), "יא");
            //Assert.AreEqual(cv.getClass("שמינית"), "יב");
            //bad

            Assert.AreEqual(cv.getClass("אני באקסטרני"), null);

            //sad
            Assert.AreEqual(cv.getClass("asassasaas"), null);
        }

        //[TestMethod()]  //NOT_IMPLAMENTED_IN_THIS_VERSION
        //public void getGeneralFeelingTest()
        //{
        //    //good
        //    //  Assert.AreEqual(cv.getName("קוראים לי יוחאי"), "יוחאי");
        //    Assert.AreEqual(cv.getGeneralFeeling("סבבה"), "good");
        //    Assert.AreEqual(cv.getGeneralFeeling("באסה"), "bad");
        //    //Assert.AreEqual(cv.getClass("שמינית"), "יב");
        //    //bad

        //    Assert.AreEqual(cv.getGeneralFeeling("גו גו פאוור רנג'ר"), null);

        //    //sad
        //    Assert.AreEqual(cv.getGeneralFeeling("asassasaas"), null);
        //}


    }
}