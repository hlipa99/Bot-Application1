using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_Application1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Bot_Application1.Exceptions;
using System.Threading.Tasks;
using Moq;
using Model.Models;
using NLP.Models;
using Model;
using Model.dataBase;
using UnitTestProject1;

namespace Bot_Application1.Controllers.Tests
{
    [TestClass()]
    public class EducationControllerTests : MockObjectTestBase
    {

        EducationController eduCtrl;
        [TestInitialize()]
        public void EducationControllerTest()
        {
            initializeMocksObject();
            eduCtrl = new EducationController(mockUserMus.Object, mockStudySession.Object, mockConvCtrl.Object);
        }

       

        [TestMethod()]
        public void getQuestionTest()
        {
            eduCtrl.Db = mockDB.Object;


            //good
            mockStudySession.Object.Category = "לאומיות";
            Assert.AreEqual(eduCtrl.getQuestion(), mockQuestion1.Object);

            //bad
            mockStudySession.Setup(x => x.Category).Returns("נושא שלא קיים");
            Assert.AreEqual(eduCtrl.getQuestion(), null);
          
            //ugly
            try
            {
                mockStudySession.Setup(x => x.Category).Returns("לאומיות");
                mockStudySession.Setup(x => x.QuestionAsked).Returns(new List<IQuestion>(mockDB.Object.getQuestion("לאומיות")));
                eduCtrl.getQuestion();
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.AreEqual(typeof(CategoryOutOfQuestionException), ex.GetType());
            }
    
        }


    private string EnumVal(Pkey key)
    {
        return Enum.GetName(typeof(Pkey), key);
    }
}
}