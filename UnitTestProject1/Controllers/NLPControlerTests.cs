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
            nlpCtrl.Ma = mockMorfAnalizer.Object;
        }


        [TestMethod()]
        public void AnalizeTest()
        {
            var res = nlpCtrl.Analize("בן גוריון הקריא את מגילת העצמאות בהכרזת העצמאות וההגנה היתה מחתרת לוחמת");
            //good
            Assert.IsTrue(res.Contains(new PersonObject("דוד בן גוריון")));
            Assert.IsTrue(res.Contains(new ConceptObject("מגילת העצמאות")));
            Assert.IsTrue(res.Contains(new EventObject("הכרזת העצמאות")));
            Assert.IsTrue(res.Contains(new OrganizationObject("ההגנה")));
            Assert.IsTrue(res.Count() == 4);

            //bad
            Assert.IsFalse(res.Contains(new PersonObject("")));

            //ugly
            Assert.IsFalse(res.Contains(null));

        }

        [TestMethod()]
        public void AnalizeTestWithGufContext()
        {
            var res = nlpCtrl.Analize("הוא הקריא את היא ב זה ו היא היתה מחתרת לוחמת","דוד בן גוריון שמפו קשקשים ההגנה נום נום הכרזת העצמאות בלה מגילת העצמאות");
            //good
            Assert.IsTrue(res.Contains(new PersonObject("דוד בן גוריון")));
            Assert.IsTrue(res.Contains(new ConceptObject("מגילת העצמאות")));
            Assert.IsTrue(res.Contains(new EventObject("הכרזת העצמאות")));
            Assert.IsTrue(res.Contains(new OrganizationObject("ההגנה")));
            Assert.IsTrue(res.Count() == 4);

            //bad
            Assert.IsFalse(res.Contains(new PersonObject("")));
            Assert.IsFalse(res.Contains(new PersonObject("שמפו")));

            //ugly
            Assert.IsFalse(res.Contains(null));

        }


        //integration
        //[TestMethod()]
        //public void AnalizeTest()
        //{
        //    var obj = nlpCtrl.Analize("תשובה עם כל הדברים ממש טובה");
        //    //good
        //    Assert.IsTrue(obj.Contains(new PersonObject("טוב")));
        //    var o = new ConceptObject("דבר");
        //    o.DefiniteArticle = true;
        //    Assert.IsTrue(obj.Contains(o));

        //    //bad
        //    Assert.IsFalse(obj.Contains(new ConceptObject("תשובה")));
        //    Assert.IsFalse(obj.Contains(new ConceptObject("ממש")));

        //    //ugly

        //     obj = nlpCtrl.Analize("");
        //    Assert.IsTrue(obj.Count == 0);

        //}

        //[TestMethod()]
        //public void AnalizeTestWithGufContext()
        //{
        //    var obj = nlpCtrl.Analize("היא ממש והם קצת פחות", "הדברים טובה");
        //    //good
        //    Assert.IsTrue(obj.Contains(new PersonObject("טוב")));
        //    var word = new ConceptObject("דבר");
        //    word.DefiniteArticle = true;
        //    var s = obj.Contains(word);
        //    Assert.IsTrue(s);

        //    //bad
        //    Assert.IsFalse(obj.Contains(new ConceptObject("תשובה")));
        //    Assert.IsFalse(obj.Contains(new ConceptObject("ממש")));

        //    //ugly

        //    obj = nlpCtrl.Analize("");
        //    Assert.IsTrue(obj.Count == 0);
        //}
    }
}