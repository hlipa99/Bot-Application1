using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLPtest.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.Controllers.Tests
{
    [TestClass()]
    public class OuterAPIControllerTests
    {

        OuterAPIController outerAPICtrl;

        [TestInitialize()]
        public void ConversationControllerTest()
        {
            outerAPICtrl = new OuterAPIController();
        }



        [TestMethod()]
        public void getIntentApiAiTest()
        {

            //good
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("בגין אמר לבן גוריון שהערבים כבשו את העיר העתיקה", "QuestionDialog"), "historyAnswer");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("מה אתה אוהב לאכול", ""), "bot_questions");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("מצטער אבל אינני מעוניין", "yesNoQuestion"), "No");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("קללל ברור בטח", "yesNoQuestion"), "Yes");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("ממצב גבר?", "hello"), "historyAnswer");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("אתה מכיר בדיחות טובות?", ""), "funny");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("תספר לי משהו מעניין", ""), "intresting");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("תחזור לתפריט הראשי", ""), "menu");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("טוב די מספיק עם השאלות", ""), "stopSession");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("וואלה אין לי שמץ", ""), "dontKnow");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("להתראות גבר, כל טוב", ""), "goodbye");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi(" שלום לך, אני יוחאי", ""), "hello");



            //bad
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("בגין אמר לבן גוריון שהערבים כבשו את העיר העתיקה", ""), "Default Fallback Intent");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi(". קללל ברור,בטח", ""), "Default Fallback Intent");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("מצטער אבל אינני מעוניין", ""), "Default Fallback Intent");

            //ugly
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("", ""), "Default Fallback Intent");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi(null, null), "Default Fallback Intent");

        }

        [TestMethod()]
        public void sendToHebrewMorphAnalizerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void correctSpellingTest()
        {
            Assert.Fail();
        }
    }
}