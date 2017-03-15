using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Model;
using Bot_Application1.Controllers;
using Model.dataBase;

namespace UnitTestProject1
{
    [TestClass]
    public class MainDialogTest : DialogsTestsBase
    {



        [TestInitialize]
        public void TestInitializeAttribute()
        {
            var task = sendMessage("/deleteprofile");
            var response = task.Result;
            Assert.IsTrue(Contains(response, new string[] { "User profile deleted!" }));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConvID1 = "";
            // client = null;
        }

        [TestMethod]
        public void testDeleteProfileDialog()
        {
            createUser("יוחאי", "בן", "יא");
            endConversation();
            var respone = sendBot("היי");
            Assert.IsTrue(respone.Contains("יוחאי"));
            var response = sendBot("/deleteprofile");
            Assert.IsTrue(Contains(response, new string[] { "User profile deleted!" }));
            response = sendBot("/deleteprofile");
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.selfIntroduction)));
        }


        [TestMethod]
        public void testMenu()
        {
            var response = createUser("יוחאי", "בן", "יא");
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MainMenuText)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MenuLearn)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MenuNotLearn)));

            var options = getOptions(response[2]);
            //bad - try not learn
            var res = sendBot(options[1]);
            Assert.IsTrue(Contains(res, DBbotPhrase(Pkey.NotImplamented)));

            //ugly
            res = sendBot("משהו שלא נמצא בתפריט");
            Assert.IsTrue(Contains(res, DBbotPhrase(Pkey.NotAnOption)));

            //good - lets learn
            res = sendBot(options[0]);
            Assert.IsTrue(Contains(res, DBbotPhrase(Pkey.letsLearn)));

            //[TestMethod]
            //public void testLearningDialog()
            //{
            //    options = getOptions(response[2]);
            //    response = sendBot(options[1]);   //learning options
            //    Assert.IsTrue(response.Count > 2); //learning options number

            //    response = sendBot("בלה בלה");   //learning topic options
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.NotAnOption))); 

            //    response = sendBot("לאומיות");   //learning topic options
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.letsLearn))); //class assert

            //    response = sendBot("תשובה 1");   //class options
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.notAnAnswer))); //class assert

            //    response = sendBot("sdsdds");   //evaluation wrong option
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.notNumber)));

            //    response = sendBot("100");   //evaluation wrong option
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck)));

            //    response = sendBot(" תשובה תשובה 2 2");   //class options
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.goodAnswer))); //class assert

            //    response = sendBot("100");   //evaluation wrong option
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck)));

            //    response = sendBot(" תשובה תשובה 3 3");   //class options
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.goodAnswer))); //class assert

            //    response = sendBot("100");   //evaluation wrong option
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck)));
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.endOfSession)));
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.goodSessionEnd)));
            //    Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MainMenuText)));
            //}

        }


        [TestMethod]
        public void testMenuFreeText()
        {
            var response = createUser("יוחאי", "בן", "יא");
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MainMenuText)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MenuLearn)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MenuNotLearn)));

            var options = getOptions(response[2]);
            //bad - try not learn
            var res = sendBot("בא לי ללכת לבריכה");
            Assert.IsTrue(Contains(res, DBbotPhrase(Pkey.NotImplamented)));

            //ugly
            res = sendBot("משהו שלא נמצא בתפריט");
            Assert.IsTrue(Contains(res, DBbotPhrase(Pkey.NotAnOption)));

            //good - lets learn
            res = sendBot("קדימה בוא נלמד");
            Assert.IsTrue(Contains(res, DBbotPhrase(Pkey.letsLearn)));

        }
    }
    }
    
