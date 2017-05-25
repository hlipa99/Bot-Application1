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
    public class NewUserDialogTest : DialogsTestsBase
    {

        [TestInitialize]
        public async void testInitializeAttribute()
        {
            deleteProfile();

        }


        [TestCleanup]
        public async void testCleanup()
        {
            ConvID = "";
            // client = null;
        }

        [TestMethod]
        public async void testcreateUserDialog()
        {
            var res = await sendBot("היי");
            Assert.IsTrue(res.Count == 3);
            res = await sendBot("לא בא לי להגיד לך");
            AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            res = await sendBot("יוחאי");
            AssertNLP.contains(res, DBbotPhrase(Pkey.LetsStart));

            res = await sendBot("מגדר זה רק אשלייה","אתה יודע");
            AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            res = await sendBot("בן");
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            res = await sendBot("אני לומד באוניברסיטה של החיים","ברחוב");
            AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            res = await sendBot("יא");
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));
        }

        private async Task<List<string>> endConversation()
        {
            var res = await sendBot("ביי");
            res = await sendBot("יום טוב");
            return res;
        }

        [TestMethod]
        public async void testNewUserDialogMenu()
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
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.NotAnOption)); 

            //    response = await sendBot("לאומיות");   //learning topic options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.letsLearn)); //class assert

            //    response = await sendBot("תשובה 1");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.notAnAnswer)); //class assert

            //    response = await sendBot("sdsdds");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.notNumber));

            //    response = await sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck));

            //    response = await sendBot(" תשובה תשובה 2 2");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodAnswer)); //class assert

            //    response = await sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck));

            //    response = await sendBot(" תשובה תשובה 3 3");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodAnswer)); //class assert

            //    response = await sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.endOfSession));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodSessionEnd));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            //}

        }


        [TestMethod]
        public async void testMenuFreeText()
        {
            var response = await createUser("יוחאי", "בן", "יא'");
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

            var options = getOptions(response[2]);
            //bad - try not learn
            var res = await sendBot("בא לי ללכת לבריכה");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));

            //ugly
            res = await sendBot("משהו שלא נמצא בתפריט");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

            //good - lets learn
            res = await sendBot("קדימה בוא נלמד");
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));

        }
    }
    }
    
