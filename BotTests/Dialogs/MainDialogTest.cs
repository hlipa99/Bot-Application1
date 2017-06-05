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
        public async Task testCleanup()
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
            AssertNLP.contains(response, DBbotPhrase(Pkey.selfIntroduction));
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

            //[TestMethod]
            //public async void testLearningDialog()
            //{
            //    options = await getOptions(response[2]);
            //    response = await sendBot(options[1]);   //learning options
            //    Assert.IsTrue(response.Count > 2); //learning options number

            //    response = await sendBot("בלה בלה");   //learning topic options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.NotAnOption))); 

            //    response = await sendBot("לאומיות");   //learning topic options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.letsLearn))); //class assert

            //    response = await sendBot("תשובה 1");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.notAnAnswer))); //class assert

            //    response = await sendBot("sdsdds");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.notNumber)));

            //    response = await sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck)));

            //    response = await sendBot(" תשובה תשובה 2 2");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodAnswer))); //class assert

            //    response = await sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck)));

            //    response = await sendBot(" תשובה תשובה 3 3");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodAnswer))); //class assert

            //    response = await sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck)));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.endOfSession)));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodSessionEnd)));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText)));
            //}

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
    
