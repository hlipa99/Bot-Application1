using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLP.HebWords;
using NLP.NLP;
using NLP.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLP.NLP.Tests
{
    [TestClass()]
    public class SemanticAnalizerIntegrationTests : ObjectTestBase
    {

        SemanticAnalizer sAnal;
        List<ITemplate> listTemplate = new List<ITemplate>();


        [TestInitialize]
        public void Init()
        {
            sAnal = new SemanticAnalizer();
            initializeObject();
        }

        [TestMethod()]
        public void findTemplateIntegrationTest()
        {

            listTemplate.AddRange(new ITemplate[] { WordObject1, WordObject2 , WordObject3 , WordObject4  });
            string log = "";
            var res = sAnal.findTemplate(listTemplate.ToArray(), out log);
            //good
            Assert.IsTrue(res.Contains(new OrganizationObject("ערבי")));
            Assert.IsTrue(res.Contains(new LocationObject("ירושלים")));
            Assert.IsTrue(res.Contains(new ConceptObject("פליטים")));
            Assert.IsTrue(res.Contains(new OrganizationObject("ירדן")));
            Assert.IsTrue(res.Count() == 4);


 
        }

        [TestMethod()]
        public void findGufContextTest()
        {
            listTemplate.AddRange(new ITemplate[] { WordObject1   });
            var guftemplate = new WordObject[] { WordObject2, WordGufObjectThey  };




            string log = "";
            var listListObject = new List<List<WordObject>>();
            listListObject.Add(guftemplate.ToList());
            var res = sAnal.findGufContext(listListObject, listTemplate);
            var single = res.Single().Cast<WordObject>();

 
            //good
            Assert.IsTrue(single.Where(x=> x == WordObject1 ).Count() == 1);
            Assert.IsTrue(single.Where(x => x == WordObject2 ).Count() == 1);
            Assert.IsTrue(single.Count() == 2);

            listTemplate.Clear();
            //bad
             res = sAnal.findGufContext(listListObject, listTemplate);
             single = res.Single().Cast<WordObject>();

            Assert.IsFalse(single.Where(x => x == WordObject1 ).Count() == 1);
            Assert.IsTrue(single.Count() == 2);

            //ugly
            res = sAnal.findGufContext(listListObject, null);
            single = res.Single().Cast<WordObject>();

            Assert.IsFalse(single.Where(x => x == WordObject1 ).Count() == 1);
            Assert.IsTrue(single.Where(x => x == WordObject2 ).Count() == 1);
            Assert.IsFalse(single.Where(x => x == WordObject3 ).Count() == 1);
            Assert.IsFalse(single.Where(x => x == WordObject4 ).Count() == 1);
            Assert.IsTrue(single.Count() == 2);
        }

    }
}