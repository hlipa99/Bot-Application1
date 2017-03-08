using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLPtest.HebWords;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.HebWords.Tests
{
    [TestClass()]
    public class WordObjectTests
    {

        WordObject word;
        [TestInitialize]
        public void Init()
        {
            word = new WordObject();
        }



        [TestMethod()]
        public void WordObjectTest()
        {
            Assert.AreNotEqual(word, null);
        }

        [TestMethod()]
        public void WordObjectTest1()
        {
            word = new WordObject("",WordObject.WordType.adjectiveWord);
            Assert.AreNotEqual(word, null);
        }

        [TestMethod()]
        public void WordObjectTest2()
        {
            word = new WordObject("", WordObject.WordType.adjectiveWord,null);
            Assert.AreNotEqual(word, null);
        }

        [TestMethod()]
        public void WordObjectTest3()
        {
            word = new WordObject("O","text", "feminine", "dual", "any", "polarity", "verb", "unknown", new string[] { "ה" }, "none", "","","","",true,"lemma");

            Assert.AreNotEqual(word, null);
            Assert.AreEqual(word.Amount, personObject.amountType.dual);
            Assert.AreEqual(word.Gender,personObject.genderType.feminine);
            Assert.AreEqual(word.IsDefinite, true);
            Assert.AreEqual(word.Lemma, "lemma");
            Assert.AreEqual(word.Ner, "O");
            Assert.AreEqual(word.Pos, "verb" );
            Assert.AreEqual(word.WordT, WordObject.WordType.verbWord);
            Assert.AreEqual(word.WorldObject, new VerbObject("text"));
            Assert.AreEqual(word.Time, personObject.timeType.none);
            Assert.AreEqual(word.Text, "text");
            Assert.IsTrue(word.Prefixes.Single() == "ה");
            Assert.AreEqual(word.Person, personObject.personType.any);

            Assert.AreEqual(word.Text, "text");

        }


        [TestMethod()]
        public void isATest()
        {
            //good
            word = new WordObject("O", "text", "feminine", "dual", "any", "polarity", "verb", "unknown", new string[] { "ה" }, "none", "", "", "", "", true, "lemma");
            Assert.IsTrue(word.isA(WordObject.WordType.verbWord));

            //bad
            word = new WordObject("O", "text", "feminine", "dual", "any", "polarity", "sddsdsds", "unknown", new string[] { "ה" }, "none", "", "", "", "", true, "lemma");
            Assert.IsTrue(word.isA(WordObject.WordType.unknownWord));

            //evil
            word = new WordObject("O", "text", "feminine", "dual", "any", "polarity", null, "unknown", new string[] { "ה" }, "none", "", "", "", "", true, "lemma");
            Assert.IsTrue(word.isA(WordObject.WordType.unknownWord));
        }

        [TestMethod()]
        public void ObjectTypeTest()
        {
            Assert.AreEqual(word.ObjectType(), 0);
        }

        [TestMethod()]
        public void haveTypeOfTest()
        {
            word = new WordObject("O", "dsfds", "feminine", "dfd", "any", "sdds", "noun", "unknown", new string[] { "ה" }, "גדגד", "", "", "", "", true, "lemma");
            var word2 = new WordObject("O", "text", "masculine", "dual", "sdds", "polarity", "noun", "dfdf", new string[] { "ת" }, "none", "", "", "", "", true, "lemגדגדma");
           
            //good
            Assert.IsTrue(word.haveTypeOf(word2));

            //bad
            word2 = new WordObject("O", "text", "masculine", "dual", "sdds", "polarity", "verb", "dfdf", new string[] { "ת" }, "none", "", "", "", "", true, "lemגדגדma");
            Assert.IsFalse(word.haveTypeOf(word2));


            //ugly
            word2 = new WordObject("O", "text", "masculine", "dual", "sdds", "polarity", null, "dfdf", new string[] { "ת" }, "none", "", "", "", "", true, "lemגדגדma");
            Assert.IsFalse(word.haveTypeOf(word2));
        }
    }
}