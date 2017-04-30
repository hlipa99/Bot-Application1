using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Model;
using Bot_Application1.Controllers;
using Model.dataBase;
using BotTests;
using System.Threading;

namespace UnitTestProject1
{
    [TestClass]
    public class GreetingDialogTest : DialogsTestsBase
    {

        [TestInitialize]
        public void TestInitializeAttribute()
        {
            deleteProfile();
        }

       

        [TestCleanup]
        public void TestCleanup()
        {
            ConvID = "";
            // client = null;
        }

        [TestMethod]
        public void testGreetingDialog()
        {
            var res = getToLearningMenu();
           res = sendBot("לאומיות");
            res = sendBot("טוב ביי");
            res = sendBot("כן");
            AssertNLP.contains(res, DBbotPhrase(Pkey.goodbye));
            res = sendBot("להתראות");
            Assert.IsTrue(res.Count == 0);
            res = sendBot("ארוואר");

            res = sendBot("היי");
            AssertNLP.contains(res, DBbotPhrase(Pkey.shortHello));

        }
        
    }
    }
    
