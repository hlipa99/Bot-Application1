﻿
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
    public class QAEnginTests: MockObjectTestBase
    {
        QAEngin qae = null;
     
        public Mock<NLPControler> mockNLP = new Mock<NLPControler>();

        [TestInitialize]
        public void Init()
        {
            qae = new QAEngin();
         

            initializeMocksObject();




        }

        [TestMethod()]
        public void matchAnswersWithListsTest()
        {
            var feedback = qae.matchAnswers(list2, list1);

            Assert.AreEqual(feedback.score,68);
        

            //good
            Assert.AreEqual(feedback.missingEntitis[0].entityValue,eventO.Word);
            Assert.AreEqual(feedback.missingEntitis[0].entityType, "eventWord");
            Assert.AreEqual(feedback.score, 68);

            //bad
            Assert.AreEqual(feedback.missingEntitis.Count, 1);


            list1.Clear();
            //ugly
            Assert.AreEqual(qae.matchAnswers(list1, list1).missingEntitis.Count, 0);
            Assert.AreEqual(qae.matchAnswers(list1, list1).score, 100);

        }

        [TestMethod()]
        public void matchAnswersWithUserAnswerTest()
        {

            mockNLP.Setup(x => x.Analize(It.Is<string>(y => y.Contains("user")), It.IsAny<String>())).Returns(list2);
            mockNLP.Setup(x => x.Analize(It.Is<string>(y => y.Contains("system")))).Returns(list1);

            qae.Nlp = mockNLP.Object;
           
            mockSubQqestion1.Setup(x => x.answerText).Returns("system דוד בן גוריון הודיע שהאו\"ם הכריז על סיום המנדט");
            var feedback = qae.matchAnswers(mockSubQqestion1.Object, "user דוד בן גוריון הקריא את הכרזת העצמאות לאחר האישור באו\"ם");
            Assert.AreEqual(feedback.answersFeedbacks[0].score, 68);

            //good
            Assert.IsTrue(feedback.missingEntitis.Where(x=>x.entityValue == eventO.Word && x.entityType == "eventWord").Count() == 1);
            Assert.AreEqual(feedback.answersFeedbacks[0].score, 68);

            //bad
            Assert.AreEqual(feedback.missingEntitis.Count, 1);


            list1.Clear();
            //ugly
            mockSubQqestion1.Setup(x => x.answerText).Returns("system כמה פעמים אפשר לדחוף פיל במעלית");
             feedback = qae.matchAnswers(mockSubQqestion1.Object, "");

            Assert.AreEqual(feedback.missingEntitis.Count, 0);
            Assert.AreEqual(feedback.score, 0);
        }
    }
}