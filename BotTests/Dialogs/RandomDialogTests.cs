using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProject1;
using System.Text;
using Model;
using Model.dataBase;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotTests.Integration_Tests
{
    [TestClass]
    public class RandomDialogTests : DialogsTestsBase
    {
 
        DataBaseController db = new DataBaseController();
        [TestInitialize]
        public void testInitializeAttribute()
        {
            initRand();
            deleteProfile();

        }

        [TestMethod]
        public async Task RandomDialogTest()
        {

            var res = await RandomCreateAccount();
            Assert.IsTrue(res.Count > 0);
            res = await chooseMainMenu(res);
            
        }

        private async Task<List<string>> chooseMainMenu(List<string> res)
        {
            var options = getOptions(res[2]);

            if (rand.Next(2) == 0)
            {
                res = await sendBot(res[0]);
                AssertNLP.contains(res, DBbotPhrase(Pkey.NotImplamented));
                await chooseMainMenu(res);
            }
            else if (rand.Next(2) == 0)
            {
                res = await sendBot(res[1]);
                AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));
                res = await LearningMenu(res);
            }
            else if (rand.Next(2) == 0)
            {
                res = await sendBot(res[3]);
            }
            else if (rand.Next(2) == 0)
            {
                res = await randomRequest();
                await chooseMainMenu(res);
            }
            else
            {
                res = await sendBot(randomAnswer());
                AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));
                await chooseMainMenu(res);
            }

            return res;

        }

        private async Task<List<string>> randomRequest()
        {

            if (rand.Next(2) == 0)
            {
                return await sendJokeRequest();
            }else if (rand.Next(2) == 0)
            {
                return await sendInfoRequest();
            }
            else 
            {
                return await sendSwear();
            }

        }

        private async Task<List<string>> sendSwear()
        {
            var res = await sendBot("בן זונה");
            AssertNLP.contains(res, DBbotPhrase(Pkey.swearResponse));
            return res;
        }

        private async Task<List<string>> sendInfoRequest()
        {
            var res = await sendBot("תספר לי משהו מעניין");
            AssertNLP.contains(res, DBbotPhrase(Pkey.mightHaveSomthing));
            return res;
        }

        private async Task<List<string>> sendJokeRequest()
        {
            var res = await sendBot("תספר לי משהו מצחיק");
            AssertNLP.contains(res, DBbotPhrase(Pkey.mightHaveSomthing));
            return res;
        }

        private async Task<List<string>> LearningMenu(List<string> res)
        {

            AssertNLP.contains(res, DBbotPhrase(Pkey.areUReaddyToLearn));
            var categories = db.getAllCategory();
            AssertNLP.contains(res, categories);
            res = await sendBot(categories[rand.Next(categories.Length)]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));
            return res;

        }

        private async Task<List<string>> RandomCreateAccount()
        {
            var res = await sendBot(randomAnswer());
            if (rand.Next(2) == 0)
            {
                res = await sendBot("יוחאי");
            }
            else
            {
                res = await sendBot("ךלדגילחךדגיגדלחיגכלחיגכדלגדכיג");
            }

            if (rand.Next(2) == 0)
            {
                res = await sendBot(randomAnswer());

                res = await sendBot("בן");
            }
            else
            {
                res = await sendBot("בת");
            }


            if (rand.Next(2) == 0)
            {
                res = await sendBot(randomAnswer());

                res = await sendBot("'י");
            }
            else if (rand.Next(2) == 0)
            {
                res = await sendBot(randomAnswer());

                res = await sendBot("י\"ב");
            }
            else
            {
                res = await sendBot("יא");
            }

            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));

            Assert.IsTrue(res.Count == 3);
            return res;
        }

        private string randomAnswer()
        {
            var len = rand.Next(5000);
            StringBuilder strBuild = new StringBuilder();

            for(int i =0;i<len;i++)
            {
                int val = 0;
                var wordLen = rand.Next(50);
                if (i % wordLen == 0)
                {
                    val = rand.Next(0x20, 0x7F);
                }
                if (i % wordLen == 1)
                {
                    val = ' ';
                }
                else
                {
                    val = rand.Next(0x05BE, 0x05F4);
                   
                }

                strBuild.Append((char)val, 1);
             }

            return strBuild.ToString();
        }
    }
}
