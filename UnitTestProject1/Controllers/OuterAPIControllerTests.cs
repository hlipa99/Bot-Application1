using Bot_Application1.YAndex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.Controllers.Tests
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




            //bad
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("בגין אמר לבן גוריון שהערבים כבשו את העיר העתיקה", ""), "Default Fallback Intent");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi(". קללל ברור,בטח", ""), "Default Fallback Intent");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("מצטער אבל אינני מעוניין", ""), "Default Fallback Intent");



            //good
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("בגין אמר לבן גוריון שהערבים כבשו את העיר העתיקה", "QuestionDialog"), "historyAnswer");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("מה אתה אוהב לאכול", ""), "bot_questions");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("מצטער אבל אינני מעוניין", "yesNoQuestionDialog"), "no");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("קללל ברור בטח", "yesNoQuestionDialog"), "yes");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("ממצב גבר?", "GreetingDialog"), "howAreYou");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("תספר לי משהו מעניין", ""), "intresting");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("תחזור לתפריט הראשי", ""), "stopSession");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("?אתה מכיר בדיחות טובות", ""), "funny");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("להתראות גבר, כל טוב", "farewell"), "goodbye");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("שלום לך, אני יוחאי", "startConv"), "hello");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("וואלה אין לי שמץ", "QuestionDialog"), "dontKnow");
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("טוב די מספיק עם השאלות", "LerningDialog"), "stopSession");

            //ugly
            Assert.AreEqual(outerAPICtrl.getIntentApiAi("", ""), null);
            Assert.AreEqual(outerAPICtrl.getIntentApiAi(null, null), null);

        }

        [TestMethod()]
        public void sendToHebrewMorphAnalizerTest()
        {
            //good
               var res =  outerAPICtrl.sendToHebrewMorphAnalizer("אתמול בחמש אחרי הצהריים הלכתי עם אמא למכולת ובדרך ראינו");
            Assert.AreEqual(res, "[{\"lemma\":\"אתמול\",\"ner\":\"O\",\"text\":\"אתמול\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"adverb\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"CARD5\",\"ner\":\"O\",\"text\":\"בחמש\",\"gender\":\"feminine\",\"number\":\"singular\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"numeral\",\"posType\":\"numeral cardinal\",\"prefixes\":[\"ב\"],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"אחרי\",\"ner\":\"O\",\"text\":\"אחרי\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"preposition\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"צהריים\",\"ner\":\"O\",\"text\":\"הצהריים\",\"gender\":\"masculine\",\"number\":\"plural\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"noun\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":true},{\"lemma\":\"הלכה\",\"ner\":\"O\",\"text\":\"הלכתי\",\"gender\":\"feminine\",\"number\":\"singular\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"noun\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"suffixFunction\":\"possessive\",\"suffixGender\":\"masculine and feminine\",\"suffixNumber\":\"singular\",\"suffixPerson\":\"1\",\"isDefinite\":false},{\"lemma\":\"עם\",\"ner\":\"O\",\"text\":\"עם\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"preposition\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"אמא\",\"ner\":\"O\",\"text\":\"אמא\",\"gender\":\"feminine\",\"number\":\"singular\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"noun\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"מכולת\",\"ner\":\"O\",\"text\":\"למכולת\",\"gender\":\"feminine\",\"number\":\"singular\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"noun\",\"posType\":\"unspecified\",\"prefixes\":[\"ל\"],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"דרך\",\"ner\":\"O\",\"text\":\"ובדרך\",\"gender\":\"feminine\",\"number\":\"singular\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"noun\",\"posType\":\"unspecified\",\"prefixes\":[\"ו\",\"ב\"],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"ראה\",\"ner\":\"O\",\"text\":\"ראינו\",\"gender\":\"masculine and feminine\",\"number\":\"plural\",\"person\":\"1\",\"polarity\":\"unspecified\",\"pos\":\"verb\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"past\",\"isDefinite\":false}]");

            //bad
             res = outerAPICtrl.sendToHebrewMorphAnalizer("dfwwfd כגדךלקדכחכד aslkjdsa");
            Assert.AreEqual(res, "[{\"lemma\":\"dfwwfd\",\"ner\":\"I_LOC\",\"text\":\"dfwwfd\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"foreign\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"כגדךלקדכחכד\",\"ner\":\"O\",\"text\":\"כגדךלקדכחכד\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"properName\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false},{\"lemma\":\"aslkjdsa\",\"ner\":\"I_ORG\",\"text\":\"aslkjdsa\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"foreign\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false}]");


            //ugly
            res = outerAPICtrl.sendToHebrewMorphAnalizer("");
            Assert.AreEqual(res, null);
            res = outerAPICtrl.sendToHebrewMorphAnalizer(null);
            Assert.AreEqual(res, null);

        }



        [TestMethod()]
        public void TranslateTest()
        {
            ControlerTranslate ct = new ControlerTranslate();
            //good
            Assert.AreEqual(ControlerTranslate.Translate("yochai"), "יוחאי");

            //bad
            Assert.AreNotEqual(ControlerTranslate.Translate("יוחאי"), "yochai");

            //ugly
            Assert.AreEqual(ControlerTranslate.Translate(""), "");

        }
    }
}