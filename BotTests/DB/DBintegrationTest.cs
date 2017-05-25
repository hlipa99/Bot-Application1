using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.dataBase;
using System.Linq;
using NLP.HebWords;
using Bot_Application1.Controllers;

namespace BotTests.Integration_Tests
{
    [TestClass]
    public class DBintegrationTest
    {
        DataBaseController db = new DataBaseController();

        [TestMethod]
        public async void testEntityDB()
        {
            var entities = db.getEntitys();
            foreach (var e in entities) {
                Assert.IsTrue(e.entitySynonimus != null &&
                               !e.entitySynonimus.Contains(";;") &&
                               e.entitySynonimus.StartsWith(";") &&
                               e.entitySynonimus.EndsWith(";") &&
                               Enum.Parse(typeof(WordObject.WordType), e.entityType) != null &&
                               e.entityValue != null
                            ,e.entityValue + "," + e.entitySynonimus);
            }

            

        }

            [TestMethod]
        public async void testMultyEntityDB()
        {
            var entities = db.getEntitys();
            var multyEntities = db.getMultyEntitys();
            foreach (var e in multyEntities)
            {
                Assert.IsTrue(
                    e.parts != null &&
                   e.entityValue != null &&
                  e.parts.StartsWith(";") &&
                  e.parts.EndsWith(";") &&
                  Enum.Parse(typeof(WordObject.WordType), e.entityType) != null &&
                  e.entityValue != null
               ,e.entityValue);

                foreach(var me in e.parts.Split(';'))
                {
                    foreach (var p in me.Split('#'))
                    {
                        Assert.IsTrue(p == "" || entities.Count(x=>x.entityValue == p.Trim()) > 0,e.entityValue + ":" + p);
                    }
                }

            }


        }

        [TestMethod]
        public async void testQuestionDB()
        {
            MediaController mc = new MediaController();
            foreach (var subject in db.getAllCategory())
            {

                var questions = db.getQuestion(subject);
                foreach (var q in questions)
                {
                    Assert.IsTrue(q.Category == subject &&
                    q.SubCategory != null &&
                    q.SubCategory != "" &&
                    q.QuestionText != "" &&
                     q.QuestionText != null);


                    if (q.questionType.Trim() == "sorcePic")
                    {
                        Assert.IsTrue(mc.getFileUrl(q.questionMedia) != null && mc.getFileUrl(q.questionMedia) != "");
                    }


                    Assert.IsTrue(
                        q.SubQuestion.All(sq =>
                            sq.answerText != null &&
                            sq.answerText.Trim() != "" &&
                            sq.questionID == q.QuestionID));

                    var n = q.SubQuestion.Count;
                    Assert.AreEqual(
                        q.SubQuestion.Select(sq => int.Parse(sq.subQuestionID)).Sum(), n * (n + 1) / 2);
                }
            }
        }

     

        [TestMethod]
        public async void testBotPrasesDB()
        {

            foreach (Model.Pkey k in Enum.GetValues(typeof(Model.Pkey)))
            {
                var p = db.getBotPhrase(k, new string[] { }, new string[] { });
                Assert.IsTrue(p.Any(), Enum.GetName(typeof(Model.Pkey), k));
            }
        }

        [TestMethod]
        public async void testmediaForCategory()
        {
            foreach(var s in db.getAllCategory())
            {
                var media = db.getMedia(s, "img", null);
                Assert.IsTrue(media.Any());
            }
        }




    }
}
