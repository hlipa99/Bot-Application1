using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProject1;
using System.Text;
using Model;
using Model.dataBase;

namespace BotTests.Integration_Tests
{
    [TestClass]
    public class RandomDialogTests : DialogsTestsBase
    {

        [TestInitialize]
        public void TestInitializeAttribute()
        {
            deleteProfile();
        }

        DataBaseController db = new DataBaseController();


        [TestMethod]
        public void RandomDialogTest()
        {
            Random seedRand = new Random();
            int seed = seedRand.Next();
            Random rand = new Random(seed);
            Console.WriteLine("seed:" + seed);

            var res = sendBot("היי");
            if(rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));
                AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
                res = sendBot("יוחאי");
            }
            else
            {
                res = sendBot("יוחאי");
            }

            if (rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));
                AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            }

            res = sendBot("יוחאי");
            if (rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));

                res = sendBot("בן");
            }
            else
            {
                res = sendBot("בת");
            }
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            Assert.IsTrue(res.Count == 3);


            if (rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));
                AssertNLP.contains(res, DBbotPhrase(Pkey.MissingUserInfo));
            }

            res = sendBot("יוחאי");
            if (rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));

                res = sendBot("י");
            }
            else if(rand.Next(2) == 0)
            {
                res = sendBot("יא");
            }
            else
            {
                res = sendBot("יב");
            }
            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
            Assert.IsTrue(res.Count == 3);


            var options = getOptions(res[2]);

            if (rand.Next(2) == 0)
            {
                res = sendBot(res[2]);
            }
            else if (rand.Next(2) == 0)
            {
                res = sendBot(res[1]);
            }
            else if (rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));
            }

            res = sendBot(res[1]);
            AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));
            var categories = db.getAllCategory();

            if (rand.Next(2) == 0)
            {
                res = sendBot(randomAnswer(rand));
            }

            res = sendBot(categories[rand.Next(categories.Length)]);


        }

        private string randomAnswer(Random rand)
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
