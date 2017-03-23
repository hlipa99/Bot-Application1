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

       
        [TestInitialize()]
        public void ConversationControllerTest()
        {
            initializeObject();
            //ConvCtrl = new ConversationController( UserFem , StudySession );
        }

        //[TestMethod()]
        //public void FindMatchFromOptionsTest()
        //{
        //    //good
        //   Assert.AreEqual(ConvCtrl.FindMatchFromOptions("3 אופציה", new string[] { "1", "2", "3" }),"3");
        //    //bad
        //    Assert.AreEqual(ConvCtrl.FindMatchFromOptions("dsds אופציה", new string[] { "1", "2", "3" }), null);
        //    //sad
        //    Assert.AreEqual(ConvCtrl.FindMatchFromOptions("", new string[] { "1", "2", "3" }), null);
        //}

        //[TestMethod()]
        //public void isStopSessionTest()
        //{
        //    //good
        //    Assert.IsTrue(ConvCtrl.isStopSession("מספיק"));
        //    //bad
        //    Assert.IsFalse(ConvCtrl.isStopSession("תמשיך"));
        //    //sad
        //    Assert.IsFalse(ConvCtrl.isStopSession(""));
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

            ConvCtrl = new ConversationController(new User(),ss );

 
            //good

            AssertNLP.contains(ConvCtrl.endOfSession(),DBbotPhrase(Pkey.goodSessionEnd));


                       ss = new StudySession();
            Question1.AnswerScore = 45;
            Question2.AnswerScore = 34;
            Question3.AnswerScore = 12;

            ss.QuestionAsked.Add(Question1);
            ss.QuestionAsked.Add(Question2);
            ss.QuestionAsked.Add(Question3);

            ConvCtrl = new ConversationController(new User(), ss);

            //bad
            AssertNLP.contains(ConvCtrl.endOfSession(), DBbotPhrase(Pkey.badSessionEnd));


            ss = new StudySession();
            Question1.AnswerScore = 45;
            Question2.AnswerScore = 34;


            ss.QuestionAsked.Add(Question1);
            ss.QuestionAsked.Add(Question2);


            ConvCtrl = new ConversationController(new User(), ss);
            
            AssertNLP.contains(ConvCtrl.endOfSession(), DBbotPhrase(Pkey.earlyDiparture));




            ss = new StudySession();


            ss.QuestionAsked.Add(Question1);
            ss.QuestionAsked.Add(Question2);
            ss.QuestionAsked.Add(Question3);

            ConvCtrl = new ConversationController(new User(), ss);//sad


            AssertNLP.contains(ConvCtrl.endOfSession(), DBbotPhrase(Pkey.goodSessionEnd));

        }

      

        [TestMethod()]
        public void getNumIntegrationTest()
        {
            //good
            Assert.AreEqual(ConvCtrl.getNum("50"), 50);
          //  Assert.AreEqual(ConvCtrl.getNum("מאה"), 100);


            //bad
            Assert.AreEqual(ConvCtrl.getNum("גדגדגדגד"), -1);

            //sad
            Assert.AreEqual(ConvCtrl.getNum(int.MinValue.ToString()), -1);
        }

        [TestMethod()]
        public void getNameIntegrationTest()
        {
            //good
          //  Assert.AreEqual(ConvCtrl.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(ConvCtrl.getName("יוחאי"), "יוחאי");


            //bad
            Assert.AreEqual(ConvCtrl.getName("אין לי שם"), null);

            //sad
            Assert.AreEqual(ConvCtrl.getName(""), null);
            Assert.AreEqual(ConvCtrl.getName(null), null);
        }

        [TestMethod()]
        public void getGenderValueIntegrationTest()
        {
           
            //good
            //  Assert.AreEqual(ConvCtrl.getName("קוראים לי יוחאי"), "יוחאי");
            Assert.AreEqual(ConvCtrl.getGenderValue("בן"), "masculine");
            Assert.AreEqual(ConvCtrl.getGenderValue("בת"), "feminine");
            //Assert.AreEqual(ConvCtrl.getGenderValue("בחור"), "masculine");
            //Assert.AreEqual(ConvCtrl.getGenderValue("בחורה"), "feminine");
            //bad

            Assert.AreEqual(ConvCtrl.getGenderValue("אני א-מיני"), null);

            //sad
            Assert.AreEqual(ConvCtrl.getGenderValue("asassasaas"), null);
        }

        [TestMethod()]
        public void getClassIntegrationTest()
        {
            //good
            //  Assert.AreEqual(ConvCtrl.getName("קוראים לי יוחאי"), "יוחאי");


            Assert.AreEqual(ConvCtrl.getClass("יא"), "יא");
            //Assert.AreEqual(ConvCtrl.getClass("שמינית"), "יב");
            //bad

            Assert.AreEqual(ConvCtrl.getClass("אני באקסטרני"), null);

            //sad
            Assert.AreEqual(ConvCtrl.getClass("asassasaas"), null);
        }

        //[TestMethod()]  //NOT_IMPLAMENTED_IN_THIS_VERSION
        //public void getGeneralFeelingTest()
        //{
        //    //good
        //    //  Assert.AreEqual(ConvCtrl.getName("קוראים לי יוחאי"), "יוחאי");
        //    Assert.AreEqual(ConvCtrl.getGeneralFeeling("סבבה"), "good");
        //    Assert.AreEqual(ConvCtrl.getGeneralFeeling("באסה"), "bad");
        //    //Assert.AreEqual(ConvCtrl.getClass("שמינית"), "יב");
        //    //bad

        //    Assert.AreEqual(ConvCtrl.getGeneralFeeling("גו גו פאוור רנג'ר"), null);

        //    //sad
        //    Assert.AreEqual(ConvCtrl.getGeneralFeeling("asassasaas"), null);
        //}


    }
}