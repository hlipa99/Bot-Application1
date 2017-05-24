using Bot_Application1.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLP.Controllers;
using NLP.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLP.Controllers.Tests
{
    [TestClass()]
    public class NLPControlerIntegrationTests :  ObjectTestBase
    {

        NLPControler nlpCtrl;

        [TestInitialize()]
        public void ConversationControllerTest()
        {
            initializeObject();
            nlpCtrl = new NLPControler();
            nlpCtrl.Ma =  MorfAnalizer ;
        }


        [TestMethod()]
        public void AnalizeIntegrationTest()
        {
            var res = nlpCtrl.Analize("בן גוריון הקריא את מגילת העצמאות בהכרזת העצמאות וההגנה היתה מחתרת לוחמת",null);
            //good
            Assert.IsTrue(res.Contains(new PersonObject("דוד בן גוריון")));
            Assert.IsTrue(res.Contains(new ConceptObject("מגילת העצמאות")));
            Assert.IsTrue(res.Contains(new EventObject("הכרזת העצמאות")));
            var ob = new OrganizationObject("ההגנה");
            ob.DefiniteArticle = true;
            Assert.IsTrue(res.Contains(ob));
        

            //bad
            Assert.IsFalse(res.Contains(new PersonObject("")));

            //ugly
            Assert.IsFalse(res.Contains(null));
        }

           

        [TestMethod()]
        public void AnalizeTestWithGufContextIntegration()
        {
            var res = nlpCtrl.Analize("הוא הקריא את מגילת העצמאות בהכרזת העצמאות והם היו מחתרת לוחמת", "ההגנה ובן גוריון");
            //good
            Assert.IsTrue(res.Contains(new PersonObject("דוד בן גוריון")));
            Assert.IsTrue(res.Contains(new ConceptObject("מגילת העצמאות")));
            Assert.IsTrue(res.Contains(new EventObject("הכרזת העצמאות")));
            var ob = new OrganizationObject("ההגנה");
            ob.DefiniteArticle = true;
            Assert.IsTrue(res.Contains(ob));
 
            //bad
            Assert.IsFalse(res.Contains(new PersonObject("")));

            //ugly
            Assert.IsFalse(res.Contains(null));
        }
    }
}