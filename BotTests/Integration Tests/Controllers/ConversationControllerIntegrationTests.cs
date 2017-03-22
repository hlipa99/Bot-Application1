using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_Application1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Model.dataBase;
using NLP.Models;
using Model;
using Model.Models;
using UnitTestProject1;
using BotTests;

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class ConversationControllerIntegrationTests : ObjectTestBase
    {

        ConversationController convCtrl;

        [TestInitialize()]
        public void ConversationControllerTest()
        {
            initializeObject();
            //convCtrl = new ConversationController( UserFem , StudySession );
        }

        //[TestMethod()]
        //public void FindMatchFromOptionsTest()
        //{
        //    //good
        //   Assert.AreEqual(convCtrl.FindMatchFromOptions("3 אופציה", new string[] { "1", "2", "3" }),"3");
        //    //bad
        //    Assert.AreEqual(convCtrl.FindMatchFromOptions("dsds אופציה", new string[] { "1", "2", "3" }), null);
        //    //sad
        //    Assert.AreEqual(convCtrl.FindMatchFromOptions("", new string[] { "1", "2", "3" }), null);
        //}

        //[TestMethod()]
        //public void isStopSessionTest()
        //{
        //    //good
        //    Assert.IsTrue(convCtrl.isStopSession("מספיק"));
        //    //bad
        //    Assert.IsFalse(convCtrl.isStopSession("תמשיך"));
        //    //sad
        //    Assert.IsFalse(convCtrl.isStopSession(""));
        //}

        [TestMethod()]
        public void endOfSessionIntegrationTest()
        {
            var ss = new StudySession();
            Question1.AnswerScore = 100;
            Question2.AnswerScore = 90;
            Question3.AnswerScore = 80;

            ss.QuestionAsked.Add(Question1); 
            ss.QuestionAsked.Add(Question2);
            ss.QuestionAsked.Add(Question3);

            convCtrl = new ConversationController(new User(),ss );

 
            //good

            AssertNLP.contains(convCtrl.endOfSession(),DBbotPhrase(Pkey.goodSessionEnd));


                       ss = new StudySession();
            Question1.AnswerScore = 45;
            Question2.AnswerScore = 34;
            Question3.AnswerScore = 12;

            ss.QuestionAsked.Add(Question1);
            ss.QuestionAsked.Add(Question2);
            ss.QuestionAsked.Add(Question3);

            convCtrl = new ConversationController(new User(), ss);

            //bad
            AssertNLP.contains(convCtrl.endOfSession(), DBbotPhrase(Pkey.badSessionEnd));


            ss = new StudySession();
            Question1.AnswerScore = 45;
            Question2.AnswerScore = 34;


            ss.QuestionAsked.Add(Question1);
            ss.QuestionAsked.Add(Question2);


            convCtrl = new ConversationController(new User(), ss);
            
            AssertNLP.contains(convCtrl.endOfSession(), DBbotPhrase(Pkey.earlyDiparture));




            ss = new StudySession();


            ss.QuestionAsked.Add(Question1);
            ss.QuestionAsked.Add(Question2);
            ss.QuestionAsked.Add(Question3);

            convCtrl = new ConversationController(new User(), ss);//sad


            AssertNLP.contains(convCtrl.endOfSession(), DBbotPhrase(Pkey.goodSessionEnd));

        }

      

        [TestMethod()]
        public void getNumIntegrationTest()
        {
            //good
            Assert.AreEqual(convCtrl.getNum("50"), 50);
          //  Assert.AreEqual(convCtrl.getNum("מאה"), 100);


            //bad
            Assert.AreEqual(convCtrl.getNum("גדגדגדגד"), -1);

            //sad
            Assert.AreEqual(convCtrl.getNum(int.MinValue.ToString()), -1);
        }

        [TestMethod()]
        public void getNameIntegrationTest()
        {
            //good
          //  Assert.AreEqual(convCtrl.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(convCtrl.getName("יוחאי"), "יוחאי");


            //bad
            Assert.AreEqual(convCtrl.getName("אין לי שם"), null);

            //sad
            Assert.AreEqual(convCtrl.getName("asassasaas"), null);
        }

        [TestMethod()]
        public void getGenderValueIntegrationTest()
        {
           
            //good
            //  Assert.AreEqual(convCtrl.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(convCtrl.getGenderValue("בן"), "masculine");
            Assert.AreEqual(convCtrl.getGenderValue("בת"), "feminine");
            //Assert.AreEqual(convCtrl.getGenderValue("בחור"), "masculine");
            //Assert.AreEqual(convCtrl.getGenderValue("בחורה"), "feminine");
            //bad

            Assert.AreEqual(convCtrl.getGenderValue("אני א-מיני"), null);

            //sad
            Assert.AreEqual(convCtrl.getGenderValue("asassasaas"), null);
        }

        [TestMethod()]
        public void getClassIntegrationTest()
        {
            //good
            //  Assert.AreEqual(convCtrl.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(convCtrl.getClass("יא"), "יא");
            //Assert.AreEqual(convCtrl.getClass("שמינית"), "יב");
            //bad

            Assert.AreEqual(convCtrl.getClass("אני באקסטרני"), null);

            //sad
            Assert.AreEqual(convCtrl.getClass("asassasaas"), null);
        }

        //[TestMethod()]  //NOT_IMPLAMENTED_IN_THIS_VERSION
        //public void getGeneralFeelingTest()
        //{
        //    //good
        //    //  Assert.AreEqual(convCtrl.getName("קוראים לי יוחאי"), "יוחאי");
        //    Assert.AreEqual(convCtrl.getGeneralFeeling("סבבה"), "good");
        //    Assert.AreEqual(convCtrl.getGeneralFeeling("באסה"), "bad");
        //    //Assert.AreEqual(convCtrl.getClass("שמינית"), "יב");
        //    //bad

        //    Assert.AreEqual(convCtrl.getGeneralFeeling("גו גו פאוור רנג'ר"), null);

        //    //sad
        //    Assert.AreEqual(convCtrl.getGeneralFeeling("asassasaas"), null);
        //}


    }
}