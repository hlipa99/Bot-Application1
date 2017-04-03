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
        public void TestInitializeAttribute()
        {
            var r= deleteProfile();
            AssertNLP.contains(r, "User profile deleted!");

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
            var res = sendBot("היי");
            Assert.IsTrue(res.Count == 3);
            res = sendBot("לא בא לי להגיד לך");
            AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            res = sendBot("יוחאי");
            AssertNLP.contains(res, DBbotPhrase(Pkey.LetsStart));

            res = sendBot("מגדר זה רק אשלייה","אתה יודע");
            AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            res = sendBot("בן");
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            res = sendBot("אני לומד באוניברסיטה של החיים","ברחוב");
            AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            res = sendBot("יא");
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));
        }

        private List<string> endConversation()
        {
            var res = sendBot("ביי");
            res = sendBot("יום טוב");
            return res;
        }

        private List<string> createUser(string v1, string v2, string v3)
        {
            var res = sendBot("היי");
            res = sendBot("יוחאי");
            res = sendBot("בן");
            res = sendBot("יא");
            return res;
        }


        [TestMethod]
        public void testNewUserDialogMenu()
        {
            var response = createUser("יוחאי", "בן", "יא");
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

             var options = getOptions(response[2]);
            //bad - try not learn
            var res = sendBot(options[1]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));

            //ugly
            res = sendBot("משהו שלא נמצא בתפריט");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

            //good - lets learn
            res = sendBot(options[0]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));

            //[TestMethod]
            //public void testLearningDialog()
            //{
            //    options = getOptions(response[2]);
            //    response = sendBot(options[1]);   //learning options
            //    Assert.IsTrue(response.Count > 2); //learning options number

            //    response = sendBot("בלה בלה");   //learning topic options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.NotAnOption)); 

            //    response = sendBot("לאומיות");   //learning topic options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.letsLearn)); //class assert

            //    response = sendBot("תשובה 1");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.notAnAnswer)); //class assert

            //    response = sendBot("sdsdds");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.notNumber));

            //    response = sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck));

            //    response = sendBot(" תשובה תשובה 2 2");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodAnswer)); //class assert

            //    response = sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck));

            //    response = sendBot(" תשובה תשובה 3 3");   //class options
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodAnswer)); //class assert

            //    response = sendBot("100");   //evaluation wrong option
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.GeneralAck));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.endOfSession));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.goodSessionEnd));
            //    AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            //}

        }


        [TestMethod]
        public void testMenuFreeText()
        {
            var response = createUser("יוחאי", "בן", "יא");
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

            var options = getOptions(response[2]);
            //bad - try not learn
            var res = sendBot("בא לי ללכת לבריכה");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));

            //ugly
            res = sendBot("משהו שלא נמצא בתפריט");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

            //good - lets learn
            res = sendBot("קדימה בוא נלמד");
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));

        }
    }
    }
    
