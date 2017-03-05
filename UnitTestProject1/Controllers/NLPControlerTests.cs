using Bot_Application1.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLPtest.Controllers;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLPtest.Controllers.Tests
{
    [TestClass()]
    public class NLPControlerTests : MockObjectTestBase
    {

        NLPControler nlpCtrl;

        [TestInitialize()]
        public void ConversationControllerTest()
        {
            initializeMocksObject();
            nlpCtrl = new NLPControler();
        }

        [TestMethod()]
        public void AnalizeTest()
        {
            var obj = nlpCtrl.Analize("תשובה עם כל הדברים ממש טובה");
            //good
            Assert.IsTrue(obj.Contains(new PersonObject("טובה")));
            Assert.IsTrue(obj.Contains(new ConceptObject("הדברים")));

            //bad
            Assert.IsFalse(obj.Contains(new ConceptObject("תשובה")));
            Assert.IsFalse(obj.Contains(new ConceptObject("ממש")));

            //ugly

             obj = nlpCtrl.Analize("");
            Assert.IsTrue(obj.Count == 0);

        }

        [TestMethod()]
        public void AnalizeTestWithGufContext()
        {
            var obj = nlpCtrl.Analize("ממש היא עם כל הם", "הדברים טובה");
            //good
            Assert.IsTrue(obj.Contains(new PersonObject("טוב")));
            var word = new ConceptObject("דבר");
            word.DefiniteArticle = true;
            var s = obj.Contains(word);
            Assert.IsTrue(s);

            //bad
            Assert.IsFalse(obj.Contains(new ConceptObject("תשובה")));
            Assert.IsFalse(obj.Contains(new ConceptObject("ממש")));

            //ugly

            obj = nlpCtrl.Analize("");
            Assert.IsTrue(obj.Count == 0);
        }
    }
}