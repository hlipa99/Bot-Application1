using Bot_Application1.Controllers;
using Model;
using Model.dataBase;
using Model.Models;
using Moq;
using NLPtest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class MockObjectTestBase
    {
       public Mock<ConversationController> mockConvCtrl = new Mock<ConversationController>();
        public Mock<EducationController> mockEduCtrl = new Mock<EducationController>();
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

            mockEntity1.Setup(x => x.EntityValue).Returns("טובה");
            mockEntity1.Setup(x => x.EntitySynonimus).Returns(";טובה;");
            mockEntity1.Setup(x => x.EntityType).Returns("personWord");
                                 
            mockEntity2.Setup(x => x.EntityValue).Returns("הדברים");
            mockEntity2.Setup(x => x.EntitySynonimus).Returns(";הדברים;");
            mockEntity2.Setup(x => x.EntityType).Returns("conceptWord");

            mockEntity3.Setup(x => x.EntityValue).Returns("מקום");
            mockEntity3.Setup(x => x.EntitySynonimus).Returns(";מקום;");
            mockEntity3.Setup(x => x.EntityType).Returns("locationWord");

            mockStudySession.Setup(x => x.Category).Returns("לאומיות");
            mockStudySession.Setup(x => x.SessionLength).Returns(3);
            mockStudySession.Setup(x => x.QuestionAsked).Returns(new HashSet<IQuestion>());
            mockStudySession.Setup(x => x.CurrentSubQuestion).Returns(mockSubQqestion1.Object);

            mockDB.Setup(x => x.getAllCategory()).Returns(new string[] { "לאומיות" });
            mockDB.Setup(x => x.getQuestion("לאומיות")).Returns(new IQuestion[] { mockQuestion1.Object });
            mockDB.Setup(x => x.getBotPhrase(It.IsAny<Pkey>(), new string[] { }, new string[] { })).Returns((Pkey key, string[] a, string[] b) => new string[] { Enum.GetName(typeof(Pkey), key) });
            mockDB.Setup(x => x.getEntitys()).Returns(new HashSet<Ientity>(new Ientity[] { mockEntity1.Object,mockEntity2.Object }));




        }

        public string EnumVal(Pkey key)
        {
            return Enum.GetName(typeof(Pkey), key);
        }
    }
}
