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
        public void TestEntityDB()
        {
            var entities = db.getEntitys();
            Assert.IsTrue(entities.All(
                e => e.entitySynonimus != null &&
               !e.entitySynonimus.Contains(";;") &&
               e.entitySynonimus.StartsWith(";") &&
               e.entitySynonimus.EndsWith(";") &&
               e.entitySynonimus.Contains(";" + e.entityValue + ";") &&
               Enum.Parse(typeof(WordObject.WordType), e.entityType) != null &&
               e.entityValue != null
            ));

        }

            [TestMethod]
        public void TestMultyEntityDB()
        {
            var entities = db.getEntitys();
            var multyEntities = db.getMultyEntitys();

            Assert.IsTrue(multyEntities.All(
               e => e.parts != null &&
               e.entityValue != null &&
               e.parts.Split(';').All(me => me == "" || me.Split('#').All(p => entities.Count(ent => ent.entityValue == p) > 0)) &&
              e.parts.StartsWith(";") &&
              e.parts.EndsWith(";") &&
              Enum.Parse(typeof(WordObject.WordType), e.entityType) != null &&
              e.entityValue != null
           ));


        }

        [TestMethod]
        public void TestQuestionDB()
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
        public void TestBotPrasesDB()
        {

            foreach (var k in Enum.GetValues(typeof(Model.Pkey)))
            {
                var p = db.getBotPhrase((Model.Pkey)k, new string[] { }, new string[] { });
                Assert.IsTrue(p.Any());
            }
        }

        [TestMethod]
        public void TestmediaForCategory()
        {
            foreach(var s in db.getAllCategory())
            {
                var media = db.getMedia(s, "img", null);
                Assert.IsTrue(media.Any());
            }
        }




    }
}
