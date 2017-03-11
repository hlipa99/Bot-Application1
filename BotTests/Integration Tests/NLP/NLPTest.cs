
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLP;
using NLP.NLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace NLP.Tests
{
    [TestClass()]
    public class MorfAnalizerIntegrationTest :  ObjectTestBase
    {

        MorfAnalizer ma = new MorfAnalizer();

       [TestInitialize]
        public void Init()
        {
            ma = new MorfAnalizer();
            initializeObject();
        }


        [TestMethod()]
        public void meniAnalizeTest()
        {
            ma.HttpCtrl =  OuterAPICtrl ;
            ma.DBctrl1 =  DB ;
           var res = ma.meniAnalize(userAnswerSubQuestion1, true);


            //good
            Assert.IsTrue(res[1].Contains(WordObject1));
            Assert.IsTrue(res.FirstOrDefault().Contains(WordObject2));

            //bad
            Assert.IsFalse(res.FirstOrDefault().Contains(WordObject3));
            Assert.IsFalse(res.FirstOrDefault().Contains(WordObject4));


            //ugly
            res = ma.meniAnalize("", false);
            Assert.AreEqual(res.Count, 0);

            res = ma.meniAnalize(null, false);
            Assert.AreEqual(res.Count, 0);

        }

        //[TestMethod()]
        //public void getClassTest()
        //{
        //    var res = 
        //    Assert.Equals(ma.getClass("א"),"א");
        //    Assert.Equals(ma.getClass("יב"), "יב");
        //    Assert.Equals(ma.getClass("'יב"), "יב");
        //    Assert.Equals(ma.getClass("שמינית"), "יב");
        //    Assert.Equals(ma.getClass("יג"), null);
        //    Assert.Equals(ma.GetGender("דגדשגכדכג"), null);
        //    Assert.Equals(ma.GetGender("dffdfd"), null);
        //}

        //[TestMethod()]
        //public void getNameTest()
        //{
        //    Assert.Equals(ma.getName("יוחאי"), "יוחאי");
        //    Assert.Equals(ma.getName("היי אני נירן"), "נירן");
        //    Assert.Equals(ma.getName("םןחיגדכגד"), null);
        //    Assert.Equals(ma.getName("gfgfdfgg"), null);
        //}

        //[TestMethod()]
        //public void GetGenderTest()
        //{
        //    Assert.Equals(ma.GetGender("גבר"), "masculine");
        //    Assert.Equals(ma.GetGender("בן"), "masculine");
        //    Assert.Equals(ma.GetGender("בחור"), "masculine");
        //    Assert.Equals(ma.GetGender("איש"), "masculine");
        //    Assert.Equals(ma.GetGender("אני גבר"), "masculine");
        //    Assert.Equals(ma.GetGender("אישה"), "feminine");
        //    Assert.Equals(ma.GetGender("בחורה"), "feminine");
        //    Assert.Equals(ma.GetGender("ילדה"), "feminine");
        //    Assert.Equals(ma.GetGender("בת"), "feminine");
        //    Assert.Equals(ma.GetGender("מה אכפת לך"), null);
        //    Assert.Equals(ma.GetGender("למה זה משנה"), null);
        //    Assert.Equals(ma.GetGender("דגדשגכדכג"), null);
        //    Assert.Equals(ma.GetGender("dffdfd"), null);
        //}

        //[TestMethod()]
        //public void GetGeneralFeelingTest()
        //{
        //    Assert.Equals(ma.GetGender("הכל טוב"), "good");
        //    Assert.Equals(ma.GetGender("טוב"), "good");
        //    Assert.Equals(ma.GetGender("בסדר"), "good");
        //    Assert.Equals(ma.GetGender("סבבה"), "good");
        //    Assert.Equals(ma.GetGender("אחלה"), "good");
        //    Assert.Equals(ma.GetGender("מצויין"), "good");
        //    Assert.Equals(ma.GetGender("בננה"), "netural");
        //    Assert.Equals(ma.GetGender("רבין"), "netural");
        //    Assert.Equals(ma.GetGender("בלה בלה"), "netural");
        //    Assert.Equals(ma.GetGender("רע"), "bad");
        //    Assert.Equals(ma.GetGender("לא טוב"), "bad");
        //    Assert.Equals(ma.GetGender("על הפנים"), "bad");
        //    Assert.Equals(ma.GetGender("גרוע"), "bad");
        //    Assert.Equals(ma.GetGender("לא משהו"), "bad");
        //}
    }
}