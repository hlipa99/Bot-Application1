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
        public async Task testcreateUserDialog()
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
        public async Task testNewUserDialogMenu()
        {
            var response = await createUser("יוחאי", "בן", "יא'");
            AssertNLP.contains(response, DBbotPhrase(Pkey.MainMenuText));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuLearn));
            AssertNLP.contains(response, DBbotPhrase(Pkey.MenuNotLearn));

             var options = getOptions(response[2]);
            //bad - try not learn
            var res = await sendBot(options[2]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));

            //ugly
            res = await sendBot("משהו שלא נמצא בתפריט");
            AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

            //good - lets learn
            res = await sendBot(options[1]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));

            

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
    
