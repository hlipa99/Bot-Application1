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
using Model.Models;
using System.Diagnostics;
using System.Threading;

namespace UnitTestProject1
{
    [TestClass]
    public class LearningDialogTest : DialogsTestsBase
    {


        [TestInitialize]
        public void testInitializeAttribute()
        {
            initRand();
            deleteProfile();

        }


        [TestCleanup]
        public async void testCleanup()
        {
            ConvID = "";
            // client = null;
        }



        [TestMethod]
        public async Task testLearningMenu() {
            var db = new DataBaseController();

            var optionsRes = await getToLearningMenu();
            AssertNLP.contains(optionsRes, DBbotPhrase(Pkey.chooseStudyUnits));

            var allCategory = db.getAllCategory();
            var category = allCategory[rand.Next(allCategory.Length)];

                //subject is avialabale

                AssertNLP.contains(optionsRes, category);

                //bad - try somthing else
                var res = await sendBot("ביולוגיה");
                AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

                //ugly
              //  res = await sendBot("");
              //  Assert.AreEqual(res.Count,0);

                //good - lets learn
                res = await sendBot(category);
                AssertNLP.contains(res, DBbotPhrase(Pkey.letsLearn));
                AssertNLP.contains(res, DBbotPhrase(Pkey.firstQuestion));



                    var questions = new DataBaseController().getQuestion(category);
                Debug.WriteLine(category);
                Debug.WriteLine(questions.Length);



                List<IQuestion> questionOpt = null;
                foreach (var r in res)
                {
                    questionOpt = new List<IQuestion>(questions).FindAll(x => r.Contains(x.QuestionText.Trim()));
                    if (questionOpt.Count > 0) break;
                }
                
              //  var questionOpt = new List<IQuestion>(questions).FindAll(x => AssertNLP.contains(res.ToArray(), x.QuestionText));
                    Assert.AreEqual(questionOpt.Count, 1);
                    var question = questionOpt[0];
                
                    Assert.AreEqual(question.Category,category);

                    var counter = 0;
                    foreach (var subQuestion in question.SubQuestion)
                    {
                        AssertNLP.contains(res, subQuestion.questionText);

                    var rnd = rand.Next(5);
                    switch (rnd)
                    {

                        case 0:
                            res = await sendBot("הפסקה");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                            res = await sendBot("כן");
                            //AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
                            AssertNLP.contains(res, DBbotPhrase(Pkey.imWaiting));
                            Thread.Sleep(10000);
                            res = await sendBot("חזרתי");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.letsContinue));
                            AssertNLP.contains(res, subQuestion.questionText);
                            break;

                        case 1:
                            res = await sendBot("מספיק");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.earlyDiparture));
                            AssertNLP.contains(res, DBbotPhrase(Pkey.areYouSure));
                            res = await sendBot("לא");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.keepLearning));
                            AssertNLP.contains(res, subQuestion.questionText);
                            break;

                        default:
                            break;
                    }

                    res = await sendBot(subQuestion.answerText);

                    AssertNLP.contains(res, DBbotPhrase(Pkey.veryGood));

                    AssertNLP.contains(res, DBbotPhrase(Pkey.moveToNextSubQuestion));

                    }


        }

        [TestMethod]
        public async Task testLearningSpecialValues()
        {
            var db = new DataBaseController();

            var optionsRes = await getToLearningMenu();
            AssertNLP.contains(optionsRes, DBbotPhrase(Pkey.chooseStudyUnits));

            var allCategory = db.getAllCategory();
            var category = allCategory[rand.Next(allCategory.Length)];
            var res = await sendBot(category);

             res = await sendBot("ספר לי משהו מצחיק");
            AssertNLP.contains(res, DBbotPhrase(Pkey.mightHaveSomthing));
            res = await sendBot("ספר לי משהו מעניין");
            AssertNLP.contains(res, DBbotPhrase(Pkey.mightHaveSomthing));
            res = await sendBot("לך תזדיין");
            AssertNLP.contains(res, DBbotPhrase(Pkey.swearResponse));
            res = await sendBot("בן זונה");
            AssertNLP.contains(res, DBbotPhrase(Pkey.swearResponse));
            res = await sendBot("שרמוטה");
            AssertNLP.contains(res, DBbotPhrase(Pkey.swearSuspention));
            res = await sendBot("טוב בא נמשיך");
            AssertNLP.contains(res, DBbotPhrase(Pkey.duringSwearSuspention));
            res = await sendBot("סליחה");

            AssertNLP.contains(res, DBbotPhrase(Pkey.letsContinue));
                

        }



        public async void testLearningMenuStopSession()
        {
            var db = new DataBaseController();

            var res = await getToLearningMenu();
            AssertNLP.contains(res, DBbotPhrase(Pkey.chooseStudyUnits));
            var options = getOptions(res[1]);
            res = await sendBot("תשובה כלשהי");
            res = await sendBot("מספיק");
            AssertNLP.contains(res, DBbotPhrase(Pkey.earlyDiparture));
            AssertNLP.contains(res, DBbotPhrase(Pkey.areYouSure));
            res = await sendBot("כן");
            AssertNLP.contains(res, DBbotPhrase(Pkey.goodbye));
            res = await sendBot("בי");
            Assert.AreEqual(res, new string[] { });
        }

    }


    }


