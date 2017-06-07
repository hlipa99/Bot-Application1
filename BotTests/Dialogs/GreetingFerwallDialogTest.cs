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
        public void testInitializeAttribute()
        {
            deleteProfile();
        }

       

        [TestCleanup]
        public void testCleanup()
        {
            ConvID = "";
            // client = null;
        }

        [TestMethod]
        public async Task testGreetingDialog()
        {
            var res = await getToLearningMenu();
           res = await sendBot("לאומיות");
            res = await sendBot("טוב ביי");
            res = await sendBot("כן");
            AssertNLP.contains(res, DBbotPhrase(Pkey.goodbye));
            res = await sendBot("להתראות");
            Assert.IsTrue(res.Count == 0);
            res = await sendBot("ארוואר");

            res = await sendBot("היי");
            AssertNLP.contains(res, DBbotPhrase(Pkey.shortHello));

        }
        
    }
    }
    
