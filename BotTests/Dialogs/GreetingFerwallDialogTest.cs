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

namespace UnitTestProject1
{
    [TestClass]
    public class GreetingDialogTest : DialogsTestsBase
    {

        [TestInitialize]
        public void TestInitializeAttribute()
        {
            var task = sendMessage("/deleteprofile");
            var response = task.Result;
            AssertNLP.contains(response, "User profile deleted!" );
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConvID = "";
            // client = null;
        }

        [TestMethod]
        public void testcreateUserDialog()
        {
            var res = createUser("מיה", "בת", "יב");
            res = sendBot("טוב ביי");
            AssertNLP.contains(res, DBbotPhrase(Pkey.goodbye));
            res = sendBot("להתראות");
            Assert.IsTrue(res.Count == 0);
            res = sendBot("היי");
            AssertNLP.contains(res, DBbotPhrase(Pkey.shortHello));
            res = sendBot("מה קורה?");
            AssertNLP.contains(res, DBbotPhrase(Pkey.IAmFine));
            AssertNLP.contains(res, DBbotPhrase(Pkey.howAreYou));
            res = sendBot("אני בסדר");
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            res = sendBot("להתראות");
            AssertNLP.contains(res, DBbotPhrase(Pkey.goodbye));
            res = sendBot("לילה טוב");
            Assert.IsTrue(res.Count == 0);
            res = sendBot( "היי","מה קורה?" );
            AssertNLP.contains(res, DBbotPhrase(Pkey.shortHello));
            AssertNLP.contains(res, DBbotPhrase(Pkey.IAmFine));
            AssertNLP.contains(res, DBbotPhrase(Pkey.howAreYou));
        }
        
    }
    }
    
