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

namespace UnitTestProject1
{
    [TestClass]
    public class LearningDialogTest : DialogsTestsBase
    {

        [TestInitialize]
        public void TestInitializeAttribute()
        {
            var task = sendMessage("/deleteprofile");
            var response = task.Result;
            AssertNLP.contains(response,  "User profile deleted!" );
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConvID1 = "";
            // client = null;
        }

        private List<string> getToLearningMenu()
        {
            var res = sendBot("היי");
            res = sendBot("יוחאי");
            res = sendBot("בן");
            res = sendBot("יא");
            var options = getOptions(res[2]);
            res = sendBot(options[2]);
            return res;
        }

        Random r = new Random();
        [TestMethod]
        public void testLearningMenu() {
            var db = new DataBaseController();

            var res = getToLearningMenu();
            AssertNLP.contains(res, DBbotPhrase(Pkey.chooseStudyUnits));
            var options = getOptions(res[1]);


            foreach (var category in db.getAllCategory()) {

                //subject is avialabale
                Assert.IsTrue(new List<string>(options).Contains(category));

                //bad - try somthing else
                res = sendBot("ביולוגיה");
                AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

                //ugly
                res = sendBot("");
                AssertNLP.contains(res, DBbotPhrase(Pkey.NotAnOption));

                //good - lets learn
                res = sendBot(category);
                AssertNLP.contains(res, DBbotPhrase(Pkey.areUReaddyToLearn));


                AssertNLP.contains(res, DBbotPhrase(Pkey.firstQuestion));
                foreach (var i in options) {
                    var questions = new DataBaseController().getQuestion(category);
                    var questionOpt = new List<IQuestion>(questions).FindAll(x => res.Contains(x.QuestionText));
                    Assert.AreEqual(questionOpt.Count, 1);
                    var question = questionOpt[0];

                    Assert.AreEqual(question.Category,category);

                    var counter = 0;
                    foreach (var subQuestion in question.SubQuestion)
                    {
                        AssertNLP.contains(res, subQuestion.questionText);

                        var rnd = r.Next(3);
                        switch (rnd){
                            case 0:
                                res = sendBot("הפסקה");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                                res = sendBot("לא");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.keepLearning));
                                AssertNLP.contains(res, subQuestion.questionText);
                                break;
                            case 1:
                                res = sendBot("הפסקה");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                                res = sendBot("כן");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.imWaiting));
                                res = sendBot("חזרתי");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.letsContinue));
                                AssertNLP.contains(res, subQuestion.questionText);
                                break;
                  
                            case 2:
                                res = sendBot("הפסקה");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                                res = sendBot("לא");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.keepLearning));
                                AssertNLP.contains(res, subQuestion.questionText);
                                res = sendBot("הפסקה");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                                res = sendBot("כן");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.imWaiting));
                                res = sendBot("חזרתי");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.letsContinue));
                                AssertNLP.contains(res, subQuestion.questionText);
                                break;

                            case 4:
                                res = sendBot("מספיק");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.earlyDiparture));
                                AssertNLP.contains(res, DBbotPhrase(Pkey.areYouSure));
                                res = sendBot("לא");
                                AssertNLP.contains(res, DBbotPhrase(Pkey.keepLearning));
                                AssertNLP.contains(res, subQuestion.questionText);
                                break;

                            case 5:
                            default:
                                break;
                        }
                      
                        res = sendBot(subQuestion.answerText);

                        AssertNLP.contains(res, DBbotPhrase(Pkey.veryGood));

                        if (counter < 3)
                        {
                            AssertNLP.contains(res, DBbotPhrase(Pkey.moveToNextSubQuestion));
                        }
                        else
                        {
                            AssertNLP.contains(res, DBbotPhrase(Pkey.suggestBreak));
                            res = sendBot("לא");
                            AssertNLP.contains(res, DBbotPhrase(Pkey.ok));
                            AssertNLP.contains(res, DBbotPhrase(Pkey.moveToNextSubQuestion));
                        }
                    }

                    AssertNLP.contains(res, DBbotPhrase(Pkey.SubjectNotAvialable));
                    res = sendBot("כן");
                }
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


