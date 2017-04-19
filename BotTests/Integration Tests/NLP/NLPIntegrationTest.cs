
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLP;
using NLP.NLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLP.Tests
{
    [TestClass()]
    public class NLPIntegrationTest :  ObjectTestBase
    {

        MorfAnalizer ma = new MorfAnalizer();

       [TestInitialize]
        public void Init()
        {
            ma = new MorfAnalizer();
            initializeObject();
        }


        [TestMethod()]
        public void meniAnalizeIntegrationTest()
        {
         //   ma.HttpCtrl =  OuterAPICtrl ;
         //   ma.DBctrl1 =  DB;
           var res = ma.meniAnalize(userAnswerSubQuestion1, true);


            //good
            Assert.IsTrue(res.FirstOrDefault().Contains(WordObject1));
            Assert.IsTrue(res.FirstOrDefault().Contains(WordObject2));
            Assert.IsTrue(res.FirstOrDefault().Contains(WordObject3));

            //bad
            Assert.IsFalse(res.FirstOrDefault().Contains(WordObject4));


            //ugly
            res = ma.meniAnalize("", false);
            Assert.AreEqual(res.Count, 0);

            res = ma.meniAnalize(null, false);
            Assert.AreEqual(res.Count, 0);

        }

    }
}