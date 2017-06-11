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
                res = await sendBot(randomAnswer());
                res = await sendBot("יוחאי");
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
                //res = await sendBot(randomAnswer());

                res = await sendBot("י");
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

            Assert.IsTrue(res.Count == 2);
            return res;
        }

        private string randomAnswer()
        {
            return ("להלהלה");
            var len = rand.Next(500);
            StringBuilder strBuild = new StringBuilder();

            for(int i =0;i<len;i++)
            {
                int val = 0;
                var wordLen = rand.Next(40) + 1;
                if(i%7 == rand.Next(7))
                {
                    val = ' ';
                }
                else if (i % 2 == 0)
                {
                    val = rand.Next(0x00020, 0x0007D);
                }
                else
                {
                    val = rand.Next(0x005D0, 0x005EA);

                }
                

                strBuild.Append((char)val, 1);
             }

            return strBuild.ToString();
        }
    }
}
