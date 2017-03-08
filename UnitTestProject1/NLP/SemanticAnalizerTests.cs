using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLPtest.HebWords;
using NLPtest.NLP;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLPtest.NLP.Tests
{
    [TestClass()]
    public class SemanticAnalizerTests: MockObjectTestBase
    {

        SemanticAnalizer sAnal;
        List<ITemplate> listTemplate = new List<ITemplate>();


        [TestInitialize]
        public void Init()
        {
            sAnal = new SemanticAnalizer();
            initializeMocksObject();
        }

        [TestMethod()]
        public void findTemplateTest()
        {

            listTemplate.AddRange(new ITemplate[] { moqWordObject1.Object, moqWordObject2.Object, moqWordObject3.Object, moqWordObject4.Object });
            string log = "";
            var res = sAnal.findTemplate(listTemplate.ToArray(), out log);
            //good
            Assert.IsTrue(res.Contains(new PersonObject("דוד בן גוריון")));
            Assert.IsTrue(res.Contains(new ConceptObject("מגילת העצמאות")));
            Assert.IsTrue(res.Contains(new EventObject("הכרזת העצמאות")));
            Assert.IsTrue(res.Contains(new OrganizationObject("ההגנה")));
            Assert.IsTrue(res.Count() == 4);




        }

        [TestMethod()]
        public void findGufContextTest()
        {
            listTemplate.AddRange(new ITemplate[] { moqWordObject1.Object, moqWordObject2.Object, moqWordObject3.Object, moqWordObject4.Object });
            var guftemplate = new WordObject[] { moqWordGufObjectShe.Object, moqWordGufObjectHe.Object, moqWordGufObjectIt.Object, moqWordGufObjectThey.Object };




            string log = "";
            var listListObject = new List<List<WordObject>>();
            listListObject.Add(guftemplate.ToList());
            var res = sAnal.findGufContext(listListObject, listTemplate);
            //good
            Assert.IsTrue(res.FirstOrDefault().Contains(moqWordObject1.Object));
            Assert.IsTrue(res.FirstOrDefault().Contains(moqWordObject2.Object));
            Assert.IsTrue(res.FirstOrDefault().Contains(moqWordObject3.Object));
            Assert.IsTrue(res.FirstOrDefault().Contains(moqWordObject4.Object));
            Assert.IsTrue(res.FirstOrDefault().Count() == 4);

        }

    }
}