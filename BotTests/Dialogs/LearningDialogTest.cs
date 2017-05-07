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
        public void TestInitializeAttribute()
        {
            deleteProfile();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConvID = "";
            // client = null;
        }



      

        Random r = new Random();
        [TestMethod]
        public void testLearningMenu() {
            var db = new DataBaseController();

            var optionsRes = getToLearningMenu();
            AssertNLP.contains(optionsRes, DBbotPhrase(Pkey.chooseStudyUnits));



            foreach (var category in db.getAllCategory()) {

                //subject is avialabale

                AssertNLP.contains(optionsRes, category);

                //bad - try somthing else
                var res = sendBot("ביולוגיה");
                AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

                //ugly
              //  res = sendBot("");
              //  Assert.AreEqual(res.Count,0);

                //good - lets learn
                res = sendBot(category);
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

                    var rnd = r.Next(5);
                    switch (rnd)
                    {

                        case 0:
                            res = sendBot("הפסקה");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                            res = sendBot("כן");
                            //AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
                            AssertNLP.contains(res, DBbotPhrase(Pkey.imWaiting));
                            Thread.Sleep(10000);
                            res = sendBot("חזרתי");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.letsContinue));
                            AssertNLP.contains(res, subQuestion.questionText);
                            break;

                        case 1:
                            res = sendBot("מספיק");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.earlyDiparture));
                            AssertNLP.contains(res, DBbotPhrase(Pkey.areYouSure));
                            res = sendBot("לא");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.keepLearning));
                            AssertNLP.contains(res, subQuestion.questionText);
                            break;

                        default:
                            break;
                    }

                    res = sendBot(subQuestion.answerText);

                    AssertNLP.contains(res, DBbotPhrase(Pkey.veryGood));

                    AssertNLP.contains(res, DBbotPhrase(Pkey.moveToNextSubQuestion));

                    }

                    res = sendBot("תפריט");
                 //   res = sendBot("כן");

                  
                
            }
        }


        public void testLearningMenuStopSession()
        {
            var db = new DataBaseController();

            var res = getToLearningMenu();
            AssertNLP.contains(res, DBbotPhrase(Pkey.chooseStudyUnits));
            var options = getOptions(res[1]);
            res = sendBot("תשובה כלשהי");
            res = sendBot("מספיק");
            AssertNLP.contains(res, DBbotPhrase(Pkey.earlyDiparture));
            AssertNLP.contains(res, DBbotPhrase(Pkey.areYouSure));
            res = sendBot("כן");
            AssertNLP.contains(res, DBbotPhrase(Pkey.goodbye));
            res = sendBot("בי");
            Assert.AreEqual(res, new string[] { });
        }

    }


    }


