using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLPtest;
using NLPtest.NLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.Tests
{
    [TestClass()]
    public class UnitTest1
    {

        MorfAnalizer ma = new MorfAnalizer();

       [TestInitialize]
        public void Init()
        {
            ma = new MorfAnalizer();
        }

        [TestCleanup]
        public void cleanUp()
        {
            ma = null;
        }

        [TestMethod()]
        public void MorfAnalizerTest()
        {
            Assert.IsNotNull(ma);
        }


        [TestMethod()]
        public void meniAnalizeTest()
        {
           var res = ma.meniAnalize("אחת שתיים שלוש");
            Assert.IsTrue(res.Count == 3);
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