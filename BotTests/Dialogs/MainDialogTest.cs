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
    public class MainDialogTest : DialogsTestsBase
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
        public async Task testDeleteProfileDialog()
        {

            createUser("יוחאי", "בן", "יא");

            var response = await sendBot("/deleteprofile");
            AssertNLP.contains(response, "User profile deleted!");
            response = await sendBot("היי");
            AssertNLP.contains(response, DBbotPhrase(Pkey.NewUserGetName));
        }

        private async void startLearning(string v1, string v2, string v3)
        {
            var res = await createUser("יוחאי", "בן", "יא");
            var options = getOptions(res[2]);
         
            res = await sendBot(options[1]);
            res = await sendBot("בונים מדינה");
   


        }

        [TestMethod]
        public async Task testMainDialogTest()
        {
            var response = await createUser("יוחאי", "בן", "יא'");
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

            var options = getOptions(response[2]);
            //bad - try not learn
            var res = await sendBot(options[1]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));

            //ugly
            res = await sendBot("משהו שלא נמצא בתפריט");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

            //good - lets learn
            res = await sendBot(options[0]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));

  
        }


        [TestMethod]
        public async Task testGetUserStatistics()
        {
            var newUser = "testuser2";
            var response = await createUser("יוחאי", "בן", "יא'", newUser);
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

            var options = getOptions(response[1]);

            //good - statistic unknown user
            var res = await sendBot(options[3],true,newUser);
            AssertNLP.contains(res, DBbotPhrase(Pkey.notEnoughAnswersForStat));

             response = await createUser("יוחאי", "בן", "יא'");

             options = getOptions(response[1]);

            //good - statistic unknown user
             res = await sendBot(options[3]);

            AssertNLP.contains(res, DBbotPhrase(Pkey.userStatistics));

        }


        [TestMethod]
        public async void testDialogMenuFreeText()
        {
            var response = await createUser("יוחאי", "בן", "יא'");
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

            var options = getOptions(response[2]);
            //bad - try not learn
            var res = await sendBot(options[0]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));

            //ugly
            res = await sendBot("משהו שלא נמצא בתפריט");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

            //good - lets learn
            res = await sendBot(options[1]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));

        }
    }
    }
    
