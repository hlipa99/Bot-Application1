using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.dataBase;
using NLP.QnA;

namespace BotTests.Integration_Tests.QnA
{
    [TestClass]
    public class answerQualityTest
    {
        DataBaseController db = new DataBaseController();
        QAEngin qna = new QAEngin();


        [TestMethod]
        public void answerQualityCompareTest()
        {
            var questions = db.getSampleQuestions();
            foreach(var a in questions)
            {
                var q = new SubQuestion();
                q.answerText = a.question;
                q.questionText = "";
                q.flags = "";
                var score = qna.matchAnswers(q, a.userAnswer).score;
                db.updateSampleQuestion(a,score);
                var logScore = int.Parse(a.entities); 
                Assert.IsTrue(score > logScore - 10 && score < logScore + 10, "id: " + a.id+"\nlogScore:" +logScore + " != score:" + score + "\n1:" + a.question + "\n2:" + a.userAnswer);
            }
        }
    }
}
