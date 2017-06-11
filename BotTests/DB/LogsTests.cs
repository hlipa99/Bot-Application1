using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.dataBase;
using System.Linq;
using NLP.HebWords;
using Bot_Application1.Controllers;
using Model;

namespace BotTests.Integration_Tests
{
    [TestClass]
    public class LogsTests
    {
        DataBaseController db = new DataBaseController();
        

        [TestMethod]
        public async void ansersLogsTests()
        {
            var dbVar = db.updateDBmanual();
            var count = dbVar.answersLog.Where(x=>x.questionID == "test").Count();
            Logger.addAnswerOutput("anserTest", "answerTest", new Model.Models.AnswerFeedback(), "testUser", "test");
            var count2 = dbVar.answersLog.Where(x => x.questionID == "test").Count();
            Assert.AreEqual(count + 1, count);
        }

        [TestMethod]
        public async void errorLogsTests()
        {
            var dbVar = db.updateDBmanual();
            var count = dbVar.ErrorLog.Where(x => x.error == "test").Count();
            Logger.addErrorLog("anserTest", "test");
            var count2 = dbVar.ErrorLog.Where(x => x.error == "test").Count();
            Assert.AreEqual(count + 1, count);
        }
    }
        
   
}
