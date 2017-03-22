using Bot_Application1.Controllers;
using Model;
using Model.dataBase;
using Model.Models;
using Moq;
using NLP;
using NLP.Controllers;
using NLP.HebWords;
using NLP.Models;
using NLP.NLP;
using NLP.WorldObj;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NLP.WorldObj.personObject;

namespace UnitTestProject1
{
    public class MockObjectTestBase
    {
       public Mock<ConversationController> mockConvCtrl = new Mock<ConversationController>();
        public Mock<EducationController> mockEduCtrl = new Mock<EducationController>();
        public Mock<NLPControler> mockNLPCtrl = new Mock<NLPControler>();
        public Mock<MorfAnalizer> mockMorfAnalizer = new Mock<MorfAnalizer>();

        public Mock<OuterAPIController> mockOuterAPICtrl = new Mock<OuterAPIController>();


        public Mock<IUser> mockUserMus = new Mock<IUser>();
        public Mock<IUser> mockUserFem = new Mock<IUser>();
        public Mock<IStudySession> mockStudySession = new Mock<IStudySession>();
       public Mock<IQuestion> mockQuestion1 = new Mock<IQuestion>();
       public Mock<IQuestion> mockQuestion2 = new Mock<IQuestion>();
       public Mock<IQuestion> mockQuestion3 = new Mock<IQuestion>();
       public Mock<ISubQuestion> mockSubQqestion1 = new Mock<ISubQuestion>();
        public Mock<ISubQuestion> mockSubQqestion2 = new Mock<ISubQuestion>();
        public Mock<ISubQuestion> mockSubQqestion3 = new Mock<ISubQuestion>();
        public Mock<DataBaseController> mockDB = new Mock<DataBaseController>();
        public Mock<Ientity> mockEntity1 = new Mock<Ientity>();
        public Mock<Ientity> mockEntity2 = new Mock<Ientity>();
        public Mock<Ientity> mockEntity3 = new Mock<Ientity>();
        public Mock<Ientity> mockEntity4 = new Mock<Ientity>();
        public Mock<Ientity> mockEntity5 = new Mock<Ientity>();
        public Mock<Ientity> mockEntity6 = new Mock<Ientity>();
        public Mock<Ientity> mockEntity7 = new Mock<Ientity>();

        public Mock<WordObject> moqWordObject1 = new Mock<WordObject>();
        public Mock<WordObject> moqWordObject2 = new Mock<WordObject>();
        public Mock<WordObject> moqWordObject3 = new Mock<WordObject>();
        public Mock<WordObject> moqWordObject4 = new Mock<WordObject>();

        public Mock<WordObject> moqWordGufObjectShe = new Mock<WordObject>();
        public Mock<WordObject> moqWordGufObjectThey = new Mock<WordObject>();
        public Mock<WordObject> moqWordGufObjectHe = new Mock<WordObject>();
        public Mock<WordObject> moqWordGufObjectIt = new Mock<WordObject>();


        public List<List<WordObject>> wordObjectsListList;
        public List<WordObject> wordObjectsList;

        public EventObject eventO = new EventObject("הכרזת העצמאות");

        public OrganizationObject orgO = new OrganizationObject("אום");
        public PersonObject persO = new PersonObject("בן גוריון");
        public ConceptObject concO = new ConceptObject("מנדט");
        public List<WorldObject> list1 = new List<WorldObject>();
        public List<WorldObject> list2 = new List<WorldObject>();
  

        internal void initializeMocksObject()
        {
            DataBaseController.setStubInstance(mockDB.Object);


            mockConvCtrl.Setup(x => x.StudySession).Returns(mockStudySession.Object);
            mockConvCtrl.Setup(x => x.Db).Returns(mockDB.Object);
            mockConvCtrl.Setup(x => x.User).Returns(mockUserMus.Object);

            mockUserMus.Setup(x => x.UserName).Returns("יוחאי");
            mockUserMus.Setup(x => x.UserClass).Returns("יא");
            mockUserMus.Setup(x => x.UserGender).Returns("musculine");
            mockUserFem.Setup(x => x.UserName).Returns("מיה");
            mockUserFem.Setup(x => x.UserClass).Returns("י");
            mockUserFem.Setup(x => x.UserGender).Returns("feminine");

            mockQuestion1.Setup(x => x.AnswerScore).Returns(100);
            mockQuestion1.Setup(x => x.Category).Returns("לאומיות");
            mockQuestion2.Setup(x => x.AnswerScore).Returns(100);
            mockQuestion3.Setup(x => x.AnswerScore).Returns(100);

            mockSubQqestion1.Setup(x => x.AnswerScore).Returns(100);
            mockSubQqestion1.Setup(x => x.answerText).Returns("תשובה טובה לתת שאלה 1");

            mockEntity1.Setup(x => x.entityValue).Returns("טובה");
            mockEntity1.Setup(x => x.entitySynonimus).Returns(";טובה;");
            mockEntity1.Setup(x => x.entityType).Returns("personWord");


            mockEntity2.Setup(x => x.entityValue).Returns("הדברים");
            mockEntity2.Setup(x => x.entitySynonimus).Returns(";הדברים;");
            mockEntity2.Setup(x => x.entityType).Returns("conceptWord");

            mockEntity3.Setup(x => x.entityValue).Returns("מקום");
            mockEntity3.Setup(x => x.entitySynonimus).Returns(";מקום;");
            mockEntity3.Setup(x => x.entityType).Returns("locationWord");

            mockEntity4.Setup(x => x.entityValue).Returns("הכרזת העצמאות");
            mockEntity4.Setup(x => x.entitySynonimus).Returns(";הכרזת העצמאות;");
            mockEntity4.Setup(x => x.entityType).Returns("eventWord");

            mockEntity5.Setup(x => x.entityValue).Returns("מגילת העצמאות");
            mockEntity5.Setup(x => x.entitySynonimus).Returns(";מגילת העצמאות;");
            mockEntity5.Setup(x => x.entityType).Returns("conceptWord");

            mockEntity6.Setup(x => x.entityValue).Returns("ההגנה");
            mockEntity6.Setup(x => x.entitySynonimus).Returns(";ההגנה;");
            mockEntity6.Setup(x => x.entityType).Returns("organizationWord");

            mockEntity7.Setup(x => x.entityValue).Returns("דוד בן גוריון");
            mockEntity7.Setup(x => x.entitySynonimus).Returns(";דוד בן גוריון;");
            mockEntity7.Setup(x => x.entityType).Returns("personWord");

            mockStudySession.Setup(x => x.Category).Returns("לאומיות");
            mockStudySession.Setup(x => x.SessionLength).Returns(3);
            mockStudySession.Setup(x => x.QuestionAsked).Returns(new List<IQuestion>());
            mockStudySession.Setup(x => x.CurrentSubQuestion).Returns(mockSubQqestion1.Object);



            moqWordGufObjectShe.Setup(x => x.WordT).Returns(WordObject.WordType.gufWord);
            moqWordGufObjectShe.Setup(x => x.Text).Returns("היא");
            moqWordGufObjectShe.Setup(x => x.haveTypeOf(It.Is<WordObject>(z => z.WordT == WordObject.WordType.gufWord))).Returns(true);
            moqWordGufObjectShe.Setup(x => x.Gender).Returns(genderType.feminine);
            moqWordGufObjectShe.Setup(x => x.Amount).Returns(amountType.singular);

            moqWordGufObjectThey.Setup(x => x.WordT).Returns(WordObject.WordType.gufWord);
            moqWordGufObjectThey.Setup(x => x.Text).Returns("הם");
            moqWordGufObjectThey.Setup(x => x.haveTypeOf(It.Is<WordObject>(z => z.WordT == WordObject.WordType.gufWord))).Returns(true);
            moqWordGufObjectThey.Setup(x => x.Gender).Returns(genderType.masculine);
            moqWordGufObjectThey.Setup(x => x.Amount).Returns(amountType.plural);


            moqWordGufObjectHe.Setup(x => x.WordT).Returns(WordObject.WordType.gufWord);
            moqWordGufObjectHe.Setup(x => x.Text).Returns("הוא");
            moqWordGufObjectHe.Setup(x => x.Gender).Returns(genderType.masculine);
            moqWordGufObjectHe.Setup(x => x.Amount).Returns(amountType.singular);


            moqWordGufObjectIt.Setup(x => x.WordT).Returns(WordObject.WordType.gufWord);
            moqWordGufObjectIt.Setup(x => x.Text).Returns("זה");
            moqWordGufObjectIt.Setup(x => x.Gender).Returns(genderType.unspecified);
            moqWordGufObjectIt.Setup(x => x.Amount).Returns(amountType.unspecified);



            mockDB.Setup(x => x.getAllCategory()).Returns(new string[] { "לאומיות" });
            mockDB.Setup(x => x.getQuestion("לאומיות")).Returns(new IQuestion[] { mockQuestion1.Object });
            mockDB.Setup(x => x.getBotPhrase(It.IsAny<Pkey>(), new string[] { }, new string[] { })).Returns((Pkey key, string[] a, string[] b) => new string[] { Enum.GetName(typeof(Pkey), key) });
            mockDB.Setup(x => x.getEntitys()).Returns(new HashSet<Ientity>(new Ientity[] { mockEntity1.Object,mockEntity2.Object, mockEntity4.Object, mockEntity5.Object, mockEntity6.Object, mockEntity7.Object}));




            //mockNLPCtrl.Setup(x => x.Analize(It.IsAny<string>())).Returns(y => new WorldObject[] { new WordObject(y)});
            eventO.Entity = mockEntity4.Object;
             list1.AddRange(new WorldObject[] { eventO, orgO, persO });
            list2.AddRange(new WorldObject[] {  orgO, persO ,concO});


            moqWordObject1.Setup(x => x.WordT).Returns(WordObject.WordType.conceptWord);
            moqWordObject1.Setup(x => x.Text).Returns("מגילת העצמאות");
            moqWordObject1.Setup(x => x.haveTypeOf(It.Is<WordObject>(z=>z.WordT == WordObject.WordType.conceptWord))).Returns(true);
            moqWordObject1.Setup(x => x.Gender).Returns(genderType.feminine);
            moqWordObject1.Setup(x => x.Amount).Returns(amountType.singular);

            moqWordObject2.Setup(x => x.WordT).Returns(WordObject.WordType.organizationWord);
            moqWordObject2.Setup(x => x.Text).Returns("ההגנה");
            moqWordObject2.Setup(x => x.haveTypeOf(It.Is<WordObject>(z => z.WordT == WordObject.WordType.organizationWord))).Returns(true);
            moqWordObject2.Setup(x => x.Gender).Returns(genderType.masculine);
            moqWordObject2.Setup(x => x.Amount).Returns(amountType.plural);


            moqWordObject3.Setup(x => x.WordT).Returns(WordObject.WordType.personWord);
            moqWordObject3.Setup(x => x.Text).Returns("דוד בן גוריון");
            moqWordObject3.Setup(x => x.haveTypeOf(It.Is<WordObject>(z => z.WordT == WordObject.WordType.personWord))).Returns(true);
            moqWordObject3.Setup(x => x.Gender).Returns(genderType.masculine);
            moqWordObject3.Setup(x => x.Amount).Returns(amountType.singular);


            moqWordObject4.Setup(x => x.WordT).Returns(WordObject.WordType.eventWord);
            moqWordObject4.Setup(x => x.Text).Returns("הכרזת העצמאות");
            moqWordObject4.Setup(x => x.haveTypeOf(It.Is<WordObject>(z => z.WordT == WordObject.WordType.eventWord))).Returns(true);
            moqWordObject4.Setup(x => x.Gender).Returns(genderType.unspecified);
            moqWordObject4.Setup(x => x.Amount).Returns(amountType.unspecified);


            mockOuterAPICtrl.Setup(x => x.correctSpelling(It.IsAny<String>())).Returns((String y)=> y);
            mockOuterAPICtrl.Setup(x => x.getIntentApiAi(It.IsAny<String>(), It.IsAny<String>())).Returns((String text,String context) => "historyAnswer");
            mockOuterAPICtrl.Setup(x => x.sendToHebrewMorphAnalizer(It.IsAny<String>())).Returns((String text) =>
            "[{\"lemma\":\"מגילת העצמאות\",\"ner\":\"O\",\"text\":\"מגילת העצמאות\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"foreign\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false}," +
            "{\"lemma\":\"הכרזת העצמאות\",\"ner\":\"O\",\"text\":\"הכרזת העצמאות\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"foreign\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false}," +
             "{\"lemma\":\"דוד בן גוריון\",\"ner\":\"O\",\"text\":\"דוד בן גוריון\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"foreign\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false}," +
            "{\"lemma\":\"ההגנה\",\"ner\":\"O\",\"text\":\"ההגנה\",\"gender\":\"unspecified\",\"number\":\"unspecified\",\"person\":\"unspecified\",\"polarity\":\"unspecified\",\"pos\":\"foreign\",\"posType\":\"unspecified\",\"prefixes\":[],\"tense\":\"unspecified\",\"isDefinite\":false}]");


            wordObjectsListList = new List<List<WordObject>>();
            wordObjectsList = new WordObject[] {
                moqWordObject1.Object,
                moqWordObject2.Object,
                moqWordObject3.Object,
                moqWordObject4.Object,
        }.ToList();
            wordObjectsListList.Add(wordObjectsList);
            mockMorfAnalizer.Setup(x => x.meniAnalize(It.IsAny<string>(), It.IsAny<bool>())).Returns(wordObjectsListList);

        }






        public string EnumVal(Pkey key)
        {
            return Enum.GetName(typeof(Pkey), key);
        }
    }
}
