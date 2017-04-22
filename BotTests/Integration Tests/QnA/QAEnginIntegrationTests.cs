
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.dataBase;
using Moq;
using NLP.Controllers;
using NLP.QnA;
using NLP.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLP.QnA.Tests
{
    [TestClass()]
    public class QAEnginIntegrationTests: ObjectTestBase
    {
        QAEngin qae = null;
     
        NLPControler  NLP = new NLPControler();

        [TestInitialize]
        public void Init()
        {
            qae = new QAEngin();
            initializeObject();




        }

        [TestMethod()]
        public void matchAnswersWithListsIntegrationTest()
        {
            
            var feedback = qae.matchAnswers(SubQqestion1, userAnswerSubQuestion1);

       

            //good
            Assert.IsTrue(feedback.missingEntitis.Contains(Entity4));
            Assert.IsFalse(feedback.missingEntitis.Contains(Entity1));
            Assert.IsFalse(feedback.missingEntitis.Contains(Entity2));
            Assert.IsFalse(feedback.missingEntitis.Contains(Entity3));

            Assert.AreEqual(feedback.score, 45);

            //bad
           feedback = qae.matchAnswers(SubQqestion1, "");
            Assert.IsTrue(feedback.missingEntitis.Contains(Entity4));
            Assert.IsTrue(feedback.missingEntitis.Contains(Entity1));
            Assert.IsTrue(feedback.missingEntitis.Contains(Entity2));
            Assert.IsTrue(feedback.missingEntitis.Contains(Entity3));
            Assert.AreEqual(feedback.score, 0);



            //ugly
            feedback = qae.matchAnswers(null, "");
            Assert.AreEqual(feedback.missingEntitis.Count, 0);
            Assert.AreEqual(feedback.score, 0);
        }

    }
}